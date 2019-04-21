using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Spells
{
	public class SpiritualLink : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Lightning Strike", "Sphir Wahj",
				212,
				9041,
				GetSpellGems.GetGems( typeof(SpiritualLink) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public static Hashtable m_LinkTable = new Hashtable();

		public static Mobile GetSpiritualLink( Mobile m )
		{
			if ( m == null )
				return null;

			Mobile link = (Mobile)m_LinkTable[m];

			if ( link == m )
				link = null;

			return link;
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}

		public SpiritualLink( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to link yourself to." );
		}

		public void Target( PlayerMobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( m_LinkTable.Contains( Caster ) )
			{
				Caster.SendMessage( "Your Spiritual Link has been broken." );
				m_LinkTable.Remove( Caster );
				Caster.EndAction( typeof(SpiritualLink) );
				Caster.CloseGump( typeof( SpiritualLinkButtonGump ) );
				if ( m_LinkTable.Contains( m ) )
				{
					m.SendMessage( "Your Spiritual Link has been broken." );
					m_LinkTable.Remove( m );
					m.EndAction( typeof(SpiritualLink) );
				}
			}
			else if ( m_LinkTable.Contains( m ) )
			{
				m.SendMessage( "Your Spiritual Link has been broken." );
				m_LinkTable.Remove( m );
				m.EndAction( typeof(SpiritualLink) );
			}
			else if ( !Caster.CanBeginAction( typeof(SpiritualLink) ) )
			{
				Caster.SendMessage( "You are already bonded in a Spiritual Link." );
			}
			else if ( !m.CanBeginAction( typeof(SpiritualLink) ) )
			{
				if ( m.Player )
					Caster.SendMessage( "This person is already bonded in a Spiritual Link." );
				else
					Caster.SendMessage( "That creature is already bonded in a Spiritual Link." );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				SpellHelper.Turn( Caster, m );
				m.SendMessage( Caster.Name +" would like to form a spiritual link with you." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				TimeSpan duration = TimeSpan.FromMinutes( (runelevel*(Caster.Skills.Inscribe.Fixed/30) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, runelevel*(Caster.Skills.Inscribe.Fixed/30)) );

				m.SendGump( new SpiritualLinkGump( m, Caster, duration, Rune, this ) );
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private SpiritualLink m_Owner;

			public InternalTarget( SpiritualLink owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is PlayerMobile )
					if ( (PlayerMobile)o != from )
						m_Owner.Target( (PlayerMobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}	
	}
		public class SpiritualLinkButtonGump : Gump
		{
			private Mobile m_Mobile;
			private Mobile m_Caster;
			private SpellRune Rune;
			private SpiritualLink m_Spell;

			public SpiritualLinkButtonGump( Mobile mobile, Mobile caster, SpellRune rune, SpiritualLink spell ) : base( 25, 50 )
			{
				Closable = false;

				m_Mobile = mobile;
				m_Caster = caster;
				Rune = rune;
				m_Spell = spell;

				AddPage( 0 );

				AddButton( 10, 390, 1209, 1210, 1, GumpButtonType.Reply, 0 );
				AddLabel( 30, 390, 1152, "Heal" );
				AddButton( 10, 410, 1209, 1210, 2, GumpButtonType.Reply, 0 );
				AddLabel( 30, 410, 1152, "Cure" );
				AddButton( 60, 390, 1209, 1210, 3, GumpButtonType.Reply, 0 );
				AddLabel( 90, 390, 1152, "Refresh" );
				AddButton( 60, 410, 1209, 1210, 4, GumpButtonType.Reply, 0 );
				AddLabel( 90, 410, 1152, "Mana Refresh" );
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				if ( Rune == null )
				{
					return;
				}
				int runelevel = Rune.Level;
				if ( runelevel >= 0 )
					runelevel = 1;

				if ( info.ButtonID == 1 )
				{
					if ( m_Mobile.Hits < m_Mobile.HitsMax )
					{
						int toHeal = ( m_Mobile.HitsMax - m_Mobile.Hits );
						toHeal /= 5;
						toHeal *= runelevel;
						toHeal += RSCA.CalculateSI(m_Caster, toHeal);
						toHeal += ImperialOrb.IncI( m_Caster, m_Spell );
						m_Mobile.Heal( (int)(toHeal * RuneSpell.TMRG) );
						AOS.Damage( m_Caster, m_Caster, toHeal, 20, 20, 20, 20, 20 );
						m_Mobile.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
						m_Mobile.PlaySound( 0x1F2 );
					}
					else
						m_Caster.SendMessage( m_Mobile.Name +" does not need to be healed." );

					m_Caster.SendGump( new SpiritualLinkButtonGump( m_Mobile, m_Caster, Rune, m_Spell ) );
				}
				if ( info.ButtonID == 2 )
				{
					AOS.Damage( m_Caster, m_Caster, (20-RSCA.CalculateSI(m_Caster, 20))-ImperialOrb.IncI( m_Caster, m_Spell ), 20, 20, 20, 20, 20 );
					if ( m_Mobile.Poisoned && m_Mobile.CurePoison( m_Caster ) )
					{
						m_Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!
						m_Mobile.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
						m_Mobile.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
						m_Mobile.PlaySound( 0x1E0 );
					}
					else
						m_Caster.SendMessage( m_Mobile.Name +" does not need to be cured." );

					m_Caster.SendGump( new SpiritualLinkButtonGump( m_Mobile, m_Caster, Rune, m_Spell ) );
				}
				if ( info.ButtonID == 3 )
				{
					if ( m_Mobile.Stam < m_Mobile.StamMax )
					{
						int toRefresh = ( m_Mobile.StamMax - m_Mobile.Stam );
						toRefresh /= 5;
						toRefresh *= runelevel;
						toRefresh += RSCA.CalculateSI(m_Caster, toRefresh);
						toRefresh += ImperialOrb.IncI( m_Caster, m_Spell );
						m_Mobile.Stam += (int)(toRefresh);
						m_Caster.Stam -= toRefresh;
						m_Mobile.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
						m_Mobile.PlaySound( 0x1F2 );
					}
					else
						m_Caster.SendMessage( m_Mobile.Name +" does not need to have their stamina refreshed." );

					m_Caster.SendGump( new SpiritualLinkButtonGump( m_Mobile, m_Caster, Rune, m_Spell ) );
				}
				if ( info.ButtonID == 4 )
				{
					if ( m_Mobile.Mana < m_Mobile.ManaMax )
					{
						int toRefresh = ( m_Mobile.ManaMax - m_Mobile.Mana );
						toRefresh /= 5;
						toRefresh *= runelevel;
						toRefresh += RSCA.CalculateSI(m_Caster, toRefresh);
						toRefresh += ImperialOrb.IncI( m_Caster, m_Spell );
						m_Mobile.Mana += (int)(toRefresh);
						m_Caster.Mana -= toRefresh;
						m_Mobile.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
						m_Mobile.PlaySound( 0x1F2 );
					}
					else
						m_Caster.SendMessage( m_Mobile.Name +" does not need to have their mana refreshed." );

					m_Caster.SendGump( new SpiritualLinkButtonGump( m_Mobile, m_Caster, Rune, m_Spell ) );
				}
			}
		}
	public class SpiritualLinkGump : Gump
	{
		private Mobile m_Mobile;
		private Mobile m_Caster;
		private TimeSpan m_Expire;
		private SpiritualLink m_Spell;
		private SpellRune Rune;

		public SpiritualLinkGump( Mobile mobile, Mobile caster, TimeSpan duration, SpellRune rune, SpiritualLink spell ) : base( 25, 50 )
		{
			m_Mobile = mobile;
			m_Caster = caster;
			m_Expire = duration;
			m_Spell = spell;
			Rune = rune;

			AddPage( 0 );

			AddBackground( 0, 0, 420, 150, 5054 );

			AddImageTiled( 10, 10, 400, 130, 2624 );
			AddAlphaRegion( 10, 10, 400, 130 );

			AddLabel( 30, 30, 1152, "This link will let "+ m_Caster.Name +" know when you are harmed, heal you," );
			AddLabel( 30, 50, 1152, "and cure you from any distance." );

			AddButton( 100, 80, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddLabel( 130, 80, 1152, "Accept" );
			AddButton( 100, 105, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddLabel( 130, 105, 1152, "Decline" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 0 )
			{
				m_Caster.SendMessage( m_Mobile.Name +" has declined your spiritual link offer." );
				m_Mobile.SendMessage( "You have declined "+ m_Caster.Name +"'s spiritual link offer." );
			}
			if ( info.ButtonID == 1 )
			{
				m_Caster.SendMessage( m_Mobile.Name +" has accepted your spiritual link offer." );
				m_Mobile.SendMessage( "You have accepted "+ m_Caster.Name +"'s spiritual link offer." );
				m_Caster.BeginAction( typeof(SpiritualLink) );
				m_Mobile.BeginAction( typeof(SpiritualLink) );
				Spells.SpiritualLink.m_LinkTable[m_Caster] = m_Caster;
				Spells.SpiritualLink.m_LinkTable[m_Mobile] = m_Caster;
				new SpiritualLinkTimer( m_Caster, m_Mobile, m_Expire, m_Spell ).Start();
				m_Caster.SendGump( new SpiritualLinkButtonGump( m_Mobile, m_Caster, Rune, m_Spell ) );
			}
		}
	}

	public class SpiritualLinkTimer : Timer
	{
		private Mobile m_Caster;
		private Mobile m_Target;
		private DateTime m_End;
		private SpiritualLink m_Spell;

		public SpiritualLinkTimer( Mobile caster, Mobile target, TimeSpan delay, SpiritualLink spell ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
		{
			m_Caster = caster;
			m_Target = target;
			m_End = DateTime.Now + delay;
			m_Spell = spell;

			Priority = TimerPriority.FiftyMS;
		}

		protected override void OnTick()
		{
			if ( m_Target.CanBeginAction( typeof(SpiritualLink) ) || m_Caster.CanBeginAction( typeof(SpiritualLink) ) || m_Caster.Deleted || m_Target.Deleted || !m_Caster.Alive || !m_Target.Alive || DateTime.Now >= m_End )
			{
				m_Caster.CloseGump( typeof( SpiritualLinkButtonGump ) );
				m_Caster.SendMessage( "Your Spiritual Link has been broken." );
				m_Target.SendMessage( "Your Spiritual Link has been broken." );

				Spells.SpiritualLink.m_LinkTable.Remove( m_Caster );
				Spells.SpiritualLink.m_LinkTable.Remove( m_Target );
				m_Caster.EndAction( typeof(SpiritualLink) );
				m_Target.EndAction( typeof(SpiritualLink) );

				Stop();
			}
		}
	}
}
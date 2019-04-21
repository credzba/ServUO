using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Spells
{
	public class LinkOfTheTree : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Link Of The Tree", "Cadre Buwo",
				212,
				9041,
				GetSpellGems.GetGems( typeof(LinkOfTheTree) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public static Hashtable m_LinkTable = new Hashtable();

		public static Mobile GetLinkOfTheTree( Mobile m )
		{
			if ( m == null )
				return null;

			Mobile link = (Mobile)m_LinkTable[m];

			if ( link == m )
				link = null;

			return link;
		}

		public LinkOfTheTree( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
				Caster.SendMessage( "Your Link of the Tree has been broken." );
				m_LinkTable.Remove( Caster );
				Caster.EndAction( typeof(LinkOfTheTree) );
				if ( m_LinkTable.Contains( m ) )
				{
					m.SendMessage( "Your Link of the Tree has been broken." );
					m_LinkTable.Remove( m );
					m.EndAction( typeof(LinkOfTheTree) );
				}
			}
			else if ( m_LinkTable.Contains( m ) )
			{
				m.SendMessage( "Your Link of the Tree has been broken." );
				m_LinkTable.Remove( m );
				m.EndAction( typeof(LinkOfTheTree) );
			}
			else if ( !Caster.CanBeginAction( typeof(LinkOfTheTree) ) )
			{
				Caster.SendMessage( "You are already bonded in a Link of the Tree." );
			}
			else if ( !m.CanBeginAction( typeof(LinkOfTheTree) ) )
			{
				if ( m.Player )
					Caster.SendMessage( "This person is already bonded in a Link of the Tree." );
				else
					Caster.SendMessage( "That creature is already bonded in a Link of the Tree." );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				SpellHelper.Turn( Caster, m );
				m.SendMessage( Caster.Name +" would like to form a Link of the Tree with you." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				TimeSpan duration = TimeSpan.FromMinutes( (runelevel*(Caster.Skills.Inscribe.Value/60) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, runelevel*(Caster.Skills.Inscribe.Value/60)) );

				m.SendGump( new LinkOfTheTreeGump( m, Caster, duration, Rune, this ) );
			}
			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;
			private Mobile m_Target;
			private DateTime m_End;
			private LinkOfTheTree m_Spell;

			public InternalTimer( Mobile caster, Mobile target, TimeSpan delay, LinkOfTheTree spell ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
			{
				m_Caster = caster;
				m_Target = target;
				m_End = DateTime.Now + delay;
				m_Spell = spell;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Caster.Deleted || m_Target.Deleted || !m_Caster.Alive || !m_Target.Alive || DateTime.Now >= m_End )
				{
					m_Caster.SendMessage( "Your Link of the Tree has been broken." );
					m_Target.SendMessage( "Your Link of the Tree has been broken." );

					Spells.LinkOfTheTree.m_LinkTable.Remove( m_Caster );
					Spells.LinkOfTheTree.m_LinkTable.Remove( m_Target );
					m_Caster.EndAction( typeof(LinkOfTheTree) );
					m_Target.EndAction( typeof(LinkOfTheTree) );

					Stop();
				}
			}
		}

		private class InternalTarget : Target
		{
			private LinkOfTheTree m_Owner;

			public InternalTarget( LinkOfTheTree owner ) : base( 12, true, TargetFlags.None )
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

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}
	}
	public class LinkOfTheTreeGump : Gump
	{
		private Mobile m_Mobile;
		private Mobile m_Caster;
		private TimeSpan m_Expire;
		private LinkOfTheTree m_Spell;
		private SpellRune Rune;

		public LinkOfTheTreeGump( Mobile mobile, Mobile caster, TimeSpan duration, SpellRune rune, LinkOfTheTree spell ) : base( 25, 50 )
		{
			m_Mobile = mobile;
			m_Caster = caster;
			m_Expire = duration;
			m_Spell = spell;
			Rune = rune;

			AddPage( 0 );
			AddBackground( 0, 0, 420, 170, 5054 );
			AddImageTiled( 10, 10, 400, 150, 2624 );
			AddAlphaRegion( 10, 10, 400, 150 );
			AddLabel( 30, 30, 1152, "This link will let "+ m_Caster.Name +" take the damage instead of you." );
			AddButton( 100, 100, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddLabel( 130, 100, 1152, "Accept" );
			AddButton( 100, 125, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddLabel( 130, 125, 1152, "Decline" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 0 )
			{
				m_Caster.SendMessage( m_Mobile.Name +" has declined your Link of the Tree offer." );
				m_Mobile.SendMessage( "You have declined "+ m_Caster.Name +"'s Link of the Tree offer." );
			}
			if ( info.ButtonID == 1 )
			{
				m_Caster.SendMessage( m_Mobile.Name +" has accepted your Link of the Tree offer." );
				m_Mobile.SendMessage( "You have accepted "+ m_Caster.Name +"'s Link of the Tree offer." );
				m_Caster.BeginAction( typeof(LinkOfTheTree) );
				m_Mobile.BeginAction( typeof(LinkOfTheTree) );
				Spells.LinkOfTheTree.m_LinkTable[m_Caster] = m_Caster;
				Spells.LinkOfTheTree.m_LinkTable[m_Mobile] = m_Caster;
				new InternalTimer( m_Caster, m_Mobile, m_Expire, m_Spell ).Start();
			}
		}
	}

	public class InternalTimer : Timer
	{
		private Mobile m_Caster;
		private Mobile m_Target;
		private DateTime m_End;
		private LinkOfTheTree m_Spell;

		public InternalTimer( Mobile caster, Mobile target, TimeSpan delay, LinkOfTheTree spell ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
		{
			m_Caster = caster;
			m_Target = target;
			m_End = DateTime.Now + delay;
			m_Spell = spell;

			Priority = TimerPriority.FiftyMS;
		}

		protected override void OnTick()
		{
			if ( m_Caster.Deleted || m_Target.Deleted || !m_Caster.Alive || !m_Target.Alive || DateTime.Now >= m_End )
			{
				m_Caster.SendMessage( "Your Link of the Tree has been broken." );
				m_Target.SendMessage( "Your Link of the Tree has been broken." );

				Spells.LinkOfTheTree.m_LinkTable.Remove( m_Caster );
				Spells.LinkOfTheTree.m_LinkTable.Remove( m_Target );
				m_Caster.EndAction( typeof(LinkOfTheTree) );
				m_Target.EndAction( typeof(LinkOfTheTree) );

				Stop();
			}
		}
	}
}
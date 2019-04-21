using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class CircleOfLightning : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Circle Of Lightning", "Artu Tid",
				212,
				9041,
				GetSpellGems.GetGems( typeof(CircleOfLightning) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public override int RuneGainMax{ get{ return 60; } }
		public override int RuneGainMin{ get{ return 30; } }

		public CircleOfLightning( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to cast this spell onto." );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( !m.CanBeginAction( typeof(CircleOfLightning) ))
			{
				m.SendMessage( "You take the spell off." );
				m.EndAction( typeof( CircleOfLightning ) );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				m.SendMessage( "Your body has been enveloped by an array of lightning." );
				m.BeginAction( typeof( CircleOfLightning ) );

				m.BoltEffect( 0 );

				DateTime Expire = DateTime.Now + TimeSpan.FromMinutes( ((Caster.Skills[SkillName.Inscribe].Value / 40.0)*RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				Expire += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, Caster.Skills[SkillName.Inscribe].Value / 40.0) );
				new InternalTimer( m, Expire, Caster, Rune, this ).Start();
				new Internal2Timer( m, Expire, this ).Start();
			}
			FinishSequence();
		}

		public static void InternalCallback( Mobile attacker, Mobile defender, Mobile caster, SpellRune rune, RuneSpell spell )
		{
			if ( rune == null )
			{
				return;
			}
			if ( !defender.CanBeginAction( typeof( CircleOfLightning ) ) && Utility.RandomBool() )
			{
				defender.DoHarmful( attacker );

				int damage = rune.Level;
				if ( damage <= 0 )
					damage = 1;

				int baseDamage = 6 + (int)(damage*defender.Skills[SkillName.Inscribe].Value / 40);

				int enedmg = Utility.RandomMinMax( baseDamage, baseDamage + 5 );

				enedmg = (int)(enedmg * RuneSpell.TMRG);

				enedmg += RSCA.CalculateSI(caster, enedmg);

				enedmg += ImperialOrb.IncI( caster, spell );

				SpellHelper.Damage( TimeSpan.Zero, attacker, defender, enedmg, 0, 0, 0, 0, 100 );

				attacker.BoltEffect( 0 );
				attacker.SendMessage( "You have been struck by a bolt of lightning surrounding "+ defender.Name +"." );
			}
		}

		private class Internal2Timer : Timer
		{
			private CircleOfLightning Spell;
			private Mobile Source;
			private DateTime Expire;

			public Internal2Timer( Mobile from, DateTime end, CircleOfLightning spell ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.5 ) )
			{
				Source = from;
				Expire = end;
				Spell = spell;
			}

			protected override void OnTick()
			{
				Source.BoltEffect( 0 );
				if ( DateTime.Now >= Expire || Source.Mana <= 0 || !Source.CheckAlive() || Source.CanBeginAction( typeof( CircleOfLightning ) ) )
				{
					Stop();
				}
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;
			private Mobile Source;
			private DateTime Expire;
			private SpellRune Rune;
			private RuneSpell m_Spell;

			public InternalTimer( Mobile from, DateTime end, Mobile caster, SpellRune rune, RuneSpell spell ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 2.5 ) )
			{
				Source = from;
				Expire = end;
				m_Caster = caster;
				Rune = rune;
				m_Spell = spell;
			}

			protected override void OnTick()
			{
				if ( Rune == null )
				{
					Stop();
				}
				Source.Mana -= 3;
				ArrayList mlist = new ArrayList();

				foreach ( Mobile m in Source.GetMobilesInRange( Rune.Level ) )
				{
					if ( Source != m && SpellHelper.ValidIndirectTarget( Source, m ) /*&& Source.CanBeHarmful( m, false ) && !Spellbreaker.BreakSpell( m, m_Caster, m_Spell )*/ )
						mlist.Add( m );
				}
				foreach ( Mobile m in mlist ) 
				{
					InternalCallback( m, Source, m_Caster, Rune, m_Spell );
				}
				if ( DateTime.Now >= Expire || Source.Mana <= 0 || !Source.CheckAlive() || Source.CanBeginAction( typeof( CircleOfLightning ) ) )
				{
					Source.EndAction( typeof( CircleOfLightning ) );
					Source.SendMessage( "The lightning surrounding your body fades away." );
					Stop();
				}
			}
		}
		private class InternalTarget : Target
		{
			private CircleOfLightning m_Owner;

			public InternalTarget( CircleOfLightning owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
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
}
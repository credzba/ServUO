using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class Rejuvenation : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Rejuvenation", "Wecu Xent",
				203,
				9041,
				GetSpellGems.GetGems( typeof(Rejuvenation) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public Rejuvenation( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
			else if ( !m.CanBeginAction( typeof(Rejuvenation) ))
			{
				m.SendMessage( "You take the spell off." );
				m.EndAction( typeof( Rejuvenation ) );
			}
			else if ( CheckBSequence( m ) )//else if ( CheckHSequence( m ) ) //breaker edit
			{
				GainExp();
				SpellHelper.Turn( Caster, m );
				m.SendMessage( "You feel your body being rejuvenated." );
				m.BeginAction( typeof(Rejuvenation) );

				TimeSpan duration;

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				duration = TimeSpan.FromMinutes( ((runelevel*Caster.Skills.Inscribe.Value)/100 * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, (runelevel*Caster.Skills.Inscribe.Value)/100) );

				new InternalTimer( Caster, m, Rune, this, duration ).Start();
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster, m_Source;
			private DateTime NextTick;
			private DateTime m_Expire;
			private SpellRune m_Rune;
			private RuneSpell m_Spell;

			public InternalTimer( Mobile caster, Mobile source, SpellRune rune, RuneSpell spell, TimeSpan expire ) : base( TimeSpan.FromSeconds( 0.1 ), TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Caster = caster;
				m_Source = source;
				m_Expire = DateTime.Now + expire;
				NextTick = DateTime.Now + TimeSpan.FromSeconds( 0.3 );
				m_Rune = rune;
				m_Spell = spell;
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Rune == null )
				{
					Stop();
				}
				if ( !m_Source.CheckAlive() || DateTime.Now >= m_Expire || m_Source.CanBeginAction( typeof(Rejuvenation) ) )
				{
					m_Source.EndAction( typeof( Rejuvenation ) );
					m_Source.SendMessage( "Your body stops rejuvenating." );
					Stop();
				}

				if ( DateTime.Now >= NextTick && !m_Source.CanBeginAction( typeof(Rejuvenation) ) )
				{
					int toHeal = m_Rune.Level + (int)(m_Caster.Skills[SkillName.Inscribe].Value / 50.0 * RuneSpell.TMRG);
					toHeal += RSCA.CalculateSI(m_Caster, toHeal);

					toHeal += ImperialOrb.IncI( m_Caster, m_Spell );

					m_Source.Hits += toHeal;

					m_Source.PlaySound( 0x202 );
					m_Source.FixedParticles( 0x376A, 1, 62, 9923, 3, 3, EffectLayer.Waist );
					m_Source.FixedParticles( 0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist );
					NextTick = DateTime.Now + TimeSpan.FromSeconds( 4-RSCA.CalculateLLS(m_Caster, 4) );
				}
			}
		}

		private class InternalTarget : Target
		{
			private Rejuvenation m_Owner;

			public InternalTarget( Rejuvenation owner ) : base( 12, false, TargetFlags.Beneficial ) //Harmful
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
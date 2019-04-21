using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class FrostBite : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Frost Bite", "Iwne Meru",
				203,
				9041,
				GetSpellGems.GetGems( typeof(FrostBite) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public override int RuneGainMax{ get{ return 60; } }
		public override int RuneGainMin{ get{ return 30; } }

		public FrostBite( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
			else if ( !m.CanBeginAction( typeof(FrostBite) ) )
			{
				if ( m.Player )
					Caster.SendMessage( "This person already has frost bite." );
				else
					Caster.SendMessage( "That creature already has frost bite." );
			}
			else if ( CheckHSequence( m ) )
			{
				GainExp();
				//if ( Spellbreaker.BreakSpell( m, Caster, this ) )
				//	return;

				SpellHelper.Turn( Caster, m );
				m.SendMessage( "Your body has been given frost bite." );
				m.BeginAction( typeof(FrostBite) );
				m.FixedParticles( 14201, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
				m.PlaySound( 696 );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				TimeSpan duration = TimeSpan.FromMinutes( (runelevel*(Caster.Skills.Inscribe.Value/100) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, runelevel*(Caster.Skills.Inscribe.Value/100)) );

				new InternalTimer( Caster, m, duration ).Start();
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;
			private Mobile m_Target;

			public InternalTimer( Mobile caster, Mobile target, TimeSpan duration ) : base( duration )
			{
				m_Caster = caster;
				m_Target = target;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				m_Target.EndAction( typeof(FrostBite) );
				m_Target.SendMessage( "The frost bite wears off." );
				Stop();
			}
		}

		private class InternalTarget : Target
		{
			private FrostBite m_Owner;

			public InternalTarget( FrostBite owner ) : base( 12, false, TargetFlags.Harmful )
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
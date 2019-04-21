using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class IceDagger : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Ice Dagger", "Iwne Dral",
				203,
				9041,
				GetSpellGems.GetGems( typeof(IceDagger) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public IceDagger( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
			else if ( CheckHSequence( m ) )
			{
				GainExp();
				//if ( Spellbreaker.BreakSpell( m, Caster, this ) )
				//	return;

				SpellHelper.Turn( Caster, m );
				m.SendMessage( "Your body has been struck by pieces of ice shapped into daggers." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				int damage = (int)(runelevel*Caster.Skills[SkillName.Inscribe].Value / 10 * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(Caster, damage);

				damage += ImperialOrb.IncI( Caster, this );

				Caster.MovingEffect( m, 0x1BFE, 7, 1, false, false, 5, 0 );
				SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 100, 0, 0 );
				new InternalTimer( Caster, m, Rune.Level ).Start();
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;
			private Mobile m_Target;
			private int m_Amount;
			private int cnt = 0;

			public InternalTimer( Mobile caster, Mobile target, int amount ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
			{
				m_Caster = caster;
				m_Target = target;
				m_Amount = amount;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				cnt++;
				if ( m_Amount <= 0 )
					Stop();
				if ( cnt == 0 )
				{
					if ( m_Amount >= 1 )
					{
						m_Caster.MovingEffect( m_Target, 0x1BFE, 7, 1, false, false, 5, 0 );
					}
				}
				if ( cnt == 1 )
				{
					if ( m_Amount >= 2 )
					{
						m_Caster.MovingEffect( m_Target, 0x1BFE, 7, 1, false, false, 5, 0 );
					}
				}
				if ( cnt == 2 )
				{
					if ( m_Amount >= 3 )
					{
						m_Caster.MovingEffect( m_Target, 0x1BFE, 7, 1, false, false, 5, 0 );
					}
				}
				if ( cnt == 3 )
				{
					if ( m_Amount >= 4 )
					{
						m_Caster.MovingEffect( m_Target, 0x1BFE, 7, 1, false, false, 5, 0 );
					}
					Stop();
				}
			}
		}

		private class InternalTarget : Target
		{
			private IceDagger m_Owner;

			public InternalTarget( IceDagger owner ) : base( 12, false, TargetFlags.Harmful )
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
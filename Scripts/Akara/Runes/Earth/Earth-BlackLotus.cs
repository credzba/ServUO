using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class BlackLotus : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Black Lotus", "Cadre Hiqu",
				203,
				9041,
				GetSpellGems.GetGems( typeof(BlackLotus) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public BlackLotus( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
				Caster.SendMessage( "You plant the black lotus seed in them." );

				TimeSpan duration = TimeSpan.FromSeconds( (Caster.Skills[SkillName.Inscribe].Value / 50.0 - RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromSeconds( RSCA.CalculateLLS(Caster, Caster.Skills[SkillName.Inscribe].Value / 50.0) );

				new InternalTimer( m, Caster, Rune, this, duration ).Start();
			}
			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile Source;
			private Mobile m_Caster;
			private SpellRune Rune;
			private BlackLotus m_Spell;

			public InternalTimer( Mobile source, Mobile caster, SpellRune rune, BlackLotus spell, TimeSpan duration ) : base( duration )
			{
				Source = source;
				m_Caster = caster;
				Rune = rune;
				m_Spell = spell;
			}

			protected override void OnTick()
			{
				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int damage = (int)(runelevel*(m_Caster.Skills[SkillName.Inscribe].Value / 25) * RuneSpell.TMRG); 

				damage += RSCA.CalculateSI(m_Caster, damage);

				damage += ImperialOrb.IncI( m_Caster, m_Spell );

				SpellHelper.Damage( TimeSpan.Zero, Source, m_Caster, damage, 100, 0, 0, 0, 0 );
				Source.FixedParticles( 14154, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
				Source.PlaySound( 696 );

				Stop();
			}
		}

		private class InternalTarget : Target
		{
			private BlackLotus m_Owner;

			public InternalTarget( BlackLotus owner ) : base( 12, false, TargetFlags.Harmful )
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
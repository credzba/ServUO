using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class LightningStrike : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Lightning Strike", "Artu Itra",
				203,
				9041,
				GetSpellGems.GetGems( typeof(LightningStrike) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public LightningStrike( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
				m.SendMessage( "You have been struck by a bolt of lightning." );

				int damage = (int)(Caster.Skills[SkillName.Inscribe].Value / 5 * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(Caster, (int)(Caster.Skills[SkillName.Inscribe].Value / 5));

				damage += ImperialOrb.IncI( Caster, this );

				m.BoltEffect(0);
				SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 100, 0, 0, 0 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private LightningStrike m_Owner;

			public InternalTarget( LightningStrike owner ) : base( 12, false, TargetFlags.Harmful )
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
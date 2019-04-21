using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class SoulLeech : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Soul Leech", "Sphir Jwun",
				212,
				9041,
				GetSpellGems.GetGems( typeof(SoulLeech) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public SoulLeech( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
			else if ( CheckSequence() )
			{
				GainExp();
				//if ( Spellbreaker.BreakSpell( m, Caster, this ) )
				//	return;

				SpellHelper.Turn( Caster, m );
				m.SendMessage( "You feel as though you are a flame that is about to go out." );
				m.FixedEffect( 0x37C4, 10, 42, 4, 3 );
				Caster.SendMessage( "Your body begins to feel more alive." );
				Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );

				int leech = (int)(Caster.Skills[SkillName.Inscribe].Value / 5 * RuneSpell.TMRG);

				leech += RSCA.CalculateSI(Caster, leech);

				leech += ImperialOrb.IncI( Caster, this );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				int leechot = leech / 3;

				Caster.Heal( leechot );
				m.Damage( leechot );
				Caster.Stam += leechot;
				m.Stam -= leechot;
				Caster.Mana += leechot;
				m.Mana -= leechot;
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private SoulLeech m_Owner;

			public InternalTarget( SoulLeech owner ) : base( 12, true, TargetFlags.None )
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
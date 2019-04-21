using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class VenomInjection : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Venom Injection", "Tose Zezu",
				203,
				9041,
				GetSpellGems.GetGems( typeof(VenomInjection) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public VenomInjection( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a piece of food to poison." );
		}

		public void Target( Food f )
		{
			if ( !Caster.CanSee( f ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( f.Poison == Poison.Lesser || f.Poison == Poison.Regular || f.Poison == Poison.Greater || f.Poison == Poison.Deadly || f.Poison == Poison.Lethal )
			{
				Caster.SendMessage( "That piece of food is already poisoned." );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				SpellHelper.Turn( Caster, f );
				Caster.SendMessage( "You inject the piece of food with poison." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				int total = (int)(runelevel*Caster.Skills[SkillName.Inscribe].Value * RuneSpell.TMRG);

				total += RSCA.CalculateSI(Caster, total);

				total += ImperialOrb.IncI( Caster, this );

				if ( total > 900.0 )
					f.Poison = Poison.Lethal;
				else if ( total > 700.0 )
					f.Poison = Poison.Deadly;
				else if ( total > 500.0 )
					f.Poison = Poison.Greater;
				else if ( total > 300.0 )
					f.Poison = Poison.Regular;
				else
					f.Poison = Poison.Lesser;
				f.Poisoner = Caster;
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private VenomInjection m_Owner;

			public InternalTarget( VenomInjection owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Food )
					m_Owner.Target( (Food)o );
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
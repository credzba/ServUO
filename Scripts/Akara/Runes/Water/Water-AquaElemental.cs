using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class AquaElementalSpell : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Aqua Elemental", "Wecu Draf",
				212,
				9041,
				GetSpellGems.GetGems( typeof(AquaElementalSpell) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public override int RuneGainMax{ get{ return 60; } }
		public override int RuneGainMin{ get{ return 30; } }

		public AquaElementalSpell( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to summon the creatures to." );
		}
		public void Target( Mobile m )
		{
			if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( !m.CanBeginAction( typeof(UndeadLegionSpell) ) || !m.CanBeginAction( typeof(AquaElementalSpell) ) || !m.CanBeginAction( typeof(NoFaceSpell) ) || !m.CanBeginAction( typeof(TornadoElementalSpell) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else
			{
				GainExp();
				m.BeginAction( typeof(AquaElementalSpell) );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				TimeSpan duration = TimeSpan.FromHours( (runelevel*(Caster.Skills.Inscribe.Fixed/30) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromHours( RSCA.CalculateLLS(Caster, runelevel*(Caster.Skills.Inscribe.Fixed/30)) );

				int i = 0;
				while( i < runelevel )
				{
					SpellHelper.Summon( new AquaElemental( this, Caster, m, Rune ), m, 0x216, duration, false, false );
					i++;
				}
			}
			FinishSequence();
		}
		private class InternalTarget : Target
		{
			private AquaElementalSpell m_Owner;

			public InternalTarget( AquaElementalSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					Mobile m = (Mobile)o;
					if ( m.Player && m is PlayerMobile )
						m_Owner.Target( m );
				}
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
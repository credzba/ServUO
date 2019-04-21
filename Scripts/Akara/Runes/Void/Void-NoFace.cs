using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class NoFaceSpell : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"No Face", "Elra Nukj",
				212,
				9041,
				GetSpellGems.GetGems( typeof(NoFaceSpell) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public override int RuneGainMax{ get{ return 100; } }
		public override int RuneGainMin{ get{ return 50; } }

		public NoFaceSpell( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to summon the creatures to." );
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
			else if ( !m.CanBeginAction( typeof(UndeadLegionSpell) ) || !m.CanBeginAction( typeof(AquaElementalSpell) ) || !m.CanBeginAction( typeof(NoFaceSpell) ) || !m.CanBeginAction( typeof(TornadoElementalSpell) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else if ( CheckHSequence( m ) )
			{
				GainExp();
				m.BeginAction( typeof(NoFaceSpell) );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				TimeSpan duration = TimeSpan.FromHours( (runelevel*(Caster.Skills.Inscribe.Fixed/30) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromHours( RSCA.CalculateLLS(Caster, runelevel*(Caster.Skills.Inscribe.Fixed/30)) );

				int i = 0;
				while( i < runelevel )
				{
					SpellHelper.Summon( new NoFace( this, Caster, m, Rune ), m, 0x216, duration, false, false );
					i++;
				}
			}
			FinishSequence();
		}
		private class InternalTarget : Target
		{
			private NoFaceSpell m_Owner;

			public InternalTarget( NoFaceSpell owner ) : base( 12, true, TargetFlags.None )
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
using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class Hallucinogen : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Hallucinogen", "Tose Hajl",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(Hallucinogen) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public override int RuneGainMax{ get{ return 60; } }
		public override int RuneGainMin{ get{ return 30; } }

		public Hallucinogen( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
			else if ( !m.CanBeginAction( typeof(Hallucinogen) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else if ( CheckSequence() )
			{
				GainExp();
				//if ( Spellbreaker.BreakSpell( m, Caster, this ) )
				//	return;

				SpellHelper.Turn( Caster, m );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				m.BeginAction( typeof(Hallucinogen) );
				m.SendMessage( "Your body begins to feel a bit... odd." );
				m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
				m.PlaySound( 0x474 );
				DateTime Expire = DateTime.Now + TimeSpan.FromMinutes( (runelevel*(Caster.Skills.Inscribe.Value/100) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				Expire += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, runelevel*(Caster.Skills.Inscribe.Value/100)) );
				new InternalTimer( m, Expire ).Start();
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile Source;
			private DateTime Expire;
			private int cnt, saying = 0;

			public InternalTimer( Mobile from, DateTime end ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				Source = from;
				Expire = end;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= Expire || !Source.CheckAlive() || Source.CanBeginAction( typeof(Hallucinogen) ) )
				{
					Source.EndAction( typeof( Hallucinogen ) );
					Source.SendMessage( "Your head stops spinning, the spell has worn off." );
					Stop();
				}
				cnt++;
				if ( cnt == 5 )
				{
					cnt = 0;
					Source.Mana -= 3;
					saying = Utility.RandomMinMax( 0, 11 );

					if ( saying == 0 )
					{
						Source.PlaySound( Source.Female ? 783 : 1054 );
						Source.Say( "*woohoo!*" );
					}
					else if ( saying == 1 )
					{
						Source.PlaySound( Source.Female ? 812 : 1086 );
						Source.Say( "*oops*" );
						Source.PlaySound( Source.Female ? 801 : 1073 );
						Source.Say( "*laughs*" );
					}
					else if ( saying == 2 )
					{
						Source.PlaySound( Source.Female ? 794 : 1066 );
						Source.Say( "*giggles*" );
					}
					else if ( saying == 3 )
					{
						Source.PlaySound( Source.Female ? 793 : 1065 );
						Source.Say( "*gasp!*" );
						Source.PlaySound( Source.Female ? 783 : 1054 );
						Source.Say( "*woohoo!*" );
					}
					else if ( saying == 4 )
					{
						Source.PlaySound( Source.Female ? 793 : 1065 );
						Source.Say( "*gasp!*" );
						Source.PlaySound( Source.Female ? 794 : 1066 );
						Source.Say( "*giggles*" );
					}
					else if ( saying == 5 )
					{
						Source.PlaySound( Source.Female ? 803 : 1075 );
						Source.Say( "*oh!*" );
						Source.PlaySound( Source.Female ? 801 : 1073 );
						Source.Say( "*laughs*" );
					}
					else if ( saying == 6 )
					{
						Source.PlaySound( Source.Female ? 801 : 1073 );
						Source.Say( "*laughs*" );
					}
					else if ( saying == 7 )
					{
						Source.PlaySound( Source.Female ? 812 : 1086 );
						Source.Say( "*oops*" );
					}
					else if ( saying == 8 )
					{
						Source.PlaySound( Source.Female ? 811 : 1085 );
						Source.Say( "*oooh*" );
					}
					else if ( saying == 9 )
					{
						Source.PlaySound( Source.Female ? 803 : 1075 );
						Source.Say( "*oh!*" );
					}
					else if ( saying == 10 )
					{
						Source.PlaySound( Source.Female ? 812 : 1086 );
						Source.Say( "*oops*" );
						Source.PlaySound( Source.Female ? 783 : 1054 );
						Source.Say( "*woohoo!*" );
					}
					else if ( saying == 11 )
					{
						Source.PlaySound( Source.Female ? 793 : 1065 );
						Source.Say( "*gasp!*" );
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private Hallucinogen m_Owner;

			public InternalTarget( Hallucinogen owner ) : base( 12, true, TargetFlags.None )
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
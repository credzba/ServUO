using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class PureSilence : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Pure Silence", "Elra Kruy",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(PureSilence) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public PureSilence( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof(PureSilence) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
				return false;
			}
			else if ( Caster.Mana == Caster.ManaMax && Caster.Hits == Caster.HitsMax && Caster.Stam == Caster.StamMax )
			{
				Caster.SendMessage( "There is no need to cast this spell." );
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				GainExp();
				new InternalTimer( Caster, Caster.Location, Rune, this ).Start();
				Caster.BeginAction( typeof( PureSilence ) );

				Caster.SendMessage( "Your start your meditation." );
				Caster.Location = new Point3D( 0, 0, -30 );
			}
			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private PureSilence Spell;
			private Mobile Source;
			private Point3D Loc;
			private SpellRune m_Rune;

			public InternalTimer( Mobile from, Point3D loc, SpellRune rune, PureSilence spell ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				Source = from;
				Spell = spell;
				m_Rune = rune;
				Loc = loc;
			}

			protected override void OnTick()
			{
				if ( m_Rune == null )
				{
					Source.EndAction( typeof( PureSilence ) );
					Source.Squelched = false;
					Source.Hidden = false;
					Source.Frozen = false;
					Source.Location = Loc;
					Stop();
				}
				int runelevel = m_Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				Source.Mana += (int)(runelevel * RuneSpell.TMRG) + RSCA.CalculateSI(Source, runelevel)+ImperialOrb.IncI( Source, Spell );
				Source.Hits += (int)(runelevel * RuneSpell.TMRG) + RSCA.CalculateSI(Source, runelevel)+ImperialOrb.IncI( Source, Spell );
				Source.Stam += (int)(runelevel * RuneSpell.TMRG) + RSCA.CalculateSI(Source, runelevel)+ImperialOrb.IncI( Source, Spell );
				Source.Squelched = true;
				Source.Hidden = true;
				Source.Frozen = true;
				if ( Source.Mana == Source.ManaMax && Source.Hits == Source.HitsMax && Source.Stam == Source.StamMax )
				{
					Source.EndAction( typeof( PureSilence ) );
					Source.SendMessage( "You have finished your meditation." );
					Source.Location = Loc;
					Source.Squelched = false;
					Source.Hidden = false;
					Source.Frozen = false;
					Stop();
				}
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
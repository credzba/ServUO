using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;

namespace Server.Spells
{
	public class MagicFood : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Magical Food", "Wecu Sder",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(MagicFood) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public MagicFood( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				GainExp();
				Caster.SendMessage( "You have created a magical food." );
				Caster.AddToBackpack( new InternalItem( Rune, Caster ) );
			}
			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Food
		{
			private Mobile m_Caster;
			private SpellRune Rune;
			
			public InternalItem( SpellRune rune, Mobile caster ) : base( 1, Utility.RandomList( 0x9C9, 0x9eb, 0x9D2, 0x9D0, 0x9B7, 0x9C0, 0x9F2, 0x97B ) )
			{
				Hue = 6;
				Rune = rune;
				m_Caster = caster;

				if ( ItemID == 0x9C9 )
					FillFactor = 5;
				else if ( ItemID == 0x9eb )
					FillFactor = 4;
				else if ( ItemID == 0x9D2 )
					FillFactor = 1;
				else if ( ItemID == 0x9D0 )
					FillFactor = 1;
				else if ( ItemID == 0x9D1 )
					FillFactor = 1;
				else if ( ItemID == 0x9B7 )
					FillFactor = 5;
				else if ( ItemID == 0x9C0 )
					FillFactor = 4;
				else if ( ItemID == 0x97B )
					FillFactor = 3;
				else if ( ItemID == 0x97D )
					FillFactor = 3;
			}

			public override bool Eat( Mobile from )
			{
				if ( Rune == null )
					return base.Eat( from );
				else if ( from.Hunger >= 21 )
					from.SendMessage( "You are too full to eat any more food." );
				else if ( Rune.Level > Utility.Random( 5 ) )
				{
					StatMod mod;
					mod = from.GetStatMod( "[Magic] Str Offset" );
					if ( mod != null && mod.Offset < 0 )
						from.RemoveStatMod( "[Magic] Str Offset" );

					mod = from.GetStatMod( "[Magic] Dex Offset" );
					if ( mod != null && mod.Offset < 0 )
						from.RemoveStatMod( "[Magic] Dex Offset" );

					mod = from.GetStatMod( "[Magic] Int Offset" );
					if ( mod != null && mod.Offset < 0 )
						from.RemoveStatMod( "[Magic] Int Offset" );

					from.PlaySound( 0x1E0 );
					from.CurePoison( m_Caster );
					CurseSpell.RemoveEffect( from );
					from.Paralyzed = false;			
					EvilOmenSpell.TryEndEffect( from );
					StrangleSpell.RemoveCurse( from );
					CorpseSkinSpell.RemoveCurse( from );
					BleedAttack.EndBleed( from, true );
					MortalStrike.EndWound( from );
					from.EndAction( typeof( FrostBite ) );
					from.EndAction( typeof( Hallucinogen ) );
					MindRotSpell.ClearMindRotScalar( from );
					from.SendMessage( "The food has cured all of your illnesses." );
				}
				else
					from.SendMessage( "The food fails to cure any of your illnesses." );

				return base.Eat( from );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_Caster );
				writer.Write( Rune );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_Caster = reader.ReadMobile();
						Rune = reader.ReadItem() as SpellRune;

						goto case 0;
					}
					case 0:
					{
						break;
					}
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

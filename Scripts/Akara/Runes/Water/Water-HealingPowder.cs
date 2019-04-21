using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Necromancy;

namespace Server.Spells
{
	public class HealingPowder : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Healing Powder", "Wecu Qir",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(HealingPowder) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public override int RuneGainMax{ get{ return 100; } }
		public override int RuneGainMin{ get{ return 50; } }

		public HealingPowder( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
				Caster.SendMessage( "You have created a healing powder." );
				Caster.AddToBackpack( new InternalItem( this, Rune, Caster ) );
			}
			FinishSequence();
		}
		[DispellableField]
		private class InternalItem : Item
		{
			private Mobile m_Caster;
			private SpellRune Rune;
			private int ContAmt;
			private RuneSpell m_Spell;

			public InternalItem( RuneSpell spell, SpellRune rune, Mobile caster ) : base( 4102 )
			{
				m_Spell = spell;
				Hue = 6;
				Rune = rune;
				m_Caster = caster;
				ContAmt = Rune.Level + 1;
				Name = "Healing Powder";

				if ( ContAmt <= 3 )
					ItemID = 4103;
			}

			public override void OnDoubleClick( Mobile from )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				if ( from.Poisoned )
					from.SendMessage( "You are poisoned, you may not use the healing powder." );
				else if ( Rune.Level > Utility.Random( 5 ) )
				{
					from.PlaySound( 0x1F2 );
					int runelevel = Rune.Level;
					if ( runelevel <= 0 )
						runelevel = 1;

					int toHeal = (int)( ( m_Caster.Skills[SkillName.Inscribe].Value * runelevel ) / 10 * RuneSpell.TMRG );

					toHeal += RSCA.CalculateSI(m_Caster, toHeal);

					toHeal += ImperialOrb.IncI( m_Caster, m_Spell );

					from.Heal( toHeal );

					ContAmt -=1;

					if ( ContAmt <= 3 )
						ItemID = 4103;
					if ( ContAmt <= 0 )
						Delete();
				}
				else
				{
					from.PlaySound( 0x1F2 );
					int runelevel = Rune.Level;
					if ( runelevel <= 0 )
						runelevel = 1;

					int toHeal = (int)( ( m_Caster.Skills[SkillName.Inscribe].Value * runelevel ) / 40 * RuneSpell.TMRG );

					toHeal += RSCA.CalculateSI(m_Caster, toHeal);

					toHeal += ImperialOrb.IncI( m_Caster, m_Spell );

					from.Heal( toHeal );

					ContAmt -=1;

					if ( ContAmt <= 3 )
						ItemID = 4103;
					if ( ContAmt >= 0 )
						Delete();

					from.SendMessage( "The powder heals you, but not enough to amount to anything." );
				}
			}

			public override void GetProperties( ObjectPropertyList list )
			{
				base.GetProperties( list );

				list.Add( 1060658, "Uses\t{0}", ContAmt );
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
				writer.Write( (int)ContAmt );
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
						ContAmt = reader.ReadInt();

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
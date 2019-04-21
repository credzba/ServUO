using System;
using Server;

namespace Server.Items
{
	public class CloakOfInvisibility : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 2; } }
		public override int BaseFireResistance{ get{ return 4; } }
		public override int BaseColdResistance{ get{ return 3; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 4; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		public override int AosStrReq{ get{ return 35; } }
		public override int OldStrReq{ get{ return 35; } }

		public override int ArmorBase{ get{ return 16; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.Half; } }

		private Gems m_Gems;

		[CommandProperty( AccessLevel.GameMaster )]
		public Gems Gems{ get{ if ( m_Gems == null )return m_Gems = new Gems( this );return m_Gems; } set{} }

		[Constructable]
		public CloakOfInvisibility() : base( 0x1515 )
		{
			Name = "Cloak of Invisibility";
			Hue = 706;
			LootType = LootType.Blessed;
			Weight = 2.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( Parent != from )
				from.SendMessage( "Cloak of Invisibility must be equiped to use it." );
			else if ( from.Hidden )
				from.SendMessage( "You are already hidden." );
			else
			{
				double app = 7.5;
				double rp = 0.1;
				double amt = 10.0;//initial amt 10.0

				foreach ( Mobile check in from.GetMobilesInRange( 18 ) )
				{
					if ( check.InLOS( from ) && check.Combatant == from )
					{
						double dist = from.GetDistanceToSqrt( check );
						amt += (app/(rp*dist));
					}
				}

				if ( !Gems.CheckConsumePower( amt ) )
					from.SendMessage( "Cloak of Invisibility does not have enough power." );
				else
				{
					from.Hidden = true;
					from.SendMessage( "You are now hidden." );
					Gems.ConsumePower( amt );
				}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060659, "Power\t{0}", Gems.GetPower() );
		}

		public CloakOfInvisibility( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
              		m_Gems.Serialize( writer );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Gems = new Gems();
              		m_Gems.Deserialize( reader );
		}
	}
}
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EarthWeaponScroll : BaseRuneScroll
	{
		[Constructable]
		public EarthWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
		public EarthWeaponScroll( int amount ) : base( amount, 5, RuneType.Earth, 0xEF3 )
		{
			Name = "Earth Weapon Scroll";
			Hue = 1149;
		}

		public EarthWeaponScroll( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
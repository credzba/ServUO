using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LightningWeaponScroll : BaseRuneScroll
	{
		[Constructable]
		public LightningWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
		public LightningWeaponScroll( int amount ) : base( amount, 5, RuneType.Lightning, 0xEF3 )
		{
			Name = "Lightning Weapon Scroll";
			Hue = 1153;
		}

		public LightningWeaponScroll( Serial serial ) : base( serial )
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
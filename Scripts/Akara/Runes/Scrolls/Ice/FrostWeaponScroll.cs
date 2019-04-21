using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FrostWeaponScroll : BaseRuneScroll
	{
		[Constructable]
		public FrostWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
		public FrostWeaponScroll( int amount ) : base( amount, 5, RuneType.Ice, 0xEF3 )
		{
			Name = "Frost Weapon Scroll";
			Hue = 1152;
		}

		public FrostWeaponScroll( Serial serial ) : base( serial )
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
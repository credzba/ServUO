using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FieryWeaponScroll : BaseRuneScroll
	{
		[Constructable]
		public FieryWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
		public FieryWeaponScroll( int amount ) : base( amount, 5, RuneType.Fire, 0xEF3 )
		{
			Name = "Fiery Weapon Scroll";
			Hue = 1174;
		}

		public FieryWeaponScroll( Serial serial ) : base( serial )
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
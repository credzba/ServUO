using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ToxicWeaponScroll : BaseRuneScroll
	{
		[Constructable]
		public ToxicWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
		public ToxicWeaponScroll( int amount ) : base( amount, 5, RuneType.Toxic, 0xEF3 )
		{
			Name = "Toxic Weapon Scroll";
			Hue = 79;
		}

		public ToxicWeaponScroll( Serial serial ) : base( serial )
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
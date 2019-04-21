using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CarrySoundScroll : BaseRuneScroll
	{
		[Constructable]
		public CarrySoundScroll() : this( 1 )
		{
		}

		[Constructable]
		public CarrySoundScroll( int amount ) : base( amount, 1, RuneType.Wind, 0xEF3 )
		{
			Name = "Carry Sound Scroll";
			Hue = 906;
		}

		public CarrySoundScroll( Serial serial ) : base( serial )
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
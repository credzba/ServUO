using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class TravelingDirtScroll : BaseRuneScroll
	{
		[Constructable]
		public TravelingDirtScroll() : this( 1 )
		{
		}

		[Constructable]
		public TravelingDirtScroll( int amount ) : base( amount, 3, RuneType.Earth, 0xEF3 )
		{
			Name = "TravelingDirt Scroll";
			Hue = 1149;
		}

		public TravelingDirtScroll( Serial serial ) : base( serial )
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
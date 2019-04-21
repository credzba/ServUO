using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BlackLotusScroll : BaseRuneScroll
	{
		[Constructable]
		public BlackLotusScroll() : this( 1 )
		{
		}

		[Constructable]
		public BlackLotusScroll( int amount ) : base( amount, 2, RuneType.Earth, 0xEF3 )
		{
			Name = "Black Lotus Scroll";
			Hue = 1149;
		}

		public BlackLotusScroll( Serial serial ) : base( serial )
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
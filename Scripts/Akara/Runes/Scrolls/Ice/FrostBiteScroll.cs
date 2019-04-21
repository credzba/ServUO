using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FrostBiteScroll : BaseRuneScroll
	{
		[Constructable]
		public FrostBiteScroll() : this( 1 )
		{
		}

		[Constructable]
		public FrostBiteScroll( int amount ) : base( amount, 3, RuneType.Ice, 0xEF3 )
		{
			Name = "Frost Bite Scroll";
			Hue = 1152;
		}

		public FrostBiteScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class NoFaceScroll : BaseRuneScroll
	{
		[Constructable]
		public NoFaceScroll() : this( 1 )
		{
		}

		[Constructable]
		public NoFaceScroll( int amount ) : base( amount, 4, RuneType.Void, 0xEF3 )
		{
			Name = "No Face Scroll";
			Hue = 1;
		}

		public NoFaceScroll( Serial serial ) : base( serial )
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
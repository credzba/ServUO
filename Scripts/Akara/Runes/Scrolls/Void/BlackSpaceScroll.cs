using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BlackSpaceScroll : BaseRuneScroll
	{
		[Constructable]
		public BlackSpaceScroll() : this( 1 )
		{
		}

		[Constructable]
		public BlackSpaceScroll( int amount ) : base( amount, 3, RuneType.Void, 0xEF3 )
		{
			Name = "Black Space Scroll";
			Hue = 1;
		}

		public BlackSpaceScroll( Serial serial ) : base( serial )
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
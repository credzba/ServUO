using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PureSilenceScroll : BaseRuneScroll
	{
		[Constructable]
		public PureSilenceScroll() : this( 1 )
		{
		}

		[Constructable]
		public PureSilenceScroll( int amount ) : base( amount, 1, RuneType.Void, 0xEF3 )
		{
			Name = "Pure Silence Scroll";
			Hue = 1;
		}

		public PureSilenceScroll( Serial serial ) : base( serial )
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
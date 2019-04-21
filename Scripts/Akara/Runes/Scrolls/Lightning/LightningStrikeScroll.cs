using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LightningStrikeScroll : BaseRuneScroll
	{
		[Constructable]
		public LightningStrikeScroll() : this( 1 )
		{
		}

		[Constructable]
		public LightningStrikeScroll( int amount ) : base( amount, 1, RuneType.Lightning, 0xEF3 )
		{
			Name = "Lightning Strike Scroll";
			Hue = 1153;
		}

		public LightningStrikeScroll( Serial serial ) : base( serial )
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
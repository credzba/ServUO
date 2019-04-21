using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SpiritualLinkScroll : BaseRuneScroll
	{
		[Constructable]
		public SpiritualLinkScroll() : this( 1 )
		{
		}

		[Constructable]
		public SpiritualLinkScroll( int amount ) : base( amount, 3, RuneType.Spirit, 0xEF3 )
		{
			Name = "Spiritual Link Scroll";
			Hue = 1109;
		}

		public SpiritualLinkScroll( Serial serial ) : base( serial )
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
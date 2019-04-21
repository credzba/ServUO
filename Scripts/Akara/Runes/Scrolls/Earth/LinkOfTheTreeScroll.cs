using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LinkOfTheTreeScroll : BaseRuneScroll
	{
		[Constructable]
		public LinkOfTheTreeScroll() : this( 1 )
		{
		}

		[Constructable]
		public LinkOfTheTreeScroll( int amount ) : base( amount, 1, RuneType.Earth, 0xEF3 )
		{
			Name = "Link Of The Tree Scroll";
			Hue = 1149;
		}

		public LinkOfTheTreeScroll( Serial serial ) : base( serial )
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
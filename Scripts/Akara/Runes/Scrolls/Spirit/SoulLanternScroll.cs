using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SoulLanternScroll : BaseRuneScroll
	{
		[Constructable]
		public SoulLanternScroll() : this( 1 )
		{
		}

		[Constructable]
		public SoulLanternScroll( int amount ) : base( amount, 2, RuneType.Spirit, 0xEF3 )
		{
			Name = "Soul Lantern Scroll";
			Hue = 1109;
		}

		public SoulLanternScroll( Serial serial ) : base( serial )
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
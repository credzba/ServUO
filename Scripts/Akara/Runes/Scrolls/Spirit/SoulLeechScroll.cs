using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SoulLeechScroll : BaseRuneScroll
	{
		[Constructable]
		public SoulLeechScroll() : this( 1 )
		{
		}

		[Constructable]
		public SoulLeechScroll( int amount ) : base( amount, 1, RuneType.Spirit, 0xEF3 )
		{
			Name = "Soul Leech Scroll";
			Hue = 1109;
		}

		public SoulLeechScroll( Serial serial ) : base( serial )
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
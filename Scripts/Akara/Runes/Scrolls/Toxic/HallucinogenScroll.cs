using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HallucinogenScroll : BaseRuneScroll
	{
		[Constructable]
		public HallucinogenScroll() : this( 1 )
		{
		}

		[Constructable]
		public HallucinogenScroll( int amount ) : base( amount, 3, RuneType.Toxic, 0xEF3 )
		{
			Name = "Hallucinogen Scroll";
			Hue = 79;
		}

		public HallucinogenScroll( Serial serial ) : base( serial )
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
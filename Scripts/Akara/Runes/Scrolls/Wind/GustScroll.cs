using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class GustScroll : BaseRuneScroll
	{
		[Constructable]
		public GustScroll() : this( 1 )
		{
		}

		[Constructable]
		public GustScroll( int amount ) : base( amount, 2, RuneType.Wind, 0xEF3 )
		{
			Name = "Gust Scroll";
			Hue = 906;
		}

		public GustScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WhirlpoolScroll : BaseRuneScroll
	{
		[Constructable]
		public WhirlpoolScroll() : this( 1 )
		{
		}

		[Constructable]
		public WhirlpoolScroll( int amount ) : base( amount, 4, RuneType.Water, 0xEF3 )
		{
			Name = "Whirlpool Scroll";
			Hue = 6;
		}

		public WhirlpoolScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChainOfFireScroll : BaseRuneScroll
	{
		[Constructable]
		public ChainOfFireScroll() : this( 1 )
		{
		}

		[Constructable]
		public ChainOfFireScroll( int amount ) : base( amount, 3, RuneType.Fire, 0xEF3 )
		{
			Name = "Chain Of Fire Scroll";
			Hue = 1174;
		}

		public ChainOfFireScroll( Serial serial ) : base( serial )
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
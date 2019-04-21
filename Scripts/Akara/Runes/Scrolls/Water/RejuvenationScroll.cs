using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RejuvenationScroll : BaseRuneScroll
	{
		[Constructable]
		public RejuvenationScroll() : this( 1 )
		{
		}

		[Constructable]
		public RejuvenationScroll( int amount ) : base( amount, 1, RuneType.Water, 0xEF3 )
		{
			Name = "Rejuvenation Scroll";
			Hue = 6;
		}

		public RejuvenationScroll( Serial serial ) : base( serial )
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
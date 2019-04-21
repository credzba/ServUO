using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class IceBondsScroll : BaseRuneScroll
	{
		[Constructable]
		public IceBondsScroll() : this( 1 )
		{
		}

		[Constructable]
		public IceBondsScroll( int amount ) : base( amount, 2, RuneType.Ice, 0xEF3 )
		{
			Name = "Ice Bonds Scroll";
			Hue = 1152;
		}

		public IceBondsScroll( Serial serial ) : base( serial )
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
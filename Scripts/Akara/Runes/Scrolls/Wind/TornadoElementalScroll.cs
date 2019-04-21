using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class TornadoElementalScroll : BaseRuneScroll
	{
		[Constructable]
		public TornadoElementalScroll() : this( 1 )
		{
		}

		[Constructable]
		public TornadoElementalScroll( int amount ) : base( amount, 3, RuneType.Wind, 0xEF3 )
		{
			Name = "Tornado Elemental Scroll";
			Hue = 906;
		}

		public TornadoElementalScroll( Serial serial ) : base( serial )
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
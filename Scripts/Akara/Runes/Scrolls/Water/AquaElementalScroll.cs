using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class AquaElementalScroll : BaseRuneScroll
	{
		[Constructable]
		public AquaElementalScroll() : this( 1 )
		{
		}

		[Constructable]
		public AquaElementalScroll( int amount ) : base( amount, 3, RuneType.Water, 0xEF3 )
		{
			Name = "Aqua Elemental Scroll";
			Hue = 6;
		}

		public AquaElementalScroll( Serial serial ) : base( serial )
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
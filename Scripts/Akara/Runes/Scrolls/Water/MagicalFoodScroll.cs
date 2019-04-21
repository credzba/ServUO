using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MagicalFoodScroll : BaseRuneScroll
	{
		[Constructable]
		public MagicalFoodScroll() : this( 1 )
		{
		}

		[Constructable]
		public MagicalFoodScroll( int amount ) : base( amount, 2, RuneType.Water, 0xEF3 )
		{
			Name = "Magical Food Scroll";
			Hue = 6;
		}

		public MagicalFoodScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HealingPowderScroll : BaseRuneScroll
	{
		[Constructable]
		public HealingPowderScroll() : this( 1 )
		{
		}

		[Constructable]
		public HealingPowderScroll( int amount ) : base( amount, 5, RuneType.Water, 0xEF3 )
		{
			Name = "Healing Powder Scroll";
			Hue = 6;
		}

		public HealingPowderScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class GemBag : Bag
	{
		[Constructable]
		public GemBag() : this( 50 )
		{
		}

		[Constructable]
		public GemBag( int amount )
		{
			Name = "Gem Bag";

			DropItem( new Amber( amount ) );
			DropItem( new Amethyst( amount ) );
			DropItem( new Citrine( amount ) );
			DropItem( new Diamond( amount ) );
			DropItem( new Emerald( amount ) );
			DropItem( new Ruby( amount ) );
			DropItem( new Sapphire( amount ) );
			DropItem( new StarSapphire( amount ) );
			DropItem( new Tourmaline( amount ) );
		}

		public GemBag( Serial serial ) : base( serial )
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
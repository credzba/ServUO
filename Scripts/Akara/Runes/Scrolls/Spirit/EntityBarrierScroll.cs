using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EntityBarrierScroll : BaseRuneScroll
	{
		[Constructable]
		public EntityBarrierScroll() : this( 1 )
		{
		}

		[Constructable]
		public EntityBarrierScroll( int amount ) : base( amount, 5, RuneType.Spirit, 0xEF3 )
		{
			Name = "Entity Barrier Scroll";
			Hue = 1109;
		}

		public EntityBarrierScroll( Serial serial ) : base( serial )
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
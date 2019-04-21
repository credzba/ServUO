using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CreateFireScroll : BaseRuneScroll
	{
		[Constructable]
		public CreateFireScroll() : this( 1 )
		{
		}

		[Constructable]
		public CreateFireScroll( int amount ) : base( amount, 1, RuneType.Fire, 0xEF3 )
		{
			Name = "Create Fire Scroll";
			Hue = 1174;
		}

		public CreateFireScroll( Serial serial ) : base( serial )
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
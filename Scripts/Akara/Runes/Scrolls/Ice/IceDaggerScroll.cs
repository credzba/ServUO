using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class IceDaggerScroll : BaseRuneScroll
	{
		[Constructable]
		public IceDaggerScroll() : this( 1 )
		{
		}

		[Constructable]
		public IceDaggerScroll( int amount ) : base( amount, 1, RuneType.Ice, 0xEF3 )
		{
			Name = "Ice Dagger Scroll";
			Hue = 1152;
		}

		public IceDaggerScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class IceMeteorScroll : BaseRuneScroll
	{
		[Constructable]
		public IceMeteorScroll() : this( 1 )
		{
		}

		[Constructable]
		public IceMeteorScroll( int amount ) : base( amount, 4, RuneType.Ice, 0xEF3 )
		{
			Name = "Ice Meteor Scroll";
			Hue = 1152;
		}

		public IceMeteorScroll( Serial serial ) : base( serial )
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
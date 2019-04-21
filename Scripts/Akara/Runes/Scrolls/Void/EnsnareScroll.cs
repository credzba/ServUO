using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EnsnareScroll : BaseRuneScroll
	{
		[Constructable]
		public EnsnareScroll() : this( 1 )
		{
		}

		[Constructable]
		public EnsnareScroll( int amount ) : base( amount, 2, RuneType.Void, 0xEF3 )
		{
			Name = "Ensnare Scroll";
			Hue = 1;
		}

		public EnsnareScroll( Serial serial ) : base( serial )
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
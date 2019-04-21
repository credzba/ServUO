using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RevitalizationScroll : BaseRuneScroll
	{
		[Constructable]
		public RevitalizationScroll() : this( 1 )
		{
		}

		[Constructable]
		public RevitalizationScroll( int amount ) : base( amount, 4, RuneType.Wind, 0xEF3 )
		{
			Name = "Revitalization Scroll";
			Hue = 906;
		}

		public RevitalizationScroll( Serial serial ) : base( serial )
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
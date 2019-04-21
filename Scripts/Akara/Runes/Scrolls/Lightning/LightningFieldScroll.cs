using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LightningFieldScroll : BaseRuneScroll
	{
		[Constructable]
		public LightningFieldScroll() : this( 1 )
		{
		}

		[Constructable]
		public LightningFieldScroll( int amount ) : base( amount, 2, RuneType.Lightning, 0xEF3 )
		{
			Name = "Lightning Field Scroll";
			Hue = 1153;
		}

		public LightningFieldScroll( Serial serial ) : base( serial )
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
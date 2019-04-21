using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HeatWaveScroll : BaseRuneScroll
	{
		[Constructable]
		public HeatWaveScroll() : this( 1 )
		{
		}

		[Constructable]
		public HeatWaveScroll( int amount ) : base( amount, 4, RuneType.Fire, 0xEF3 )
		{
			Name = "Heat Wave Scroll";
			Hue = 1174;
		}

		public HeatWaveScroll( Serial serial ) : base( serial )
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
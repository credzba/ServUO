using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EnergySourceScroll : BaseRuneScroll
	{
		[Constructable]
		public EnergySourceScroll() : this( 1 )
		{
		}

		[Constructable]
		public EnergySourceScroll( int amount ) : base( amount, 4, RuneType.Lightning, 0xEF3 )
		{
			Name = "Energy Source Scroll";
			Hue = 1153;
		}

		public EnergySourceScroll( Serial serial ) : base( serial )
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
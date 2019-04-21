using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class VenomInjectionScroll : BaseRuneScroll
	{
		[Constructable]
		public VenomInjectionScroll() : this( 1 )
		{
		}

		[Constructable]
		public VenomInjectionScroll( int amount ) : base( amount, 1, RuneType.Toxic, 0xEF3 )
		{
			Name = "Venom Injection Scroll";
			Hue = 79;
		}

		public VenomInjectionScroll( Serial serial ) : base( serial )
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
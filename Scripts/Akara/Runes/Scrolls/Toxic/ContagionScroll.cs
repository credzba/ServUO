using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ContagionScroll : BaseRuneScroll
	{
		[Constructable]
		public ContagionScroll() : this( 1 )
		{
		}

		[Constructable]
		public ContagionScroll( int amount ) : base( amount, 2, RuneType.Toxic, 0xEF3 )
		{
			Name = "Contagion Scroll";
			Hue = 79;
		}

		public ContagionScroll( Serial serial ) : base( serial )
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
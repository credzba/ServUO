using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ArchRevitalizationScroll : BaseRuneScroll
	{
		[Constructable]
		public ArchRevitalizationScroll() : this( 1 )
		{
		}

		[Constructable]
		public ArchRevitalizationScroll( int amount ) : base( amount, 5, RuneType.Wind, 0xEF3 )
		{
			Name = "Arch Revitalization Scroll";
			Hue = 906;
		}

		public ArchRevitalizationScroll( Serial serial ) : base( serial )
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
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class UndeadLegionScroll : BaseRuneScroll
	{
		[Constructable]
		public UndeadLegionScroll() : this( 1 )
		{
		}

		[Constructable]
		public UndeadLegionScroll( int amount ) : base( amount, 4, RuneType.Spirit, 0xEF3 )
		{
			Name = "Undead Legion Scroll";
			Hue = 1109;
		}

		public UndeadLegionScroll( Serial serial ) : base( serial )
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
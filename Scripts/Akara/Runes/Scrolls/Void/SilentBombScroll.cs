using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SilentBombScroll : BaseRuneScroll
	{
		[Constructable]
		public SilentBombScroll() : this( 1 )
		{
		}

		[Constructable]
		public SilentBombScroll( int amount ) : base( amount, 5, RuneType.Void, 0xEF3 )
		{
			Name = "Silent Bomb Scroll";
			Hue = 1;
		}

		public SilentBombScroll( Serial serial ) : base( serial )
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
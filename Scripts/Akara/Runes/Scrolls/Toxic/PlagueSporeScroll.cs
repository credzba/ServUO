using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PlagueSporeScroll : BaseRuneScroll
	{
		[Constructable]
		public PlagueSporeScroll() : this( 1 )
		{
		}

		[Constructable]
		public PlagueSporeScroll( int amount ) : base( amount, 4, RuneType.Toxic, 0xEF3 )
		{
			Name = "Plague Spore Scroll";
			Hue = 79;
		}

		public PlagueSporeScroll( Serial serial ) : base( serial )
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
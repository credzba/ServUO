using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class StoneSkinScroll : BaseRuneScroll
	{
		[Constructable]
		public StoneSkinScroll() : this( 1 )
		{
		}

		[Constructable]
		public StoneSkinScroll( int amount ) : base( amount, 4, RuneType.Earth, 0xEF3 )
		{
			Name = "Stone Skin Scroll";
			Hue = 1149;
		}

		public StoneSkinScroll( Serial serial ) : base( serial )
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
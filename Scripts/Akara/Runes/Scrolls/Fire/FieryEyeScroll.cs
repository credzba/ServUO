using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FieryEyeScroll : BaseRuneScroll
	{
		[Constructable]
		public FieryEyeScroll() : this( 1 )
		{
		}

		[Constructable]
		public FieryEyeScroll( int amount ) : base( amount, 2, RuneType.Fire, 0xEF3 )
		{
			Name = "Fiery Eye Scroll";
			Hue = 1174;
		}

		public FieryEyeScroll( Serial serial ) : base( serial )
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

////////////////////////////////////////
//                                    //
//   Generated by CEO's YAAAG - V1.2  //
// (Yet Another Arya Addon Generator) //
//                                    //
////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SkullRugEastAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {14496, 2, -3, 0}, {14494, 0, -3, 0}, {14495, -1, -3, 0}// 1	2	3	
			, {14493, 1, -3, 0}, {14492, 2, -2, 0}, {14491, -1, -2, 0}// 4	5	6	
			, {14490, 0, -2, 0}, {14489, 1, -2, 0}, {14488, 2, -1, 0}// 7	8	9	
			, {14487, -1, -1, 0}, {14486, 0, -1, 0}, {14485, 1, -1, 0}// 10	11	12	
			, {14484, 2, 0, 0}, {14483, -1, 0, 0}, {14482, 0, 0, 0}// 13	14	15	
			, {14481, 1, 0, 0}, {14480, 2, 1, 0}, {14479, -1, 1, 0}// 16	17	18	
			, {14478, 0, 1, 0}, {14477, 1, 1, 0}, {14476, 2, 2, 0}// 19	20	21	
			, {14475, -1, 2, 0}, {14474, 0, 2, 0}, {14473, 1, 2, 0}// 22	23	24	
			, {14472, -1, 3, 0}, {14471, 0, 3, 0}, {14470, 1, 3, 0}// 25	26	27	
			, {14469, 2, 3, 0}// 28	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new SkullRugEastAddonDeed();
			}
		}

		[ Constructable ]
		public SkullRugEastAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public SkullRugEastAddon( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SkullRugEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new SkullRugEastAddon();
			}
		}

		[Constructable]
		public SkullRugEastAddonDeed()
		{
			Name = "SkullRugEast";
		}

		public SkullRugEastAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
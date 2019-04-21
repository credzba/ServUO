using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicFootwear: RunicItem 
   	{ 
		[Constructable]
		public RunicFootwear( bool set, int amount ) : base( set, amount, Utility.RandomList( 5903, 5905, 5899, 5901 ) )
		{
			Weight = 10.0;

			if ( ItemID == 5903 )
				Name = "Runic Boots";
			if ( ItemID == 5905 )
				Name = "Runic Thigh Boots";
			if ( ItemID == 5899 )
				Name = "Runic Shoes";
			if ( ItemID == 5901 )
				Name = "Runic Sandles";

			Layer = Layer.Shoes;
			MaxCharges = 10;
		}

            	public RunicFootwear( Serial serial ) : base ( serial ) 
            	{             
           	} 

           	public override void Serialize( GenericWriter writer ) 
           	{ 
              		base.Serialize( writer ); 
              		writer.Write( (int) 0 ); 
           	} 
            
           	public override void Deserialize( GenericReader reader ) 
           	{ 
              		base.Deserialize( reader ); 
              		int version = reader.ReadInt(); 
           	} 
        } 
} 
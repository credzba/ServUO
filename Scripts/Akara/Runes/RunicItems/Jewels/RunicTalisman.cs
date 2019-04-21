using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicTalisman: RunicItem 
   	{ 
		[Constructable]
		public RunicTalisman( bool set, int amount ) : base( set, amount, 0x1088 )
		{
			Weight = 0.1;
			Name = "Runic Talisman";
			Layer = Layer.Neck;
			MaxCharges = 5;
		}

            	public RunicTalisman( Serial serial ) : base ( serial ) 
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
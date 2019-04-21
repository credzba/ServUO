using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicBracelet: RunicItem 
   	{ 
		[Constructable]
		public RunicBracelet( bool set, int amount ) : base( set, amount, 0x1086 )
		{
			Weight = 0.1;
			Name = "Runic Bracelet";
			Layer = Layer.Bracelet;
			MaxCharges = 5;
		}

            	public RunicBracelet( Serial serial ) : base ( serial ) 
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
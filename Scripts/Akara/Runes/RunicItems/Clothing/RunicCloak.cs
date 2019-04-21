using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicCloak: RunicItem 
   	{ 
		[Constructable]
		public RunicCloak( bool set, int amount ) : base( set, amount, 0x1515 )
		{
			Weight = 10.0;
			Name = "Runic Cloak";
			Layer = Layer.Cloak;
			MaxCharges = 20;
		}

            	public RunicCloak( Serial serial ) : base ( serial ) 
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
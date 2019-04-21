using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicRing: RunicItem 
   	{ 
		[Constructable]
		public RunicRing( bool set, int amount ) : base( set, amount, 0x108A )
		{
			Weight = 0.1;
			Name = "Runic Ring";
			Layer = Layer.Ring;
			MaxCharges = 5;
		}

            	public RunicRing( Serial serial ) : base ( serial ) 
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
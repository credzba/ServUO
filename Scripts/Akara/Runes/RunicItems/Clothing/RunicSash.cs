using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicSash: RunicItem 
   	{ 
		[Constructable]
		public RunicSash( bool set, int amount ) : base( set, amount, 0x1541 )
		{
			Weight = 10.0;
			Name = "Runic Sash";
			Layer = Layer.MiddleTorso;
			MaxCharges = 10;
		}

            	public RunicSash( Serial serial ) : base ( serial ) 
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
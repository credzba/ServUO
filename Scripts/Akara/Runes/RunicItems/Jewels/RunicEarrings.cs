using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicEarrings: RunicItem 
   	{ 
		[Constructable]
		public RunicEarrings( bool set, int amount ) : base( set, amount, 0x1087 )
		{
			Weight = 0.0;
			Name = "Runic Earrings";
			Layer = Layer.Earrings;
			MaxCharges = 5;
		}

            	public RunicEarrings( Serial serial ) : base ( serial ) 
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
using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicShroud: RunicItem 
   	{ 
		[Constructable]
		public RunicShroud( bool set, int amount ) : base( set, amount, 0x2684 )
		{
			Weight = 10.0;
			Name = "Runic Shroud";
			Layer = Layer.OuterTorso;
			MaxCharges = 20;
		}

            	public RunicShroud( Serial serial ) : base ( serial ) 
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
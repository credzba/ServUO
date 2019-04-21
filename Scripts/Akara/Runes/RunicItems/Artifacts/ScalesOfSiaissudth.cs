using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class ScalesOfSiaissudth: RunicItem 
   	{ 
		[Constructable]
		public ScalesOfSiaissudth() : base( false, 0, 0x1515 )
		{
			Weight = 10.0;
			Name = "Scales Of Siaissudth";
			Hue = 56;
			Layer = Layer.Cloak;
			MaxCharges = 20;
			SpellIncrease = 10;
			InscribeInc = 5;
			FasterCasting = 2;
		}

            	public ScalesOfSiaissudth( Serial serial ) : base ( serial ) 
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
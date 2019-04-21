using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class ShroudOfVoratheth: RunicItem 
   	{ 
		[Constructable]
		public ShroudOfVoratheth() : base( false, 0, 0x2684 )
		{
			Weight = 10.0;
			Name = "Shroud Of Voratheth";
			Hue = 11;
			Layer = Layer.OuterTorso;
			MaxCharges = 30;
			LongerLastingSpells = 10;
			InscribeInc = 10;
			FasterCasting = 1;
			FasterCastRecovery = 2;
		}

            	public ShroudOfVoratheth( Serial serial ) : base ( serial ) 
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
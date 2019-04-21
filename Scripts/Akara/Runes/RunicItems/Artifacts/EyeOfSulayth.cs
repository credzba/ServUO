using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class EyeOfSulayth: RunicItem 
   	{ 
		[Constructable]
		public EyeOfSulayth() : base( false, 0, 0x108A )
		{
			Weight = 0.1;
			Name = "Eye Of Sulayth";
			Hue = 44;
			Layer = Layer.Ring;
			MaxCharges = 10;
			SpellIncrease = 10;
			InscribeInc = 10;
			FasterCastRecovery = 3;
		}

            	public EyeOfSulayth( Serial serial ) : base ( serial ) 
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
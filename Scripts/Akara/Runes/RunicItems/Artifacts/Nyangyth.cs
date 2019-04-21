using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class Nyangyth: RunicItem 
   	{ 
		[Constructable]
		public Nyangyth() : base( false, 0, 0x1086 )
		{
			Weight = 0.1;
			Name = "Nyangyth";
			Hue = 79;
			Layer = Layer.Bracelet;
			MaxCharges = 15;
			LongerLastingSpells = 15;
			SpellIncrease = 5;
			FasterCasting = 1;
		}

            	public Nyangyth( Serial serial ) : base ( serial ) 
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
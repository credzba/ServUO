using System; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public class RunicHats: RunicItem 
   	{ 
		[Constructable]
		public RunicHats( bool set, int amount ) : base( set, amount, Utility.RandomList( 5907, 5908, 5909, 5910, 5911, 5912, 5913, 5914, 5915, 5916, 5440, 5443 ) )
		{
			Weight = 10.0;

			if ( ItemID == 5907 )
				Name = "Runic Floppy Hat";
			if ( ItemID == 5908 )
				Name = "Runic Wide-Brim Hate";
			if ( ItemID == 5909 )
				Name = "Runic Cap";
			if ( ItemID == 5910 )
				Name = "Runic Tall Straw Hat";
			if ( ItemID == 5911 )
				Name = "Runic Straw Hat";
			if ( ItemID == 5912 )
				Name = "Runic Wizard's Hat";
			if ( ItemID == 5913 )
				Name = "Runic Bonnet";
			if ( ItemID == 5914 )
				Name = "Runic Feathered Hat";
			if ( ItemID == 5915 )
				Name = "Runic Tricorne Hat";
			if ( ItemID == 5916 )
				Name = "Runic Jester Hat";
			if ( ItemID == 5440 )
				Name = "Runic Bandana";
			if ( ItemID == 5443 )
				Name = "Runic Skullcap";

			Layer = Layer.Helm;
			MaxCharges = 10;
		}

            	public RunicHats( Serial serial ) : base ( serial ) 
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
using System;
using Server.Items;

namespace Server.Spells
{
	public class GetSpellGems
	{
		public static Type[] GetGems( Type type )
		{
			int amount = 3;

			if ( type == typeof( HealingPowder ) )
				amount = 9;
			if ( type == typeof( FieryWeapon ) || type == typeof( EarthWeapon ) || type == typeof( LightningWeapon ) || 
				type == typeof( ToxicWeapon ) || type == typeof( FrostWeapon ) )
				amount = 4;

			Type[] types = new Type[amount];

			if ( type == typeof( CreateFire ) )
			{
				types[0] = ( RuneGems.Sapphire );
				types[1] = ( RuneGems.Amber );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( FieryEye ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.Citrine );
			}
			if ( type == typeof( ChainOfFire ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Tourmaline );
				types[2] = ( RuneGems.Amber );
			}
			if ( type == typeof( HeatWave ) )
			{
				types[0] = ( RuneGems.Amber );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Citrine );
			}
			if ( type == typeof( FieryWeapon ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
				types[3] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( Rejuvenation ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.Ruby );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( Whirlpool ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.StarSapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( AquaElementalSpell ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( MagicFood ) )
			{
				types[0] = ( RuneGems.Sapphire );
				types[1] = ( RuneGems.Amber );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( HealingPowder ) )
			{
				types[0] = ( RuneGems.Amber );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Citrine );
				types[3] = ( RuneGems.Diamond );
				types[4] = ( RuneGems.Emerald );
				types[5] = ( RuneGems.Ruby );
				types[6] = ( RuneGems.Sapphire );
				types[7] = ( RuneGems.StarSapphire );
				types[8] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( CarrySound ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.StarSapphire );
			}
			if ( type == typeof( Gust ) )
			{
				types[0] = ( RuneGems.Amethyst );
				types[1] = ( RuneGems.Citrine );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( TornadoElementalSpell ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( Revitalization ) )
			{
				types[0] = ( RuneGems.Amethyst );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.StarSapphire );
			}
			if ( type == typeof( ArchRevitalization ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.StarSapphire );
			}
			if ( type == typeof( LinkOfTheTree ) )
			{
				types[0] = ( RuneGems.StarSapphire );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Amethyst );
			}
			if ( type == typeof( BlackLotus ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.Amethyst );
			}
			if ( type == typeof( TravelingDirt ) )
			{
				types[0] = ( RuneGems.Amethyst );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.Ruby );
			} 
			if ( type == typeof( StoneSkin ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Emerald );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( EarthWeapon ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
				types[3] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( SoulLeech ) )
			{
				types[0] = ( RuneGems.StarSapphire );
				types[1] = ( RuneGems.Tourmaline );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( SoulLantern ) )
			{
				types[0] = ( RuneGems.Citrine );
				types[1] = ( RuneGems.StarSapphire );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( SpiritualLink ) )
			{
				types[0] = ( RuneGems.Amethyst );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( UndeadLegionSpell ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( EntityBarrier ) )
			{
				types[0] = ( RuneGems.Amber );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( LightningStrike ) )
			{
				types[0] = ( RuneGems.Citrine );
				types[1] = ( RuneGems.Diamond );
				types[2] = ( RuneGems.Emerald );
			}
			if ( type == typeof( LightningField ) )
			{
				types[0] = ( RuneGems.Amethyst );
				types[1] = ( RuneGems.Citrine );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( CircleOfLightning ) )
			{
				types[0] = ( RuneGems.Sapphire );
				types[1] = ( RuneGems.StarSapphire );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( EnergySource ) )
			{
				types[0] = ( RuneGems.Amber );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( LightningWeapon ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
				types[3] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( VenomInjection ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( Contagion ) )
			{
				types[0] = ( RuneGems.Sapphire );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( Hallucinogen ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Amber );
			}
			if ( type == typeof( PlagueSpore ) )
			{
				types[0] = ( RuneGems.Amber );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Amethyst );
			}
			if ( type == typeof( ToxicWeapon ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
				types[3] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( IceDagger ) )
			{
				types[0] = ( RuneGems.Sapphire );
				types[1] = ( RuneGems.StarSapphire );
				types[2] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( IceBonds ) )
			{
				types[0] = ( RuneGems.StarSapphire );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Amethyst );
			}
			if ( type == typeof( FrostBite ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Sapphire );
			}
			if ( type == typeof( IceMeteor ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( FrostWeapon ) )
			{
				types[0] = ( RuneGems.Diamond );
				types[1] = ( RuneGems.Amethyst );
				types[2] = ( RuneGems.Ruby );
				types[3] = ( RuneGems.Tourmaline );
			}
			if ( type == typeof( PureSilence ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.StarSapphire );
			}
			if ( type == typeof( Ensnare ) )
			{
				types[0] = ( RuneGems.Citrine );
				types[1] = ( RuneGems.Ruby );
				types[2] = ( RuneGems.Amber );
			}
			if ( type == typeof( BlackSpace ) )
			{
				types[0] = ( RuneGems.Tourmaline );
				types[1] = ( RuneGems.StarSapphire );
				types[2] = ( RuneGems.Amethyst );
			}
			if ( type == typeof( NoFaceSpell ) )
			{
				types[0] = ( RuneGems.Emerald );
				types[1] = ( RuneGems.Sapphire );
				types[2] = ( RuneGems.Ruby );
			}
			if ( type == typeof( SilentBomb ) )
			{
				types[0] = ( RuneGems.Amethyst );
				types[1] = ( RuneGems.Ruby );
				types[2] = ( RuneGems.StarSapphire );
			}

			return types;
		}
	}
}
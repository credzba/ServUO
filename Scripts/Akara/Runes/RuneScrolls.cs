using System;
using Server.Items;

namespace Server.Spells
{
	public class RuneScrolls
	{
		public static Type[] Types()
		{
			Type[] types = new Type[45];

			types[0] = typeof( BlackLotusScroll );
			types[1] = typeof( EarthWeaponScroll );
			types[2] = typeof( LinkOfTheTreeScroll );
			types[3] = typeof( StoneSkinScroll );
			types[4] = typeof( TravelingDirtScroll );
			types[5] = typeof( ChainOfFireScroll );
			types[6] = typeof( CreateFireScroll );
			types[7] = typeof( FieryEyeScroll );
			types[8] = typeof( FieryWeaponScroll );
			types[9] = typeof( HeatWaveScroll );
			types[10] = typeof( FrostBiteScroll );
			types[11] = typeof( FrostWeaponScroll );
			types[12] = typeof( IceBondsScroll );
			types[13] = typeof( IceDaggerScroll );
			types[14] = typeof( IceMeteorScroll );
			types[15] = typeof( CircleOfLightningScroll );
			types[16] = typeof( EnergySourceScroll );
			types[17] = typeof( LightningFieldScroll );
			types[18] = typeof( LightningStrikeScroll );
			types[19] = typeof( LightningWeaponScroll );
			types[20] = typeof( EntityBarrierScroll );
			types[21] = typeof( SoulLanternScroll );
			types[22] = typeof( SoulLeechScroll );
			types[23] = typeof( SpiritualLinkScroll );
			types[24] = typeof( UndeadLegionScroll );
			types[25] = typeof( ContagionScroll );
			types[26] = typeof( HallucinogenScroll );
			types[27] = typeof( PlagueSporeScroll );
			types[28] = typeof( ToxicWeaponScroll );
			types[29] = typeof( VenomInjectionScroll );
			types[30] = typeof( BlackSpaceScroll );
			types[31] = typeof( EnsnareScroll );
			types[32] = typeof( NoFaceScroll );
			types[33] = typeof( PureSilenceScroll );
			types[34] = typeof( SilentBombScroll );
			types[35] = typeof( AquaElementalScroll );
			types[36] = typeof( HealingPowderScroll );
			types[37] = typeof( MagicalFoodScroll );
			types[38] = typeof( RejuvenationScroll );
			types[39] = typeof( WhirlpoolScroll );
			types[40] = typeof( ArchRevitalizationScroll );
			types[41] = typeof( RevitalizationScroll );
			types[42] = typeof( TornadoElementalScroll );
			types[43] = typeof( CarrySoundScroll );
			types[44] = typeof( GustScroll );

			return types;
		}
	}
}
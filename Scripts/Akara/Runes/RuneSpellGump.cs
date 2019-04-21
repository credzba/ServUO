using System; 
using System.IO;
using System.Collections;
using Server.Items; 
using Server.Network; 
using Server.Gumps; 
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	public class RuneSpellGump : Gump
	{
                private SpellRune m_Rune;

                public RuneSpellGump( Mobile from, SpellRune rune ) : base( 125, 125 )
                {
			m_Rune = rune;

			AddPage( 0 ); 

			from.CloseGump( typeof( RuneSpellGump ) );

			AddBackground( 0, 0, 255, 160, 0x2436 );
			AddBackground( 0, 160, 255, 240, 0x2436 );

			AddLabel( 50, 20, 5, "Rune Spells" );

			AddLabel( 140, 20, 1152, "Type: "+ m_Rune.Type );

			AddLabel( 170, 270, 906, "Created By" );
			AddLabel( 170, 290, 906, "~Raelis~ & RahvinTheEvil" );

			if ( m_Rune.Spell1 )AddImage( 15, 180, 5224 );
			if ( m_Rune.Spell2 )AddImage( 15, 205, 5224 );
			if ( m_Rune.Spell3 )AddImage( 15, 230, 5224 );
			if ( m_Rune.Spell4 )AddImage( 15, 255, 5224 );
			if ( m_Rune.Spell5 )AddImage( 15, 280, 5224 );
			if ( m_Rune.Spell1 )AddTextEntry( 45, 180, 75, 15, 1152, 0, m_Rune.Spell1Keyword );
			if ( m_Rune.Spell2 )AddTextEntry( 45, 205, 75, 15, 1152, 1, m_Rune.Spell2Keyword );
			if ( m_Rune.Spell3 )AddTextEntry( 45, 230, 75, 15, 1152, 2, m_Rune.Spell3Keyword );
			if ( m_Rune.Spell4 )AddTextEntry( 45, 255, 75, 15, 1152, 3, m_Rune.Spell4Keyword );
			if ( m_Rune.Spell5 )AddTextEntry( 45, 280, 75, 15, 1152, 4, m_Rune.Spell5Keyword );

			if ( !m_Rune.Spell1 || !m_Rune.Spell2 || !m_Rune.Spell3 || !m_Rune.Spell4 || !m_Rune.Spell5 )
			{
				AddButton( 15, 360, 4005, 4007, 47, GumpButtonType.Reply, 0 );
				AddLabel( 45, 360, 1152, "Add Spell" );
			}

			int type = 0;
			if ( m_Rune.Type == RuneType.Fire ) type = 1;
			if ( m_Rune.Type == RuneType.Water ) type = 2;
			if ( m_Rune.Type == RuneType.Wind ) type = 3;
			if ( m_Rune.Type == RuneType.Earth ) type = 4;
			if ( m_Rune.Type == RuneType.Spirit ) type = 5;
			if ( m_Rune.Type == RuneType.Lightning ) type = 6;
			if ( m_Rune.Type == RuneType.Toxic ) type = 7;
			if ( m_Rune.Type == RuneType.Ice ) type = 8;
			if ( m_Rune.Type == RuneType.Void ) type = 9;

			if ( m_Rune.Spell1 )AddButton( 15, 40, 1209, 1210, 0, GumpButtonType.Page, 2 );
			if ( m_Rune.Spell2 )AddButton( 15, 60, 1209, 1210, 0, GumpButtonType.Page, 3 );
			if ( m_Rune.Spell3 )AddButton( 15, 80, 1209, 1210, 0, GumpButtonType.Page, 4 );
			if ( m_Rune.Spell4 )AddButton( 15, 100, 1209, 1210, 0, GumpButtonType.Page, 5 );
			if ( m_Rune.Spell5 )AddButton( 15, 120, 1209, 1210, 0, GumpButtonType.Page, 6 );

			if ( type == 1 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 1, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Create Fire" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 2, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Fiery Eye" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 3, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Chain Of Fire" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 4, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Heat Wave" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 5, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Fiery Weapon" );
			}
			else if ( type == 2 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 6, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Rejuvination" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 7, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Magical Food" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 8, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Aqua Elemental" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 9, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Whirlpool" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 10, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Healing Powder" );
			}
			else if ( type == 3 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 11, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Carry Sound" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 12, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Gust" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 13, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Tornado Elemental" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 14, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Revitalization" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 15, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Arch Revitalization" );
			}
			else if ( type == 4 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 16, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Link Of The Tree" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 17, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Black Lotus" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 18, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Traveling Dirt" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 19, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Stone Skin" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 20, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Earth Weapon" );
			}
			else if ( type == 5 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 21, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Soul Leech" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 22, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Soul Lantern" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 23, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Spiritual Link" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 24, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Undead Legion" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 25, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Entity Barrier" );
			}
			else if ( type == 6 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 26, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Lightning Strike" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 27, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Lightning Field" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 28, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Circle Of Lightning" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 29, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Energy Source" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 30, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Lightning Weapon" );
			}
			else if ( type == 7 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 31, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Venom Injection" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 32, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Contagion" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 33, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Hallucinogen" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 34, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Plauge Spore" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 35, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Toxic Weapon" );
			}
			else if ( type == 8 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 36, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Ice Dagger" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 37, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Ice Bonds" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 38, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Frost Bite" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 39, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "Ice Meteor" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 40, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Frost Weapon" );
			}
			else if ( type == 9 )
			{
				if ( m_Rune.Spell1 )AddButton( 45, 40, 4005, 4007, 41, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell1 )AddLabel( 75, 40, 1152, "Pure Silence" );

				if ( m_Rune.Spell2 )AddButton( 45, 60, 4005, 4007, 42, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell2 )AddLabel( 75, 60, 1152, "Ensnare" );

				if ( m_Rune.Spell3 )AddButton( 45, 80, 4005, 4007, 43, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell3 )AddLabel( 75, 80, 1152, "Black Source" );

				if ( m_Rune.Spell4 )AddButton( 45, 100, 4005, 4007, 44, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell4 )AddLabel( 75, 100, 1152, "No Face" );

				if ( m_Rune.Spell5 )AddButton( 45, 120, 4005, 4007, 45, GumpButtonType.Reply, 0 );
				if ( m_Rune.Spell5 )AddLabel( 75, 120, 1152, "Silent Bomb" );
			}

			if ( !m_Rune.UseKeywords )
				AddButton( 15, 310, 210, 211, 46, GumpButtonType.Reply, 0 );
			else
				AddButton( 15, 310, 211, 210, 46, GumpButtonType.Reply, 0 );
			AddLabel( 45, 310, 1152, "Use Keywords" );

			AddButton( 15, 335, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
			AddLabel( 45, 335, 33, "Close" );

			AddPage( 1 );

			if ( type == 1 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Create Fire", "The caster summons a flame at any location, this flame will inflict damage upon anyone who walks over it, except the caster.", GetSpellGems.GetGems( typeof(CreateFire) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Fiery Eye", "A mass aura of flames in the shape of an eye at any location, these flames will inflict damage upon anyone who walks over it, except the caster.", GetSpellGems.GetGems( typeof(FieryEye) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Chain Of Fire", "The caster calls forth a flame upon an enemy, the flame then has a chance (depends on your skill and the level of the rune) to jump to another enemy, if any are around.", GetSpellGems.GetGems( typeof(ChainOfFire) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Heat Wave", "A wave of heat is drawn out from all sides of the caster, damaging anyone that is near the caster.", GetSpellGems.GetGems( typeof(HeatWave) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Fiery Weapon", "The caster summons a weapon to any person they choose, the weapon will have 100% fire damage, and will do a specail attack when you hit an enemy, this weapon's type will depend on the target's skills. If they do not have any high weapon skills, it will give them a higher resistance to fire.", GetSpellGems.GetGems( typeof(FieryWeapon) ) );
			}
			else if ( type == 2 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Rejuvenation", "Whoever the caster targets has a very small portion of their life restored every few seconds, amount restored depends on caster's inscribe skill and rune's level, the spell's lasting depends on the caster's inscribe skill and the rune's level.", GetSpellGems.GetGems( typeof(Rejuvenation) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Magical Food", "The caster creates a piece of food with magical curing abilities when eaten.", GetSpellGems.GetGems( typeof(MagicFood) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Aqua Elemental", "For each level the rune has, an aqua elemental (creature) will be summoned to act as a drone, you cannot command them, but they will follow you and will attack whatever you are attacking.", GetSpellGems.GetGems( typeof(AquaElementalSpell) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Whilrpool", "The caster can target a recall rune, and a whirlpool will appear, whoever steps on the whirlpool will be sucked down, and then will appear at the recall rune's locaton.", GetSpellGems.GetGems( typeof(Whirlpool) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Healing Powder", "The caster creates a powdery substance, which when poured upon wounds will close them up and you will feel as if they had never been. It cannot, however, cure poisons, and if you are poisoned you cannot use it.", GetSpellGems.GetGems( typeof(HealingPowder) ) );
			}
			else if ( type == 3 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Carry Sound", "When the caster targets a location, in a radius depending on the rune's level x 3, they will be able to hear whatever is spoken in that radius, it will appear at the bottom lefthand corner of the screen.", GetSpellGems.GetGems( typeof(CarrySound) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Gust", "When the caster targets a recall rune, the caster will be sent to the recall rune's location.", GetSpellGems.GetGems( typeof(Gust) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Tornado Elemental", "For each level the rune has, a tornado elemental (creature) will be summoned to act as a drone, you cannot command them, but they will follow you and will attack whatever you are attacking.", GetSpellGems.GetGems( typeof(TornadoElementalSpell) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Revitalization", "Whoever the caster targets has a very small portion of their stamina restored every few seconds, amount restored depends on caster's inscribe skill and rune's level, the spell's lasting depends on the caster's inscribe skill and the rune's level.", GetSpellGems.GetGems( typeof(Revitalization) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Arch Revitalization", "The caster targets an area, and for each ally within a radius depending on the rune's level will have the revitalization spell cast upon them.", GetSpellGems.GetGems( typeof(ArchRevitalization) ) );
			}
			else if ( type == 4 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Link Of The Tree", "Whoever the caster targets is linked to the caster, and whenever the target is damaged, the caster will take the damage instead of the target.", GetSpellGems.GetGems( typeof(LinkOfTheTree) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Black Lotus", "The caster plants a black lotus seed onto anyone they target, in a few seconds that black lotus will sprout and damage them.", GetSpellGems.GetGems( typeof(BlackLotus) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Traveling Dirt", "Creates a piece of dirt that you can add items and pets to. When you bury the dirt, the items will be sent to your bank, and the pets will be sent to your stables.", GetSpellGems.GetGems( typeof(TravelingDirt) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Stone Skin", "The caster summons a set of magical armor to any person they choose, the armor's type will depend on the target's skills.", GetSpellGems.GetGems( typeof(StoneSkin) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Earth Weapon", "The caster summons a weapon to any person they choose, the weapon will have 100% physical damage, and will do a specail attack when you hit an enemy, this weapon's type will depend on the target's skills. If they do not have any high weapon skills, it will give them a higher resistance to physical.", GetSpellGems.GetGems( typeof(EarthWeapon) ) );
			}
			else if ( type == 5 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Soul Leech", "The caster leeches mana, stamina, and life points from a target, a small amount is taken from the target, and a third of that amount is given to the caster.", GetSpellGems.GetGems( typeof(SoulLeech) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Soul Lantern", "A lantern is summoned to the caster's target, whenever the possessor of the lantern says 'collect souls', for each corpse around them that has not been channeled, it's soul will be drawn into the lantern. Whenever the lantern is used (double-clicked) if the target is the caster or the possessor, then it will heal them for a certain amount, if it's an enemy, it will damage them for a certain amount, the amounts depend on the amount of souls in the lantern.", GetSpellGems.GetGems( typeof(SoulLantern) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Spiritual Link", "Whoever the caster targets is linked to the caster, the caster will know when their link is damaged, be able to heal, cure, refresh their stamina, and refresh their link's mana at any distance, the amount of the action will depend on the rune's level and the caster's inscribe skill.", GetSpellGems.GetGems( typeof(SpiritualLink) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Undead Legion", "For each level the rune has, an undead legion (skeletal warrior) will be summoned to act as a drone, you cannot command them, but they will follow you and will attack whatever you are attacking.", GetSpellGems.GetGems( typeof(UndeadLegionSpell) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Entity Barrier", "The caster targets an enemy, and that enemy is surrounded by a barrier, which no one but the caster can see if their inscribe skill is not high, if the enemy or anyone on the outside of the barrier gets too close to the barrier, it will inflict damage upon them, no one my cast on anything inside the barrier, and anyone inside the barrier cannot cast upon anything outside it.", GetSpellGems.GetGems( typeof(EntityBarrier) ) );
			}
			else if ( type == 6 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Lightning Strike", "The caster calls forth a lightning bolt to strike any enemy they choose.", GetSpellGems.GetGems( typeof(LightningStrike) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Lightning Field", "The caster creates a field of lightning that will strike a bolt upon anyone except the caster that walks over it.", GetSpellGems.GetGems( typeof(LightningField) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Circle Of Lightning", "Whoever the caster targets recieves an aura that will strike a bolt upon any enemy that walks near them within a radius of the rune's level.", GetSpellGems.GetGems( typeof(CircleOfLightning) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Energy Source", "A stone is spawned at any location the caster chooses and whoever walks within a radius depending on the rune's level will be struck by a bolt of lightning, except the caster.", GetSpellGems.GetGems( typeof(EnergySource) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Lightning Weapon", "The caster summons a weapon to any person they choose, the weapon will have 100% energy damage, and will do a specail attack when you hit an enemy, this weapon's type will depend on the target's skills. If they do not have any high weapon skills, it will give them a higher resistance to energy.", GetSpellGems.GetGems( typeof(LightningWeapon) ) );
			}
			else if ( type == 7 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Venom Injection", "The caster targets a piece of food, and that food is poisoned, the level of poison depends on the caster's inscribe skill and the rune's level.", GetSpellGems.GetGems( typeof(VenomInjection) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Contagion", "The caster targets a location, whoever is within a radius depending on the rune's level is poisoned, the level of poison depends on the caster's inscribe skill and the rune's level and the distance from the location targeted.", GetSpellGems.GetGems( typeof(Contagion) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Hallucinogen", "Whoever the caster targets has their defense lowered, they won't hit as much, and their attack is lowered. As well, every few seconds they will cry out sounds.", GetSpellGems.GetGems( typeof(Hallucinogen) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Plague Spore", "A spore is spawned at any location the caster chooses and whoever walks within a radius depending on the rune's level will be damaged and poisoned, the level of the poison depeneds on the rune's level and the caster's inscribe skill.", GetSpellGems.GetGems( typeof(PlagueSpore) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Toxic Weapon", "The caster summons a weapon to any person they choose, the weapon will have 100% poison damage, and will do a specail attack when you hit an enemy, this weapon's type will depend on the target's skills. If they do not have any high weapon skills, it will give them a higher resistance to poison.", GetSpellGems.GetGems( typeof(ToxicWeapon) ) );
			}
			else if ( type == 8 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Ice Dagger", "The caster summons pieces of ice in the shape of daggers flying towards any enemy they choose, the amount of daggers summoned depends on the rune's level.", GetSpellGems.GetGems( typeof(IceDagger) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Ice Bonds", "Bonds of ice appear around any target the caster chooses, they paralyze the target and you can even cast upon them without them becoming unparalyzed again, but does not last for very long.", GetSpellGems.GetGems( typeof(IceBonds) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Frost Bite", "Whoever the caster targets recieves frost bite. Thus lowering the amount of damage they deal, the amount lowered depends on the caster's inscribe skill and the rune's level, how long the spell lasts depends on the rune's level and the caster's inscribe skill.", GetSpellGems.GetGems( typeof(FrostBite) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "Ice Meteor", "The caster targets an area, and within a radius depending on the rune's level meteors of ice are summoned flying toward enemies, the amount of meteors summoned depends on the rune's level.", GetSpellGems.GetGems( typeof(IceMeteor) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Frost Weapon", "The caster summons a weapon to any person they choose, the weapon will have 100% cold damage, and will do a specail attack when you hit an enemy, this weapon's type will depend on the target's skills. If they do not have any high weapon skills, it will give them a higher resistance to cold.", GetSpellGems.GetGems( typeof(FrostWeapon) ) );
			}
			else if ( type == 9 )
			{
				if ( m_Rune.Spell1 )AddSpell( 2, "Pure Silence", "The caster is sent to a secluded location to meditate, their mana, stamina, and life points will regan swiftly, and once they reach full mana, stamina, and life points, they will be sent back to the location they cast the spell from.", GetSpellGems.GetGems( typeof(PureSilence) ) );
				if ( m_Rune.Spell2 )AddSpell( 3, "Ensnare", "The caster targets a location, at that location, a trap is placed. Whenever someone walks within a radius depending on the rune's level, four barriers are setup around them so they cannot move, thus they are trapped.", GetSpellGems.GetGems( typeof(Ensnare) ) );
				if ( m_Rune.Spell3 )AddSpell( 4, "Black Space", "A swirling mist is summoned at any location the caster chooses, any enemies within a radius depending upon the rune's level will be drawn to the mist and will be dealt a very, very small amount of damage each time it draws them.", GetSpellGems.GetGems( typeof(BlackSpace) ) );
				if ( m_Rune.Spell4 )AddSpell( 5, "No Face", "For each level the rune has, a no face (creature) will be summoned to act as a drone, you cannot command them, but they will follow you and will attack whatever you are attacking.", GetSpellGems.GetGems( typeof(NoFaceSpell) ) );
				if ( m_Rune.Spell5 )AddSpell( 6, "Silent Bomb", "A bomb is summoned at any location the caster chooses, if any enemy walks within a radius depending on the rune's level it will explode and inflict damage upon any enemy with in a radius depending on the rune's level.", GetSpellGems.GetGems( typeof(SilentBomb) ) );
			}
		}
		private void AddSpell( int page, string name, string text, params Type[] gems )
		{
			ArrayList glist = new ArrayList();
			ArrayList newglist = new ArrayList();
			int amber = 0;
			int amethyst = 0;
			int citrine = 0;
			int diamond = 0;
			int emerald = 0;
			int ruby = 0;
			int sapphire = 0;
			int starsapphire = 0;
			int tourmaline = 0;
			foreach( Type type in gems )
			{
				if ( type == typeof( Amber ) )
					amber += 1;
				else if ( type == typeof( Amethyst ) )
					amethyst += 1;
				else if ( type == typeof( Citrine ) )
					citrine += 1;
				else if ( type == typeof( Diamond ) )
					diamond += 1;
				else if ( type == typeof( Emerald ) )
					emerald += 1;
				else if ( type == typeof( Ruby ) )
					ruby += 1;
				else if ( type == typeof( Sapphire ) )
					sapphire += 1;
				else if ( type == typeof( StarSapphire ) )
					starsapphire += 1;
				else if ( type == typeof( Tourmaline ) )
					tourmaline += 1;
			}
			foreach( Type type in gems )
			{
				if ( type == typeof( Amber ) )
					if ( !(glist.Contains( "Amber" )) )
						glist.Add( "Amber" );
				if ( type == typeof( Amethyst ) )
					if ( !(glist.Contains( "Amethyst" )) )
						glist.Add( "Amethyst" );
				if ( type == typeof( Citrine ) )
					if ( !(glist.Contains( "Citrine" )) )
						glist.Add( "Citrine" );
				if ( type == typeof( Diamond ) )
					if ( !(glist.Contains( "Diamond" )) )
						glist.Add( "Diamond" );
				if ( type == typeof( Emerald ) )
					if ( !(glist.Contains( "Emerald" )) )
						glist.Add( "Emerald" );
				if ( type == typeof( Ruby ) )
					if ( !(glist.Contains( "Ruby" )) )
						glist.Add( "Ruby" );
				if ( type == typeof( Sapphire ) )
					if ( !(glist.Contains( "Sapphire" )) )
						glist.Add( "Sapphire" );
				if ( type == typeof( StarSapphire ) )
					if ( !(glist.Contains( "Star Sapphire" )) )
						glist.Add( "Star Sapphire" );
				if ( type == typeof( Tourmaline ) )
					if ( !(glist.Contains( "Tourmaline" )) )
						glist.Add( "Tourmaline" );
			}
			foreach( string gem in glist )
			{
				string newgem = "";
				if ( gem == "Amber" )
					newgem = amber +" Amber";
				else if ( gem == "Amethyst" )
					newgem = amethyst +" Amethyst";
				else if ( gem == "Citrine" )
					newgem = citrine +" Citrine";
				else if ( gem == "Diamond" )
					newgem = diamond +" Diamond";
				else if ( gem == "Emerald" )
					newgem = emerald +" Emerald";
				else if ( gem == "Ruby" )
					newgem = ruby +" Ruby";
				else if ( gem == "Sapphire" )
					newgem = sapphire +" Sapphire";
				else if ( gem == "Star Sapphire" )
					newgem = starsapphire +" Star Sapphire";
				else if ( gem == "Tourmaline" )
					newgem = tourmaline +" Tourmaline";

				newglist.Add( newgem );
			}

			AddPage( page );
			AddBackground( 255, 0, 255, 400, 0x2436 );

			AddButton( 420, 40, 1209, 1210, 0, GumpButtonType.Page, 1 );
			AddLabel( 440, 37, 33, "Close" );

			AddLabel( 305, 20, 1152, name );
			AddLabel( 295, 40, 1152, "Gems:" );
			int gumpy = 40;
			int maxlength = (380-((newglist.Count*13)+90));
			if ( newglist.Count > 0 )
			{
				for ( int i = 0; i < newglist.Count; ++i )
				{
					string gem = (string)newglist[i];
					AddLabel( 270, gumpy+13, 79, gem );
					gumpy += 13;
				}
			}
			else
				AddLabel( 270, gumpy+15, 1152, "None" );
			AddLabel( 295, gumpy+20, 1152, "Description:" );
			AddHtml( 270, gumpy+33, 225, maxlength, String.Format( "<basefont color=#FFFFFF>{0}</font>", text ), false, false );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( m_Rune == null || m_Rune.Deleted )
				return;

			if ( info.ButtonID >= 0 )
			{
				try
				{
					string spell1 = "";
					string spell2 = "";
					string spell3 = "";
					string spell4 = "";
					string spell5 = "";	

					string[] tr = new string[ 5 ];
					foreach( TextRelay t in info.TextEntries )
					{
						tr[ t.EntryID ] = t.Text;
					}
					if ( tr[ 0 ] != null )
					{
						spell1 = tr[ 0 ];
					}
					if ( tr[ 1 ] != null )
					{
						spell2 = tr[ 1 ];
					}
					if ( tr[ 2 ] != null )
					{
						spell3 = tr[ 2 ];
					}
					if ( tr[ 3 ] != null )
					{
						spell4 = tr[ 3 ];
					}
					if ( tr[ 4 ] != null )
					{
						spell5 = tr[ 4 ];
					}

					m_Rune.Spell1Keyword = spell1;
					m_Rune.Spell2Keyword = spell2;
					m_Rune.Spell3Keyword = spell3;
					m_Rune.Spell4Keyword = spell4;
					m_Rune.Spell5Keyword = spell5;
				}
				
				catch ( Exception e )
				{
					Console.WriteLine("RuneSpellGump.OnResponse Crash Prevented: "+e);
				}
			}

			if ( info.ButtonID == 0 )
			{
				from.SendMessage( "Closed." );
			}

			//Fire
			else if ( info.ButtonID == 1 && !SpellRune.DisFire )
			{
				if ( m_Rune.Spell1 )new CreateFire( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 2 && !SpellRune.DisFire )
			{
				if ( m_Rune.Spell2 )new FieryEye( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 3 && !SpellRune.DisFire )
			{
				if ( m_Rune.Spell3 )new ChainOfFire( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 4 && !SpellRune.DisFire )
			{
				if ( m_Rune.Spell4 )new HeatWave( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 5 && !SpellRune.DisFire )
			{
				if ( m_Rune.Spell5 )new FieryWeapon( from, null, m_Rune ).Cast();
			}
			//Water
			else if ( info.ButtonID == 6 && !SpellRune.DisWater )
			{
				if ( m_Rune.Spell1 )new Rejuvenation( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 7 && !SpellRune.DisWater )
			{
				if ( m_Rune.Spell2 )new MagicFood( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 8 && !SpellRune.DisWater )
			{
				if ( m_Rune.Spell3 )new AquaElementalSpell( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 9 && !SpellRune.DisWater )
			{
				if ( m_Rune.Spell4 )new Server.Spells.Whirlpool( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 10 && !SpellRune.DisWater )
			{
				if ( m_Rune.Spell5 )new HealingPowder( from, null, m_Rune ).Cast();
			}
			//Wind
			else if ( info.ButtonID == 11 && !SpellRune.DisWind )
			{
				if ( m_Rune.Spell1 )new CarrySound( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 12 && !SpellRune.DisWind )
			{
				if ( m_Rune.Spell2 )new Gust( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 13 && !SpellRune.DisWind )
			{
				if ( m_Rune.Spell3 )new TornadoElementalSpell( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 14 && !SpellRune.DisWind )
			{
				if ( m_Rune.Spell4 )new Revitalization( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 15 && !SpellRune.DisWind )
			{
				if ( m_Rune.Spell5 )new ArchRevitalization( from, null, m_Rune ).Cast();
			}
			//Earth
			else if ( info.ButtonID == 16 && !SpellRune.DisEarth )
			{
				if ( m_Rune.Spell1 )new LinkOfTheTree( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 17 && !SpellRune.DisEarth )
			{
				if ( m_Rune.Spell2 )new BlackLotus( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 18 && !SpellRune.DisEarth )
			{
				if ( m_Rune.Spell3 )new TravelingDirt( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 19 && !SpellRune.DisEarth )
			{
				if ( m_Rune.Spell4 )new StoneSkin( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 20 && !SpellRune.DisEarth )
			{
				if ( m_Rune.Spell5 )new EarthWeapon( from, null, m_Rune ).Cast();
			}
			//Spirit
			else if ( info.ButtonID == 21 && !SpellRune.DisSpirit )
			{
				if ( m_Rune.Spell1 )new SoulLeech( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 22 && !SpellRune.DisSpirit )
			{
				if ( m_Rune.Spell2 )new SoulLantern( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 23 && !SpellRune.DisSpirit )
			{
				if ( m_Rune.Spell3 )new SpiritualLink( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 24 && !SpellRune.DisSpirit )
			{
				if ( m_Rune.Spell4 )new UndeadLegionSpell( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 25 && !SpellRune.DisSpirit )
			{
				if ( m_Rune.Spell5 )new EntityBarrier( from, null, m_Rune ).Cast();
			}
			//Lightning
			else if ( info.ButtonID == 26 && !SpellRune.DisLightning )
			{
				if ( m_Rune.Spell1 )new LightningStrike( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 27 && !SpellRune.DisLightning )
			{
				if ( m_Rune.Spell2 )new LightningField( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 28 && !SpellRune.DisLightning )
			{
				if ( m_Rune.Spell3 )new CircleOfLightning( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 29 && !SpellRune.DisLightning )
			{
				if ( m_Rune.Spell4 )new EnergySource( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 30 && !SpellRune.DisLightning )
			{
				if ( m_Rune.Spell5 )new LightningWeapon( from, null, m_Rune ).Cast();
			}
			//Toxic
			else if ( info.ButtonID == 31 && !SpellRune.DisToxic )
			{
				if ( m_Rune.Spell1 )new VenomInjection( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 32 && !SpellRune.DisToxic )
			{
				if ( m_Rune.Spell2 )new Contagion( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 33 && !SpellRune.DisToxic )
			{
				if ( m_Rune.Spell3 )new Hallucinogen( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 34 && !SpellRune.DisToxic )
			{
				if ( m_Rune.Spell4 )new PlagueSpore( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 35 && !SpellRune.DisToxic )
			{
				if ( m_Rune.Spell5 )new ToxicWeapon( from, null, m_Rune ).Cast();
			}
			//Ice
			else if ( info.ButtonID == 36 && !SpellRune.DisIce )
			{
				if ( m_Rune.Spell1 )new IceDagger( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 37 && !SpellRune.DisIce )
			{
				if ( m_Rune.Spell2 )new IceBonds( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 38 && !SpellRune.DisIce )
			{
				if ( m_Rune.Spell3 )new FrostBite( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 39 && !SpellRune.DisIce )
			{
				if ( m_Rune.Spell4 )new IceMeteor( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 40 && !SpellRune.DisIce )
			{
				if ( m_Rune.Spell5 )new FrostWeapon( from, null, m_Rune ).Cast();
			}
			//Void
			else if ( info.ButtonID == 41 && !SpellRune.DisVoid )
			{
				if ( m_Rune.Spell1 )new PureSilence( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 42 && !SpellRune.DisVoid )
			{
				if ( m_Rune.Spell2 )new Ensnare( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 43 && !SpellRune.DisVoid )
			{
				if ( m_Rune.Spell3 )new BlackSpace( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 44 && !SpellRune.DisVoid )
			{
				if ( m_Rune.Spell4 )new NoFaceSpell( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 45 && !SpellRune.DisVoid )
			{
				if ( m_Rune.Spell5 )new SilentBomb( from, null, m_Rune ).Cast();
			}
			else if ( info.ButtonID == 46 )
			{
				if ( m_Rune.UseKeywords )
					m_Rune.UseKeywords = false;
				else
					m_Rune.UseKeywords = true;

				from.SendGump( new RuneSpellGump( from, m_Rune ) );
			}
			else if ( info.ButtonID == 47 )
			{
				from.Target = new AddRuneSpellTarget( m_Rune );
			}
		}
	}
	public class AddRuneSpellTarget : Target
	{
		private SpellRune m_Rune;

		public AddRuneSpellTarget( SpellRune rune ) : base( 0, false, TargetFlags.None )
		{
			m_Rune = rune;
		}

		protected override void OnTarget( Mobile from, object o )
		{
			if ( o is BaseRuneScroll )
			{
				BaseRuneScroll scroll = (BaseRuneScroll)o;

				if ( m_Rune.Level < scroll.Level - 1 )
					from.SendMessage( "The rune's level is not high enough to accept this spell." );
				else if ( scroll.Type == m_Rune.Type )
				{
					if ( scroll.Level == 1 && !m_Rune.Spell1 )
					{
						m_Rune.Spell1 = true;
						from.SendGump( new RuneSpellGump( from, m_Rune ) );
						from.SendMessage( "You add '"+scroll.Name+"' to the rune." );
						if (scroll.Amount == 1)
						scroll.Delete();
						else
						scroll.Amount--;
						
					}
					else if ( scroll.Level == 2 && !m_Rune.Spell2 )
					{
						m_Rune.Spell2 = true;
						from.SendGump( new RuneSpellGump( from, m_Rune ) );
						from.SendMessage( "You add '"+scroll.Name+"' to the rune." );
						if (scroll.Amount == 1)
						scroll.Delete();
						else
						scroll.Amount--;
					}
					else if ( scroll.Level == 3 && !m_Rune.Spell3 )
					{
						m_Rune.Spell3 = true;
						from.SendGump( new RuneSpellGump( from, m_Rune ) );
						from.SendMessage( "You add '"+scroll.Name+"' to the rune." );
						if (scroll.Amount == 1)
						scroll.Delete();
						else
						scroll.Amount--;
					}
					else if ( scroll.Level == 4 && !m_Rune.Spell4 )
					{
						m_Rune.Spell4 = true;
						from.SendGump( new RuneSpellGump( from, m_Rune ) );
						from.SendMessage( "You add '"+scroll.Name+"' to the rune." );
						if (scroll.Amount == 1)
						scroll.Delete();
						else
						scroll.Amount--;
					}
					else if ( scroll.Level == 5 && !m_Rune.Spell5 )
					{
						m_Rune.Spell5 = true;
						from.SendGump( new RuneSpellGump( from, m_Rune ) );
						from.SendMessage( "You add '"+scroll.Name+"' to the rune." );
						if (scroll.Amount == 1)
						scroll.Delete();
						else
						scroll.Amount--;
					}
					else
						from.SendMessage( "That rune already has that spell." );
				}
				else
					from.SendMessage( "This rune does not accept those types spells." );
			}
			else
				from.SendMessage( "That target is invalid." );
		}
	}
}

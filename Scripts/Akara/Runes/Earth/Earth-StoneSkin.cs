using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class StoneSkin : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Stone Skin", "Cadre Cutis",
				212,
				9041,
				GetSpellGems.GetGems( typeof(StoneSkin) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public override int RuneGainMax{ get{ return 80; } }
		public override int RuneGainMin{ get{ return 40; } }

		public StoneSkin( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to summon the armor to." );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( !m.CanBeginAction( typeof(StoneSkin) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else if ( CheckSequence() )
			{
				if ( m.Player )
				{
					GainExp();
					TimeSpan duration = TimeSpan.FromMinutes( (Caster.Skills[SkillName.Inscribe].Value / 5.0 * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
					duration += TimeSpan.FromSeconds( RSCA.CalculateLLS(Caster, Caster.Skills[SkillName.Inscribe].Value / 5.0) );
					m.BeginAction( typeof( StoneSkin ) );

					if ( m.Skills[SkillName.Parry].Value >= 75.0 )
					{
						m.AddToBackpack( new InternalHeaterShield( this, Caster, Rune, m, duration ) );
					}
					if ( m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Archery].Value &&
						m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Magery].Value
					)
					{
						m.AddToBackpack( new InternalPlateArms( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateLegs( this, Caster, Rune, m, duration ) );
						if ( !m.Female )
							m.AddToBackpack( new InternalPlateChest( this, Caster, Rune, m, duration ) );
					 	else
							m.AddToBackpack( new InternalFemalePlateChest( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateGloves( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateGorget( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateHelm( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Archery].Value &&
						m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Magery].Value
					)
					{
						m.AddToBackpack( new InternalPlateArms( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateLegs( this, Caster, Rune, m, duration ) );
						if ( !m.Female )
							m.AddToBackpack( new InternalPlateChest( this, Caster, Rune, m, duration ) );
					 	else
							m.AddToBackpack( new InternalFemalePlateChest( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateGloves( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateGorget( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateHelm( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Archery].Value &&
						m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Magery].Value
					)
					{
						m.AddToBackpack( new InternalPlateArms( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateLegs( this, Caster, Rune, m, duration ) );
						if ( !m.Female )
							m.AddToBackpack( new InternalPlateChest( this, Caster, Rune, m, duration ) );
					 	else
							m.AddToBackpack( new InternalFemalePlateChest( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateGloves( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateGorget( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalPlateHelm( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Magery].Value
					)
					{
						m.AddToBackpack( new InternalStuddedArms( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalStuddedLegs( this, Caster, Rune, m, duration ) );
						if ( !m.Female )
							m.AddToBackpack( new InternalStuddedChest( this, Caster, Rune, m, duration ) );
					 	else
							m.AddToBackpack( new InternalStuddedBustierArms( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalStuddedGloves( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalStuddedGorget( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalLeatherCap( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Magery].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Magery].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Magery].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Magery].Value >= m.Skills[SkillName.Archery].Value
					)
					{
						m.AddToBackpack( new InternalLeatherArms( this, Caster, Rune, m, duration ) );
						if ( !m.Female )
						{
							m.AddToBackpack( new InternalLeatherChest( this, Caster, Rune, m, duration ) );
							m.AddToBackpack( new InternalLeatherLegs( this, Caster, Rune, m, duration ) );
						}
					 	else
						{
							m.AddToBackpack( new InternalLeatherBustierArms( this, Caster, Rune, m, duration ) );
							if ( 0.5 > Utility.RandomDouble() )
								m.AddToBackpack( new InternalLeatherSkirt( this, Caster, Rune, m, duration ) );
							else
								m.AddToBackpack( new InternalLeatherShorts( this, Caster, Rune, m, duration ) );
						}
						m.AddToBackpack( new InternalLeatherGloves( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalLeatherGorget( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalLeatherCap( this, Caster, Rune, m, duration ) );
					}
					else
					{
						m.AddToBackpack( new InternalLeatherArms( this, Caster, Rune, m, duration ) );
						if ( !m.Female )
						{
							m.AddToBackpack( new InternalLeatherChest( this, Caster, Rune, m, duration ) );
							m.AddToBackpack( new InternalLeatherLegs( this, Caster, Rune, m, duration ) );
						}
					 	else
						{
							m.AddToBackpack( new InternalLeatherBustierArms( this, Caster, Rune, m, duration ) );
							if ( 0.5 > Utility.RandomDouble() )
								m.AddToBackpack( new InternalLeatherSkirt( this, Caster, Rune, m, duration ) );
							else
								m.AddToBackpack( new InternalLeatherShorts( this, Caster, Rune, m, duration ) );
						}
						m.AddToBackpack( new InternalLeatherGloves( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalLeatherGorget( this, Caster, Rune, m, duration ) );
						m.AddToBackpack( new InternalLeatherCap( this, Caster, Rune, m, duration ) );
					}
				}
			}
			FinishSequence();
		}
		[FlipableAttribute( 0x1c04, 0x1c05 )]
		private class InternalFemalePlateChest : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalFemalePlateChest( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1C04 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalFemalePlateChest( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1415, 0x1416 )]
		private class InternalPlateChest : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalPlateChest( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1415 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalPlateChest( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1410, 0x1417 )]
		private class InternalPlateArms : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalPlateArms( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1410 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalPlateArms( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1411, 0x141a )]
		private class InternalPlateLegs : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalPlateLegs( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1411 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalPlateLegs( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1414, 0x1418 )]
		private class InternalPlateGloves : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalPlateGloves( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1414 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalPlateGloves( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		private class InternalPlateGorget : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalPlateGorget( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1413 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalPlateGorget( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		private class InternalPlateHelm : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

			[Constructable]
			public InternalPlateHelm( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1412 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalPlateHelm( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1c0a, 0x1c0b )]
		private class InternalLeatherBustierArms : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherBustierArms( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1c0a )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherBustierArms( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13cc, 0x13d3 )]
		private class InternalLeatherChest : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherChest( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13cc )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherChest( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13cd, 0x13c5 )]
		private class InternalLeatherArms : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherArms( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13cd )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherArms( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13cb, 0x13d2 )]
		private class InternalLeatherLegs : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherLegs( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13cb )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherLegs( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[Flipable]
		private class InternalLeatherGloves : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherGloves( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13C6 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherGloves( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		private class InternalLeatherGorget : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherGorget( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13C7 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherGorget( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1db9, 0x1dba )]
		private class InternalLeatherCap : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherCap( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1DB9 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherCap( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1c00, 0x1c01 )]
		private class InternalLeatherShorts : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherShorts( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1c00 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherShorts( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1c08, 0x1c09 )]
		private class InternalLeatherSkirt : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

			[Constructable]
			public InternalLeatherSkirt( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1C08 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalLeatherSkirt( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x1c0c, 0x1c0d )]
		private class InternalStuddedBustierArms : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }

			[Constructable]
			public InternalStuddedBustierArms( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1c0c )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalStuddedBustierArms( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13db, 0x13e2 )]
		private class InternalStuddedChest : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }

			[Constructable]
			public InternalStuddedChest( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13db )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalStuddedChest( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13dc, 0x13d4 )]
		private class InternalStuddedArms : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }

			[Constructable]
			public InternalStuddedArms( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13dc )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalStuddedArms( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13da, 0x13e1 )]
		private class InternalStuddedLegs : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }

			[Constructable]
			public InternalStuddedLegs( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13da )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalStuddedLegs( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		[FlipableAttribute( 0x13d5, 0x13dd )]
		private class InternalStuddedGloves : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }

			[Constructable]
			public InternalStuddedGloves( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13d5 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalStuddedGloves( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		private class InternalStuddedGorget : BaseArmor
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }
			public override int OldStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }

			[Constructable]
			public InternalStuddedGorget( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13D6 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalStuddedGorget( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		private class InternalHeaterShield : BaseShield
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;private RuneSpell m_Spell;

			public override int BasePhysicalResistance{ get{ return 10; } }
			public override int BaseFireResistance{ get{ return 10; } }
			public override int BaseColdResistance{ get{ return 10; } }
			public override int BasePoisonResistance{ get{ return 10; } }
			public override int BaseEnergyResistance{ get{ return 10; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override int AosStrReq{ get{ return 100; } }

			public override int ArmorBase{ get{ return 100; } }

			[Constructable]
			public InternalHeaterShield( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1B76 )
			{
				m_Spell = spell;m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 5.0;
				Layer = Layer.TwoHanded;
				Hue = 905;
				BlessedFor = owner;
				Name = "Stone Skin";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();

				Attributes.ReflectPhysical = (15 + RSCA.CalculateSI(m_Caster, 15));SkillBonuses.SetValues( 0, SkillName.Parry, (5.0 + (double)RSCA.CalculateSI(m_Caster, 5)) );
			}

			public InternalHeaterShield( Serial serial ) : base( serial )
			{
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The stone surrounding your skin falls off." );
				m_Owner.EndAction( typeof( StoneSkin ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				;writer.Write( m_Caster );
			}
		
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();

				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();Rune = reader.ReadItem() as SpellRune;m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();

				if ( Weight == 1.0 )
					Weight = 4.0;
			}
		}
		private class InternalTimer : Timer
		{
			private BaseArmor m_Arm;
			private DateTime m_Expire;

			public InternalTimer( BaseArmor arm, DateTime expire ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Arm = arm;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					m_Arm.Delete();
					Stop();
				}
			}
		}

		private class InternalTarget : Target
		{
			private StoneSkin m_Owner;

			public InternalTarget( StoneSkin owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}
	}
}
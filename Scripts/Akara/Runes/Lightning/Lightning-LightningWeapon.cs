using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class LightningWeapon : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Lightning Weapon", "Artu Iquan",
				212,
				9041,
				GetSpellGems.GetGems( typeof(LightningWeapon) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public override int RuneGainMax{ get{ return 100; } }
		public override int RuneGainMin{ get{ return 50; } }

		public LightningWeapon( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to summon the weapon to." );
		}

		private static Hashtable m_Table = new Hashtable();

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
			else if ( !m.CanBeginAction( typeof(LightningWeapon) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else if ( CheckSequence() )
			{
				if ( m.Player )
				{
					GainExp();
					m.BoltEffect( 0 );
					TimeSpan duration = TimeSpan.FromMinutes( (Caster.Skills[SkillName.Inscribe].Value / 15.0 * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
					duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, Caster.Skills[SkillName.Inscribe].Value / 15.0) );
					m.BeginAction( typeof( LightningWeapon ) );

					if ( m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Fencing].Value >= m.Skills[SkillName.Archery].Value &&
						m.Skills[SkillName.Fencing].Value >= 75.0
					)
					{
						m.AddToBackpack( new InternalKryss( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Swords].Value >= m.Skills[SkillName.Archery].Value &&
						m.Skills[SkillName.Swords].Value >= 75.0
					)
					{
						m.AddToBackpack( new InternalKatana( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Macing].Value >= m.Skills[SkillName.Archery].Value &&
						m.Skills[SkillName.Macing].Value >= 75.0
					)
					{
						m.AddToBackpack( new InternalHammer( this, Caster, Rune, m, duration ) );
					}
					else if ( m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Fencing].Value &&
						m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Swords].Value &&
						m.Skills[SkillName.Archery].Value >= m.Skills[SkillName.Macing].Value &&
						m.Skills[SkillName.Archery].Value >= 75.0
					)
					{
						m.AddToBackpack( new InternalBow( this, Caster, Rune, m, duration ) );
					}
					else
					{
						m.SendMessage( "Since you don't have any high weapon skills, you will get a bonus to your energy resist." );
						if ( m != Caster )
							Caster.SendMessage( "Since they don't have any high weapon skills, they will get a bonus to their energy resist." );

						ResistanceMod[] mods = (ResistanceMod[])m_Table[m];

						if ( mods == null )
						{
							mods = new ResistanceMod[1]
								{
									new ResistanceMod( ResistanceType.Energy, Rune.Level + (int)(Caster.Skills[SkillName.Inscribe].Value/5) )
								};

							m_Table[m] = mods;

							for ( int i = 0; i < mods.Length; ++i )
								m.AddResistanceMod( mods[i] );
							new Internal2Timer( m, m_Table, duration );
						}
						else
						{
							m_Table.Remove( m );

							for ( int i = 0; i < mods.Length; ++i )
								m.RemoveResistanceMod( mods[i] );
						}
					}
				}
			}
			FinishSequence();
		}
		private class Internal2Timer : Timer
		{
			private Mobile Source;
			private Hashtable Table;

			public Internal2Timer( Mobile source, Hashtable table, TimeSpan duration ) : base( duration )
			{
				Source = source;
				Table = table;
			}

			protected override void OnTick()
			{
				ResistanceMod[] mods = (ResistanceMod[])m_Table[Source];
				if ( mods != null )
				{
					Source.EndAction( typeof( LightningWeapon ) );
					m_Table.Remove( Source );

					for ( int i = 0; i < mods.Length; ++i )
						Source.RemoveResistanceMod( mods[i] );
				}
			}
		}

		[FlipableAttribute( 0x1439, 0x1438 )]
		private class InternalHammer : BaseBashing
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;
			private RuneSpell m_Spell;

			public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
			public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.CrushingBlow; } }

			public override int AosStrengthReq{ get{ return 10; } }
			public override int AosMinDamage{ get{ return 17; } }
			public override int AosMaxDamage{ get{ return 18; } }
			public override int AosSpeed{ get{ return 28; } }

			public override int OldStrengthReq{ get{ return 40; } }
			public override int OldMinDamage{ get{ return 8; } }
			public override int OldMaxDamage{ get{ return 36; } }
			public override int OldSpeed{ get{ return 31; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash2H; } }

			[Constructable]
			public InternalHammer( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1439 )
			{
				m_Spell = spell;
				m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.TwoHanded;
				Hue = 0x481;
				BlessedFor = owner;
				Name = "Lightning Hammer";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
			{
				phys = fire = cold = pois = chaos = direct = 0;
				nrgy = 100;
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The weapon blinks out." );
				m_Owner.EndAction( typeof( LightningWeapon ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void OnHit( Mobile attacker, IDamageable m, double damageBonus )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				base.OnHit( attacker, m, damageBonus );
                // Felix needs fix
                //m.SendMessage( "Your body has been struck by lightning." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int damage = (int)(runelevel*(m_Caster.Skills[SkillName.Inscribe].Value / 25) * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(m_Caster, damage);

				SpellHelper.Damage( TimeSpan.Zero, m, attacker, damage, 0, 0, 0, 0, 100 );
				m.BoltEffect( 0 );
			}

			public InternalHammer( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				writer.Write( m_Caster );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();
				Rune = reader.ReadItem() as SpellRune;
				m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}
		[FlipableAttribute( 0x13FF, 0x13FE )]
		private class InternalKatana : BaseSword
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;
			private RuneSpell m_Spell;

			public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
			public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }

			public override int AosStrengthReq{ get{ return 25; } }
			public override int AosMinDamage{ get{ return 11; } }
			public override int AosMaxDamage{ get{ return 13; } }
			public override int AosSpeed{ get{ return 46; } }

			public override int OldStrengthReq{ get{ return 10; } }
			public override int OldMinDamage{ get{ return 5; } }
			public override int OldMaxDamage{ get{ return 26; } }
			public override int OldSpeed{ get{ return 58; } }

			public override int DefHitSound{ get{ return 0x23B; } }
			public override int DefMissSound{ get{ return 0x23A; } }

			public override int InitMinHits{ get{ return 31; } }
			public override int InitMaxHits{ get{ return 90; } }

			[Constructable]
			public InternalKatana( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13FF  )
			{
				m_Spell = spell;
				m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.OneHanded;
				Hue = 0x481;
				BlessedFor = owner;
				Name = "Lightning Katana";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
			{
				phys = fire = cold = pois = chaos = direct = 0;
				nrgy = 100;
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The weapon blinks out." );
				m_Owner.EndAction( typeof( LightningWeapon ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void OnHit( Mobile attacker, IDamageable m, double damageBonus )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				base.OnHit( attacker, m, damageBonus );
                // Felix needs fix
                //m.SendMessage( "Your body has been struck by lightning." );

                int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int damage = (int)(runelevel*(m_Caster.Skills[SkillName.Inscribe].Value / 25) * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(m_Caster, damage);

				SpellHelper.Damage( TimeSpan.Zero, m, attacker, damage, 0, 0, 0, 0, 100 );
				m.BoltEffect( 0 );
			}

			public InternalKatana( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				writer.Write( m_Caster );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();
				Rune = reader.ReadItem() as SpellRune;
				m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}
		[FlipableAttribute( 0x1401, 0x1400 )]
		private class InternalKryss : BaseSword
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;
			private RuneSpell m_Spell;

			public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
			public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }

			public override int AosStrengthReq{ get{ return 10; } }
			public override int AosMinDamage{ get{ return 10; } }
			public override int AosMaxDamage{ get{ return 12; } }
			public override int AosSpeed{ get{ return 53; } }

			public override int OldStrengthReq{ get{ return 10; } }
			public override int OldMinDamage{ get{ return 3; } }
			public override int OldMaxDamage{ get{ return 28; } }
			public override int OldSpeed{ get{ return 53; } }

			public override int DefHitSound{ get{ return 0x23C; } }
			public override int DefMissSound{ get{ return 0x238; } }

			public override int InitMinHits{ get{ return 31; } }
			public override int InitMaxHits{ get{ return 90; } }

			public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
			public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
			public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

			[Constructable]
			public InternalKryss( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x1401  )
			{
				m_Spell = spell;
				m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.OneHanded;
				Hue = 0x481;
				BlessedFor = owner;
				Name = "Lightning Kryss";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
			{
				phys = fire = cold = pois = chaos = direct = 0;
				nrgy = 100;
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The weapon blinks out." );
				m_Owner.EndAction( typeof( LightningWeapon ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void OnHit( Mobile attacker, IDamageable m, double damageBonus )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				base.OnHit( attacker, m, damageBonus );
                // Felix needs fix
                //m.SendMessage( "Your body has been struck by lightning." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int damage = (int)(runelevel*(m_Caster.Skills[SkillName.Inscribe].Value / 25) * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(m_Caster, damage);

				SpellHelper.Damage( TimeSpan.Zero, m, attacker, damage, 0, 0, 0, 0, 100 );
				m.BoltEffect( 0 );
			}

			public InternalKryss( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				writer.Write( m_Caster );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();
				Rune = reader.ReadItem() as SpellRune;
				m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}
		[FlipableAttribute( 0x13B2, 0x13B1 )]
		private class InternalBow : BaseRanged
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;
			private SpellRune Rune;
			private Mobile m_Caster;
			private RuneSpell m_Spell;

			public override int EffectID{ get{ return 0xF42; } }
			public override Type AmmoType{ get{ return typeof( Arrow ); } }
			public override Item Ammo{ get{ return new Arrow(); } }

			public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
			public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }

			public override int AosStrengthReq{ get{ return 30; } }
			public override int AosMinDamage{ get{ return 16; } }
			public override int AosMaxDamage{ get{ return 18; } }
			public override int AosSpeed{ get{ return 25; } }

			public override int OldStrengthReq{ get{ return 20; } }
			public override int OldMinDamage{ get{ return 9; } }
			public override int OldMaxDamage{ get{ return 41; } }
			public override int OldSpeed{ get{ return 20; } }

			public override int DefMaxRange{ get{ return 10; } }

			public override int InitMinHits{ get{ return 31; } }
			public override int InitMaxHits{ get{ return 60; } }

			public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

			[Constructable]
			public InternalBow( RuneSpell spell, Mobile caster, SpellRune rune, Mobile owner, TimeSpan duration ) : base( 0x13B2  )
			{
				m_Spell = spell;
				m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.TwoHanded;
				Hue = 0x481;
				BlessedFor = owner;
				Name = "Lightning Bow";

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
			{
				phys = fire = cold = pois = chaos = direct = 0;
				nrgy = 100;
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The weapon blinks out." );
				m_Owner.EndAction( typeof( LightningWeapon ) );

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public override void OnHit( Mobile attacker, IDamageable m, double damageBonus )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				base.OnHit( attacker, m, damageBonus );
                // Felix needs fix
				//m.SendMessage( "Your body has been struck by lightning." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int damage = (int)(runelevel*(m_Caster.Skills[SkillName.Inscribe].Value / 25) * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(m_Caster, damage);

				SpellHelper.Damage( TimeSpan.Zero, m, attacker, damage, 0, 0, 0, 0, 100 );
				m.BoltEffect( 0 );
			}

			public InternalBow( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				writer.Write( m_Caster );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();
				Rune = reader.ReadItem() as SpellRune;
				m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}
		private class InternalTimer : Timer
		{
			private BaseWeapon m_Wep;
			private DateTime m_Expire;

			public InternalTimer( BaseWeapon wep, DateTime expire ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Wep = wep;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					m_Wep.Delete();
					Stop();
				}
			}
		}

		private class InternalTarget : Target
		{
			private LightningWeapon m_Owner;

			public InternalTarget( LightningWeapon owner ) : base( 12, true, TargetFlags.None )
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
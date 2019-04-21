using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.ContextMenus;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "an undead leagion corpse" )]
	public class UndeadLegion : BaseCreature
	{
		public override double DispelDifficulty{ get{ return 185.0; } }
		public override double DispelFocus{ get{ return 95.0; } }

		private Mobile Caster;
		private Mobile Source;
		private SpellRune Rune;

		[Constructable]
		public UndeadLegion ( RuneSpell m_Spell, Mobile caster, Mobile source, SpellRune rune ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Undead Legion";
			Body = Utility.RandomList( 57, 147 );
			BaseSoundID = 451;

			Caster = caster;
			Source = source;
			Rune = rune;

			int i1 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/3)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/2)));
			int i2 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/10)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/8)));
			int i3 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/9)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/6)));
			int i4 = Utility.RandomMinMax( (int)(Rune.Level+(Caster.Skills[SkillName.Inscribe].Value/20)), (int)(Rune.Level+(Caster.Skills[SkillName.Inscribe].Value/15)));
			int i5 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/30)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/25)));
			int i6 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/15)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/10)));
			int i7 = (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/20));
			int i8 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/32)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/26)));
			int i9 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/19)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/16)));
			int i10 = (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/23));
			double i11 = (Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/12));
			double i12 = (Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/8));
			double i13 = (Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/9));
			int i14 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/15)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/12)) );

			SetStr( i1+RSCA.CalculateSI(Caster, i1) );
			SetDex( i2+RSCA.CalculateSI(Caster, i2) );
			SetInt( i3+RSCA.CalculateSI(Caster, i3) );

			SetDamage( i4+RSCA.CalculateSI(Caster, i4) );

			SetDamageType( ResistanceType.Physical, i5+RSCA.CalculateSI(Caster, i5) );
			SetDamageType( ResistanceType.Fire, i5+RSCA.CalculateSI(Caster, i5) );
			SetDamageType( ResistanceType.Cold, i6+RSCA.CalculateSI(Caster, i6) );
			SetDamageType( ResistanceType.Poison, i6+RSCA.CalculateSI(Caster, i6) );
			SetDamageType( ResistanceType.Energy, i7+RSCA.CalculateSI(Caster, i7) );

			SetResistance( ResistanceType.Physical, i8+RSCA.CalculateSI(Caster, i8) );
			SetResistance( ResistanceType.Fire, i8+RSCA.CalculateSI(Caster, i8) );
			SetResistance( ResistanceType.Cold, i9+RSCA.CalculateSI(Caster, i9) );
			SetResistance( ResistanceType.Poison, i9+RSCA.CalculateSI(Caster, i9) );
			SetResistance( ResistanceType.Energy, i10+RSCA.CalculateSI(Caster, i10) );

			SetSkill( SkillName.MagicResist, i11+(double)(RSCA.CalculateSI(Caster, (int)i11)) );
			SetSkill( SkillName.Tactics, i12+(double)(RSCA.CalculateSI(Caster, (int)i12)) );
			SetSkill( SkillName.Wrestling, i13+(double)(RSCA.CalculateSI(Caster, (int)i13)) );
			SetSkill( SkillName.Anatomy, i11+(double)(RSCA.CalculateSI(Caster, (int)i11)) );

			VirtualArmor = i14+RSCA.CalculateSI(Caster, i14);
			ControlSlots = 0;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } } // TODO: Immune to poison?
		public override bool Commandable{ get{ return false; } }
		public override bool BardImmune{ get{ return true; } }

		private bool m_LastHidden;

		public override void OnThink()
		{
			base.OnThink();

			Mobile master = Source;

			if ( master == null )
				master = ControlMaster;
			if ( master == null )
				master = SummonMaster;

			if ( master == null )
				return;

			if ( master.Deleted )
			{
				DropPackContents();
				EndRelease( null );
				return;
			}

			if ( m_LastHidden != master.Hidden )
				Hidden = m_LastHidden = master.Hidden;

			IDamageable toAttack = null;

			if ( !Hidden )
			{
				toAttack = master.Combatant;

				if ( toAttack == this )
					toAttack = master;
				else if ( toAttack == null )
					toAttack = this.Combatant;
			}

			if ( Combatant != toAttack )
				Combatant = null;

			if ( toAttack == null )
			{
				if ( !InRange( master.Location, 30 ) || Map != master.Map )
				{
					Location = master.Location;
					Map = master.Map;
				}
				if ( ControlTarget != master || ControlOrder != OrderType.Follow )
				{
					ControlTarget = master;
					ControlOrder = OrderType.Follow;
				}
			}
			else if ( ControlTarget != toAttack || ControlOrder != OrderType.Attack )
			{
				ControlTarget = toAttack;
				ControlOrder = OrderType.Attack;
			}
		}
		public override void OnDelete()
		{
			try
			{
				if ( Source != null )
				{
					Source.SendMessage( "The undead leagion drifts back to the underworld." );
					bool lastone = true;
					foreach( Mobile m in World.Mobiles.Values )
					{
						if ( m is UndeadLegion )
						{
							UndeadLegion ul = (UndeadLegion)m;
							if ( ul != this && ( ul.Source == Source || ul.ControlMaster == Source || ul.SummonMaster == Source ) )
								lastone = false;
						}
					}
					if ( lastone )
					{
						Source.EndAction( typeof( UndeadLegionSpell ) );
					}
				}

				base.OnDelete();
			}
			catch ( Exception e )
			{
				Console.WriteLine("Undead Legion Endeffect Crash Prevented: "+e);
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive && Controlled && from == ControlMaster && from.InRange( this, 14 ) )
				list.Add( new ReleaseEntry( from, this ) );
		}

		public virtual void BeginRelease( Mobile from )
		{
			if ( !Deleted && Controlled && from == ControlMaster && from.CheckAlive() )
				EndRelease( from );
		}

		public virtual void EndRelease( Mobile from )
		{
			if ( from == null || (!Deleted && Controlled && from == ControlMaster && from.CheckAlive()) )
			{
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 2100, 3, 5042, 0 );
				PlaySound( 0x201 );
				Delete();
			}
		}

		public virtual void DropPackContents()
		{
			Map map = this.Map;
			Container pack = this.Backpack;

			if ( map != null && map != Map.Internal && pack != null )
			{
				ArrayList list = new ArrayList( pack.Items );

				for ( int i = 0; i < list.Count; ++i )
					((Item)list[i]).MoveToWorld( Location, map );
			}
		}

		public UndeadLegion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( Caster );
			writer.Write( Source );
			writer.Write( Rune );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			Caster = reader.ReadMobile();
			Source = reader.ReadMobile();
			Rune = reader.ReadItem() as SpellRune;
		}

		private class ReleaseEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private UndeadLegion m_UL;

			public ReleaseEntry( Mobile from, UndeadLegion ul ) : base( 6118, 14 )
			{
				m_From = from;
				m_UL = ul;
			}

			public override void OnClick()
			{
				if ( !m_UL.Deleted && m_UL.Controlled && m_From == m_UL.ControlMaster && m_From.CheckAlive() )
					m_UL.BeginRelease( m_From );
			}
		}
	}
}
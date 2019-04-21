using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.ContextMenus;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a no faced corpse" )]
	public class NoFace : BaseCreature
	{
		public override double DispelDifficulty{ get{ return 185.0; } }
		public override double DispelFocus{ get{ return 95.0; } }

		private Mobile Caster;
		private Mobile Source;
		private SpellRune Rune;

		[Constructable]
		public NoFace ( RuneSpell m_Spell, Mobile caster, Mobile source, SpellRune rune ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "No Face";
			Body = 401;
			Hue = 0;

			Caster = caster;
			Source = source;
			Rune = rune;

			int i1 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/2.5)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/1.5)));
			int i2 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/9)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/7)));
			int i3 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/8)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/5)));
			int i4 = Utility.RandomMinMax( (int)(Rune.Level+(Caster.Skills[SkillName.Inscribe].Value/18)), (int)(Rune.Level+(Caster.Skills[SkillName.Inscribe].Value/13)));
			int i5 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/15)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/10)));
			int i6 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/19)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/16)));
			double i7 = (Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/10));
			double i8 = (Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/7));
			double i9 = (Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/8));
			int i10 = Utility.RandomMinMax( (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/10)), (int)(Rune.Level*(Caster.Skills[SkillName.Inscribe].Value/8)) );

			SetStr( i1+RSCA.CalculateSI(Caster, i1) );
			SetDex( i2+RSCA.CalculateSI(Caster, i2) );
			SetInt( i3+RSCA.CalculateSI(Caster, i3) );

			SetDamage( i4+RSCA.CalculateSI(Caster, i4) );

			SetDamageType( ResistanceType.Physical, i5+RSCA.CalculateSI(Caster, i5) );
			SetDamageType( ResistanceType.Fire, i5+RSCA.CalculateSI(Caster, i5) );
			SetDamageType( ResistanceType.Cold, i5+RSCA.CalculateSI(Caster, i5) );
			SetDamageType( ResistanceType.Poison, i5+RSCA.CalculateSI(Caster, i5) );
			SetDamageType( ResistanceType.Energy, i5+RSCA.CalculateSI(Caster, i5) );

			SetResistance( ResistanceType.Physical, i6+RSCA.CalculateSI(Caster, i6) );
			SetResistance( ResistanceType.Fire, i6+RSCA.CalculateSI(Caster, i6) );
			SetResistance( ResistanceType.Cold, i6+RSCA.CalculateSI(Caster, i6) );
			SetResistance( ResistanceType.Poison, i6+RSCA.CalculateSI(Caster, i6) );
			SetResistance( ResistanceType.Energy, i6+RSCA.CalculateSI(Caster, i6) );

			SetSkill( SkillName.MagicResist, i7+(double)(RSCA.CalculateSI(Caster, (int)i7)) );
			SetSkill( SkillName.Tactics, i8+(double)(RSCA.CalculateSI(Caster, (int)i8)) );
			SetSkill( SkillName.Wrestling, i9+(double)(RSCA.CalculateSI(Caster, (int)i9)) );
			SetSkill( SkillName.Anatomy, i7+(double)(RSCA.CalculateSI(Caster, (int)i7)) );

			VirtualArmor = i10+RSCA.CalculateSI(Caster, i10);
			ControlSlots = 0;

			Item shroud = new HoodedShroudOfShadows();
			shroud.Movable = false;
			shroud.Hue = 1;
			AddItem( shroud );
			Item gloves = new LeatherGloves();
			gloves.Movable = false;
			gloves.Hue = 1;
			AddItem( gloves );
			Item boots = new Boots();
			boots.Movable = false;
			boots.Hue = 1;
			AddItem( boots );
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
					Source.SendMessage( "The no face fades away." );
					bool lastone = true;
					foreach( Mobile m in World.Mobiles.Values )
					{
						if ( m is NoFace )
						{
							NoFace nf = (NoFace)m;
							if ( nf != this && ( nf.Source == Source || nf.ControlMaster == Source || nf.SummonMaster == Source ) )
								lastone = false;
						}
					}
					if ( lastone )
						Source.EndAction( typeof( NoFaceSpell ) );
				}
				base.OnDelete();
			}
			catch ( Exception e )
			{
				Console.WriteLine("No Face Endeffect Crash Prevented: "+e);
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

		public NoFace( Serial serial ) : base( serial )
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
			private NoFace m_NF;

			public ReleaseEntry( Mobile from, NoFace nf ) : base( 6118, 14 )
			{
				m_From = from;
				m_NF = nf;
			}

			public override void OnClick()
			{
				if ( !m_NF.Deleted && m_NF.Controlled && m_From == m_NF.ControlMaster && m_From.CheckAlive() )
					m_NF.BeginRelease( m_From );
			}
		}
	}
}

using System; 
using System.Collections; 
using System.Collections.Generic; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 
using Server.Spells.Necromancy;
using Server.Spells.Chivalry;
using Server.ACC.CSS.Systems.Cleric;
using Server.Misc;

namespace Server.Items 
{ 
   	public class ImperialOrb: Item 
   	{ 
		private Gems m_Gems;
		private Mobile m_Equiped;

		[CommandProperty( AccessLevel.GameMaster )]
		public Gems Gems{ get{ if ( m_Gems == null )return m_Gems = new Gems( this );return m_Gems; } set{} }

		private SkillMod m_SkillModM;
		private SkillMod m_SkillModE;
		private SkillMod m_SkillModI;
		private StatMod m_IntMod;
		public Mobile Equiped
		{
			get{ return m_Equiped; }
			set
			{
				if ( m_Equiped != null )
					m_Equiped.RemoveStatMod( "INTIO" );
				m_Equiped = value;

				if ( m_SkillModM != null )
					m_SkillModM.Remove();
				if ( m_SkillModE != null )
					m_SkillModE.Remove();
				if ( m_SkillModI != null )
					m_SkillModI.Remove();
				if ( m_Equiped != null )
				{
					Movable = false;

					m_SkillModM = new DefaultSkillMod( SkillName.Magery, true, 30 );
					m_Equiped.AddSkillMod( m_SkillModM );

					m_SkillModE = new DefaultSkillMod( SkillName.EvalInt, true, 30 );
					m_Equiped.AddSkillMod( m_SkillModE );

					m_SkillModI = new DefaultSkillMod( SkillName.Inscribe, true, 30 );
					m_Equiped.AddSkillMod( m_SkillModI );

					m_IntMod = new StatMod( StatType.Int, "INTIO", 40, TimeSpan.Zero );
					m_Equiped.AddStatMod( m_IntMod );
				}
				else
					Movable = true;

				InvalidateProperties();
			}
		}

		[Constructable]
		public ImperialOrb() : base( 3699 )
		{
			Weight = 0.1;
			Name = "Imperial Orb";
			Hue = 1153;
			LootType = LootType.Blessed;

			new InternalTimer( this ).Start();
		}

		public static ImperialOrb FindIO( Container c )
		{
			List<Item> list = c.Items;

			ImperialOrb io = null;

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = (Item)list[i];

				if ( item is ImperialOrb && io == null )
				{
					ImperialOrb orb = (ImperialOrb)item;

					if ( orb.Equiped != null )
						io = orb;
				}
			}
			if ( io == null )
			{
				for ( int i = 0; i < list.Count; ++i )
				{
					Item item = (Item)list[i];

					if ( item is Container && io == null )
						io = FindIO((Container)item);
				}
			}

			return io;
		}

		public static int IncI( Mobile caster, Spell spell )
		{
			if ( caster == null || spell == null || caster.Backpack == null )
				return 0;

			ImperialOrb io = FindIO( caster.Backpack );

			if ( io == null || io.Equiped == null )
				return 0;

			int inscribe = (int)caster.Skills[SkillName.Inscribe].Value;

			int magery = (int)caster.Skills[SkillName.Magery].Value;
			int evalint = (int)caster.Skills[SkillName.EvalInt].Value;

			int necro = (int)caster.Skills[SkillName.Necromancy].Value;
			int ss = (int)caster.Skills[SkillName.SpiritSpeak].Value;

			int chivalry = (int)caster.Skills[SkillName.Chivalry].Value;
			int karma = caster.Karma;

			int powerdrain = 0;
			int tdb = 1;

			SpellCircle circle = (spell as MagerySpell).Circle;
			if ( circle == SpellCircle.First )
				tdb = 12;
			else if ( circle == SpellCircle.Second )
				tdb = 11;
			else if ( circle == SpellCircle.Third )
				tdb = 10;
			else if ( circle == SpellCircle.Fourth )
				tdb = 9;
			else if ( circle == SpellCircle.Fifth )
				tdb = 8;
			else if ( circle == SpellCircle.Sixth )
				tdb = 7;
			else if ( circle == SpellCircle.Seventh )
				tdb = 6;
			else if ( circle == SpellCircle.Eighth )
				tdb = 5;

			if ( spell is NecromancerSpell )
				powerdrain = (int)((necro+ss)/tdb);
			else if ( spell is PaladinSpell )
				powerdrain = (int)((chivalry+(karma/100))/tdb);
			else if ( spell is RuneSpell )
				powerdrain = (int)((necro+ss)/tdb);
			else if ( spell is ClericSpell )
				powerdrain = (int)((magery+ss)/tdb);
			else
				powerdrain = (int)((magery+evalint)/tdb);

			int inc = 0;

			if ( powerdrain <= io.Gems.GetPower() )
			{
				inc = (int)((io.Gems.GetPower()/10)/tdb);

				io.Gems.ConsumePower( powerdrain );
				caster.PlaySound( 0x201 );
			}

			return inc;
		}
		public static double IncD( Mobile caster, Spell spell )
		{
			if ( caster == null || spell == null || caster.Backpack == null )
				return 0.0;

			ImperialOrb io = FindIO( caster.Backpack );

			if ( io == null || io.Equiped == null )
				return 0.0;

			int inscribe = (int)caster.Skills[SkillName.Inscribe].Value;

			int magery = (int)caster.Skills[SkillName.Magery].Value;
			int evalint = (int)caster.Skills[SkillName.EvalInt].Value;

			int necro = (int)caster.Skills[SkillName.Necromancy].Value;
			int ss = (int)caster.Skills[SkillName.SpiritSpeak].Value;

			int chivalry = (int)caster.Skills[SkillName.Chivalry].Value;
			int karma = caster.Karma;

			int powerdrain = 0;
			int tdb = 1;

			SpellCircle circle = (spell as MagerySpell).Circle;
			if ( circle == SpellCircle.First )
				tdb = 12;
			else if ( circle == SpellCircle.Second )
				tdb = 11;
			else if ( circle == SpellCircle.Third )
				tdb = 10;
			else if ( circle == SpellCircle.Fourth )
				tdb = 9;
			else if ( circle == SpellCircle.Fifth )
				tdb = 8;
			else if ( circle == SpellCircle.Sixth )
				tdb = 7;
			else if ( circle == SpellCircle.Seventh )
				tdb = 6;
			else if ( circle == SpellCircle.Eighth )
				tdb = 5;

			if ( spell is NecromancerSpell )
				powerdrain = (int)((necro+ss)/tdb);
			else if ( spell is PaladinSpell )
				powerdrain = (int)((chivalry+(karma/100))/tdb);
			else if ( spell is RuneSpell )
				powerdrain = (int)((necro+ss)/tdb);
			else if ( spell is ClericSpell )
				powerdrain = (int)((magery+ss)/tdb);
			else
				powerdrain = (int)((magery+evalint)/tdb);

			double inc = 0;

			if ( powerdrain <= io.Gems.GetPower() )
			{
				inc = (io.Gems.GetPower()/10)/tdb;

				io.Gems.ConsumePower( powerdrain );
				caster.PlaySound( 0x201 );
			}

			return inc;
		}

		public override void OnAfterDelete()
		{
			if ( Equiped != null )
				m_Equiped.RemoveStatMod( "INTIO" );
			if ( m_SkillModM != null )
				m_SkillModM.Remove();
			if ( m_SkillModE != null )
				m_SkillModE.Remove();
			if ( m_SkillModI != null )
				m_SkillModI.Remove();
			Equiped = null;

			base.OnAfterDelete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from ) && Equiped == null )
			{
				Equiped = from;
				from.SendMessage( "The Imperial Orb is now active." );
			}
			else if ( IsChildOf( from ) )
			{
				Equiped = null;
				from.SendMessage( "The Imperial Orb is now inactive." );
			}
			else
				from.SendMessage( "The Imperial Orb must be in your backpack to use it." );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Equiped != null )
				list.Add( 1060742 ); // active
			else
				list.Add( 1060743 ); // inactive

			list.Add( 1060658, "Power\t{0}", Gems.GetPower() );
		}

            	public ImperialOrb( Serial serial ) : base ( serial ) 
            	{             
           	} 

		public override void Serialize( GenericWriter writer ) 
           	{ 
              		base.Serialize( writer ); 
              		writer.Write( (int) 0 ); 
              		m_Gems.Serialize( writer );
              		writer.Write( m_Equiped ); 
           	} 

           	public override void Deserialize( GenericReader reader ) 
           	{ 
              		base.Deserialize( reader ); 
              		int version = reader.ReadInt(); 
			m_Gems = new Gems();
              		m_Gems.Deserialize( reader );
              		m_Equiped = reader.ReadMobile(); 

			new InternalTimer( this ).Start();
           	} 
		private class InternalTimer : Timer
		{
			private ImperialOrb m_IO;

			private int cnt = 0;
			private int basehue = 0;
			private int shade = 0;

			private int[] hues = new int[5]{ 2, 12, 37, 52, 72 };

			public InternalTimer( ImperialOrb io ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_IO = io;
			}

			protected override void OnTick()
			{
				if ( m_IO == null || m_IO.Deleted )
					Stop();

				if ( m_IO.Equiped == null )
					return;

				if ( !m_IO.Equiped.CheckAlive() )
					m_IO.Equiped = null;

				bool on = false;
				foreach ( Network.NetState state in Network.NetState.Instances )
				{
					if ( state.Mobile == null )
						continue;

					Mobile owner = (Mobile)state.Mobile;

					if ( owner == m_IO.Equiped )
						on = true;
				}

				if ( !on )
					return;

				bool hashue = false;

				for( int i = 0; i < hues.Length; ++i )
					if ( basehue == (int)hues[i] )
						hashue = true;

				if ( !hashue )
					basehue = hues[Utility.Random( hues.Length )];

				shade += 1;

				if ( shade > 4 )
				{
					basehue = hues[Utility.Random( hues.Length )];
					shade = 0;
				}

				int finalhue = basehue + shade;

				if ( m_IO.Equiped.Warmode )
					finalhue = 37;

				cnt++;
				if ( cnt == 1 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y-1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 2 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X+1, m_IO.Equiped.Y-1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 3 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X+1, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 4 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 5 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X-1, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 6 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X-1, m_IO.Equiped.Y-1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 7 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y-1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 8 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 9 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y+1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 10 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X-1, m_IO.Equiped.Y+1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 11 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 12 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X+1, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 13 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X+1, m_IO.Equiped.Y+1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 14 )
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y+1, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
				if ( cnt == 15 )
				{
					Effects.SendLocationEffect( new Point3D( m_IO.Equiped.X, m_IO.Equiped.Y, m_IO.Equiped.Z+16 ), m_IO.Map, 3699, 3, finalhue, 0 );
					cnt = 0;
				}
			}
		}
        } 
} 
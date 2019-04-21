using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Spells
{
	public class HeatWave : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Heat Wave", "Phoe Trist",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(HeatWave) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public override int RuneGainMax{ get{ return 80; } }
		public override int RuneGainMin{ get{ return 40; } }

		public HeatWave( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( CheckSequence() )
			{
				GainExp();

				Point3D loc = new Point3D( Caster.X, Caster.Y, Caster.Z );
				new InternalMItem( this, Rune, loc, Caster, Caster.Map, 1, true );
			}
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private SpellRune Rune;
			private RuneSpell m_Spell;

			public InternalItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map ) : base( Utility.RandomMinMax( 14069, 14077 ) )
			{
				m_Spell = spell;
				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				if ( map != null )
				{
					MoveToWorld( loc, map );
					int newZ = Z;
					int tz = map.GetAverageZ( X, Y );
					if ( tz > newZ )
						newZ = tz;
					for ( int x = 0; x <= 0; ++x )
					{
						for ( int y = 0; y <= 0; ++y )
						{
							StaticTile[] tiles = map.Tiles.GetStaticTiles( X+x, Y+y, true );

							for ( int i = 0; i < tiles.Length; ++i )
							{
								if ( tiles[i].Z < newZ+16 && tiles[i].Z > newZ )
									newZ = tiles[i].Z;
							}
						}
					}
					IPooledEnumerable eable = map.GetItemsInRange( Location, 0 );
					foreach ( Item item in eable )
					{
						if ( item.Z < newZ+16 && item.Z > newZ && (item.Map == this.Map)) 
							newZ = item.Z;
					}
					eable.Free();

					MoveToWorld( new Point3D( X, Y, newZ ), map );
				}

				m_Caster = caster;

				m_End = DateTime.Now + TimeSpan.FromSeconds( 1.0 );

				m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.FromSeconds( 0.4 ), caster.InLOS( this ), canFit );
				m_Timer.Start();

				ArrayList list = new ArrayList();
				foreach ( Mobile m in World.Mobiles.Values )
				{
					if ( (m.Location == this.Location && (m.Map == this.Map)) || (((m.X == X && m.Y == Y) && (m.Z >= Z && m.Z < Z+16)) &&(m.Map == this.Map) ))
					{
					
					
					
						if ( m.Player && m is PlayerMobile )
						{
							Account a = (Account)m.Account;
							bool online = false;
							for ( int j = 0; j < 5; ++j )
							{
								try {
								Mobile check = a[j];
							
								if ( check == null )
									continue;
								if ( check.NetState != null )
									online = true;
										}
								catch {
						Console.WriteLine("Null Crash avoided");
								}
							}
							if ( online )
								list.Add( m );
						}
						else
							list.Add( m );
					}
				}
				foreach ( Mobile m in list )
				{
				try
				{
				if ( !(m == null) || !(m.Blessed == true))
				{
				DoAttack( m );
				
				}
				}
				catch{
					Console.WriteLine("Null Crash avoided " + m.Name);
				}
				}
			
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_Caster );
				writer.Write( Rune );
				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_Caster = reader.ReadMobile();
						Rune = reader.ReadItem() as SpellRune;

						goto case 0;
					}
					case 0:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.Zero, true, true );
						m_Timer.Start();

						break;
					}
				}
			}

			public virtual void DoAttack( Mobile m )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				if ( m_Caster != m && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
				{
					//if ( Spellbreaker.BreakSpell( m, m_Caster, m_Spell ) )
					//	return;

					m_Caster.DoHarmful( m );

					int runelevel = Rune.Level;
					if ( runelevel <= 0 )
						runelevel = 1;

					int damage = (int)((runelevel*m_Caster.Skills[SkillName.Inscribe].Value)/17 * RuneSpell.TMRG);

					if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 30.0 ) )
					{
						damage = (int)(damage/2);

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					if ( damage <= 0 )
						damage = 1;

					damage += RSCA.CalculateSI(m_Caster, damage);

					damage += ImperialOrb.IncI( m_Caster, m_Spell );

					AOS.Damage( m, m_Caster, damage, 0, 100, 0, 0, 0 );
					m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
					m.PlaySound( 0x208 );
				}
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private RuneSpell m_Spell;
				private InternalItem m_Item;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( RuneSpell spell, SpellRune rune, InternalItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Spell = spell;
					m_Item = item;
					m_InLOS = inLOS;
					m_CanFit = canFit;
					m_Rune = rune;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Rune == null || m_Item == null )
					{
						Stop();
					}
					if ( m_Item.Deleted )
						return;

					if ( DateTime.Now > m_Item.m_End )
					{
						//Effects.SendLocationParticles( EffectItem.Create( m_Item.Location, m_Item.Map, EffectItem.DefaultDuration ), 14089, 1, 13, 0, 0, 0, 0 );
						m_Item.Delete();
						Stop();
					}
				}
			}
		}

		[DispellableField]
		private class InternalMItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private SpellRune Rune;
			private int Count;
			private bool R1;
			private Point3D CastLoc;
			private RuneSpell m_Spell;
//Crashed? Check here!
			public InternalMItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, int count, bool r1 ) : base( (r1 ? Utility.RandomMinMax( 14069, 14077 ) : 0x2198) )
			{
				m_Spell = spell;
				R1 = r1;
				CastLoc = loc;
				Count = count;
				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				if ( map != null )
				{
					MoveToWorld( loc, map );
					int newZ = Z;
					int tz = map.GetAverageZ( X, Y );
					if ( tz > newZ )
						newZ = tz;
					for ( int x = 0; x <= 0; ++x )
					{
						for ( int y = 0; y <= 0; ++y )
						{
							StaticTile[] tiles = map.Tiles.GetStaticTiles( X+x, Y+y, true );

							for ( int i = 0; i < tiles.Length; ++i )
							{
								if ( tiles[i].Z < newZ+16 && tiles[i].Z > newZ )
									newZ = tiles[i].Z;
							}
						}
					}
					IPooledEnumerable eable = map.GetItemsInRange( Location, 0 );
					foreach ( Item item in eable )
					{
						if ( item.Z < newZ+16 && item.Z > newZ && (item.Map == this.Map) )
							newZ = item.Z;
					}
					eable.Free();

					MoveToWorld( new Point3D( X, Y, newZ ), map );
				}

				m_Caster = caster;

				if ( r1 )
					m_End = DateTime.Now + TimeSpan.FromSeconds( 1.0 );
				else
					m_End = DateTime.Now + TimeSpan.FromSeconds( 0.2 );

				m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.FromSeconds( 0.1 ), caster.InLOS( this ), canFit );
				m_Timer.Start();

				ArrayList list = new ArrayList();
				foreach ( Mobile m in World.Mobiles.Values )
				{
					if ( (m.Location == this.Location && (m.Map == this.Map)) || (((m.X == X && m.Y == Y) && (m.Z >= Z && m.Z < Z+16)) &&(m.Map == this.Map)))
					{
						if ( m.Player && m is PlayerMobile )
						{
							Account a = (Account)m.Account;
							bool online = false;
							for ( int j = 0; j < 5; ++j )
							{
								Mobile check = a[j];
								if ( check == null )
									continue;
								if ( check.NetState != null )
									online = true;
							}
							if ( online )
								list.Add( m );
						}
						else
							list.Add( m );
					}
				}
				
				foreach ( Mobile m in list )
				{
				if (!(m == null))
					{
					try {
					DoAttack( m );
						}
						catch
						{
				Console.WriteLine("Null Crash Prevented");
						}
					}
				}
			
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public InternalMItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_Caster );
				writer.Write( Rune );
				writer.Write( (int)Count );
				writer.Write( (Point3D)CastLoc );
				writer.Write( (bool)R1 );
				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_Caster = reader.ReadMobile();
						Rune = reader.ReadItem() as SpellRune;
						Count = reader.ReadInt();
						CastLoc = reader.ReadPoint3D();
						R1 = reader.ReadBool();

						goto case 0;
					}
					case 0:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.Zero, true, true );
						m_Timer.Start();

						break;
					}
				}
			}

			public virtual void DoAttack( Mobile m )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				if ( R1 && m_Caster != m && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
				{
					//if ( Spellbreaker.BreakSpell( m, m_Caster, m_Spell ) )
					//	return;

					m_Caster.DoHarmful( m );

					int runelevel = Rune.Level;
					if ( runelevel <= 0 )
						runelevel = 1;

					int damage = (int)((runelevel*m_Caster.Skills[SkillName.Inscribe].Value)/17 * RuneSpell.TMRG);

					if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 30.0 ) )
					{
						damage = (int)(damage/2);

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					if ( damage <= 0 )
						damage = 1;

					damage += RSCA.CalculateSI(m_Caster, damage);

					damage += ImperialOrb.IncI( m_Caster, m_Spell );

					AOS.Damage( m, m_Caster, damage, 100, 0, 0, 0, 0 );
					m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
					m.PlaySound( 0x208 );
				}
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private InternalMItem m_Item;
				private bool m_InLOS, m_CanFit;
				private RuneSpell m_Spell;

				private static Queue m_Queue = new Queue();

				public InternalTimer( RuneSpell spell, SpellRune rune, InternalMItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 0.2 ) )
				{
					m_Spell = spell;
					m_Item = item;
					m_InLOS = inLOS;
					m_CanFit = canFit;
					m_Rune = rune;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Rune == null )
					{
						Stop();
					}
					if ( m_Item.Deleted )
						return;

					if ( DateTime.Now > m_Item.m_End )
					{
						//Effects.SendLocationParticles( EffectItem.Create( m_Item.Location, m_Item.Map, EffectItem.DefaultDuration ), 14089, 1, 13, 0, 0, 0, 0 );
						
						if ( m_Item.Count == 1 )
						{
							Point3D mloc = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							new InternalMItem( m_Spell, m_Item.Rune, mloc, m_Item.m_Caster, m_Item.m_Caster.Map, m_Item.Count+1, false );

							Point3D loc1 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc2 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc3 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc4 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc5 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc6 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc7 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc8 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );

							new InternalItem( m_Spell, m_Item.Rune, loc1, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc2, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc3, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc4, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc5, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc6, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc7, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc8, m_Item.m_Caster, m_Item.m_Caster.Map );
						//}
						
						//}
						}
						if ( m_Item.Count == 2 )
						{
							//try {
							Point3D mloc = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							new InternalMItem( m_Spell, m_Item.Rune, mloc, m_Item.m_Caster, m_Item.m_Caster.Map, m_Item.Count+1, false );

							Point3D loc1 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc2 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc3 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc4 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc5 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc6 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc7 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc8 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc9 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc10 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc11 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc12 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc13 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc14 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc15 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc16 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );

							new InternalItem( m_Spell, m_Item.Rune, loc1, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc2, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc3, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc4, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc5, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc6, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc7, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc8, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc9, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc10, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc11, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc12, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc13, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc14, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc15, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc16, m_Item.m_Caster, m_Item.m_Caster.Map );
						//}
						//catch
						//{
						//Console.WriteLine("Null Crash check");
						//}
						
						}
						if ( m_Item.Count == 3 )
						{
						//try {
							Point3D mloc = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							new InternalMItem( m_Spell, m_Item.Rune, mloc, m_Item.m_Caster, m_Item.m_Caster.Map, m_Item.Count+1, false );

							Point3D loc1 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc2 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc3 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc4 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc5 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc6 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc7 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc8 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc9 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc10 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc11 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc12 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc13 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc14 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc15 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc16 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc17 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc18 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc19 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc20 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc21 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc22 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc23 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc24 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );

							new InternalItem( m_Spell, m_Item.Rune, loc1, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc2, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc3, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc4, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc5, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc6, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc7, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc8, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc9, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc10, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc11, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc12, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc13, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc14, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc15, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc16, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc17, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc18, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc19, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc20, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc21, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc22, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc23, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc24, m_Item.m_Caster, m_Item.m_Caster.Map );
						//}
						//catch
					//{
					//	Console.WriteLine("Null Crash check");
					//	}
						
						}
						if ( m_Item.Count == 4 )
						{
					//	try{
							Point3D mloc = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							new InternalMItem( m_Spell, m_Item.Rune, mloc, m_Item.m_Caster, m_Item.m_Caster.Map, m_Item.Count+1, false );

							Point3D loc1 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc2 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc3 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y, m_Item.CastLoc.Z );
							Point3D loc4 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc5 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc6 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc7 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc8 = new Point3D( m_Item.CastLoc.X, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc9 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc10 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc11 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y-1, m_Item.CastLoc.Z );
							Point3D loc12 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y+1, m_Item.CastLoc.Z );
							Point3D loc13 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc14 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc15 = new Point3D( m_Item.CastLoc.X+1, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc16 = new Point3D( m_Item.CastLoc.X-1, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc17 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc18 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc19 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y-2, m_Item.CastLoc.Z );
							Point3D loc20 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y+2, m_Item.CastLoc.Z );
							Point3D loc21 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc22 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc23 = new Point3D( m_Item.CastLoc.X+2, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc24 = new Point3D( m_Item.CastLoc.X-2, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc25 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc26 = new Point3D( m_Item.CastLoc.X-4, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc27 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y-3, m_Item.CastLoc.Z );
							Point3D loc28 = new Point3D( m_Item.CastLoc.X+4, m_Item.CastLoc.Y+3, m_Item.CastLoc.Z );
							Point3D loc29 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc30 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y+4, m_Item.CastLoc.Z );
							Point3D loc31 = new Point3D( m_Item.CastLoc.X+3, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );
							Point3D loc32 = new Point3D( m_Item.CastLoc.X-3, m_Item.CastLoc.Y-4, m_Item.CastLoc.Z );

							new InternalItem( m_Spell, m_Item.Rune, loc1, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc2, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc3, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc4, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc5, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc6, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc7, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc8, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc9, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc10, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc11, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc12, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc13, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc14, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc15, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc16, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc17, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc18, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc19, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc20, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc21, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc22, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc23, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc24, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc25, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc26, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc27, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc28, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc29, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc30, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc31, m_Item.m_Caster, m_Item.m_Caster.Map );
							new InternalItem( m_Spell, m_Item.Rune, loc32, m_Item.m_Caster, m_Item.m_Caster.Map );
					//	}
						//catch
//{
					//	Console.WriteLine("Null Crash check");
					//	} 
						
						}

						m_Item.Delete();
						Stop();
					}
				}
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
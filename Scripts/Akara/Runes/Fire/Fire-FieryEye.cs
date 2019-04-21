using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class FieryEye : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Fiery Eye", "Phoe Vos",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(FieryEye) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public FieryEye( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			if ( Caster.CanBeginAction( typeof( FieryEye ) ) )
			{
				Caster.Target = new InternalTarget( this );
				Caster.SendMessage( "Target a location to cast this spell upon." );
			}
			else
				Caster.SendMessage( "You may only cast one of these at a time." );
		}

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				GainExp();
				SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				TimeSpan duration;

				if ( Core.AOS )
				{
					duration = TimeSpan.FromSeconds( ((15 + (Caster.Skills.Inscribe.Fixed / 5)) / 4 * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
					duration += TimeSpan.FromSeconds( RSCA.CalculateLLS(Caster, (15 + (Caster.Skills.Inscribe.Fixed / 5)) / 4) );
				}
				else
				{
					duration = TimeSpan.FromSeconds( (4.0 + (Caster.Skills[SkillName.Inscribe].Value * 0.5) * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
					duration += TimeSpan.FromSeconds( RSCA.CalculateLLS(Caster, 4.0 + (Caster.Skills[SkillName.Inscribe].Value * 0.5)) );
				}

				Point3D cloc = new Point3D( p.X, p.Y, p.Z );
				new InternalCenterItem( this, Rune, cloc, Caster, Caster.Map, duration );

				Caster.BeginAction( typeof( FieryEye ) );
				new InternalSTimer( Caster, duration ).Start();

				Point3D loc1 = new Point3D( p.X-2, p.Y-2, p.Z );
				Point3D loc2 = new Point3D( p.X-3, p.Y-1, p.Z );
				Point3D loc3 = new Point3D( p.X-3, p.Y, p.Z );
				Point3D loc4 = new Point3D( p.X-3, p.Y+1, p.Z );
				Point3D loc5 = new Point3D( p.X-2, p.Y+2, p.Z );
				Point3D loc6 = new Point3D( p.X-1, p.Y+3, p.Z );
				Point3D loc7 = new Point3D( p.X, p.Y+3, p.Z );
				Point3D loc8 = new Point3D( p.X+1, p.Y+3, p.Z );

				Point3D loc9 = new Point3D( p.X+2, p.Y+2, p.Z );
				Point3D loc10 = new Point3D( p.X+3, p.Y+1, p.Z );
				Point3D loc11 = new Point3D( p.X+3, p.Y, p.Z );
				Point3D loc12 = new Point3D( p.X+3, p.Y-1, p.Z );
				Point3D loc13 = new Point3D( p.X+2, p.Y-2, p.Z );
				Point3D loc14 = new Point3D( p.X+1, p.Y-3, p.Z );
				Point3D loc15 = new Point3D( p.X, p.Y-3, p.Z );
				Point3D loc16 = new Point3D( p.X-1, p.Y-3, p.Z );

				new InternalItem( this, Rune, loc1, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc2, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc3, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc4, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc5, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc6, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc7, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc8, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc9, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc10, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc11, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc12, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc13, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc14, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc15, Caster, Caster.Map, duration );
				new InternalItem( this, Rune, loc16, Caster, Caster.Map, duration );
			}

			FinishSequence();
		}
		private class InternalSTimer : Timer
		{
			private Mobile caster;

			private static Queue m_Queue = new Queue();

			public InternalSTimer( Mobile mcaster, TimeSpan delay ) : base( delay, TimeSpan.FromSeconds( 0.1 ) )
			{
				caster = mcaster;
			}

			protected override void OnTick()
			{
				if ( caster != null )
				{
					caster.EndAction( typeof( FieryEye ) );
					Stop();
				}
			}
		}

		[DispellableField]
		private class InternalCenterItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private SpellRune Rune;
			private RuneSpell m_Spell;

			public InternalCenterItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 14089 )
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
						if ( item.Z < newZ+16 && item.Z > newZ )
							newZ = item.Z;
					}
					eable.Free();

					MoveToWorld( new Point3D( X, Y, newZ ), map );
				}

				m_Caster = caster;
				Light = LightType.Circle150;

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.FromSeconds( 0.4 ), caster.InLOS( this ), canFit );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public InternalCenterItem( Serial serial ) : base( serial )
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

			public override void OnMovement( Mobile m, Point3D oldLocation )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}

				Point3D loc1 = new Point3D( X-2, Y, Z );
				Point3D loc2 = new Point3D( X-2, Y+1, Z );
				Point3D loc3 = new Point3D( X-2, Y-1, Z );
				Point3D loc4 = new Point3D( X, Y-2, Z );
				Point3D loc5 = new Point3D( X+1, Y-2, Z );
				Point3D loc6 = new Point3D( X-1, Y-2, Z );
				Point3D loc7 = new Point3D( X+2, Y, Z );
				Point3D loc8 = new Point3D( X+2, Y+1, Z );
				Point3D loc9 = new Point3D( X+2, Y-1, Z );
				Point3D loc10 = new Point3D( X, Y+2, Z );
				Point3D loc11 = new Point3D( X+1, Y+2, Z );
				Point3D loc12 = new Point3D( X-1, Y+2, Z );

				bool atloc = false;

				if ( m.Location == loc1 || m.Location == loc2 || m.Location == loc3 || m.Location == loc4 ||
					m.Location == loc5 || m.Location == loc6 || m.Location == loc7 || m.Location == loc8 ||
					m.Location == loc9 || m.Location == loc10 || m.Location == loc11 || m.Location == loc12
				)
					atloc = true;
				else if ( ((m.X == loc1.X && m.Y == loc1.Y) || (m.X == loc2.X && m.Y == loc2.Y) || (m.X == loc3.X && m.Y == loc3.Y) ||
					(m.X == loc4.X && m.Y == loc4.Y) || (m.X == loc5.X && m.Y == loc5.Y) || (m.X == loc6.X && m.Y == loc6.Y) ||
					(m.X == loc7.X && m.Y == loc7.Y) || (m.X == loc8.X && m.Y == loc8.Y) || (m.X == loc9.X && m.Y == loc9.Y) ||
					(m.X == loc10.X && m.Y == loc10.Y) || (m.X == loc11.X && m.Y == loc11.Y) || (m.X == loc12.X && m.Y == loc12.Y)) && (m.Z >= Z && m.Z < Z+16)
				)
					atloc = true;

				if ( Utility.InRange( Location, m.Location, 1 ) && !Utility.InRange( Location, oldLocation, 1 ) || atloc )
				{
					if ( m_Caster != m && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
					{
						m_Caster.DoHarmful( m );

						int runelevel = Rune.Level;
						if ( runelevel <= 0 )
							runelevel = 1;

						int damage = (int)((runelevel*m_Caster.Skills[SkillName.Inscribe].Value)/16 * RuneSpell.TMRG);

						if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 30.0 ) )
						{
							damage = (int)(damage/2);

							m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
						}

						if ( damage <= 0 )
							damage = 1;

						damage += RSCA.CalculateSI(m_Caster, damage);

						AOS.Damage( m, m_Caster, damage, 0, 100, 0, 0, 0 );
						m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
						m.PlaySound( 0x208 );
					}
				}
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private RuneSpell m_Spell;
				private InternalCenterItem m_Item;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( RuneSpell spell, SpellRune rune, InternalCenterItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 2.5 ) )
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
						m_Item.Delete();
						Stop();
					}
					else
					{
						Map map = m_Item.Map;
						Mobile caster = m_Item.m_Caster;

						if ( map != null && caster != null )
						{
							foreach ( Mobile m in m_Item.GetMobilesInRange( 2 ) )
							{
								Point3D loc1 = new Point3D( m_Item.X-2, m_Item.Y, m_Item.Z );
								Point3D loc2 = new Point3D( m_Item.X-2, m_Item.Y+1, m_Item.Z );
								Point3D loc3 = new Point3D( m_Item.X-2, m_Item.Y-1, m_Item.Z );
								Point3D loc4 = new Point3D( m_Item.X, m_Item.Y-2, m_Item.Z );
								Point3D loc5 = new Point3D( m_Item.X+1, m_Item.Y-2, m_Item.Z );
								Point3D loc6 = new Point3D( m_Item.X-1, m_Item.Y-2, m_Item.Z );
								Point3D loc7 = new Point3D( m_Item.X+2, m_Item.Y, m_Item.Z );
								Point3D loc8 = new Point3D( m_Item.X+2, m_Item.Y+1, m_Item.Z );
								Point3D loc9 = new Point3D( m_Item.X+2, m_Item.Y-1, m_Item.Z );
								Point3D loc10 = new Point3D( m_Item.X, m_Item.Y+2, m_Item.Z );
								Point3D loc11 = new Point3D( m_Item.X+1, m_Item.Y+2, m_Item.Z );
								Point3D loc12 = new Point3D( m_Item.X-1, m_Item.Y+2, m_Item.Z );
								Point3D loc13 = new Point3D( m_Item.X+1, m_Item.Y, m_Item.Z );
								Point3D loc14 = new Point3D( m_Item.X-1, m_Item.Y, m_Item.Z );
								Point3D loc15 = new Point3D( m_Item.X, m_Item.Y+1, m_Item.Z );
								Point3D loc16 = new Point3D( m_Item.X, m_Item.Y-1, m_Item.Z );
								Point3D loc17 = new Point3D( m_Item.X+1, m_Item.Y+1, m_Item.Z );
								Point3D loc18 = new Point3D( m_Item.X-1, m_Item.Y-1, m_Item.Z );
								Point3D loc19 = new Point3D( m_Item.X+1, m_Item.Y-1, m_Item.Z );
								Point3D loc20 = new Point3D( m_Item.X-1, m_Item.Y+1, m_Item.Z );
								Point3D loc21 = new Point3D( m_Item.X, m_Item.Y, m_Item.Z );

								bool atloc = false;

								if ( m.Location == loc1 || m.Location == loc2 || m.Location == loc3 || m.Location == loc4 ||
									m.Location == loc5 || m.Location == loc6 || m.Location == loc7 || m.Location == loc8 ||
									m.Location == loc9 || m.Location == loc10 || m.Location == loc11 || m.Location == loc12 ||
									m.Location == loc13 || m.Location == loc14 || m.Location == loc15 || m.Location == loc16 ||
									m.Location == loc17 || m.Location == loc18 || m.Location == loc19 || m.Location == loc20 ||
									m.Location == loc21
								)
									atloc = true;
								else if ( ((m.X == loc1.X && m.Y == loc1.Y) || (m.X == loc2.X && m.Y == loc2.Y) || (m.X == loc3.X && m.Y == loc3.Y) ||
									(m.X == loc4.X && m.Y == loc4.Y) || (m.X == loc5.X && m.Y == loc5.Y) || (m.X == loc6.X && m.Y == loc6.Y) ||
									(m.X == loc7.X && m.Y == loc7.Y) || (m.X == loc8.X && m.Y == loc8.Y) || (m.X == loc9.X && m.Y == loc9.Y) ||
									(m.X == loc10.X && m.Y == loc10.Y) || (m.X == loc11.X && m.Y == loc11.Y) || (m.X == loc12.X && m.Y == loc12.Y) ||
									(m.X == loc13.X && m.Y == loc13.Y) || (m.X == loc14.X && m.Y == loc14.Y) || (m.X == loc15.X && m.Y == loc15.Y) ||
									(m.X == loc16.X && m.Y == loc16.Y) || (m.X == loc17.X && m.Y == loc17.Y) || (m.X == loc18.X && m.Y == loc18.Y) ||
									(m.X == loc19.X && m.Y == loc19.Y) || (m.X == loc20.X && m.Y == loc20.Y) || (m.X == loc21.X && m.Y == loc21.Y)) && (m.Z >= m_Item.Z && m.Z < m_Item.Z+16)
								)
									atloc = true;
								if ( caster != m && atloc && (m.Z + 16) > m_Item.Z && (m_Item.Z + 12) > m.Z && SpellHelper.ValidIndirectTarget( caster, m ) && caster.CanBeHarmful( m, false ) )
									m_Queue.Enqueue( m );
							}

							while ( m_Queue.Count > 0 )
							{
								Mobile m = (Mobile)m_Queue.Dequeue();

								caster.DoHarmful( m );

								int runelevel = m_Rune.Level;
								if ( runelevel <= 0 )
									runelevel = 1;

								int damage = (int)((runelevel*caster.Skills[SkillName.Inscribe].Value)/16 * RuneSpell.TMRG);

								if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 80.0 ) )
								{
									damage = (int)(damage/2);

									m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
								}

								if ( damage <= 0 )
									damage = 1;

								damage += RSCA.CalculateSI(caster, damage);

								AOS.Damage( m, caster, damage, 0, 100, 0, 0, 0 );
								m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
								m.PlaySound( 0x208 );
							}
						}
					}
				}
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

			public InternalItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 14089 )
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
						if ( item.Z < newZ+16 && item.Z > newZ )
							newZ = item.Z;
					}
					eable.Free();

					MoveToWorld( new Point3D( X, Y, newZ ), map );
				}

				m_Caster = caster;
				Light = LightType.Circle150;

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.FromSeconds( 0.4 ), caster.InLOS( this ), canFit );
				m_Timer.Start();
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

			public override bool OnMoveOver( Mobile m )
			{
				if ( Rune == null )
				{
					Delete();
					return false;
				}

				if ( m_Caster != m && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
				{
					m_Caster.DoHarmful( m );

					int runelevel = Rune.Level;
					if ( runelevel <= 0 )
						runelevel = 1;

					int damage = (int)((runelevel*m_Caster.Skills[SkillName.Inscribe].Value)/30 * RuneSpell.TMRG);

					if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 30.0 ) )
					{
						damage = (int)(damage/2);

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					if ( damage <= 0 )
						damage = 1;

					damage += RSCA.CalculateSI(m_Caster, damage);

					AOS.Damage( m, m_Caster, damage, 0, 100, 0, 0, 0 );
					m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
					m.PlaySound( 0x208 );
				}
				return true;
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private RuneSpell m_Spell;
				private InternalItem m_Item;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( RuneSpell spell, SpellRune rune, InternalItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 2.5 ) )
				{
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
						m_Item.Delete();
						Stop();
					}
					else
					{
						Map map = m_Item.Map;
						Mobile caster = m_Item.m_Caster;

						if ( map != null && caster != null )
						{
							foreach ( Mobile m in m_Item.GetMobilesInRange( 0 ) )
							{
								if ( caster != m && (m.Z + 16) > m_Item.Z && (m_Item.Z + 12) > m.Z && SpellHelper.ValidIndirectTarget( caster, m ) && caster.CanBeHarmful( m, false ) )
									m_Queue.Enqueue( m );
							}

							while ( m_Queue.Count > 0 )
							{
								Mobile m = (Mobile)m_Queue.Dequeue();

								caster.DoHarmful( m );

								int runelevel = m_Rune.Level;
								if ( runelevel <= 0 )
									runelevel = 1;

								int damage = (int)((runelevel*caster.Skills[SkillName.Inscribe].Value)/30 * RuneSpell.TMRG);

								if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 80.0 ) )
								{
									damage = (int)(damage/2);

									m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
								}

								if ( damage <= 0 )
									damage = 1;

								damage += RSCA.CalculateSI(caster, damage);

								AOS.Damage( m, caster, damage, 0, 100, 0, 0, 0 );
								m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
								m.PlaySound( 0x208 );
							}
						}
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private FieryEye m_Owner;

			public InternalTarget( FieryEye owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o );
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
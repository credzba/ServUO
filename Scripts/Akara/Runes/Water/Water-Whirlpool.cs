using System;
using Server.Multis;
using Server.Regions;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class Whirlpool : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Whirlpool", "Wecu Sefh",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(Whirlpool) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public override int RuneGainMax{ get{ return 80; } }
		public override int RuneGainMin{ get{ return 40; } }

		public Whirlpool( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalRuneTarget( this );
			Caster.SendMessage( "Target a recall rune." );
		}

		public override bool CheckCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
				return false;
			}
			else if ( Caster.Criminal )
			{
				Caster.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
				return false;
			}
			else if ( SpellHelper.CheckCombat( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
				return false;
			}

			return SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom );
		}

		public void TargetRune( RecallRune r )
		{
			if ( !Caster.CanSee( r ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( !r.Marked )
			{
				Caster.SendLocalizedMessage( 501803 ); // That rune is not yet marked.
			}
			else if ( !r.TargetMap.CanSpawnMobile( r.Target.X, r.Target.Y, r.Target.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, r );

				Caster.Target = new InternalTarget( this, Caster, r );
				Caster.SendMessage( "Please target a location for the whirlpool to appear." );
			}
		}
		public void Target( IPoint3D p, RecallRune r, bool checkMulti )
		{
			if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom ) )
			{
			}
			else if ( !SpellHelper.CheckTravel( Caster,  r.TargetMap, r.Target, TravelCheckType.GateTo ) )
			{
			}
			else if ( !r.TargetMap.CanSpawnMobile( r.Target.X, r.Target.Y, r.Target.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( (checkMulti && SpellHelper.CheckMulti( r.Target, r.TargetMap )) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( !Caster.CanSee( p ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				Point3D loc = new Point3D( p.X, p.Y, p.Z );
				Point3D loc2 = r.Target;
				new InternalItem( loc, Caster, Caster.Map, TimeSpan.FromMinutes( 1.0 ), loc2, r.TargetMap );

				new InternalItem( loc2, Caster, r.TargetMap, TimeSpan.FromMinutes( 1.0 ), loc, Caster.Map );

				Point3D loc11 = new Point3D( loc2.X, loc2.Y-1, loc2.Z );
				Point3D loc12 = new Point3D( loc2.X-1, loc2.Y-1, loc2.Z );
				Point3D loc13 = new Point3D( loc2.X-1, loc2.Y, loc2.Z );
				Point3D loc14 = new Point3D( loc2.X-1, loc2.Y+1, loc2.Z );
				Point3D loc15 = new Point3D( loc2.X, loc2.Y+1, loc2.Z );
				Point3D loc16 = new Point3D( loc2.X+1, loc2.Y+1, loc2.Z );
				Point3D loc17 = new Point3D( loc2.X+1, loc2.Y, loc2.Z );
				Point3D loc18 = new Point3D( loc2.X+1, loc2.Y-1, loc2.Z );

				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc11,  r.TargetMap, 1 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc12,  r.TargetMap, 2 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc13,  r.TargetMap, 3 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc14,  r.TargetMap, 4 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc15,  r.TargetMap, 5 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc16,  r.TargetMap, 6 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc17,  r.TargetMap, 7 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc18,  r.TargetMap, 8 );

				Point3D loc3 = new Point3D( p.X, p.Y-1, p.Z );
				Point3D loc4 = new Point3D( p.X-1, p.Y-1, p.Z );
				Point3D loc5 = new Point3D( p.X-1, p.Y, p.Z );
				Point3D loc6 = new Point3D( p.X-1, p.Y+1, p.Z );
				Point3D loc7 = new Point3D( p.X, p.Y+1, p.Z );
				Point3D loc8 = new Point3D( p.X+1, p.Y+1, p.Z );
				Point3D loc9 = new Point3D( p.X+1, p.Y, p.Z );
				Point3D loc10 = new Point3D( p.X+1, p.Y-1, p.Z );

				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc3, Caster.Map, 1 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc4, Caster.Map, 2 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc5, Caster.Map, 3 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc6, Caster.Map, 4 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc7, Caster.Map, 5 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc8, Caster.Map, 6 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc9, Caster.Map, 7 );
				new InternalSItem( TimeSpan.FromMinutes( 1.0 ), loc10, Caster.Map, 8 );
			}
			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private Point3D Dest;
			private Map DestMap;

			public InternalItem( Point3D loc, Mobile caster, Map map, TimeSpan duration, Point3D dest, Map destmap ) : base( 13493 )
			{
				Dest = dest;
				DestMap = destmap;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 0.4 ), caster.InLOS( this ), canFit );
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
				writer.Write( (Point3D)Dest );
				writer.Write( (Map)DestMap );
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
						Dest = reader.ReadPoint3D();
						DestMap = reader.ReadMap();

						goto case 0;
					}
					case 0:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this, TimeSpan.Zero, true, true );
						m_Timer.Start();

						break;
					}
				}
			}

			public override bool OnMoveOver( Mobile m )
			{
				new InternalTeleTimer( m, this, Location ).Start();
				return true;
			}
			public override void OnDoubleClick( Mobile m )
			{
				if ( Utility.InRange( Location, m.Location, 1 ) )
					new InternalTeleTimer( m, this, m.Location ).Start();
				else
					m.SendMessage( "That is too far away." );
			}

			private class InternalTeleTimer : Timer
			{
				private InternalItem m_Item;
				private Mobile m_Source;
				private Point3D m_Loc;
				private int count = 0;

				public InternalTeleTimer( Mobile source, InternalItem item, Point3D loc ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 0.1 ) )
				{
					m_Loc = loc;
					m_Item = item;
					m_Source = source;
					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
					{
						m_Source.Location = m_Item.Dest;
						m_Source.Map = m_Item.DestMap;
						m_Source.PlaySound( m_Source.Female ? 814 : 1088 );
						Stop();
					}

					if ( m_Source.X == m_Loc.X && m_Source.Y == m_Loc.Y  )
					{
						if ( count >= 0 && count <= 50 )
						{
							count++;
							if ( (count % 5) == 0 )
								m_Source.Z -= 1;
							if ( m_Source.Direction == Direction.North )
								m_Source.Direction = Direction.Up;
							m_Source.Direction -= 1;
							if ( !m_Source.Mounted && m_Source.Body.IsHuman && ((count % 10) == 0 || count == 1) )
								m_Source.Animate( 263, 7, 1, true, false, 0 );
						}
						if ( count == 51 )
						{
							m_Source.PlaySound( m_Source.Female ? 814 : 1088 );
							m_Source.Location = m_Item.Dest;
							m_Source.Map = m_Item.DestMap;
							Stop();
						}
					}
					else
						Stop();
				}
			}
			private class InternalTimer : Timer
			{
				private InternalItem m_Item;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( InternalItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;
					m_InLOS = inLOS;
					m_CanFit = canFit;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						return;

					if ( DateTime.Now > m_Item.m_End )
					{
						m_Item.Delete();
						Stop();
					}
				}
			}
		}
		[DispellableField]
		private class InternalSItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;

			public InternalSItem( TimeSpan duration, Point3D loc, Map map, int n ) : base(0)
			{
				if ( n == 1 )
					ItemID = 6047;
				if ( n == 2 )
					ItemID = 6054;
				if ( n == 3 )
					ItemID = 6051;
				if ( n == 4 )
					ItemID = 6056;
				if ( n == 5 )
					ItemID = 6049;
				if ( n == 6 )
					ItemID = 6055;
				if ( n == 7 )
					ItemID = 6045;
				if ( n == 8 )
					ItemID = 6053;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 0.4 ), canFit );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public InternalSItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

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
						goto case 0;
					}
					case 0:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this, TimeSpan.Zero, true );
						m_Timer.Start();

						break;
					}
				}
			}

			private class InternalTimer : Timer
			{
				private InternalSItem m_Item;
				private bool m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( InternalSItem item, TimeSpan delay, bool canFit ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;
					m_CanFit = canFit;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						return;

					if ( DateTime.Now > m_Item.m_End )
					{
						m_Item.Delete();
						Stop();
					}
				}
			}
		}

		private class InternalRuneTarget : Target
		{
			private Whirlpool m_Owner;

			public InternalRuneTarget( Whirlpool owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
					m_Owner.TargetRune( (RecallRune)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
			}
		}

		private class InternalTarget : Target
		{
			private RecallRune m_R;
			private Mobile m_Caster;
			private Whirlpool m_Owner;

			public InternalTarget( Whirlpool owner, Mobile caster, RecallRune r ) : base( 12, true, TargetFlags.None )
			{
				m_R = r;
				m_Caster = caster;
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o, m_R, true );
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

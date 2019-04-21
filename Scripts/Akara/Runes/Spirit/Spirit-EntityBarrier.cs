using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class EntityBarrier : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Entity Barrier", "Sphir Ghew",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(EntityBarrier) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public override int RuneGainMax{ get{ return 100; } }
		public override int RuneGainMin{ get{ return 50; } }

		public EntityBarrier( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to cast this spell onto." );
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
			else if ( CheckSequence() )
			{
				GainExp();
				//if ( Spellbreaker.BreakSpell( m, Caster, this ) )
				//	return;

				SpellHelper.Turn( Caster, m );

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

				Point3D loc1 = new Point3D( m.X-2, m.Y-2, m.Z );
				Point3D loc2 = new Point3D( m.X-3, m.Y-1, m.Z );
				Point3D loc3 = new Point3D( m.X-3, m.Y, m.Z );
				Point3D loc4 = new Point3D( m.X-3, m.Y+1, m.Z );
				Point3D loc5 = new Point3D( m.X-2, m.Y+2, m.Z );
				Point3D loc6 = new Point3D( m.X-1, m.Y+3, m.Z );
				Point3D loc7 = new Point3D( m.X, m.Y+3, m.Z );
				Point3D loc8 = new Point3D( m.X+1, m.Y+3, m.Z );

				Point3D loc9 = new Point3D( m.X+2, m.Y+2, m.Z );
				Point3D loc10 = new Point3D( m.X+3, m.Y+1, m.Z );
				Point3D loc11 = new Point3D( m.X+3, m.Y, m.Z );
				Point3D loc12 = new Point3D( m.X+3, m.Y-1, m.Z );
				Point3D loc13 = new Point3D( m.X+2, m.Y-2, m.Z );
				Point3D loc14 = new Point3D( m.X+1, m.Y-3, m.Z );
				Point3D loc15 = new Point3D( m.X, m.Y-3, m.Z );
				Point3D loc16 = new Point3D( m.X-1, m.Y-3, m.Z );

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

				new InternalItem2( loc1, Caster, Caster.Map, duration );
				new InternalItem2( loc2, Caster, Caster.Map, duration );
				new InternalItem2( loc3, Caster, Caster.Map, duration );
				new InternalItem2( loc4, Caster, Caster.Map, duration );
				new InternalItem2( loc5, Caster, Caster.Map, duration );
				new InternalItem2( loc6, Caster, Caster.Map, duration );
				new InternalItem2( loc7, Caster, Caster.Map, duration );
				new InternalItem2( loc8, Caster, Caster.Map, duration );
				new InternalItem2( loc9, Caster, Caster.Map, duration );
				new InternalItem2( loc10, Caster, Caster.Map, duration );
				new InternalItem2( loc11, Caster, Caster.Map, duration );
				new InternalItem2( loc12, Caster, Caster.Map, duration );
				new InternalItem2( loc13, Caster, Caster.Map, duration );
				new InternalItem2( loc14, Caster, Caster.Map, duration );
				new InternalItem2( loc15, Caster, Caster.Map, duration );
				new InternalItem2( loc16, Caster, Caster.Map, duration );
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private SpellRune Rune;
			private RuneSpell m_Spell;

			public InternalItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 0x21A4 )
			{
				m_Spell = spell;
				Name = "entity barrier";
				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.FromSeconds( 0.4 ), caster.InLOS( this ), canFit );
				m_Timer.Start();
			}

			public override void SendInfoTo( NetState state, bool sendOplPacket )
			{
				Mobile mob = state.Mobile;

				if ( mob != null && ( mob == m_Caster || mob.Skills[SkillName.Inscribe].Value >= 100 ) )
					state.Send( new GMItemPacket( this ) );
				else
					state.Send( WorldPacket );

				if ( ObjectPropertyList.Enabled )
					state.Send( OPLPacket );
			}

			public sealed class GMItemPacket : Packet
			{
				public GMItemPacket( Item item ) : base( 0x1A )
				{
					this.EnsureCapacity( 0 );//20

					// 14 base length
					// +2 - Amount
					// +2 - Hue
					// +1 - Flags

					uint serial = (uint)item.Serial.Value;
					int itemID = 14068;
					int amount = item.Amount;
					Point3D loc = item.Location;
					int x = loc.X;
					int y = loc.Y;
					int hue = 906;
					int flags = item.GetPacketFlags();
					int direction = (int)item.Direction;

					if ( amount != 0 )
						serial |= 0x80000000;
					else
						serial &= 0x7FFFFFFF;

					m_Stream.Write( (uint) serial );
					m_Stream.Write( (short) (itemID & 0x7FFF) );

					if ( amount != 0 )
						m_Stream.Write( (short) amount );

					x &= 0x7FFF;

					if ( direction != 0 )
						x |= 0x8000;

					m_Stream.Write( (short) x );

					y &= 0x3FFF;

					if ( hue != 0 )
						y |= 0x8000;

					if ( flags != 0 )
						y |= 0x4000;

					m_Stream.Write( (short) y );

					if ( direction != 0 )
						m_Stream.Write( (byte) direction );

					m_Stream.Write( (sbyte) loc.Z );

					if ( hue != 0 )
						m_Stream.Write( (ushort) hue );

					if ( flags != 0 )
						m_Stream.Write( (byte) flags );
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

			public override void OnMovement( Mobile m, Point3D oldLocation )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				if ( Utility.InRange( Location, m.Location, 1 ) && !Utility.InRange( Location, oldLocation, 1 ) )
				{
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

						damage += ImperialOrb.IncI( m_Caster, m_Spell );

						AOS.Damage( m, m_Caster, damage, 20, 20, 20, 20, 20 );
						m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
						m.SendMessage( "Souls from beyond tear at your flesh, you hear distant screams." );
						m.PlaySound( 0x482 );m.PlaySound( 0x484 );m.PlaySound( 0x485 );m.PlaySound( 0x486 );
					}
				}
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private InternalItem m_Item;
				private bool m_InLOS, m_CanFit;
				private RuneSpell m_Spell;

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
							foreach ( Mobile m in m_Item.GetMobilesInRange( 1 ) )
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

								AOS.Damage( m, caster, damage, 20, 20, 20, 20, 20 );
								m.FixedParticles( 0x37C4, 10, 30, 5052, 906, 0, EffectLayer.LeftFoot );
								m.SendMessage( "Souls from beyond tear at your flesh, you hear distant screams." );
								m.PlaySound( 0x482 );m.PlaySound( 0x484 );m.PlaySound( 0x485 );m.PlaySound( 0x486 );
							}
						}
					}
				}
			}
		}
		[DispellableField]
		private class InternalItem2 : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;

			public InternalItem2( Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 0x2199 )
			{
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

			public InternalItem2( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_Caster );
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

			private class InternalTimer : Timer
			{
				private InternalItem2 m_Item;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( InternalItem2 item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
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

		private class InternalTarget : Target
		{
			private EntityBarrier m_Owner;

			public InternalTarget( EntityBarrier owner ) : base( 12, true, TargetFlags.None )
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
using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class LightningField : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Lightning Field", "Artu Ser",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(LightningField) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public LightningField( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a location to cast this spell upon." );
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

				int dx = Caster.Location.X - p.X;
				int dy = Caster.Location.Y - p.Y;
				int rx = (dx - dy) * 44;
				int ry = (dx + dy) * 44;

				bool eastToWest;

				if ( rx >= 0 && ry >= 0 )
				{
					eastToWest = false;
				}
				else if ( rx >= 0 )
				{
					eastToWest = true;
				}
				else if ( ry >= 0 )
				{
					eastToWest = true;
				}
				else
				{
					eastToWest = false;
				}

				Effects.PlaySound( p, Caster.Map, 0x20C );

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

				for ( int i = -2; i <= 2; ++i )
				{
					Point3D loc = new Point3D( eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z );

					new InternalMobile( this, Rune, loc, Caster, Caster.Map, duration, i );
				}
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalMobile : BaseCreature
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private Timer m_Timer2;
			private SpellRune Rune;
			private RuneSpell m_Spell;

			public InternalMobile( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration, int val ) : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
			{
				m_Spell = spell;
				Body = 0;

				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				CantWalk = true;
				Blessed = true;

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

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( m_Spell, Rune, this, TimeSpan.FromSeconds( Math.Abs( val ) * 0.2 ), caster.InLOS( this ), canFit );
				m_Timer.Start();
				m_Timer2 = new Internal2Timer( this );
				m_Timer2.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
				if ( m_Timer2 != null )
					m_Timer2.Stop();
			}

			public InternalMobile( Serial serial ) : base( serial )
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
						m_Timer2 = new Internal2Timer( this );
						m_Timer2.Start();

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
				if ( InRange( m.Location, 0 ) && !InRange( oldLocation, 0 ) )
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

						AOS.Damage( m, m_Caster, damage, 0, 0, 0, 0, 100 );
						m.BoltEffect( 0 );
						m.Stam = m.StamMax;
					}
				}
				if ( InRange( m.Location, 1 ) && !InRange( oldLocation, 1 ) )
				{
					m.Stam = m.StamMax;
				}
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private RuneSpell m_Spell;
				private InternalMobile m_Item;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();

				public InternalTimer( RuneSpell spell, SpellRune rune, InternalMobile item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 2.5 ) )
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

								damage += ImperialOrb.IncI( caster, m_Spell );

								AOS.Damage( m, caster, damage, 0, 100, 0, 0, 0 );
								m.BoltEffect( 0 );
								m.Stam = m.StamMax;
							}
						}
					}
				}
			}
			private class Internal2Timer : Timer
			{
				private InternalMobile m_Mobile;

				public Internal2Timer( InternalMobile item ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.5 ) )
				{
					m_Mobile = item;
				}

				protected override void OnTick()
				{
					if ( m_Mobile == null )
						Stop();
					if ( m_Mobile.Deleted )
						Stop();

					m_Mobile.BoltEffect( 0 );
				}
			}
		}

		private class InternalTarget : Target
		{
			private LightningField m_Owner;

			public InternalTarget( LightningField owner ) : base( 12, true, TargetFlags.None )
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
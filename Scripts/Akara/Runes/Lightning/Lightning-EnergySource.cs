using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class EnergySource : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Energy Source", "Artu Soirv",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(EnergySource) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public override int RuneGainMax{ get{ return 80; } }
		public override int RuneGainMin{ get{ return 40; } }

		public EnergySource( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a location to summon this source to." );
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

				new InternalItem( this, Rune, new Point3D( p.X, p.Y, p.Z ), Caster, Caster.Map, duration );
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

			public InternalItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 2275 )
			{
				m_Spell = spell;
				Name = "energy source";

				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;

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

			public override void OnMovement( Mobile m, Point3D oldLocation )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				if ( Utility.InRange( Location, m.Location, Rune.Level ) && !Utility.InRange( Location, oldLocation, Rune.Level ) )
				{
					if ( m_Caster != m && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
					{
						m_Caster.DoHarmful( m );

						int damage = (int)((m_Caster.Skills[SkillName.Inscribe].Value)/5 * RuneSpell.TMRG);

						if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 50.0 ) )
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
						--m_Caster.Mana;
						if ( m_Caster.Mana <= 0 )
							Delete();
					}
				}
			}

			private class InternalTimer : Timer
			{
				private SpellRune m_Rune;
				private InternalItem m_Item;
				private RuneSpell m_Spell;
				private bool m_InLOS, m_CanFit;

				private static Queue m_Queue = new Queue();
				private static int count = 0; 	//Iceman's Edit 7.31.2012 to fix effect spamming

				public InternalTimer( RuneSpell spell, SpellRune rune, InternalItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 2.5 ) )
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
							foreach ( Mobile m in m_Item.GetMobilesInRange( m_Rune.Level ) )
							{
								if ( (m.Z + 16) > m_Item.Z && (m_Item.Z + 12) > m.Z && SpellHelper.ValidIndirectTarget( caster, m ) && caster.CanBeHarmful( m, false ) )
									m_Queue.Enqueue( m );
							}

							while ( m_Queue.Count > 0 )
							{
								Mobile m = (Mobile)m_Queue.Dequeue();

								if ( m != caster )
								{
									caster.DoHarmful( m );

									int damage = (int)((caster.Skills[SkillName.Inscribe].Value)/5 * RuneSpell.TMRG);

									if ( !Core.AOS && m.CheckSkill( SkillName.MagicResist, 0.0, 50.0 ) )
									{
										damage = (int)(damage/2);

										m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
									}

									if ( damage <= 0 )
										damage = 1;

									damage += RSCA.CalculateSI(caster, damage);

									damage += ImperialOrb.IncI( caster, m_Spell );

									AOS.Damage( m, caster, damage, 0, 0, 0, 0, 100 );
									
									if (count > 10) 						//Iceman Edit 7.31.2012 to fix effect spamming
									{										//Using this rune in champs would knock players offline
									m.BoltEffect( 0 );						//and create severe clientside lag for anyone in the vicinity
									count =0 ;								//of the caster.  If successful this edit should be applied to 
									}										//all other runespells as well.
									else									//Comments-RahvinTheEvil
									count++;								//end of Iceman's Edits
									
									--caster.Mana;
									if ( caster.Mana <= 0 )
										m_Item.Delete();
								}
							}
						}
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private EnergySource m_Owner;

			public InternalTarget( EnergySource owner ) : base( 12, true, TargetFlags.None )
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
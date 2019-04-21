using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class Ensnare : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"No Face", "Elra Vewn",
				212,
				9041,
				GetSpellGems.GetGems( typeof(Ensnare) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public Ensnare( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a location to summon the trap to." );
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

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

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

				new InternalTriggerItem( this, Rune, new Point3D( p.X, p.Y, p.Z ), Caster, Caster.Map, duration );
			}
			FinishSequence();
		}
		[DispellableField]
		private class InternalTriggerItem : Item
		{
			private Mobile m_Caster;
			private SpellRune Rune;
			private RuneSpell m_Spell;
			private DateTime m_Expire;

			public InternalTriggerItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 3699 )
			{
				m_Spell = spell;
				m_Expire = DateTime.Now + duration;
				Hue = 1;
				Name = "ensnare trap";

				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;

				new InternalTimer( this, duration ).Start();
			}

			public InternalTriggerItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version

				writer.Write( m_Caster );
				writer.Write( Rune );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 0:
					{
						m_Caster = reader.ReadMobile();
						Rune = reader.ReadItem() as SpellRune;
						Delete();

						break;
					}
				}
			}

			public override bool HandlesOnMovement{ get{ return true; } }
			public override void OnMovement( Mobile m, Point3D oldLocation )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				if ( Utility.InRange( Location, m.Location, Rune.Level ) )
				{
					if ( m.CanBeginAction( typeof(Ensnare) ) && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
					{
						if ( m != m_Caster )
						{
							Effects.PlaySound( m, m.Map, 667 );
							m_Caster.DoHarmful( m );

							int runelevel = Rune.Level;
							if ( runelevel <= 0 )
								runelevel = 1;

							m.SendMessage( "You have been ensnared." );

							Point3D loc1 = new Point3D( m.X+1, m.Y, m.Z );
							Point3D loc2 = new Point3D( m.X-1, m.Y, m.Z );
							Point3D loc3 = new Point3D( m.X, m.Y+1, m.Z );
							Point3D loc4 = new Point3D( m.X, m.Y-1, m.Z );

							TimeSpan duration = TimeSpan.FromSeconds( ((15 + (m_Caster.Skills.Inscribe.Fixed / 10)) / 8  * RuneSpell.TMRG)+ImperialOrb.IncD( m_Caster, m_Spell ));
							duration += TimeSpan.FromSeconds( RSCA.CalculateLLS(m_Caster, (15 + (m_Caster.Skills.Inscribe.Fixed / 10)) / 8) );

							m.BeginAction( typeof(Ensnare) );
							new InternalItem( m, m.Map, duration, loc1, 14678 );
							new InternalItem( m, m.Map, duration, loc2, 14678 );
							new InternalItem( m, m.Map, duration, loc3, 14662 );
							new InternalItem( m, m.Map, duration, loc4, 14662 );
						}
					}
				}
			}
			private class InternalTimer : Timer
			{
				private InternalTriggerItem m_Item;

				public InternalTimer( InternalTriggerItem item, TimeSpan delay ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						Stop();

					if ( DateTime.Now > m_Item.m_Expire )
					{
						m_Item.Delete();
						Stop();
					}
				}
			}
		}
		[DispellableField]
		private class InternalItem : Item
		{
			private DateTime m_Expire;
			private Mobile m_Source;

			public InternalItem( Mobile source, Map map, TimeSpan duration, Point3D loc, int itemID ) : base( itemID )
			{
				m_Expire = DateTime.Now + duration;
				Hue = 1;
				Name = "ensnare trap";
				m_Source = source;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				new InternalTimer( m_Source, this, duration ).Start();
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Source );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 0:
					{
						m_Source = reader.ReadMobile();
						m_Source.EndAction( typeof(Ensnare) );
						Delete();

						break;
					}
				}
			}
			private class InternalTimer : Timer
			{
				private InternalItem m_Item;
				private Mobile n_Source;

				public InternalTimer( Mobile source, InternalItem item, TimeSpan delay ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;
					n_Source = source;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						Stop();

					if ( DateTime.Now > m_Item.m_Expire )
					{
						m_Item.Delete();
						n_Source.EndAction( typeof(Ensnare) );
						Stop();
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private Ensnare m_Owner;

			public InternalTarget( Ensnare owner ) : base( 12, true, TargetFlags.None )
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
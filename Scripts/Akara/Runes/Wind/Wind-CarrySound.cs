using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class CarrySound : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Carry Sound", "Wilo Fevj",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(CarrySound) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public override int RuneGainMax{ get{ return 10; } }
		public override int RuneGainMin{ get{ return 5; } }

		public CarrySound( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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
			else if ( !Caster.CanBeginAction( typeof(CarrySound) ))
			{
				Caster.SendMessage( "You take the spell off." );
				Caster.EndAction( typeof( CarrySound ) );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				Caster.BeginAction( typeof( CarrySound ) );
				SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				Effects.PlaySound( p, Caster.Map, 0x20C );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				TimeSpan duration;

				duration = TimeSpan.FromMinutes( ((runelevel*Caster.Skills.Inscribe.Value)/100 * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
				duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, (runelevel*Caster.Skills.Inscribe.Value)/100) );

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

			public InternalItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration ) : base( 0x2198 )
			{
				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( m_Caster, this, TimeSpan.FromSeconds( 0.4 ), caster.InLOS( this ), canFit );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();

				if ( m_Caster != null )
					m_Caster.EndAction( typeof( CarrySound ) );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override bool HandlesOnSpeech{ get{ return true; } }

			public override void OnSpeech( SpeechEventArgs e )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				base.OnSpeech( e );

				Mobile from = e.Mobile;

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				int range = (runelevel*3);
				range += RSCA.CalculateSI(m_Caster, range);

				if ( Utility.InRange( Location, from.Location, range ) && m_Caster != null && !m_Caster.CanBeginAction( typeof(CarrySound) ) && (from.AccessLevel < m_Caster.AccessLevel || from.AccessLevel == AccessLevel.Player || (from.AccessLevel >= m_Caster.AccessLevel && !from.Hidden)) )
					m_Caster.SendMessage( 33, "Carry Sound Spell-"+from.Name+": "+e.Speech );

				if ( m_Caster.CanBeginAction( typeof(CarrySound) ) )
					Delete();
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

						m_Timer = new InternalTimer( m_Caster, this, TimeSpan.Zero, true, true );
						m_Timer.Start();

						break;
					}
				}
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;
				private Mobile m_Caster2;
				private bool m_InLOS, m_CanFit;

				public InternalTimer( Mobile caster, InternalItem item, TimeSpan delay, bool inLOS, bool canFit ) : base( delay, TimeSpan.FromSeconds( 0.1 ) )
				{
					m_Item = item;
					m_InLOS = inLOS;
					m_CanFit = canFit;
					m_Caster2 = caster;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						Stop();

					if ( DateTime.Now > m_Item.m_End || m_Caster2.CanBeginAction( typeof(CarrySound) ) )
					{
						m_Item.Delete();
						Stop();
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private CarrySound m_Owner;

			public InternalTarget( CarrySound owner ) : base( 12, true, TargetFlags.None )
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
				return TimeSpan.FromSeconds( 7 * CastDelaySecondsPerTick );
			}
		}
	}
}
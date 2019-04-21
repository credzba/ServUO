using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class IceBonds : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Ice Bonds", "Iwne Frajk",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(IceBonds) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public IceBonds( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to cast this spell onto." );
		}

		public void Target( Mobile m )
		{
			if ( m.AccessLevel >= AccessLevel.Counselor )
				m = Caster;
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( !m.CanBeginAction( typeof(IceBonds) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else if ( CheckHSequence(m) )  //CheckSequence not attking - checkH(armful)Sequence - flags
			{
				GainExp();
				//if ( Spellbreaker.BreakSpell( m, Caster, this ) )
				//	return;

				m.SendMessage( "Your body has been enveloped by ice, you cannot move." );
				m.BeginAction( typeof( IceBonds ) );

				SpellHelper.Turn( Caster, m );

				Effects.PlaySound( m.Location, Caster.Map, 0x20C );

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

				m.Frozen = true;
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 0 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 2 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 4 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 6 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 8 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 10 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 12 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 14 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 16 );
				new InternalItem( m, Rune, new Point3D( m.X, m.Y, m.Z ), Caster, Caster.Map, duration, 18 );
			}
			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;
			private Mobile m_Source;
			private SpellRune Rune;

			public InternalItem( Mobile source, SpellRune rune, Point3D loc, Mobile caster, Map map, TimeSpan duration, int za ) : base( 14360 )
			{
				Name = "ice bond";
				Hue = 6;

				Rune = rune;

				Movable = false;

				MoveToWorld( loc, map );
				Z += za;

				m_Caster = caster;
				m_Source = source;

				m_End = DateTime.Now + duration;

				m_Timer = new InternalTimer( this );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();

				m_Source.Frozen = false;

				if ( X == m_Source.X )
				{
					m_Source.EndAction( typeof( IceBonds ) );
					m_Source.SendMessage( "The ice melts away." );
				}
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_Caster );
				writer.Write( m_Source );
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
						m_Source = reader.ReadMobile();
						Rune = reader.ReadItem() as SpellRune;

						goto case 0;
					}
					case 0:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this );
						m_Timer.Start();

						break;
					}
				}
			}
			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				public InternalTimer( InternalItem item ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
				{
					m_Item = item;

					Priority = TimerPriority.FiftyMS;
				}

				protected override void OnTick()
				{
					m_Item.m_Source.Frozen = true;
					if ( m_Item == null || m_Item.Deleted || DateTime.Now > m_Item.m_End || m_Item.m_Source == null || !m_Item.m_Source.Alive || m_Item.m_Source.Deleted )
					{
						m_Item.Delete();
						Stop();
					}
				}
			}
		}

		private class InternalTarget : Target
		{
			private IceBonds m_Owner;

			public InternalTarget( IceBonds owner ) : base( 12, true, TargetFlags.None )
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
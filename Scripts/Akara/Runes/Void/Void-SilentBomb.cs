using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class SilentBomb : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Silent Bomb", "Elra Stri",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(SilentBomb) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public override int RuneGainMax{ get{ return 80; } }
		public override int RuneGainMin{ get{ return 40; } }

		public SilentBomb( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a location to summon the bomb to." );
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
				TimeSpan duration;  //breaker edit start

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

				new InternalItem( this, Rune, new Point3D( p.X, p.Y, p.Z ), Caster, Caster.Map, duration ); //added ", duration" after .Map
				
				
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Mobile m_Caster;
			private SpellRune Rune;
			private RuneSpell m_Spell;
			private DateTime m_Expire; //breaker add - fixing never decay
			
			public InternalItem( RuneSpell spell, SpellRune rune, Point3D loc, Mobile caster, Map map,  TimeSpan duration  ) : base( 3699 )
			{
				m_Spell = spell;
				m_Expire = DateTime.Now + duration; //breaker add 
				Hue = 1;
				Name = "silent bomb";

				Rune = rune;

				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;
				new InternalTimer( this, duration ).Start(); //breaker add
				
			}

			public InternalItem( Serial serial ) : base( serial )
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
						Delete();  //delete all on restart
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
				if ( !m.Alive )
					return;

				if ( Utility.InRange( Location, m.Location, Rune.Level ) )
				{
					if ( SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
					{
						Delete();
						m_Caster.DoHarmful( m );

						int runelevel = Rune.Level;
						if ( runelevel <= 0 )
							runelevel = 1;

						int damage = (int)( ( m_Caster.Skills[SkillName.Inscribe].Value * runelevel ) / 3 * RuneSpell.TMRG );
						damage += RSCA.CalculateSI(m_Caster, damage);

						damage += ImperialOrb.IncI( m_Caster, m_Spell );

						AOS.Damage( m, m_Caster, damage, 20, 20, 20, 20, 20 );
						new EffectSoundTimer( m ).Start();

           					m.PlaySound( 0x307 );
						Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z + 4 ), m.Map, 0x36BD, 13 );
						Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x36BD, 13 );
					}
				}
			}
			//new timer for deletion
			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				public InternalTimer( InternalItem item, TimeSpan delay ) : base( delay, TimeSpan.FromSeconds( 1.0 ) )
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
			}  //end new timer
			
			private class EffectSoundTimer : Timer
			{
				private Mobile m;

				private int cnt;

				public EffectSoundTimer( Mobile m_From ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
				{
					m = m_From;
				}

				protected override void OnTick()
				{
					cnt++;

									
					switch ( cnt )
					{
						case 1:
						{
           						m.PlaySound( 0x307 );
							Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z - 4 ), m.Map, 0x36BD, 13 );
							Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z + 4 ), m.Map, 0x36BD, 13 );
							break;
						}
						case 2:
						{
           						m.PlaySound( 0x307 );
							Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x36BD, 13 );
							Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z - 4 ), m.Map, 0x36BD, 13 );
							break;
						}
						case 3:
						{
           						m.PlaySound( 0x307 );
							Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 11 ), m.Map, 0x36BD, 13 );
							Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 7 ), m.Map, 0x36BD, 13 );
							break;
						}
						case 4:
						{
           						m.PlaySound( 0x307 );
							Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 3 ), m.Map, 0x36BD, 13 );
							Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z - 1 ), m.Map, 0x36BD, 13 );

							Stop();
							break;
						}
					}
				}
			}
			
		}

		private class InternalTarget : Target
		{
			private SilentBomb m_Owner;

			public InternalTarget( SilentBomb owner ) : base( 12, true, TargetFlags.None )
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
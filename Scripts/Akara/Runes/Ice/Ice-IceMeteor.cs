using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class IceMeteor : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Ice Meteor", "Iwne Nuis",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(IceMeteor) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public override int RuneGainMax{ get{ return 80; } }
		public override int RuneGainMin{ get{ return 40; } }

		public IceMeteor( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to cast this spell onto." );
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

				if ( p is Item )
					p = ((Item)p).GetWorldLocation();

				SpellHelper.GetSurfaceTop( ref p );

				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p.X, p.Y, p.Z ), runelevel );

					foreach ( Mobile m in eable )
					{
						if ( Caster != m && SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) /*&& !Spellbreaker.BreakSpell( m, Caster, this )*/ )
							targets.Add( m );
					}
					eable.Free();
				}

				if ( targets.Count > 0 )
				{
					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile m = (Mobile)targets[i];

						Caster.DoHarmful( m );
						m.SendMessage( "Your body has been struck by meteors of ice." );

						int damage = (int)((runelevel*Caster.Skills[SkillName.Inscribe].Value) / 20 * RuneSpell.TMRG);

						damage += RSCA.CalculateSI(Caster, damage);

						damage += ImperialOrb.IncI( Caster, this );

						Effects.PlaySound( p, Caster.Map, 0x160 );
						Effects.PlaySound( p, Caster.Map, 0x20C );
						Caster.MovingEffect( m, 0x36D4, 7, 1, false, false, 5, 0 );
						SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 100, 0, 0 );
						new InternalTimer( Caster, m, Rune.Level ).Start();
					}
				}
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;
			private Mobile m_Target;
			private int m_Amount;
			private int cnt = 0;

			public InternalTimer( Mobile caster, Mobile target, int amount ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ) )
			{
				m_Caster = caster;
				m_Target = target;
				m_Amount = amount;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				cnt++;
				if ( m_Amount <= 0 )
					Stop();
				if ( cnt == 0 )
				{
					if ( m_Amount >= 1 )
					{
						m_Caster.MovingEffect( m_Target, 0x36D4, 7, 1, false, false, 5, 0 );
					}
				}
				if ( cnt == 1 )
				{
					if ( m_Amount >= 2 )
					{
						m_Caster.MovingEffect( m_Target, 0x36D4, 7, 1, false, false, 5, 0 );
					}
				}
				if ( cnt == 2 )
				{
					if ( m_Amount >= 3 )
					{
						Effects.PlaySound( m_Target.Location, m_Caster.Map, 0x160 );
						Effects.PlaySound( m_Target.Location, m_Caster.Map, 0x20C );
						m_Caster.MovingEffect( m_Target, 0x36D4, 7, 1, false, false, 5, 0 );
					}
				}
				if ( cnt == 3 )
				{
					if ( m_Amount >= 4 )
					{
						m_Caster.MovingEffect( m_Target, 0x36D4, 7, 1, false, false, 5, 0 );
					}
					Stop();
				}
			}
		}

		private class InternalTarget : Target
		{
			private IceMeteor m_Owner;

			public InternalTarget( IceMeteor owner ) : base( 12, true, TargetFlags.None )
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
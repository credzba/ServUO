using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public class Contagion : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Contagion", "Tose Coih",
				215,
				9041,
				false,
				GetSpellGems.GetGems( typeof(Contagion) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public Contagion( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a loation to cast this spell upon." );
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

				int runelevel = Rune.Level*2;
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

						int total = (int)(runelevel*Caster.Skills[SkillName.Inscribe].Value);

						int distx = m.X-p.X;
						distx *= distx;
						int disty = m.Y-p.Y;
						disty *= disty;

						int dist = distx+disty;

						dist = (int)Math.Sqrt(dist);

						if ( dist >= (int)(runelevel-(runelevel/2)) )
							total -= (int)((dist - (double)(runelevel-(runelevel/2))) * 50 * RuneSpell.TMRG);

						total += RSCA.CalculateSI(Caster, total);

						total += ImperialOrb.IncI( Caster, this );

						if ( total > 900.0 )
							m.Poison = Poison.Lethal;
						else if ( total > 700.0 )
							m.Poison = Poison.Deadly;
						else if ( total > 500.0 )
							m.Poison = Poison.Greater;
						else if ( total > 300.0 )
							m.Poison = Poison.Regular;
						else
							m.Poison = Poison.Lesser;

						m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
						m.PlaySound( 0x474 );
					}
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private Contagion m_Owner;

			public InternalTarget( Contagion owner ) : base( 12, true, TargetFlags.None )
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
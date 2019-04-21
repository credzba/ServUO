using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class ChainOfFire : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Chain Of Fire", "Phoe Gradi",
				212,
				9041,
				GetSpellGems.GetGems( typeof(ChainOfFire) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public override int RuneGainMax{ get{ return 60; } }
		public override int RuneGainMin{ get{ return 30; } }

		public ChainOfFire( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
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

				m.SendMessage( "A flame leaps to your body." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int damage = (int)((runelevel*Caster.Skills[SkillName.Inscribe].Value) / 13 * RuneSpell.TMRG);

				damage += RSCA.CalculateSI(Caster, damage);

				damage += ImperialOrb.IncI( Caster, this );

				m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
				m.PlaySound( 0x208 );
				SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 100, 0, 0, 0 );

				double chance = 0.5;

				int i2 = 0;
				while ( i2 < runelevel )
				{
					chance += 0.1;
					i2++;
				}

				if ( chance >= Utility.RandomDouble() )
				{
					m.SendMessage( "The flame leaps to another." );
					new InternalTimer( this, Caster, m, Rune, 1 ).Start();
				}
			}
			FinishSequence();
		}
		private class InternalTimer : Timer
		{
			private Mobile m_Caster;
			private Mobile Source;
			private SpellRune Rune;
			private RuneSpell m_Spell;
			private int number;

			public InternalTimer( RuneSpell spell, Mobile caster, Mobile source, SpellRune rune, int Number ) : base( TimeSpan.FromSeconds( 2.0-RuneSpell.TMRG ) )
			{
				m_Spell = spell;
				m_Caster = caster;
				Source = source;
				Rune = rune;
				number = Number+1;
			}

			protected override void OnTick()
			{
				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				ArrayList targets = new ArrayList();

				Map map = m_Caster.Map;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( Source.X, Source.Y, Source.Z ), runelevel );

					foreach ( Mobile m in eable )
					{
						if ( Source != m && m_Caster != m && SpellHelper.ValidIndirectTarget( m_Caster, m ) /*&& m_Caster.CanBeHarmful( m, false ) && !Spellbreaker.BreakSpell( m, m_Caster, m_Spell )*/ )
						{
							targets.Add( m );
						}
					}
					eable.Free();
				}
				if ( targets.Count > 0 )
				{
					int tn = Utility.RandomMinMax( 0, targets.Count-1 );

					Mobile m = (Mobile)targets[tn];

					if ( m == null )
						return;
					else if ( m.Deleted )
						return;
					else if ( m.Alive )
					{
						m.SendMessage( "A flame leaps to your body." );

						int damage = (int)((runelevel*m_Caster.Skills[SkillName.Inscribe].Value) / 13 * RuneSpell.TMRG);

						if ( m_Caster.Player )
						{
							damage += (int)(m_Caster.Int * 0.001);
							damage += (int)(AosAttributes.GetValue( m_Caster, AosAttribute.SpellDamage ) * 0.01);
						}

						damage += RSCA.CalculateSI(m_Caster, damage);

						damage += ImperialOrb.IncI( m_Caster, m_Spell );

						m.FixedParticles( 0x3709, 10, 30, 5052, 0, 0, EffectLayer.LeftFoot );
						m.PlaySound( 0x208 );
						SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, damage, 0, 100, 0, 0, 0 );

						double chance = 0.5;

						int i2 = 0;
						while ( i2 < runelevel )
						{
							chance += 0.1;
							i2++;
						}

						if ( chance >= Utility.RandomDouble() && number < runelevel*2 )
						{
							m.SendMessage( "The flame leaps to another." );
							new InternalTimer( m_Spell, m_Caster, m, Rune, number ).Start();
						}
					}
				}
				Stop();
			}
		}
		private class InternalTarget : Target
		{
			private ChainOfFire m_Owner;

			public InternalTarget( ChainOfFire owner ) : base( 12, true, TargetFlags.None )
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
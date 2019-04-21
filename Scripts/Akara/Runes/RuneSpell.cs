using System;
using System.Collections;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells.Necromancy;
using Server.Spells.Second;

namespace Server.Spells
{
	public abstract class RuneSpell : MagerySpell
	{
		public static double TMRG{ get{ return 1.0; } }

		public abstract int RuneGainMax{ get; }
		public abstract int RuneGainMin{ get; }

		public override SkillName CastSkill{ get{ return SkillName.Inscribe; } }
		public override bool ClearHandsOnCast{ get{ return false; } }

		public SpellRune Rune;

		public RuneSpell( Mobile caster, Item scroll, SpellInfo info, SpellRune rune ) : base( caster, scroll, info )
		{
			Rune = rune;
		}
		public override void SayMantra()
		{
			base.SayMantra();
			Caster.PlaySound( Utility.RandomList( 696, 697, 698, 699, 700 ) );
		}
		/*public override TimeSpan GetCastRecovery()
		{
			//if ( !Core.AOS )
			//	return NextSpellDelay;

			int fcr = RSCA.GetFCR( Caster );
			if ( fcr > 5 )
				fcr = 5;
			int circleDelay = CastRecoveryCircleScalar * (1 + (int)Circle); // Note: Circle is 0-based so we must offset
			int fcrDelay = -(CastRecoveryFastScalar * fcr);

			int delay = CastRecoveryBase + circleDelay + fcrDelay;

			if ( delay < CastRecoveryMinimum )
				delay = CastRecoveryMinimum;

			return TimeSpan.FromSeconds( (double)delay / CastRecoveryPerSecond );
		}
		public override TimeSpan GetCastDelay()
		{
			if ( Scroll is BaseWand )
				return TimeSpan.Zero;

			if ( !Core.AOS )
				return TimeSpan.FromSeconds( 0.5 + (0.25 * (int)Circle) );

			int fc = RSCA.GetFC( Caster );

			if ( fc > 6 )
				fc = 6;

			if ( ProtectionSpell.Registry.Contains( Caster ) )
				fc -= 2;

			int circleDelay = CastDelayCircleScalar * (1 + (int)Circle); // Note: Circle is 0-based so we must offset
			int fcDelay = -(CastDelayFastScalar * fc);

			int delay = CastDelayBase + circleDelay + fcDelay;

			if ( delay < CastDelayMinimum )
				delay = CastDelayMinimum;

			return TimeSpan.FromSeconds( (double)delay / CastDelayPerSecond );
		}
		public override bool ConsumeReagents()
		{
			if ( Scroll != null || !Caster.Player )
				return true;

			Container pack = Caster.Backpack;

			if ( pack == null )
				return false;

			if ( pack.ConsumeTotal( Info.Reagents, Info.Amounts ) == -1 )
				return true;

			if ( GetType().BaseType == typeof( RuneSpell ) )
			{
				if ( RunicItem.ConsumeCharges( Caster, 1+(int)Circle ) )
					return true;
			}

			return false;
		}*/

		private static int[] m_RuneManaTable = new int[]{ 0, 0, 0, 14, 18, 24, 29, 34 };

		public override int GetMana()
		{
			if ( Scroll is BaseWand )
				return 0;

			return m_RuneManaTable[(int)Circle];
		}
		public override void DoFizzle()
		{
			Caster.PlaySound( 776 );
			Caster.NextSpellTime = Core.TickCount;
		}
		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 777 );
		}
		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 777 );
		}
		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}
		public override bool CheckSequence()
		{
			int mana = ScaleMana( GetMana() );

			if ( Caster.Deleted || !Caster.Alive || Caster.Spell != this || State != SpellState.Sequencing )
			{
				DoFizzle();
			}
			else if ( Scroll != null && !(Scroll is Runebook) && (Scroll.Amount <= 0 || Scroll.Deleted || Scroll.RootParent != Caster || (Scroll is BaseWand && (((BaseWand)Scroll).Charges <= 0 || Scroll.Parent != Caster))) )
			{
				DoFizzle();
			}
			else if ( !ConsumeReagents() )
			{
				Caster.PublicOverheadMessage( MessageType.Regular, 0x22, false, "More gems are needed for this spell." );
			}
			else if ( Caster.Mana < mana )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502625 ); // Insufficient mana for this spell.
			}
			else if ( Core.AOS && (Caster.Frozen || Caster.Paralyzed) )
			{
				Caster.SendLocalizedMessage( 502646 ); // You cannot cast a spell while frozen.
				DoFizzle();
			}
			else if ( CheckFizzle() )
			{
				Caster.Mana -= mana;

				if ( Scroll is SpellScroll )
					Scroll.Consume();
				else if ( Scroll is BaseWand )
					((BaseWand)Scroll).ConsumeCharge( Caster );

				if ( Scroll is BaseWand )
				{
					bool m = Scroll.Movable;

					Scroll.Movable = false;

					if ( ClearHandsOnCast )
						Caster.ClearHands();

					Scroll.Movable = m;
				}
				else
				{
					if ( ClearHandsOnCast )
						Caster.ClearHands();
				}

				int karma = ComputeKarmaAward();

				if ( karma != 0 )
					Misc.Titles.AwardKarma( Caster, karma, true );

				if ( TransformationSpellHelper.UnderTransformation( Caster, typeof( VampiricEmbraceSpell ) ) )
				{
					bool garlic = false;

					for ( int i = 0; !garlic && i < Info.Reagents.Length; ++i )
						garlic = ( Info.Reagents[i] == Reagent.Garlic );

					if ( garlic )
					{
						Caster.SendLocalizedMessage( 1061651 ); // The garlic burns you!
						AOS.Damage( Caster, Utility.RandomMinMax( 17, 23 ), 100, 0, 0, 0, 0 );
					}
				}

				return true;
			}
			else
			{
				DoFizzle();
			}

			return false;
		}
		public virtual void GainExp()
		{
			if ( Rune.Level < 5 )
			{
				Rune.Exp += Utility.RandomMinMax( RuneGainMin, RuneGainMax );
				if ( Rune.Exp >= Rune.NextLevelExp )
				{
					LevelUp();
				}
			}
		}
		public virtual void LevelUp()
		{
			if ( Rune.Exp >= Rune.NextLevelExp )
			{
				Rune.LastLevelExp = Rune.NextLevelExp;
				int NEWNextLevelExp = (int)(( ((Rune.LastLevelExp*0.15)*(Rune.Level+1))+Rune.LastLevelExp )+Rune.LastLevelExp);
				Rune.Level += 1;
				Rune.NextLevelExp = NEWNextLevelExp;
				Caster.Say( Caster.Name+"'s "+Rune.Type+" Rune has gained a level." );
				Caster.PlaySound( 61 );
			}
		}
	}
}

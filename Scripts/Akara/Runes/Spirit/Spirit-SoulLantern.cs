using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells
{
	public class SoulLantern : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Soul Lantern", "Sphir Binl",
				212,
				9041,
				GetSpellGems.GetGems( typeof(SoulLantern) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public override int RuneGainMax{ get{ return 40; } }
		public override int RuneGainMin{ get{ return 20; } }

		public SoulLantern( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Target a creature to summon the lantern to." );
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
			else if ( !m.CanBeginAction( typeof(SoulLantern) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
			}
			else if ( CheckSequence() )
			{
				if ( m.Player || m is BaseCreature ) //Allow use on Creature too.
				{
					GainExp();
					TimeSpan duration = TimeSpan.FromMinutes( (Caster.Skills[SkillName.Inscribe].Value / 5.0 * RuneSpell.TMRG)+ImperialOrb.IncD( Caster, this ) );
					duration += TimeSpan.FromMinutes( RSCA.CalculateLLS(Caster, Caster.Skills[SkillName.Inscribe].Value / 5.0) );
					m.BeginAction( typeof( SoulLantern ) );

					m.AddToBackpack( new InternalItem( this, Caster, m, Rune, duration ) );
				}
			}
			FinishSequence();
		}

		private class InternalItem : Item
		{
			public Mobile m_Owner;
			public DateTime m_Expire;
			public Timer m_Timer;
			public SpellRune Rune;
			public Mobile m_Caster;
			public int Souls;
			private RuneSpell m_Spell;

			[Constructable]
			public InternalItem( RuneSpell spell, Mobile caster, Mobile owner, SpellRune rune, TimeSpan duration ) : base( 0xA25 )
			{
				m_Spell = spell;
				m_Caster = caster;
				Rune = rune;
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.TwoHanded;
				Hue = 1109;
				BlessedFor = owner;
				Name = "Soul Lantern"; //fixed typo 9/28/2015

				m_Expire = DateTime.Now + duration;
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();
				m_Owner.SendMessage( "The lantern is absorbed by the souls within it." );
				m_Owner.EndAction( typeof( SoulLantern ) );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				if ( Souls > 0 )
				{
					if ( 0.5 >= Utility.RandomDouble() )
					{
						int toHeal = (int)(Souls*runelevel * RuneSpell.TMRG);
						toHeal += RSCA.CalculateSI(m_Caster, toHeal);
						toHeal += ImperialOrb.IncI( m_Caster, m_Spell );
						m_Owner.Heal( toHeal );
						m_Owner.FixedEffect( 0x37C4, 10, 42, 4, 3 );
						m_Owner.PlaySound( Utility.RandomList( 696, 697, 698, 699, 700 ) );
					}
					else
					{
						ArrayList targets = new ArrayList();

						foreach ( Mobile m in m_Owner.GetMobilesInRange( Rune.Level ) )
						{
							if ( m != m_Caster && m != m_Owner && SpellHelper.ValidIndirectTarget( m_Owner, m ) && m_Owner.CanBeHarmful( m, false ) )
							{
								targets.Add( m );
							}
						}
						foreach ( Mobile m in targets ) 
						{
							int damage = (int)(Souls * runelevel * RuneSpell.TMRG);
							damage += RSCA.CalculateSI(m_Caster, damage);
							damage += ImperialOrb.IncI( m_Caster, m_Spell );
							m.FixedEffect( 0x37C4, 10, 42, 4, 3 );
							m.PlaySound( Utility.RandomList( 696, 697, 698, 699, 700 ) );
							SpellHelper.Damage( TimeSpan.Zero, m, m_Owner, damage, 20, 20, 20, 20, 20 );
						}
					}
				}

				base.OnDelete();
			}

			public override void OnDoubleClick( Mobile from )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}
				from.Target = new InternalTarget2( m_Spell, this );
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

				if ( e.Speech.ToLower().IndexOf( "collect souls".ToLower() ) >= 0 )
				{
					Use( from );
				}
			}

			public virtual void Use( Mobile from )
			{
				if ( !IsChildOf( from ) )
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
					return;
				}
				else if ( m_Owner != from )
				{
					from.SendMessage( "You are not the owner of this lantern, you may not use it." );
					return;
				}

				ArrayList corpses = new ArrayList();

				Map map = from.Map;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetObjectsInRange( from.Location, Rune.Level );

					foreach ( object o in eable )
					{
						if ( o is Item )
						{
							Item item = (Item)o;
							if ( item is Corpse )
								corpses.Add( (Corpse)item );
						}
					}

					eable.Free();
				}

				int count = 0;
				int count2 = 0;
				for ( int i = 0; i < corpses.Count; ++i )
				{
					Corpse c = (Corpse)corpses[i];

					if ( c != null )
					{
						if ( !c.Channeled )
						{
							c.Channeled = true;
							Souls += 1;
							count++;
						}
						else
							count2++;
					}
				}
				if ( count > 0 )
					from.SendMessage( count +" corpse's soul"+(count > 1 ? "s " : " " )+"ha"+(count > 1 ? "ve" : "s" )+" been taken." );
				if ( count2 > 0 )
					from.SendMessage( count2 +" corpse's soul"+(count2 > 1 ? "s " : " " )+"ha"+(count2 > 1 ? "ve" : "s" )+" already been taken." );
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return base.CanEquip( m );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
				writer.Write( Rune );
				writer.Write( Souls );
				writer.Write( m_Caster );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();
				Rune = reader.ReadItem() as SpellRune;
				Souls = reader.ReadInt();
				m_Caster = reader.ReadMobile();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}
		private class InternalTimer : Timer
		{
			private InternalItem m_Item;
			private DateTime m_Expire;

			public InternalTimer( InternalItem item, DateTime expire ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Item = item;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					m_Item.Delete();
					Stop();
				}
			}
		}
		private class InternalTarget2 : Target
		{
			private InternalItem m_Item;
			private RuneSpell m_Spell;
			private Mobile m_Caster;

			public InternalTarget2( RuneSpell spell, InternalItem item ) : base( 12, true, TargetFlags.None )
			{
				m_Item = item;
				m_Spell = spell;
				m_Item.m_Caster = m_Caster;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					Mobile m = (Mobile)o;

					int runelevel = m_Item.Rune.Level;
					if ( runelevel <= 0 )
						runelevel = 1;
					if ( m == from || m == m_Caster )
					{
						if ( m_Item.Souls > 0 )
						{
							int toHeal = (int)(m_Item.Souls*runelevel * RuneSpell.TMRG);
							toHeal += RSCA.CalculateSI(m_Caster, toHeal);
							toHeal += ImperialOrb.IncI( m_Caster, m_Spell );
							m.Heal( toHeal );
							m_Item.Souls = 0;
							m.FixedEffect( 0x37C4, 10, 42, 4, 3 );
							m.PlaySound( Utility.RandomList( 696, 697, 698, 699, 700 ) );
						}
					}
					else if ( SpellHelper.ValidIndirectTarget( m_Item.m_Owner, m ) && m_Item.m_Owner.CanBeHarmful( m, false ) )
					{
						if ( m_Item.Souls > 0 )
						{
							int damage = (int)(m_Item.Souls * runelevel * RuneSpell.TMRG);
							damage += RSCA.CalculateSI(m_Caster, damage);
							damage += ImperialOrb.IncI( m_Caster, m_Spell );
							m_Item.Souls = 0;
							m.FixedEffect( 0x37C4, 10, 42, 4, 3 );
							m.PlaySound( Utility.RandomList( 696, 697, 698, 699, 700 ) );
							SpellHelper.Damage( TimeSpan.Zero, m, from, damage, 20, 20, 20, 20, 20 );
						}
					}
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
			}
		}	
		private class InternalTarget : Target
		{
			private SoulLantern m_Owner;

			public InternalTarget( SoulLantern owner ) : base( 12, true, TargetFlags.None )
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
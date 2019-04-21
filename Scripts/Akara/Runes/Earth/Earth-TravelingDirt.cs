using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Misc;

namespace Server.Spells
{
	public class TravelingDirt : RuneSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Lightning Strike", "Cadre Vrix",
				203,
				9041,
				GetSpellGems.GetGems( typeof(TravelingDirt) )
			);
		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public override int RuneGainMax{ get{ return 60; } }
		public override int RuneGainMin{ get{ return 30; } }

		public TravelingDirt( Mobile caster, Item scroll, SpellRune rune ) : base( caster, scroll, m_Info, rune )
		{
		}

		public override void OnCast()
		{
			if ( Rune == null )
			{
				Caster.SendMessage( "There is no rune to cast this from." );
			}
			else if ( CheckSequence() )
			{
				GainExp();
				Caster.SendMessage( "You create a piece of traveling dirt." );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;

				int i = 0;
				while( i < runelevel )
				{
					Caster.AddToBackpack( new InternalItem(this, Rune, Caster) );
					i++;
				}
			}
			FinishSequence();
		}
		[DispellableField]
		private class InternalItem : Item
		{
			private Mobile m_Caster;
			private SpellRune Rune;
			private TravelingDirt m_Spell;
			private ArrayList m_Items = new ArrayList();

			public new ArrayList Items{ get{ return m_Items; } set{ m_Items = value; } }

			public InternalItem( TravelingDirt spell, SpellRune rune, Mobile caster ) : base( 3969 )
			{
				m_Spell = spell;
				Hue = 1149;
				Rune = rune;
				m_Caster = caster;
				Name = "Traveling Dirt";
			}

			public override void OnDoubleClick( Mobile from )
			{
				if ( Rune == null )
				{
					Delete();
					return;
				}

				if ( IsChildOf( from ) )
					from.SendGump( new TravelingDirtGump( from, m_Caster, this, Rune, m_Spell, Items ) );
				else
					from.SendMessage( "This must be in your backpack to use." );
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
						break;
					}
				}
			}
		}
		private class TravelingDirtGump : Gump
		{
			private Mobile m_Caster;
			private Mobile m_From;
			private InternalItem m_Dirt;
			private TravelingDirt m_Spell;
			private SpellRune Rune;
			private ArrayList Items;

			public TravelingDirtGump( Mobile from, Mobile caster, InternalItem dirt, SpellRune rune, TravelingDirt spell, ArrayList items ) : base( 25, 50 )
			{
				from.CloseGump( typeof( TravelingDirtGump ) );
				m_From = from;
				m_Caster = caster;
				m_Dirt = dirt;
				m_Spell = spell;
				Rune = rune;
				Items = items;

				AddPage( 0 );

				AddBackground( 0, 0, 420, 210, 5054 );

				AddImageTiled( 10, 10, 400, 190, 2624 );
				AddAlphaRegion( 10, 10, 400, 190 );

				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int max = runelevel;
				max += RSCA.CalculateSI(m_Caster, max);

				max += ImperialOrb.IncI( m_Caster, m_Spell );

				AddLabel( 30, 30, 1152, "Traveling Dirt will send all the items you add to it to your" );
				AddLabel( 30, 50, 1152, "bank, and will send all pets you add to it to your stables." );
				AddLabel( 30, 80, 1152, "Total Items: "+Items.Count+"/"+max  );

				AddButton( 100, 100, 4005, 4007, 1, GumpButtonType.Reply, 0 );
				AddLabel( 130, 100, 1152, "Add Item" );
				AddButton( 100, 120, 4005, 4007, 2, GumpButtonType.Reply, 0 );
				AddLabel( 130, 120, 1152, "Clear Items" );
				AddButton( 100, 140, 4005, 4007, 3, GumpButtonType.Reply, 0 );
				AddLabel( 130, 140, 1152, "Bury" );
				AddButton( 100, 170, 4005, 4007, 0, GumpButtonType.Reply, 0 );
				AddLabel( 130, 170, 1152, "Close" );
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int max = runelevel;
				max += RSCA.CalculateSI(m_Caster, max);;

				if ( info.ButtonID == 1 )
				{
					if ( Items.Count < max )
					{
						m_From.Target = new InternalTarget( m_Caster, m_Dirt, Rune, m_Spell, Items );
						m_From.SendMessage( "Target an item to add to the dirt." );
					}
					else
						m_From.SendMessage( "The dirt is full, it cannot hold anything else." );
					m_From.SendGump( new TravelingDirtGump( m_From, m_Caster, m_Dirt, Rune, m_Spell, Items ) );
				}
				if ( info.ButtonID == 2 )
				{
					m_From.SendMessage( "You clear the dirt of all the items you added to it." );
					m_From.SendGump( new TravelingDirtGump( m_From, m_Caster, m_Dirt, Rune, m_Spell, new ArrayList() ) );
				}
				if ( m_Dirt != null )
					m_Dirt.Items = Items;
				if ( info.ButtonID == 3 )
				{
					m_From.SendMessage( "You bury the dirt, the items will be sent to your bank." );

					ArrayList list = Items;
					for ( int i = 0; i < list.Count; ++i )
					{
						object o = list[i];
						if ( o is Item )
						{
							Item item = (Item)o;
							if ( item.IsChildOf( m_From ) )
							{
								m_From.BankBox.DropItem( item );
							}
						}
						else if ( o is Mobile )
						{
							Mobile m = (Mobile)o;

							if ( m is BaseCreature )
							{
								BaseCreature pet = (BaseCreature)m;

								if ( pet.ControlMaster == m_From && pet.Controlled )
								{
									pet.ControlTarget = null;
									pet.ControlOrder = OrderType.Stay;
									pet.Internalize();

									pet.SetControlMaster( null );
									pet.SummonMaster = null;

									pet.IsStabled = true;
									m_From.Stabled.Add( pet );
								}
							}
						}
					}
					m_Dirt.Delete();
				}
			}
		}
		private class InternalTarget : Target
		{
			private Mobile m_Caster;
			private InternalItem m_Dirt;
			private TravelingDirt m_Spell;
			private SpellRune Rune;
			private ArrayList Items;

			public InternalTarget( Mobile caster, InternalItem dirt, SpellRune rune, TravelingDirt spell, ArrayList items ) : base( 12, true, TargetFlags.None )
			{
				m_Caster = caster;
				m_Dirt = dirt;
				m_Spell = spell;
				Rune = rune;
				Items = items;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				int runelevel = Rune.Level;
				if ( runelevel <= 0 )
					runelevel = 1;
				int max = (int)(runelevel*(m_Caster.Skills[SkillName.Inscribe].Value/40));
				max += RSCA.CalculateSI(m_Caster, max);;

				if ( Items.Count >= max )
					return;

				if ( o is Item )
				{
					Item item = (Item)o;
					if ( item.IsChildOf( from ) )
					{
						if ( !Items.Contains( item ) )
							Items.Add( item );
						else
							from.SendMessage( "This item has already been added to the dirt." );
					}
					else
						from.SendMessage( "The item must be in your backpack to add it to the dirt." );

					from.SendGump( new TravelingDirtGump( from, m_Caster, m_Dirt, Rune, m_Spell, Items ) );
				}
				if ( o is BaseCreature )
				{
					BaseCreature pet = (BaseCreature)o;
					if ( pet.ControlMaster == from && pet.Controlled )
					{
						if ( !Items.Contains( pet ) )
							Items.Add( pet );
						else
							from.SendMessage( "This creature has already been added to the dirt." );
					}
					else
						from.SendMessage( "You do not control this creature." );

					from.SendGump( new TravelingDirtGump( from, m_Caster, m_Dirt, Rune, m_Spell, Items ) );
				}
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
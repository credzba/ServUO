using System; 
using System.Collections; 
using System.Collections.Generic; 
using Server.Spells;
using Server.Items; 
using Server.Targeting; 

namespace Server.Items 
{ 
   	public abstract class RunicItem: Item 
   	{ 
		//Special Attributes
		private int m_LongerLastingSpells;
		private int m_SpellIncrease;
		private int m_InscribeInc;
		private int m_FasterCasting;
		private int m_FasterCastRecovery;
		private int m_Charges;
		private int m_MaxCharges;

		//Special Attributes
		[CommandProperty( AccessLevel.GameMaster )]
		public int LongerLastingSpells{ get{ return m_LongerLastingSpells; } set{ m_LongerLastingSpells = value;InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int SpellIncrease{ get{ return m_SpellIncrease; } set{ m_SpellIncrease = value;InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int InscribeInc{ get{ return m_InscribeInc; } set{ m_InscribeInc = value;InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int FasterCasting{ get{ return m_FasterCasting; } set{ m_FasterCasting = value;InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int FasterCastRecovery{ get{ return m_FasterCastRecovery; } set{ m_FasterCastRecovery = value;InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxCharges{ get{ return m_MaxCharges; } set{ m_MaxCharges = value;InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get{ return m_Charges; }
			set
			{
				m_Charges = value;
				if ( m_Charges > MaxCharges )
					m_Charges = MaxCharges;
				InvalidateProperties();
			}
		}

		[Constructable]
		public RunicItem() : this( false, 0, 0 )
		{
			Weight = 0.0;
		}

		[Constructable]
		public RunicItem( bool set, int amount, int itemID ) : base( itemID )
		{
			Weight = 0.0;
			if ( set )
				SetAttributes(amount);
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( LongerLastingSpells > 0 )
				list.Add( 1060658, "Longer Lasting Spells\t{0}%", LongerLastingSpells );
			if ( SpellIncrease > 0 )
				list.Add( 1060659, "Spell Increase\t{0}%", SpellIncrease );
			if ( InscribeInc > 0 )
				list.Add( 1060660, "Inscribe Increase\t{0}", InscribeInc );

			if ( FasterCastRecovery > 0 )
				list.Add( 1060412, FasterCastRecovery.ToString() ); 
			if ( FasterCasting > 0 )
				list.Add( 1060413, FasterCasting.ToString() );

			list.Add( 1060661, "Charges\t{0} / {1}", Charges, MaxCharges );
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.Target = new AddGemTarget( this );
			from.SendMessage( "Target a gem to add charges to this runic item." );
		}

		public virtual void SetAttributes( int level )
		{
			if ( level > 5 )
				level = 5;
			else if ( level < 1 )
				level = 1;

			int amount = Utility.RandomMinMax( level*2, level*4 );

			while( amount > 0 )
			{
				switch ( Utility.Random( 16 ) )
				{
					case 0: LongerLastingSpells++;amount--;break;
					case 1: SpellIncrease++;amount--;break;
					case 2: InscribeInc++;amount--;break;
					case 3: if(FasterCasting<1)FasterCasting++;amount--;break;
					case 4: InscribeInc++;amount--;break;
					case 5: LongerLastingSpells++;amount--;break;
					case 6: if(FasterCastRecovery<2)FasterCastRecovery++;amount--;break;
					case 7: LongerLastingSpells++;amount--;break;
					case 8: SpellIncrease++;amount--;break;
					case 9: InscribeInc++;amount--;break;
					case 10: LongerLastingSpells++;amount--;break;
					case 11: LongerLastingSpells++;amount--;break;
					case 12: InscribeInc++;amount--;break;
					case 13: LongerLastingSpells++;amount--;break;
					case 14: InscribeInc++;amount--;break;
					case 15: SpellIncrease++;amount--;break;
				}
			}
		}

		public static bool ConsumeCharges( Mobile from, int amount )
		{
			List<Item> items = from.Items;
			int avail = 0;

			for ( int i = 0; i < items.Count; ++i )
			{
				object obj = items[i];

				if ( obj is RunicItem )
				{
					RunicItem ri = (RunicItem)obj;

					if ( ri.Parent == from )
						avail += ri.Charges;
				}
			}

			if ( avail < amount )
				return false;

			List<Item> items2 = from.Items;

			for ( int i = 0; i < items2.Count; ++i )
			{
				object obj = items2[i];

				if ( obj is RunicItem )
				{
					RunicItem ir = (RunicItem)obj;

					if ( ir.Charges > amount )
					{
						ir.Charges -= amount;
						break;
					}
					else
					{
						amount -= ir.Charges;
						ir.Charges = 0;
					}
				}
			}

			return true;
		}

		private SkillMod m_SkillMod;
		public override bool OnEquip( Mobile from )
		{
			m_SkillMod = new DefaultSkillMod( SkillName.Inscribe, true, InscribeInc );
			from.AddSkillMod( m_SkillMod );
			return base.OnEquip( from );
		}

		public override void OnRemoved( object parent )
		{
			if ( parent is Mobile )
			{
				Mobile from = (Mobile) parent;

				if ( m_SkillMod != null )
					m_SkillMod.Remove();
			}

			base.OnRemoved( parent );
		}

            	public RunicItem( Serial serial ) : base ( serial ) 
            	{             
           	} 

           	public override void Serialize( GenericWriter writer ) 
           	{ 
              		base.Serialize( writer ); 
              		writer.Write( (int) 0 ); 
              		writer.Write( (int) m_LongerLastingSpells ); 
              		writer.Write( (int) m_SpellIncrease ); 
              		writer.Write( (int) m_InscribeInc ); 
              		writer.Write( (int) m_FasterCasting ); 
              		writer.Write( (int) m_FasterCastRecovery ); 
              		writer.Write( (int) m_Charges ); 
              		writer.Write( (int) m_MaxCharges ); 
           	} 
            
           	public override void Deserialize( GenericReader reader ) 
           	{ 
              		base.Deserialize( reader ); 
              		int version = reader.ReadInt(); 
              		m_LongerLastingSpells = reader.ReadInt(); 
              		m_SpellIncrease = reader.ReadInt(); 
              		m_InscribeInc = reader.ReadInt(); 
              		m_FasterCasting = reader.ReadInt(); 
              		m_FasterCastRecovery = reader.ReadInt(); 
              		m_Charges = reader.ReadInt(); 
              		m_MaxCharges = reader.ReadInt(); 
           	} 
        } 
	public class AddGemTarget : Target
	{
		private RunicItem m_RI;

		public AddGemTarget( RunicItem ri ) : base( 0, false, TargetFlags.None )
		{
			m_RI = ri;
		}

		protected override void OnTarget( Mobile from, object o )
		{
			if ( o is Amber || o is Amethyst || o is Citrine || o is Diamond || o is Emerald || o is Ruby || o is Sapphire || o is StarSapphire || o is Tourmaline )
			{
				Item item = (Item)o;

				if ( item.IsChildOf( from ) )
				{
					if ( m_RI.MaxCharges >= m_RI.Charges )
					{
						int needed = m_RI.MaxCharges-m_RI.Charges;

						if ( item.Amount <= needed )
						{
							m_RI.Charges += item.Amount;
							item.Delete();
							from.SendMessage( "The runic item absorbs the gem." );
						}
						else
						{
							m_RI.Charges += needed;
							item.Amount -= needed;
							from.SendMessage( "The runic item absorbs the gem." );
						}
					}
					else
						from.SendMessage( "This runic item has max charges." );
				}
				else
					from.SendMessage( "The gem must be in your backpack to this." );
			}
			else
				from.SendMessage( "That is not a gem." );
		}
	}
} 
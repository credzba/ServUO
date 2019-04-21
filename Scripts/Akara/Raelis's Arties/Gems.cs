using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Gumps;

namespace Server.Items
{
	[PropertyObject]
	public class Gems
	{
		private double m_Citrine;
		private double m_Amber;
		private double m_Ruby;
		private double m_Tourmaline;
		private double m_Amethyst;
		private double m_Emerald;
		private double m_Sapphire;
		private double m_StarSapphire;
		private double m_Diamond;

		[CommandProperty( AccessLevel.GameMaster )]
		public double Citrine{ get{ return m_Citrine; } set{ m_Citrine = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Amber{ get{ return m_Amber; } set{ m_Amber = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Ruby{ get{ return m_Ruby; } set{ m_Ruby = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Tourmaline{ get{ return m_Tourmaline; } set{ m_Tourmaline = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Amethyst{ get{ return m_Amethyst; } set{ m_Amethyst = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Emerald{ get{ return m_Emerald; } set{ m_Emerald = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Sapphire{ get{ return m_Sapphire; } set{ m_Sapphire = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double StarSapphire{ get{ return m_StarSapphire; } set{ m_StarSapphire = value;if(Owner != null)Owner.InvalidateProperties(); } }
		[CommandProperty( AccessLevel.GameMaster )]
		public double Diamond{ get{ return m_Diamond; } set{ m_Diamond = value;if(Owner != null)Owner.InvalidateProperties(); } }

		private Item m_Owner;
		public Item Owner{ get{ return m_Owner; } set{ m_Owner = value; } }

		public Gems()
		{
		}

		public Gems( Item owner )
		{
			m_Owner = owner;
		}

		public int GetPower()
		{
			return (int)((Citrine)+(Amber)+(Ruby)+(Tourmaline)+(Amethyst)+(Emerald)+(Sapphire)+(StarSapphire)+(Diamond));
		}

		public bool CheckConsumePower( double amount )
		{
			if ( amount <= (double)GetPower() )
				return true;

			return false;
		}

		public void ConsumePower( double amount )
		{
			if ( amount <= 0.0 )
				return;

			if ( amount > GetPower() )
			{
				Citrine = 0;
				Amber = 0;
				Ruby = 0;
				Tourmaline = 0;
				Amethyst = 0;
				Emerald = 0;
				Sapphire = 0;
				StarSapphire = 0;
				Diamond = 0;

				return;
			}

			while( amount > 0.0 )
			{
				double rnd = Utility.RandomDouble();
				switch( Utility.Random( 9 ) )
				{
					case 0:
					{
						if ( Citrine >= rnd )
						{
							Citrine -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 1:
					{
						if ( Amber >= rnd )
						{
							Amber -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 2:
					{
						if ( Ruby >= rnd )
						{
							Ruby -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 3:
					{
						if ( Tourmaline >= rnd )
						{
							Tourmaline -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 4:
					{
						if ( Amethyst >= rnd )
						{
							Amethyst -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 5:
					{
						if ( Emerald >= rnd )
						{
							Emerald -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 6:
					{
						if ( Sapphire >= rnd )
						{
							Sapphire -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 7:
					{
						if ( StarSapphire >= rnd )
						{
							StarSapphire -= rnd;
							amount -= rnd;
						}
						break;
					}
					case 8:
					{
						if ( Diamond >= rnd )
						{
							Diamond -= rnd;
							amount -= rnd;
						}
						break;
					}
				}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 ); //version

			writer.Write( m_Owner );
			writer.Write( (double) m_Citrine );
			writer.Write( (double) m_Amber );
			writer.Write( (double) m_Ruby );
			writer.Write( (double) m_Tourmaline );
			writer.Write( (double) m_Amethyst );
			writer.Write( (double) m_Emerald );
			writer.Write( (double) m_Sapphire );
			writer.Write( (double) m_StarSapphire );
			writer.Write( (double) m_Diamond );
		}

		public void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
				{
					m_Owner = reader.ReadItem();
					m_Citrine = reader.ReadDouble();
					m_Amber = reader.ReadDouble();
					m_Ruby = reader.ReadDouble();
					m_Tourmaline = reader.ReadDouble();
					m_Amethyst = reader.ReadDouble();
					m_Emerald = reader.ReadDouble();
					m_Sapphire = reader.ReadDouble();
					m_StarSapphire = reader.ReadDouble();
					m_Diamond = reader.ReadDouble();

					break;
				}
			}
		}
	}
}
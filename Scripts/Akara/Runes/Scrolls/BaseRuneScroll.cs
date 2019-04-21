using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseRuneScroll : Item
	{
		private int m_Level;
		private RuneType m_Type;

		private static Type[] m_Types = new Type[44];

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level{ get{ return m_Level; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public RuneType Type{ get{ return m_Type; } }

		[Constructable]
		public BaseRuneScroll() : this( 1 )
		{
		}

		[Constructable]
		public BaseRuneScroll( int amount, int level, RuneType type, int itemid ) : base( itemid )
		{
			Stackable = true;
			Amount = amount;
			m_Type = type;
			m_Level = level;
		}

		public BaseRuneScroll( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (int)m_Level );
			writer.Write( (int)m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Level = reader.ReadInt();
			m_Type = (RuneType)reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
		}
	}
}
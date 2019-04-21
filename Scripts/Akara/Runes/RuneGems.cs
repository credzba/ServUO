using System;
using Server.Items;

namespace Server.Spells
{
	public class RuneGems
	{
		private static Type[] m_Types = new Type[9]
			{
				typeof( Amber ),
				typeof( Amethyst ),
				typeof( Citrine ),
				typeof( Diamond ),
				typeof( Emerald ),
				typeof( Ruby ),
				typeof( Sapphire ),
				typeof( StarSapphire ),
				typeof( Tourmaline )
			};

		public Type[] Types
		{
			get{ return m_Types; }
		}

		public static Type Amber
		{
			get{ return m_Types[0]; }
			set{ m_Types[0] = value; }
		}
		public static Type Amethyst
		{
			get{ return m_Types[1]; }
			set{ m_Types[1] = value; }
		}
		public static Type Citrine
		{
			get{ return m_Types[2]; }
			set{ m_Types[2] = value; }
		}
		public static Type Diamond
		{
			get{ return m_Types[3]; }
			set{ m_Types[3] = value; }
		}
		public static Type Emerald
		{
			get{ return m_Types[4]; }
			set{ m_Types[4] = value; }
		}
		public static Type Ruby
		{
			get{ return m_Types[5]; }
			set{ m_Types[5] = value; }
		}
		public static Type Sapphire
		{
			get{ return m_Types[6]; }
			set{ m_Types[6] = value; }
		}
		public static Type StarSapphire
		{
			get{ return m_Types[7]; }
			set{ m_Types[7] = value; }
		}
		public static Type Tourmaline
		{
			get{ return m_Types[8]; }
			set{ m_Types[8] = value; }
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Spells
{
	public class RSCA
	{
		public static int CalculateSI( Mobile from, int amount )
		{
			if ( from == null )
				return 0;

			List<Item> items = from.Items;
			int si = 0;

			for ( int i = 0; i < items.Count; ++i )
			{
				object obj = items[i];

				if ( obj is RunicItem )
				{
					RunicItem ri = (RunicItem)obj;

					if ( ri.Parent == from )
						si += ri.SpellIncrease;
				}
			}

			int bonus = amount * si;

			return (int)(bonus / 100);
		}
		public static double CalculateLLS( Mobile from, double amount )
		{
			if ( from == null )
				return 0.0;

			List<Item> items = from.Items;
			int lls = 0;

			for ( int i = 0; i < items.Count; ++i )
			{
				object obj = items[i];

				if ( obj is RunicItem )
				{
					RunicItem ri = (RunicItem)obj;

					if ( ri.Parent == from )
						lls+= ri.LongerLastingSpells;
				}
			}

			double i1 = (amount / 100)*lls;

			return i1;
		}
		public static int GetFC( Mobile from )
		{
			List<Item> items = from.Items;
			int fc = 0;

			for ( int i = 0; i < items.Count; ++i )
			{
				object obj = items[i];

				if ( obj is RunicItem )
				{
					RunicItem ri = (RunicItem)obj;

					if ( ri.Parent == from )
						fc+= ri.FasterCasting;
				}
			}

			return fc;
		}
		public static int GetFCR( Mobile from )
		{
			List<Item> items = from.Items;
			int fcr = 0;

			for ( int i = 0; i < items.Count; ++i )
			{
				object obj = items[i];

				if ( obj is RunicItem )
				{
					RunicItem ri = (RunicItem)obj;

					if ( ri.Parent == from )
						fcr+= ri.FasterCastRecovery;
				}
			}

			return fcr;
		}
	}
}
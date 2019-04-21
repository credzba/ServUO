using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RuneBag : Bag
	{
		public RuneBag() : this( 5, true )
		{
		}

		public RuneBag( int level, bool addspells )
		{
			Name = "Rune Bag";

			SpellRune sr1 = new SpellRune( RuneType.Fire );
			SpellRune sr2 = new SpellRune( RuneType.Water );
			SpellRune sr3 = new SpellRune( RuneType.Wind );
			SpellRune sr4 = new SpellRune( RuneType.Earth );
			SpellRune sr5 = new SpellRune( RuneType.Spirit );
			SpellRune sr6 = new SpellRune( RuneType.Lightning );
			SpellRune sr7 = new SpellRune( RuneType.Toxic );
			SpellRune sr8 = new SpellRune( RuneType.Ice );
			SpellRune sr9 = new SpellRune( RuneType.Void );

			sr1.Level = level;
			sr2.Level = level;
			sr3.Level = level;
			sr4.Level = level;
			sr5.Level = level;
			sr6.Level = level;
			sr7.Level = level;
			sr8.Level = level;
			sr9.Level = level;

			if ( addspells )
			{
				sr1.Spell1 = sr1.Spell2 = sr1.Spell3 = sr1.Spell4 = sr1.Spell5 = true;
				sr2.Spell1 = sr2.Spell2 = sr2.Spell3 = sr2.Spell4 = sr2.Spell5 = true;
				sr3.Spell1 = sr3.Spell2 = sr3.Spell3 = sr3.Spell4 = sr3.Spell5 = true;
				sr4.Spell1 = sr4.Spell2 = sr4.Spell3 = sr4.Spell4 = sr4.Spell5 = true;
				sr5.Spell1 = sr5.Spell2 = sr5.Spell3 = sr5.Spell4 = sr5.Spell5 = true;
				sr6.Spell1 = sr6.Spell2 = sr6.Spell3 = sr6.Spell4 = sr6.Spell5 = true;
				sr7.Spell1 = sr7.Spell2 = sr7.Spell3 = sr7.Spell4 = sr7.Spell5 = true;
				sr8.Spell1 = sr8.Spell2 = sr8.Spell3 = sr8.Spell4 = sr8.Spell5 = true;
				sr9.Spell1 = sr9.Spell2 = sr9.Spell3 = sr9.Spell4 = sr9.Spell5 = true;
			}

			DropItem( sr1 );
			DropItem( sr2 );
			DropItem( sr3 );
			DropItem( sr4 );
			DropItem( sr5 );
			DropItem( sr6 );
			DropItem( sr7 );
			DropItem( sr8 );
			DropItem( sr9 );
		}

		public RuneBag( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
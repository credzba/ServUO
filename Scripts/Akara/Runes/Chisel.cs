using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	public class Chisel : Item
	{
		private int m_Uses;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Uses
		{
			get{ return m_Uses; }
			set
			{
				m_Uses = value;
				if ( value <= 0 )
					Delete();
			}
		}

		[Constructable]
		public Chisel() : this( 50 )
		{
		}

		[Constructable]
		public Chisel( int uses ) : base( 0xFB8 )
		{
			Weight = 1.0;
			Name = "Chisel";
			Hue = 1109;
			Uses = uses;
		}

		public Chisel( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.Target = new GemDiggingTarget( from, this );
			from.SendMessage( "Target some ore you would like to dig in for gems." );
		}

		public virtual int Digging( BaseOre oretype, Mobile from, int amount )
		{
			int skillbonus = (int)(from.Skills[SkillName.Mining].Base);
			int baseamount = 0;

			if ( oretype is IronOre )
				baseamount = (int)(skillbonus / 100);
			if ( oretype is DullCopperOre )
				baseamount = (int)(skillbonus / 95);
			if ( oretype is ShadowIronOre )
				baseamount = (int)(skillbonus / 90);
			if ( oretype is CopperOre )
				baseamount = (int)(skillbonus / 85);
			if ( oretype is BronzeOre )
				baseamount = (int)(skillbonus / 80);
			if ( oretype is GoldOre )
				baseamount = (int)(skillbonus / 75);
			if ( oretype is AgapiteOre )
				baseamount = (int)(skillbonus / 70);
			if ( oretype is VeriteOre )
				baseamount = (int)(skillbonus / 65);
			if ( oretype is ValoriteOre )
				baseamount = (int)(skillbonus / 60);
			if ( oretype is BlazeOre )
				baseamount = (int)(skillbonus / 55);
			if ( oretype is IceOre )
				baseamount = (int)(skillbonus / 50);
			if ( oretype is ToxicOre )
				baseamount = (int)(skillbonus / 45);
			if ( oretype is ElectrumOre )
				baseamount = (int)(skillbonus / 40);
			if ( oretype is MoonstoneOre )
				baseamount = (int)(skillbonus / 35);
			if ( oretype is BloodstoneOre )
				baseamount = (int)(skillbonus / 30);
			if ( oretype is PlatinumOre )
				baseamount = (int)(skillbonus / 25);

			int finalamount = (int)(baseamount * amount);

			return finalamount;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (int) m_Uses );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Uses = reader.ReadInt();
		}
	}

	public class GemDiggingTarget : Target
	{
		private Mobile m_Owner;
		private Chisel m_Tool;

		public GemDiggingTarget( Mobile owner, Chisel tool ) : base( 12, true, TargetFlags.None )
		{
			m_Owner = owner;
			m_Tool = tool;
		}

		protected override void OnTarget( Mobile from, object o )
		{
			if ( o is BaseOre )
			{
				BaseOre ore = (BaseOre)o;
				int amount = m_Tool.Digging( ore, from, ore.Amount );

				int skill = (int)(from.Skills[SkillName.Mining].Base/20);

				int finalskill = skill+Utility.RandomMinMax( 0, 10-skill );

				if ( finalskill < 0 )
					finalskill = 0;

				int gemc = Utility.RandomMinMax( 0, finalskill );

				Item gem = null;

				if ( gemc == 0 )
					from.SendMessage( "You fail to extract any gems." );
				if ( gemc == 1 )
					gem = new Citrine( amount );
				if ( gemc == 2 )
					gem = new Amber( amount );
				if ( gemc == 3 )
					gem = new Ruby( amount );
				if ( gemc == 4 )
					gem = new Tourmaline( amount );
				if ( gemc == 5 )
					gem = new Amethyst( amount );
				if ( gemc == 6 )
					gem = new Emerald( amount );
				if ( gemc == 7 )
					gem = new Sapphire( amount );
				if ( gemc == 8 )
					gem = new StarSapphire( amount );
				if ( gemc == 9 )
					gem = new Diamond( amount );
				if ( gemc >= 10 )
					from.SendMessage( "You fail to extract any gems." );

				string gemtype = "";

				if ( gemc == 1 )
					gemtype = "citrine";
				if ( gemc == 2 )
					gemtype = "amber";
				if ( gemc == 3 )
					gemtype = "ruby";
				if ( gemc == 4 )
					gemtype = "tourmaline";
				if ( gemc == 5 )
					gemtype = "amethyst";
				if ( gemc == 6 )
					gemtype = "emerald";
				if ( gemc == 7 )
					gemtype = "sapphire";
				if ( gemc == 8 )
					gemtype = "star sapphire";
				if ( gemc == 9 )
					gemtype = "diamond";

				if ( gem != null )
				{
					from.AddToBackpack( gem );
					from.SendMessage( "You carefully extract some "+ gemtype +" gems from the ore." );
				}

				m_Tool.Uses -= 1;
				if ( ore.Amount == 1 )
					ore.Delete();
				else if ( from.Skills[SkillName.Mining].Base >= 100 )
				{
					if ( 0.3 >= Utility.RandomDouble() )
						ore.Delete();
					else
						ore.Amount = (int)(ore.Amount / 2);
				}
				else
					ore.Delete();
			}
			else
				from.SendMessage( "That target is not valid." );
		}

		protected override void OnTargetFinish( Mobile from )
		{
		}
	}
}
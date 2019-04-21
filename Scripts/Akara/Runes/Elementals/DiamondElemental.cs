using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gem elemental corpse" )]
	public class DiamondElemental : BaseCreature
	{
		[Constructable]
		public DiamondElemental() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an diamond elemental";
			Body = 14;
			BaseSoundID = 268;

			SetStr( 240, 260 );
			SetDex( 115, 125 );
			SetInt( 71, 92 );

			SetHits( 240, 270 );

			SetDamage( 28 );

			SetDamageType( ResistanceType.Physical, 30 );
			SetDamageType( ResistanceType.Fire, 30 );
			SetDamageType( ResistanceType.Cold, 30 );
			SetDamageType( ResistanceType.Energy, 30 );

			SetResistance( ResistanceType.Physical, 75, 85 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.MagicResist, 80.0, 110.0 );
			SetSkill( SkillName.Tactics, 90.0, 130.0 );
			SetSkill( SkillName.Wrestling, 90.0, 130.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 38;

			PackItem( new Diamond( 25 ) );
		}

		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			Diamond item = new Diamond();
			bool onx = Utility.RandomBool();
			int locplus = Utility.RandomMinMax( -1, 1 );
			item.Location = Location;
			item.Map = Map;
			if ( onx )
				item.X += locplus;
			else
				item.Y += locplus;
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		private DateTime m_Next;
		private int m_Thrown;

		public override void OnActionCombat()
		{
			IDamageable combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_Next )
			{
				ThrowGem( combatant );

				m_Thrown++;

				if ( 0.75 >= Utility.RandomDouble() && (m_Thrown % 2) == 1 ) // 75% chance to quickly throw another bomb
					m_Next = DateTime.Now + TimeSpan.FromSeconds( 3.0 );
				else
					m_Next = DateTime.Now + TimeSpan.FromSeconds( 5.0 + (10.0 * Utility.RandomDouble()) ); // 5-15 seconds
			}
		}

		public void ThrowGem( IDamageable m )
		{
			DoHarmful( m );

			this.MovingParticles( m, 0xF26, 1, 0, false, false, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
            private IDamageable m_Mobile;
            private Mobile m_From;

			public InternalTimer( IDamageable m, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if ( 0.2 >= Utility.RandomDouble() )
				{
					AOS.Damage( m_Mobile, Utility.RandomMinMax( 10, 20 ), 20, 20, 20, 20, 20 );
					Diamond item = new Diamond();
					item.Location = m_From.Location;
					item.Map = m_From.Map;
				}
				else
					AOS.Damage( m_Mobile, Utility.RandomMinMax( 10, 20 ), 20, 20, 20, 20, 20 );
			}
		}

		public DiamondElemental( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
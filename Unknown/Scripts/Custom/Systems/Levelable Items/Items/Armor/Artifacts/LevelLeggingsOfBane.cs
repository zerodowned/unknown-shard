using System;
using Server;

namespace Server.Items
{
    public class LevelLeggingsOfBane : LevelChainLegs
	{
		public override int LabelNumber{ get{ return 1061100; } } // Leggings of Bane
		public override int ArtifactRarity{ get{ return 11; } }

		public override int BasePoisonResistance{ get{ return 36; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public LevelLeggingsOfBane()
		{
			Hue = 0x4F5;
			ArmorAttributes.DurabilityBonus = 100;
			Attributes.BonusStam = 8;
			Attributes.AttackChance = 20;
		}

		public LevelLeggingsOfBane( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				if ( Hue == 0x559 )
					Hue = 0x4F5;

				if ( ArmorAttributes.DurabilityBonus == 0 )
					ArmorAttributes.DurabilityBonus = 100;

				PoisonBonus = 0;
			}
		}
	}
}
using System;
using Server;

namespace Server.Items
{
	public class LevelHolyKnightsBreastplate : LevelPlateChest
	{
		public override int LabelNumber{ get{ return 1061097; } } // Holy Knight's Breastplate
		public override int ArtifactRarity{ get{ return 11; } }

		public override int BasePhysicalResistance{ get{ return 35; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public LevelHolyKnightsBreastplate()
		{
			Hue = 0x47E;
			Attributes.BonusHits = 10;
			Attributes.ReflectPhysical = 15;
		}

		public LevelHolyKnightsBreastplate( Serial serial ) : base( serial )
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
				PhysicalBonus = 0;
		}
	}
}
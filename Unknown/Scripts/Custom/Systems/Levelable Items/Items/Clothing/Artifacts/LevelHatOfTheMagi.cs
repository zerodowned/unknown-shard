using System;
using Server;

namespace Server.Items
{
	public class LevelHatOfTheMagi : LevelWizardsHat
	{
		public override int LabelNumber{ get{ return 1061597; } } // Hat of the Magi

		public override int ArtifactRarity{ get{ return 11; } }

		public override int BasePoisonResistance{ get{ return 20; } }
		public override int BaseEnergyResistance{ get{ return 20; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public LevelHatOfTheMagi()
		{
			Hue = 0x481;

			Attributes.BonusInt = 8;
			Attributes.RegenMana = 4;
			Attributes.SpellDamage = 10;
		}

		public LevelHatOfTheMagi( Serial serial ) : base( serial )
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

			switch ( version )
			{
				case 0:
				{
					Resistances.Poison = 0;
					Resistances.Energy = 0;
					break;
				}
			}
		}
	}
}
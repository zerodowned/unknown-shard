using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class FeyLeggings : ChainLegs
	{
		public override int LabelNumber{ get{ return 1075041; } } // Fey Leggings

		public override int BasePhysicalResistance{ get{ return 12; } }
		public override int BaseFireResistance{ get{ return 8; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 4; } }
		public override int BaseEnergyResistance{ get{ return 19; } }

		[Constructable]
		public FeyLeggings()
		{
			Attributes.BonusHits = 6;
			Attributes.DefendChance = 20;

			ArmorAttributes.MageArmor = 1;
		}

		public override Race RequiredRace
		{
			get
			{
				return Race.Elf;
			}
		}

		public FeyLeggings( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
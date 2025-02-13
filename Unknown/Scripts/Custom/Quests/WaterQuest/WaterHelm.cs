//Created with Maraks Script Creator 4
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class WaterHelm : PlateHelm
  {
public override int ArtifactRarity{ get{ return 10; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 200; } }

		public override int BaseColdResistance{ get{ return 2; } } 
		public override int BaseEnergyResistance{ get{ return 2; } } 
		public override int BasePhysicalResistance{ get{ return 2; } } 
		public override int BasePoisonResistance{ get{ return 2; } } 
		public override int BaseFireResistance{ get{ return 2; } } 

		public override int AosStrReq{ get{ return 15; } }

		public override int ArmorBase{ get{ return 16; } }
      
      [Constructable]
		public WaterHelm()
		{
			Weight = 1;
          Name = "Water Helm";
          Hue = 2999;
      ArmorAttributes.DurabilityBonus = 1;
      ArmorAttributes.LowerStatReq = 15;
      ArmorAttributes.MageArmor = 1;
      ArmorAttributes.SelfRepair = 1;
      Attributes.BonusDex = 2;
      Attributes.BonusHits = 2;
      Attributes.BonusInt = 2;
      Attributes.BonusMana = 2;
      Attributes.BonusStam = 2;
      Attributes.LowerManaCost = 1;
      Attributes.LowerRegCost = 1;
      Attributes.Luck = 1;
      Attributes.NightSight = 1;
      Attributes.RegenHits = 1;
      Attributes.RegenMana = 1;
      Attributes.RegenStam = 2;
      Attributes.BonusMana = 2;
      LootType = LootType.Blessed;
		}

		public WaterHelm( Serial serial ) : base( serial )
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

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class LevelOverseerSunderedBlade : LevelRadiantScimitar
	{
		public override int LabelNumber{ get{ return 1072920; } } // Overseer Sundered Blade

		[Constructable]
		public LevelOverseerSunderedBlade()
		{
			ItemID = 0x2D27;
			Hue = 0x485;

			Attributes.RegenStam = 2;
			Attributes.AttackChance = 10;
			Attributes.WeaponSpeed = 35;
			Attributes.WeaponDamage = 45;

			Hue = this.GetElementalDamageHue();
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = cold = pois = nrgy = 0;
			fire = 100;
		}

		public LevelOverseerSunderedBlade( Serial serial ) : base( serial )
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
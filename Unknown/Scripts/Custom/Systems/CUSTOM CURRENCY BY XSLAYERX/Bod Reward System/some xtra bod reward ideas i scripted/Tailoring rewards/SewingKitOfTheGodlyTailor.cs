//Please Leave Credit XSlayerX aka XLacaestX
using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class SewingKitOfTheGodlyTailor : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefTailoring.CraftSystem; } }

		[Constructable]
		public SewingKitOfTheGodlyTailor() : base( 0xF9D )
		{
			Weight = 2.0;
            UsesRemaining = 10000;
            Hue = 1152;
		}

		[Constructable]
		public SewingKitOfTheGodlyTailor( int uses ) : base( uses, 0xF9D )
		{
			Weight = 2.0;
            UsesRemaining = 10000;
		}

        public SewingKitOfTheGodlyTailor(Serial serial)
            : base(serial)
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
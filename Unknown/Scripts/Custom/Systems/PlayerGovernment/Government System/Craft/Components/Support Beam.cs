using System;
using Server;

namespace Server.Items
{
	public class SupportBeam : Item
	{
		[Constructable]
		public SupportBeam() : base( 898 )
		{
			Name = "support beam";
		}

		public SupportBeam( Serial serial ) : base( serial )
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
using System;
using Server;

namespace Server.Items
{
	public class FoundationBlock : Item
	{
		[Constructable]
		public FoundationBlock() : base( 100 )
		{
			Name = "foundation block";
		}

		public FoundationBlock( Serial serial ) : base( serial )
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
using System;

namespace Server.Items
{
	public class ShipwreckedItem : Item, IDyable
	{
		public ShipwreckedItem( int itemID ) : base( itemID )
		{
			int weight = this.ItemData.Weight;

			if ( weight >= 255 )
				weight = 1;

			this.Weight = weight;
		}

		public override void OnSingleClick( Mobile from )
		{
			this.LabelTo( from, 1050039, String.Format( "#{0}\t#1041645", LabelNumber ) );
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			list.Add( 1041645 ); // recovered from a shipwreck
		}

		public ShipwreckedItem( Serial serial ) : base( serial )
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

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			if ( ItemID >= 0x13A4 && ItemID <= 0x13AE )
			{
				Hue = sender.DyedHue;
				return true;
			}

			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}
	}
}
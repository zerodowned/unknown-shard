/* Pieced together by evilfreak*/



/////////////////////////////////////////////////
//                                             //
// Automatically generated by the              //
// AddonGenerator script by Arya               //
//                                             //
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class AG_ferretlvl1Addon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new AG_ferretlvl1AddonDeed();
			}
		}

		[ Constructable ]
		public AG_ferretlvl1Addon()
		{
			AddComponent( new AddonComponent( 1339 ), -1, -5, 0 );
			AddComponent( new AddonComponent( 1339 ), -1, -4, 0 );
			AddComponent( new AddonComponent( 6929 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 1339 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 1339 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 1339 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 6928 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 6933 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 1339 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 1339 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 6932 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 6931 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 6877 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 6926 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 6877 ), -1, 4, 0 );
			AddComponent( new AddonComponent( 6876 ), -1, 4, 0 );
			AddComponent( new AddonComponent( 7389 ), -1, 4, 4 );
			AddComponent( new AddonComponent( 6876 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 6877 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 6872 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 4651 ), -1, 5, 1 );
			AddComponent( new AddonComponent( 1 ), -1, 6, 0 );
			AddComponent( new AddonComponent( 6875 ), -1, 6, 0 );
			AddComponent( new AddonComponent( 6872 ), -1, 6, 0 );
			AddComponent( new AddonComponent( 6876 ), -1, 6, 0 );
			AddComponent( new AddonComponent( 7598 ), -1, 6, 1 );
			AddComponent( new AddonComponent( 1 ), -1, 7, 0 );
			AddComponent( new AddonComponent( 6875 ), -1, 7, 0 );
			AddComponent( new AddonComponent( 6873 ), -1, 7, 0 );
			AddComponent( new AddonComponent( 6872 ), -1, 7, 0 );
			AddComponent( new AddonComponent( 6873 ), -1, 8, 0 );
			AddComponent( new AddonComponent( 7584 ), -1, 8, 1 );
			AddComponent( new AddonComponent( 6874 ), -2, 8, 0 );
			AddComponent( new AddonComponent( 6877 ), 2, 3, 2 );
			AddComponent( new AddonComponent( 6931 ), 2, 3, 0 );
			AddComponent( new AddonComponent( 1339 ), 2, 3, 0 );
			AddComponent( new AddonComponent( 1339 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 7395 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 6872 ), 2, 5, 2 );
			AddComponent( new AddonComponent( 6877 ), 2, 5, 5 );
			AddComponent( new AddonComponent( 6935 ), 2, 5, 6 );
			AddComponent( new AddonComponent( 6876 ), 2, 4, 2 );
			AddComponent( new AddonComponent( 6923 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 6935 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 6933 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 6926 ), -3, 0, 0 );
			AddComponent( new AddonComponent( 6932 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 6931 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 7400 ), 1, -5, 0 );
			AddComponent( new AddonComponent( 1339 ), 1, -5, 0 );
			AddComponent( new AddonComponent( 6879 ), 3, 6, 5 );
			AddComponent( new AddonComponent( 6876 ), 2, 6, 5 );
			AddComponent( new AddonComponent( 1 ), 1, 6, 5 );
			AddComponent( new AddonComponent( 6875 ), 1, 6, 5 );
			AddComponent( new AddonComponent( 6878 ), 1, 6, 0 );
			AddComponent( new AddonComponent( 6879 ), 1, 6, 0 );
			AddComponent( new AddonComponent( 7401 ), 1, 6, 6 );
			AddComponent( new AddonComponent( 6873 ), 1, 7, 5 );
			AddComponent( new AddonComponent( 6879 ), 1, 7, 0 );
			AddComponent( new AddonComponent( 1 ), 1, 4, 2 );
			AddComponent( new AddonComponent( 6875 ), 1, 4, 2 );
			AddComponent( new AddonComponent( 6873 ), 1, 5, 2 );
			AddComponent( new AddonComponent( 6878 ), 1, 5, 0 );
			AddComponent( new AddonComponent( 6935 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 6933 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 6931 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 1339 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 1339 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 6931 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 7394 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 1339 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 1339 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 6935 ), -2, 3, 0 );
			AddComponent( new AddonComponent( 1339 ), 1, -6, 0 );
			AddComponent( new AddonComponent( 6931 ), -2, 4, 1 );
			AddComponent( new AddonComponent( 1 ), -2, 4, 0 );
			AddComponent( new AddonComponent( 6875 ), -2, 4, 0 );
			AddComponent( new AddonComponent( 1339 ), 1, -7, 0 );
			AddComponent( new AddonComponent( 6872 ), 2, 7, 5 );
			AddComponent( new AddonComponent( 6872 ), 0, 8, 0 );
			AddComponent( new AddonComponent( 6874 ), 0, 7, 5 );
			AddComponent( new AddonComponent( 6876 ), 0, 7, 0 );
			AddComponent( new AddonComponent( 6872 ), 0, 7, 0 );
			AddComponent( new AddonComponent( 6877 ), 0, 6, 0 );
			AddComponent( new AddonComponent( 6876 ), 0, 6, 0 );
			AddComponent( new AddonComponent( 6879 ), 0, 6, 0 );
			AddComponent( new AddonComponent( 1339 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 6874 ), 0, 5, 2 );
			AddComponent( new AddonComponent( 6877 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 6879 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 6878 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 7415 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 1339 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 6878 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 6879 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 6931 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 1339 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 6935 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 6878 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 6925 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 6931 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 1339 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 6935 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 6931 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 6927 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 6925 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 6874 ), -3, 5, 0 );
			AddComponent( new AddonComponent( 1 ), -2, 5, 0 );
			AddComponent( new AddonComponent( 6875 ), -2, 5, 0 );
			AddComponent( new AddonComponent( 6873 ), -2, 5, 0 );
			AddComponent( new AddonComponent( 1339 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 6874 ), -2, 7, 0 );
			AddComponent( new AddonComponent( 6873 ), -2, 7, 0 );
			AddComponent( new AddonComponent( 7584 ), -2, 6, 1 );
			AddComponent( new AddonComponent( 6873 ), -2, 6, 0 );
			AddComponent( new AddonComponent( 1 ), -2, 6, 0 );
			AddComponent( new AddonComponent( 6875 ), -2, 6, 0 );
			AddComponent( new AddonComponent( 6874 ), -3, 6, 0 );
			AddComponent( new AddonComponent( 6874 ), -3, 7, 0 );
			AddComponent( new AddonComponent( 6933 ), 3, 2, 0 );
			AddComponent( new AddonComponent( 6931 ), 3, 2, 0 );
			AddComponent( new AddonComponent( 6878 ), 3, 3, 2 );
			AddComponent( new AddonComponent( 6879 ), 3, 4, 2 );
			AddComponent( new AddonComponent( 6878 ), 3, 5, 5 );
			AddComponent( new AddonComponent( 1339 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 6933 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 6924 ), 3, 1, 0 );
			AddonComponent ac;
			ac = new AddonComponent( 6927 );
			AddComponent( ac, -3, -1, 0 );
			ac = new AddonComponent( 6929 );
			AddComponent( ac, -1, -3, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -1, -4, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -1, -3, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -1, -5, 0 );
			ac = new AddonComponent( 6928 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, -2, -2, 0 );
			ac = new AddonComponent( 6933 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 6926 );
			AddComponent( ac, -3, 0, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -2, -1, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 7584 );
			AddComponent( ac, -2, 6, 1 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, -2, 4, 1 );
			ac = new AddonComponent( 7389 );
			ac.Hue = 34;
			AddComponent( ac, -1, 4, 4 );
			ac = new AddonComponent( 7584 );
			AddComponent( ac, -1, 8, 1 );
			ac = new AddonComponent( 6935 );
			AddComponent( ac, -2, 3, 0 );
			ac = new AddonComponent( 4651 );
			AddComponent( ac, -1, 5, 1 );
			ac = new AddonComponent( 6932 );
			AddComponent( ac, -1, 2, 0 );
			ac = new AddonComponent( 7598 );
			AddComponent( ac, -1, 6, 1 );
			ac = new AddonComponent( 6926 );
			AddComponent( ac, -1, 3, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, -1, 2, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 1, -7, 0 );
			ac = new AddonComponent( 6923 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 6935 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 6935 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 7394 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 6933 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 6933 );
			AddComponent( ac, 3, 1, 0 );
			ac = new AddonComponent( 6925 );
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 6932 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 7400 );
			AddComponent( ac, 1, -5, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 0, -5, 0 );
			ac = new AddonComponent( 6924 );
			AddComponent( ac, 3, 1, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 1, -6, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 1, -5, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 6935 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 0, 5, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 0, 4, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 0, 3, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 2, 3, 0 );
			ac = new AddonComponent( 6935 );
			AddComponent( ac, 2, 5, 6 );
			ac = new AddonComponent( 6935 );
			AddComponent( ac, 0, 3, 0 );
			ac = new AddonComponent( 7401 );
			AddComponent( ac, 1, 6, 6 );
			ac = new AddonComponent( 6925 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 1, 3, 0 );
			ac = new AddonComponent( 6933 );
			AddComponent( ac, 3, 2, 0 );
			ac = new AddonComponent( 6933 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 7415 );
			AddComponent( ac, 0, 5, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 0, 4, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 2, 2, 0 );
			ac = new AddonComponent( 1339 );
			AddComponent( ac, 2, 3, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 3, 2, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 6931 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 7395 );
			AddComponent( ac, 2, 2, 0 );

		}

		public AG_ferretlvl1Addon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class AG_ferretlvl1AddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new AG_ferretlvl1Addon();
			}
		}

		[Constructable]
		public AG_ferretlvl1AddonDeed()
		{
			Name = "AG_ferretlvl1";
		}

		public AG_ferretlvl1AddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
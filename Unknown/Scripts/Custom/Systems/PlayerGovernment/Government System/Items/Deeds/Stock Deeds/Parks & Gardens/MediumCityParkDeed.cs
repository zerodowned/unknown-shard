using Server;
using System;
using Server.Items;
using Server.Multis;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;

namespace Server.Items
{
	public class MediumCityParkDeed : CityDeed
	{
		[Constructable]
		public MediumCityParkDeed() : base( 0x1454, new Point3D( 0, 8, 0 ) )
		{
			Name = "a medium park deed";
			Type = CivicStrutureType.MedCityPark;
		}

		public MediumCityParkDeed( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = (PlayerMobile)from;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( pm.City != null )
			{
				if ( pm.City.Mayor != from )
				{
					from.SendMessage( "You must be the mayor of your city to place this structure." );
				}
				else if ( !PlayerGovernmentSystem.IsAtCity( from ) )
				{
					from.SendMessage( "You must be inside your city to place this structure." );
				}
				else if ( !PlayerGovernmentSystem.IsCityLevelReached( from, 5 ) )
				{
					from.SendMessage( "Your city must be at least level 5 before you can place this structure." );
				}
				else if ( PlayerGovernmentSystem.NeedsForensics && from.Skills[SkillName.Forensics].Base < 80.0 )
				{
					from.SendMessage( "You lack the required skill to place this building, You need at least 80.0 points in forensics." );
				}
				else
				{
					base.OnDoubleClick( from );
				}
			}
			else
			{
				from.SendMessage( "You must be the mayor of a city in order to use this." );
			}
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

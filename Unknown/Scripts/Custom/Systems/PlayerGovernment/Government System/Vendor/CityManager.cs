using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class CityManager : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		public override NpcGuild NpcGuild{ get{ return NpcGuild.TinkersGuild; } }

		[Constructable]
		public CityManager() : base( "the city manager" )
		{
		}

		public override void InitSBInfo()
		{
			
			m_SBInfos.Add( new SBCityManager() );
		}

		public CityManager( Serial serial ) : base( serial )
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

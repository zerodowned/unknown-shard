using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class AJeweler : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public AJeweler() : base( "The Jeweler" )
		{
		    CantWalk = true;
			
			SetSkill( SkillName.ItemID, 64.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBJewel() );
		}

		public AJeweler( Serial serial ) : base( serial )
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
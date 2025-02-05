/////////////////////
///Crafted By Kail///
/////////////////////
using System; 
using Server.Items; 

namespace Server.Items 
{ 
   public class HoodRobe : BaseArmor 
   {

                public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }


		public override int InitMinHits{ get{ return 0; } }
		public override int InitMaxHits{ get{ return 0; } } 

      public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } } 

                //public override int ArtifactRarity{ get{ return 7; } } 

      [Constructable] 
      public HoodRobe() : base( 0x2683 )
      {
         Weight = 0.0;
         Name = "Hood Robe";
         Hue = 2572;

         //Attributes.CastRecovery = 1;
         //Attributes.CastSpeed = 2;
         //Attributes.LowerManaCost = 20;
         //Attributes.LowerRegCost = 65;
			
      }

      public override void OnDoubleClick( Mobile m )
      {
         if( Parent != m )
         {
            m.SendMessage( "You must be wearing the robe to use it!" );
         }
         else
         {
            if ( ItemID == 0x2683 || ItemID == 0x2684 )
            {
               m.SendMessage( "You lower the hood." );
               m.PlaySound( 0x57 );
               ItemID = 0x1F03;
               m.NameMod = null;
               m.RemoveItem(this);
               m.EquipItem(this);
               if( m.Kills >= 5)
               {
               m.Criminal = true;
                }
                if( m.GuildTitle != null)
               {
                  m.DisplayGuildTitle = true;
                }
            }
            else if ( ItemID == 0x1F03 || ItemID == 0x1F04 )
            {
               m.SendMessage( "You pull the hood over your head." );
               m.PlaySound( 0x57 );
               ItemID = 0x2683;
               m.DisplayGuildTitle = false;
               m.Criminal = false;
               m.RemoveItem(this);
               m.EquipItem(this);
            }
         }
      }

       public override bool OnEquip( Mobile from )
      {
         if ( ItemID == 0x2683 )
         {
         from.DisplayGuildTitle = false;
         from.Criminal = false;
         }
         return base.OnEquip(from);
      }

      public override void OnRemoved( Object o )
      {
      if( o is Mobile )
      {
          ((Mobile)o).NameMod = null;
      }
      if( o is Mobile && ((Mobile)o).Kills >= 5)
               {
               ((Mobile)o).Criminal = true;
                }
      if( o is Mobile && ((Mobile)o).GuildTitle != null )
               {
          ((Mobile)o).DisplayGuildTitle = true;
                }
      base.OnRemoved( o );
      }

      public HoodRobe( Serial serial ) : base( serial )
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
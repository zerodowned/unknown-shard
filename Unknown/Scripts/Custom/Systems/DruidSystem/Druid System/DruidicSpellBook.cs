using System;
using Server.Network;
using Server.Gumps;
using Server.Spells;

namespace Server.Items
{
   public class DruidicSpellbook : Spellbook
   {
      public override SpellbookType SpellbookType{ get{ return SpellbookType.Druidic; } }
      public override int BookOffset{ get{ return 301; } }
      public override int BookCount{ get{ return 16; } }

      
      [Constructable]
      public DruidicSpellbook() : this( (ulong)0 )
      {
      }

      [Constructable]
      public DruidicSpellbook( ulong content ) : base( content, 0xEFA )
      {
         Hue = 0x48C;
         Name = "Tome of Nature";
      }

      public override void OnDoubleClick( Mobile from )
      {
          Container pack = from.Backpack;

          if (Parent == from || (pack != null && Parent == pack))
              //DisplayTo(from);
          
         //if ( from.InRange( GetWorldLocation(), 1 ) )
         {
            from.CloseGump( typeof( DruidicSpellbookGump ) );
            from.SendGump( new DruidicSpellbookGump( from, this ) );
          }
          else from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
	

      }

      public DruidicSpellbook( Serial serial ) : base( serial )
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

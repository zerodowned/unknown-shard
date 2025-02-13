//old school Feb 2009

using System; 
using Server.Items; 
using System.Text; 
using Server.Gumps; 
using Server.Network; 

namespace Server.Items 
{ 
   [Serializable()] 
   public class WantedCrinminalsDeed : Item 
   { 
      [Constructable] 
      public WantedCrinminalsDeed() : base( 0x14F0 ) 
      { 
         Movable = true;
         Weight = 1.0; 
         Hue = 0; 
         Name = "Wanted Criminals"; //Change to your newspapers name and issue 
      } 

        
     
        
      public override void OnDoubleClick( Mobile from ) 
		{ 
		from.SendGump( new WantedCriminalsGump( from ) ); 
		} 
 

      public WantedCrinminalsDeed( Serial serial ) : base( serial ) 
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

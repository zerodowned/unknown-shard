using System; 
using Server.Network; 
using Server.Items; 
using Server.Mobiles; 

namespace Server.Items 
{ 
   public class AbyssmalIdol : Item 
   { 
      [Constructable] 
      public AbyssmalIdol() : base( 9772 ) 
      { 
         Movable = true; 
         Name = "Abyssmal Horror Idol";  
         LootType = LootType.Cursed; 
      } 

      public AbyssmalIdol( Serial serial ) : base( serial ) 
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

      
      public override void OnDoubleClick( Mobile from ) 
      { 
         if ( !from.InRange( GetWorldLocation(), 2 ) ) 
         { 
            from.SendLocalizedMessage( 500446 ); // That is too far away. 
         } 
         else 
         { 
            if ( from.BodyValue == 0x190 || from.BodyValue == 0x191 ) 
            { 
               from.BodyValue = 312; 
               from.PlaySound( 1105 ); 
              Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x376A, 1, 29, 0x47D, 2, 9962, 0 ); 
              Effects.SendLocationParticles( EffectItem.Create( new Point3D( from.X, from.Y, from.Z - 7 ), from.Map, EffectItem.DefaultDuration ), 0x37C4, 1, 29, 0x47D, 2, 9502, 0 ); 

            } 
            else 
            { 
               if (from.Female == true ) 
                { 
                  from.BodyValue = 0x191; 
                  from.PlaySound( 1105 ); 
                 Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x376A, 1, 29, 0x47D, 2, 9962, 0 ); 
                 Effects.SendLocationParticles( EffectItem.Create( new Point3D( from.X, from.Y, from.Z - 7 ), from.Map, EffectItem.DefaultDuration ), 0x37C4, 1, 29, 0x47D, 2, 9502, 0 ); 
    
                } 
               else 
                { 
                  from.BodyValue = 0x190; 
                  from.PlaySound( 1105 ); 
                 Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x376A, 1, 29, 0x47D, 2, 9962, 0 ); 
                 Effects.SendLocationParticles( EffectItem.Create( new Point3D( from.X, from.Y, from.Z - 7 ), from.Map, EffectItem.DefaultDuration ), 0x37C4, 1, 29, 0x47D, 2, 9502, 0 ); 

              } 
            } 
         } 
      } 
   } 
} 

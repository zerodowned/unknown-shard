// Created by Doctor Who

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "King Deer's Corpse" )]
	public class KingDeer : BaseCreature
	{
		[Constructable]
		public KingDeer() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.1 )
                            {
                                               Name = "King Deer";
                                               Title = "- Lord of The Herd";
                                               Hue = 0;
                                               Body = 0xEA; // Uncomment these lines and input values
                                               BaseSoundID = 0x0; // To use your own custom body and sound.
                                               SetStr( 100 );
                                               SetDex( 120 );
                                               SetInt( 100 );
                                               SetHits( 500, 1000 );
                                               SetDamage( 20, 30 );
                                               SetDamageType( ResistanceType.Physical, 50 );
                                               SetDamageType( ResistanceType.Cold, 0 );
                                               SetDamageType( ResistanceType.Fire, 50 );
                                               SetDamageType( ResistanceType.Energy, 50 );
                                               SetDamageType( ResistanceType.Poison, 0 );

                                               SetResistance( ResistanceType.Physical, 60 );
                                               SetResistance( ResistanceType.Cold, 90 );
                                               SetResistance( ResistanceType.Fire, 90 );
                                               SetResistance( ResistanceType.Energy, 80 );
                                               SetResistance( ResistanceType.Poison, 90 );


                                               SetSkill( SkillName.Tactics, 200, 200 );
			                       SetSkill( SkillName.Wrestling, 200, 200 );
			                       SetSkill( SkillName.Archery, 200, 200 );



                                               Fame = 0;
                                               Karma = 0;
                                               VirtualArmor = 40;
     
                                               PackGold( 2000, 3000 );

                                             }


                                               public override void OnDeath(Container c)
                                               {
                                                     base.OnDeath(c) ;

                                                    if (Utility.RandomDouble() < 0.40)

                                                     switch (Utility.Random(4))
                                                      {

                                                 case 0: PackItem(new DeerKingsDeerMask()); break;
                                       
 
                                             }
                                         }

		                                 public override void GenerateLoot()
		                                 {
			                                 AddLoot( LootPack.Rich, 5 );
		                                 }

                                 public override bool HasBreath{ get{ return true ; } }
                                 public override bool BardImmune{ get{ return true; } }
                                 public override bool Unprovokable{ get{ return true; } }
                                 public override bool AlwaysMurderer{ get{ return true; } }

public KingDeer( Serial serial ) : base( serial )
                      {
                      }

public override void Serialize( GenericWriter writer )
                      {
                                        base.Serialize( writer );
                                        writer.Write( (int) 0 );
                      }

        public override void Deserialize( GenericReader reader )
                      {
                                        base.Deserialize( reader );
                                        int version = reader.ReadInt();
                      }
    }
}

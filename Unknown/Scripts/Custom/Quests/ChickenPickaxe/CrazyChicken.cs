/*
*   Scripter : CrazyChicken
*   Author : Lord Mashadow // Avalon Team
*   Generator : Avalon Script Creator
*   Created at :  3/26/2009 2:17:39 AM
*   Thank you for using this tool, feel free to visit our web site  www.avalon.gen.tr
*/
using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName( "CrazyChicken" )]
    public class CrazyChicken : BaseCreature
    {
        [Constructable]
        public CrazyChicken() : base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
        {
            this.Name = "CrazyChicken";
            this.Hue = 1971;
            this.Body = 208;
            this.BaseSoundID = 110;

            this.SetStr( 160 );
            this.SetDex( 170 );
            this.SetInt( 100 );
            this.SetHits( 2300 );
            this.SetDamage( 5, 8 );
            this.SetDamageType( ResistanceType.Physical, 100 );
            this.SetResistance( ResistanceType.Physical, 20 );
            this.SetResistance( ResistanceType.Cold, 20 );
            this.SetResistance( ResistanceType.Fire, 20 );
            this.SetResistance( ResistanceType.Energy, 20 );
            this.SetResistance( ResistanceType.Poison, 20 );
            this.Fame = 300;
            this.VirtualArmor = 20;
        }

        public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

            c.DropItem( new Gold(1000));
        }

		public override WeaponAbility GetWeaponAbility()
		{
			switch ( Utility.Random(3 ) )
			{
				default:
                case 0: return WeaponAbility.DoubleStrike;
            }
        }

        public CrazyChicken( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}
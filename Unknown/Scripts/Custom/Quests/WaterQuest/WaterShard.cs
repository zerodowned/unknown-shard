/* This file was created with
Ilutzio's Questmaker. Enjoy! */
using System;using Server;namespace Server.Items
{
public class WaterShard : Item
{
[Constructable]
public WaterShard() : this( 1 )
{}
[Constructable]
public WaterShard( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
{}
[Constructable]

///////////The hexagon value ont he line below is the ItemID
public WaterShard( int amount ) : base( 0x1BF2 )
{


///////////Item name
Name = "WaterShard";

///////////Item hue
Hue = 2999;

///////////Stackable
Stackable = true;

///////////Weight of one item
Weight = 0.01;
Amount = amount;

}
public WaterShard( Serial serial ) : base( serial )
{}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}

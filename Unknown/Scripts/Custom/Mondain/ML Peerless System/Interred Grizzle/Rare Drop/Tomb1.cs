/* This file was created with
Ilutzio's Questmaker. Enjoy! */
using System;using Server;namespace Server.Items
{
public class Tomb1 : Item
{
[Constructable]
public Tomb1() : this( 1 )
{}
[Constructable]
public Tomb1( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
{}
[Constructable]

///////////The hexagon value ont he line below is the ItemID
public Tomb1( int amount ) : base( 3806 )
{


///////////Item name
Name = "Tombstone of The Damned";

///////////Item hue
Hue = 0;

///////////Stackable
Stackable = false;

///////////Weight of one item
Weight = 0.01;
Amount = amount;

}
public Tomb1( Serial serial ) : base( serial )
{}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}

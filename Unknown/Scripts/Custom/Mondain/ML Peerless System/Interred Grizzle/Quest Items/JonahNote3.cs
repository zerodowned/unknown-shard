/* This file was created with
Ilutzio's Questmaker. Enjoy! */
using System;using Server;namespace Server.Items
{
public class JonahNote3 : Item
{
[Constructable]
public JonahNote3() : this( 1 )
{}
[Constructable]
public JonahNote3( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
{}
[Constructable]

///////////The hexagon value ont he line below is the ItemID
public JonahNote3( int amount ) : base( 8827 )
{


///////////Item name
Name = "Master Jonas Note: Three";

///////////Item hue
Hue = 0;

///////////Stackable
Stackable = false;

///////////Weight of one item
Weight = 0.01;
Amount = amount;

}
public JonahNote3( Serial serial ) : base( serial )
{}
public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 ); // version
}
public override void Deserialize( GenericReader reader )
{
base.Deserialize( reader ); int version = reader.ReadInt(); }}}

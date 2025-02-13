// Script Package: Sleepable Beds
// Version: 1.0
// Author: Oak
// Servers: RunUO 2.0
// Date: 7/7/2006
// History: 
//  Written for RunUO 1.0 shard, Sylvan Dreams,  in February 2005. Based largely on work by David on his Sleepable NPCs scripts.
//  Modified for RunUO 2.0, removed shard specific customizations (wing layers, etc.)
using System;
using System.Collections;
using Server;
using Server.Engines.PartySystem;
using Server.Misc;
using Server.Guilds;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;

namespace Server.Items
{
	public class SleeperBedBody : Container 
	{
		private Mobile m_Owner;
		private string m_SleeperBedBodyName;	
		private bool m_Blessed;
		private Timer m_Timer;
		private ArrayList m_EquipItems;		
		private bool m_spell;
	
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
		}

		public ArrayList EquipItems
		{
			get{ return m_EquipItems; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Invuln
		{
			get{ return m_Blessed; }
		}

		[Constructable] 
		public SleeperBedBody( Mobile owner, bool blessed ) : this( owner, blessed, true )
		{
		}

		[Constructable] 
		public SleeperBedBody( Mobile owner, bool blessed, bool isSpell ) : base( 0x2006 )
		{
			Stackable = true; // To supress console warnings, stackable must be true
			Amount = owner.Body; // protocol defines that for itemid 0x2006, amount=body
			Stackable = false;
			m_Blessed = blessed;
			Movable = false;

			m_Owner = owner;
			Name = m_Owner.Name;
			m_SleeperBedBodyName = "a sleeping " + Name;
			Hue = m_Owner.Hue;
			Direction = m_Owner.Direction;
			m_spell = isSpell;

			m_EquipItems = new ArrayList();
			AddFromLayer( m_Owner, Layer.FirstValid, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.TwoHanded, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Shoes, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Pants, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Shirt, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Helm, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Gloves, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Ring, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Neck, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Hair, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Waist, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.InnerTorso, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Bracelet, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.FacialHair, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.MiddleTorso, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Earrings, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Arms, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.Cloak, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.OuterTorso, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.OuterLegs, ref m_EquipItems );
			AddFromLayer( m_Owner, Layer.LastUserValid, ref m_EquipItems );
		
				m_Timer = new InternalTimer( m_Owner, this );
				m_Timer.Start();
		}
		
		private void AddFromLayer( Mobile from, Layer layer, ref ArrayList list ) 
		{
			if( list == null )
				list = new ArrayList();

			Item worn = from.FindItemOnLayer( layer );
			if ( worn != null )
			{
				Item item = new Item(); 
				item.ItemID = worn.ItemID;
				item.Hue = worn.Hue;
				item.Layer = layer;
				DropItem( item );
				list.Add( item ); 
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage("Tickling won't work. You need to double click the bed to wake up!");
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			from.SendLocalizedMessage( 1005468 ); // Me Sleepy.

			return false;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			from.SendLocalizedMessage( 1005468 ); // Me Sleepy.

			return false;
		}
      
		public override bool CheckContentDisplay( Mobile from )
		{
			return false;
		}

		public override bool DisplaysContent{ get{ return false; } }

		public override void OnAfterDelete()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			if( m_Owner != null )
			{
				//m_Owner.Z = this.Z;
				m_Owner.Blessed = this.m_Blessed;
			}

			for( int i = 0; i < m_EquipItems.Count; i++ )
			{
				object o = m_EquipItems[i];
				if( o != null && o is Item )
				{
					Item item = (Item)o;
					item.Delete();
				}
			}

			base.OnAfterDelete();
		}
		
		public SleeperBedBody( Serial serial ) : base( serial )
		{
		}

		public override void SendInfoTo( NetState state )
		{
			base.SendInfoTo( state );

			if ( ItemID == 0x2006 )
			{
				state.Send( new SleeperBedBodyContent( state.Mobile, this ) );
				state.Send( new SleeperBedBodyEquip( state.Mobile, this ) );
			}
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( m_SleeperBedBodyName != null )
				list.Add( m_SleeperBedBodyName );
			else
				list.Add( 1049644, String.Format( "a sleeping {0}", Name ) );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, m_SleeperBedBodyName == null ? String.Format( "a sleeping {0}", Name ) : m_SleeperBedBodyName );
		}
			
		public static string GetBodyName( Mobile m )
		{
			Type t = m.GetType();

			object[] attrs = t.GetCustomAttributes( typeof( SleeperBedNameAttribute ), true );

			if ( attrs != null && attrs.Length > 0 )
			{
				SleeperBedNameAttribute attr = attrs[0] as SleeperBedNameAttribute;

				if ( attr != null )
					return attr.Name;
			}

			return m.Name;
		}
	
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 1 ); 

			writer.Write(m_spell); // version 1

			writer.Write(m_Owner); // version 0
			writer.Write(m_SleeperBedBodyName);
			writer.Write(m_Blessed);

			writer.WriteItemList( m_EquipItems, true );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			m_spell = true;
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
			switch( version )
			{
				case 1:
				{
					m_spell = reader.ReadBool();
					goto case 0;
				}
				case 0:
				{
					m_Owner = reader.ReadMobile();
					m_SleeperBedBodyName = reader.ReadString();
					m_Blessed = reader.ReadBool();

					m_EquipItems = reader.ReadItemList();
					break;
				}
			}

			// Delete on Server restart if spell action
			if( m_spell )
				this.Delete();
		} 
		
		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private Item m_Body;
			private DateTime shutitoff = DateTime.Now + TimeSpan.FromSeconds(300); 
			public InternalTimer( Mobile m, Item body ) : base( TimeSpan.FromSeconds(10),TimeSpan.FromSeconds(10) )
			{
				
				m_Owner = m;
				m_Body = body;
			}
			protected override void OnTick() 
			{ 
				if (DateTime.Now < shutitoff)
				{
					if(m_Body != null)
						m_Body.PublicOverheadMessage(0,m_Owner.SpeechHue,false,"zZz"); 
					if(m_Owner != null)
						m_Owner.PlaySound(  m_Owner.Female ? 819 : 1093);      
				}
				else
				{
					Stop();
					m_Body.PublicOverheadMessage(0,m_Owner.SpeechHue, false,"You fall into a deep, quiet sleep.");
				}
			} 
		}
	}
}

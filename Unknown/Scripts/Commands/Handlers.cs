/**************************************
*Script Name: Generic Help Menu       *
*Author: Joeku                        *
*For use with RunUO 2.0               *
*Client Tested with: 5.0.7.1          *
*Version: 1.3                         *
*Initial Release: 10/05/06            *
*Revision Date: 01/12/07              *
**************************************/

using System;
using System.IO;
using Server;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Server.Accounting;
using Server.Mobiles;
using Server.Items;
using Server.Menus;
using Server.Menus.Questions;
using Server.Menus.ItemLists;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Targets;
using Server.Gumps;
using Server.Commands.Generic;

namespace Server.Commands
{
	public class CommandHandlers
	{
		public static void Initialize()
		{
			CommandSystem.Prefix = "[";

			Properties.Register();

			Register( "Go", AccessLevel.Counselor, new CommandEventHandler( Go_OnCommand ) );

			Register( "DropHolding", AccessLevel.Counselor, new CommandEventHandler( DropHolding_OnCommand ) );

			Register( "GetFollowers", AccessLevel.GameMaster, new CommandEventHandler( GetFollowers_OnCommand ) );

			Register( "ClearFacet", AccessLevel.Administrator, new CommandEventHandler( ClearFacet_OnCommand ) );

			Register( "Where", AccessLevel.Player, new CommandEventHandler( Where_OnCommand ) );

			Register( "AutoPageNotify", AccessLevel.Counselor, new CommandEventHandler( APN_OnCommand ) );
			Register( "APN", AccessLevel.Counselor, new CommandEventHandler( APN_OnCommand ) );

			Register( "Animate", AccessLevel.GameMaster, new CommandEventHandler( Animate_OnCommand ) );

			Register( "Cast", AccessLevel.Counselor, new CommandEventHandler( Cast_OnCommand ) );

			Register( "Stuck", AccessLevel.Counselor, new CommandEventHandler( Stuck_OnCommand ) );

			Register( "Help", AccessLevel.Player, new CommandEventHandler( Server.Help.Variables.Help_OnCommand ) );
			//Register( "Help", AccessLevel.Player, new CommandEventHandler( Help_OnCommand ) );

			Register( "Save", AccessLevel.Administrator, new CommandEventHandler( Save_OnCommand ) );

			Register( "Move", AccessLevel.GameMaster, new CommandEventHandler( Move_OnCommand ) );
			Register( "Client", AccessLevel.Counselor, new CommandEventHandler( Client_OnCommand ) );

			Register( "SMsg", AccessLevel.Counselor, new CommandEventHandler( StaffMessage_OnCommand ) );
			Register( "SM", AccessLevel.Counselor, new CommandEventHandler( StaffMessage_OnCommand ) );
			Register( "S", AccessLevel.Counselor, new CommandEventHandler( StaffMessage_OnCommand ) );
       
			Register( "BCast", AccessLevel.GameMaster, new CommandEventHandler( BroadcastMessage_OnCommand ) );
			Register( "BC", AccessLevel.GameMaster, new CommandEventHandler( BroadcastMessage_OnCommand ) );
			Register( "B", AccessLevel.GameMaster, new CommandEventHandler( BroadcastMessage_OnCommand ) );

			Register( "Bank", AccessLevel.GameMaster, new CommandEventHandler( Bank_OnCommand ) );

			Register( "Echo", AccessLevel.Counselor, new CommandEventHandler( Echo_OnCommand ) );

			Register( "Sound", AccessLevel.GameMaster, new CommandEventHandler( Sound_OnCommand ) );

			Register( "ViewEquip", AccessLevel.GameMaster, new CommandEventHandler( ViewEquip_OnCommand ) );

			Register( "DumpTimers", AccessLevel.Administrator, new CommandEventHandler( DumpTimers_OnCommand ) );
			Register( "CountObjects", AccessLevel.Administrator, new CommandEventHandler( CountObjects_OnCommand ) );
			Register( "ProfileWorld", AccessLevel.Administrator, new CommandEventHandler( ProfileWorld_OnCommand ) );
			Register( "TraceInternal", AccessLevel.Administrator, new CommandEventHandler( TraceInternal_OnCommand ) );
			Register( "TraceExpanded", AccessLevel.Administrator, new CommandEventHandler( TraceExpanded_OnCommand ) );
			Register( "PacketProfiles", AccessLevel.Administrator, new CommandEventHandler( PacketProfiles_OnCommand ) );
			Register( "TimerProfiles", AccessLevel.Administrator, new CommandEventHandler( TimerProfiles_OnCommand ) );
			Register( "SetProfiles", AccessLevel.Administrator, new CommandEventHandler( SetProfiles_OnCommand ) );

			Register( "Light", AccessLevel.Counselor, new CommandEventHandler( Light_OnCommand ) );
			Register( "Stats", AccessLevel.Counselor, new CommandEventHandler( Stats_OnCommand ) );

			Register( "ReplaceBankers", AccessLevel.Administrator, new CommandEventHandler( ReplaceBankers_OnCommand ) );

			Register( "SpeedBoost", AccessLevel.Counselor, new CommandEventHandler( SpeedBoost_OnCommand ) );
		}

		public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
			CommandSystem.Register( command, access, handler );
		}

		[Usage( "SpeedBoost [true|false]" )]
		[Description( "Enables a speed boost for the invoker.  Disable with paramaters." )]
		private static void SpeedBoost_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Length <= 1 )
			{
				if ( e.Length == 1 && !e.GetBoolean( 0 ) )
				{
					from.Send( SpeedBoost.Disabled );
					from.SendMessage( "Speed boost has been disabled." );
				}
				else
				{
					from.Send( SpeedBoost.Enabled );
					from.SendMessage( "Speed boost has been enabled." );
				}
			}
			else
			{
				from.SendMessage( "Format: SpeedBoost [true|false]" );
			}
		}

		[Usage( "Where" )]
		[Description( "Tells the commanding player his coordinates, region, and facet." )]
		public static void Where_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			Map map = from.Map;

			from.SendMessage( "You are at {0} {1} {2} in {3}.", from.X, from.Y, from.Z, map );

			if ( map != null )
			{
				Region reg = from.Region;

				if ( !reg.IsDefault )
				{
					StringBuilder builder = new StringBuilder();

					builder.Append( reg.ToString() );
					reg = reg.Parent;

					while ( reg != null )
					{
						builder.Append( " <- " + reg.ToString() );
						reg = reg.Parent;
					}

					from.SendMessage( "Your region is {0}.", builder.ToString() );
				}
			}
		}

		[Usage( "DropHolding" )]
		[Description( "Drops the item, if any, that a targeted player is holding. The item is placed into their backpack, or if that's full, at their feet." )]
		public static void DropHolding_OnCommand( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( DropHolding_OnTarget ) );
			e.Mobile.SendMessage( "Target the player to drop what they are holding." );
		}

		public static void DropHolding_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile && ((Mobile)obj).Player )
			{
				Mobile targ = (Mobile)obj;
				Item held = targ.Holding;

				if ( held == null )
				{
					from.SendMessage( "They are not holding anything." );
				}
				else
				{
					if ( from.AccessLevel == AccessLevel.Counselor )
					{
						Engines.Help.PageEntry pe = Engines.Help.PageQueue.GetEntry( targ );

						if ( pe == null || pe.Handler != from )
						{
							if ( pe == null )
								from.SendMessage( "You may only use this command on someone who has paged you." );
							else
								from.SendMessage( "You may only use this command if you are handling their help page." );

							return;
						}
					}

					if ( targ.AddToBackpack( held ) )
						from.SendMessage( "The item they were holding has been placed into their backpack." );
					else
						from.SendMessage( "The item they were holding has been placed at their feet." );

					held.ClearBounce();

					targ.Holding = null;
				}
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( DropHolding_OnTarget ) );
				from.SendMessage( "That is not a player. Try again." );
			}
		}

		public static void DeleteList_Callback( Mobile from, bool okay, object state )
		{
			if ( okay )
			{
				ArrayList list = (ArrayList)state;

				CommandLogging.WriteLine( from, "{0} {1} deleting {2} objects", from.AccessLevel, CommandLogging.Format( from ), list.Count );

				NetState.Pause();

				for ( int i = 0; i < list.Count; ++i )
				{
					object obj = list[i];

					if ( obj is Item )
						((Item)obj).Delete();
					else if ( obj is Mobile )
						((Mobile)obj).Delete();
				}

				NetState.Resume();

				from.SendMessage( "You have deleted {0} object{1}.", list.Count, list.Count == 1 ? "" : "s" );
			}
			else
			{
				from.SendMessage( "You have chosen not to delete those objects." );
			}
		}

		[Usage( "ClearFacet" )]
		[Description( "Deletes all items and mobiles in your facet. Players and their inventory will not be deleted." )]
		public static void ClearFacet_OnCommand( CommandEventArgs e )
		{
			Map map = e.Mobile.Map;

			if ( map == null || map == Map.Internal )
			{
				e.Mobile.SendMessage( "You may not run that command here." );
				return;
			}

			ArrayList list = new ArrayList();

			foreach ( Item item in World.Items.Values )
			{
				if ( item.Map == map && item.Parent == null )
					list.Add( item );
			}

			foreach ( Mobile m in World.Mobiles.Values )
			{
				if ( m.Map == map && !m.Player )
					list.Add( m );
			}

			if ( list.Count > 0 )
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} starting facet clear of {2} ({3} objects)", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), map, list.Count );

				e.Mobile.SendGump(
					new WarningGump( 1060635, 30720,
					String.Format( "You are about to delete {0} object{1} from this facet.  Do you really wish to continue?",
					list.Count, list.Count == 1 ? "" : "s" ),
					0xFFC000, 360, 260, new WarningGumpCallback( DeleteList_Callback ), list ) );
			}
			else
			{
				e.Mobile.SendMessage( "There were no objects found to delete." );
			}
		}

		[Usage( "GetFollowers" )]
		[Description( "Teleports all pets of a targeted player to your location." )]
		public static void GetFollowers_OnCommand( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( GetFollowers_OnTarget ) );
			e.Mobile.SendMessage( "Target a player to get their pets." );
		}

		public static void GetFollowers_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile && ((Mobile)obj).Player )
			{
				Mobile master = (Mobile)obj;
				ArrayList pets = new ArrayList();

				foreach ( Mobile m in World.Mobiles.Values )
				{
					if ( m is BaseCreature )
					{
						BaseCreature bc = (BaseCreature)m;

						if ( (bc.Controlled && bc.ControlMaster == master) || (bc.Summoned && bc.SummonMaster == master) )
							pets.Add( bc );
					}
				}

				if ( pets.Count > 0 )
				{
					CommandLogging.WriteLine( from, "{0} {1} getting all followers of {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( master ) );

					from.SendMessage( "That player has {0} pet{1}.", pets.Count, pets.Count != 1 ? "s" : "" );

					for ( int i = 0; i < pets.Count; ++i )
					{
						Mobile pet = (Mobile)pets[i];

						if ( pet is IMount )
							((IMount)pet).Rider = null; // make sure it's dismounted

						pet.MoveToWorld( from.Location, from.Map );
					}
				}
				else
				{
					from.SendMessage( "There were no pets found for that player." );
				}
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( GetFollowers_OnTarget ) );
				from.SendMessage( "That is not a player. Try again." );
			}
		}

		public static void ReplaceBankers_OnCommand( CommandEventArgs e )
		{
			List<Mobile> list = new List<Mobile>();

			foreach ( Mobile m in World.Mobiles.Values )
				if ( (m is Banker) && !(m is BaseCreature) )
					list.Add( m );

			foreach ( Mobile m in list )
			{
				Map map = m.Map;

				if ( map != null )
				{
					bool hasBankerSpawner = false;

					foreach ( Item item in m.GetItemsInRange( 0 ) )
					{
						if ( item is Spawner )
						{
							Spawner spawner = (Spawner)item;

							for ( int i = 0; !hasBankerSpawner && i < spawner.CreaturesName.Count; ++i )
								hasBankerSpawner = Insensitive.Equals( (string)spawner.CreaturesName[i], "banker" );

							if ( hasBankerSpawner )
								break;
						}
					}

					if ( !hasBankerSpawner )
					{
						Spawner spawner = new Spawner( 1, 1, 5, 0, 4, "banker" );

						spawner.MoveToWorld( m.Location, map );
					}
				}
			}
		}

		private class ViewEqTarget : Target
		{
			public ViewEqTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !BaseCommand.IsAccessible( from, targeted ) )
				{
					from.SendMessage( "That is not accessible." );
					return;
				}

				if ( targeted is Mobile )
					from.SendMenu( new EquipMenu( from, (Mobile)targeted, GetEquip( (Mobile)targeted ) ) );
			}

			private static ItemListEntry[] GetEquip( Mobile m )
			{
				ItemListEntry[] entries = new ItemListEntry[m.Items.Count];

				for ( int i = 0; i < m.Items.Count; ++i )
				{
					Item item = m.Items[i];

					entries[i] = new ItemListEntry( String.Format( "{0}: {1}", item.Layer, item.GetType().Name ), item.ItemID, item.Hue );
				}

				return entries;
			}

			private class EquipMenu : ItemListMenu
			{
				private Mobile m_Mobile;

				public EquipMenu( Mobile from, Mobile m, ItemListEntry[] entries ) : base( "Equipment", entries )
				{
					m_Mobile = m;

					CommandLogging.WriteLine( from, "{0} {1} getting equip for {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( m ) );
				}

				public override void OnResponse( NetState state, int index )
				{
					if ( index >= 0 && index < m_Mobile.Items.Count )
					{
						Item item = m_Mobile.Items[index];

						state.Mobile.SendMenu( new EquipDetailsMenu( m_Mobile, item ) );
					}
				}

				private class EquipDetailsMenu : QuestionMenu
				{
					private Mobile m_Mobile;
					private Item m_Item;

					public EquipDetailsMenu( Mobile m, Item item ) : base( String.Format( "{0}: {1}", item.Layer, item.GetType().Name ), new string[]{"Move","Delete","Props"})
					{
						m_Mobile = m;
						m_Item = item;
					}

					public override void OnCancel( NetState state )
					{
						state.Mobile.SendMenu( new EquipMenu( state.Mobile, m_Mobile, ViewEqTarget.GetEquip( m_Mobile ) ) );
					}

					public override void OnResponse( NetState state, int index )
					{
						if ( index == 0 )
						{
							CommandLogging.WriteLine( state.Mobile, "{0} {1} moving equip item {2} of {3}", state.Mobile.AccessLevel, CommandLogging.Format( state.Mobile ), CommandLogging.Format( m_Item ), CommandLogging.Format( m_Mobile ) );
							state.Mobile.Target = new MoveTarget( m_Item );
						}
						else if ( index == 1 )
						{
							CommandLogging.WriteLine( state.Mobile, "{0} {1} deleting equip item {2} of {3}", state.Mobile.AccessLevel, CommandLogging.Format( state.Mobile ), CommandLogging.Format( m_Item ), CommandLogging.Format( m_Mobile ) );
							m_Item.Delete();
						}
						else if ( index == 2 )
						{
							CommandLogging.WriteLine( state.Mobile, "{0} {1} opening props for equip item {2} of {3}", state.Mobile.AccessLevel, CommandLogging.Format( state.Mobile ), CommandLogging.Format( m_Item ), CommandLogging.Format( m_Mobile ) );
							state.Mobile.SendGump( new PropertiesGump( state.Mobile, m_Item ) );
						}
					}
				}
			}
		}

		[Usage( "PacketProfiles" )]
		[Description( "Generates a log file containing performance information pertaining to networking data packets." )]
		public static void PacketProfiles_OnCommand( CommandEventArgs e )
		{
			try
			{
				using ( StreamWriter sw = new StreamWriter( "packetprofiles.log", true ) )
				{
					sw.WriteLine( "# Dump on {0:f}", DateTime.Now );
					sw.WriteLine( "# Core profiling for " + Core.ProfileTime );

					PacketProfile[] profiles = PacketProfile.OutgoingProfiles;

					int totalSeconds = (int) Core.ProfileTime.TotalSeconds;

					if ( totalSeconds < 1 )
						totalSeconds = 1;

					sw.WriteLine();
					sw.WriteLine( "# Outgoing:" );

					for ( int i = 0; i < profiles.Length; ++i )
					{
						PacketProfile prof = profiles[i];

						if ( prof == null )
							continue;

						sw.WriteLine( "0x{0,-10:X2} {6,10} {1,-10} {2,10} {3,-10:F2} {4,10:F5} {5,-10:F5} {7,10} {8,-10} {9,10} {10,10:F5} {11:F5}", i, prof.Count, prof.TotalByteLength, prof.AverageByteLength, prof.TotalProcTime.TotalSeconds, prof.AverageProcTime.TotalSeconds, prof.Constructed, prof.Constructed / totalSeconds, prof.Count / totalSeconds, prof.TotalByteLength / totalSeconds, prof.TotalProcTime.TotalSeconds / totalSeconds, prof.PeakProcTime.TotalSeconds );
					}

					profiles = PacketProfile.IncomingProfiles;

					sw.WriteLine();
					sw.WriteLine( "# Incoming:" );

					for ( int i = 0; i < profiles.Length; ++i )
					{
						PacketProfile prof = profiles[i];

						if ( prof == null )
							continue;

						sw.WriteLine( "0x{0,-10:X2} {1,-10} {2,10} {3,-10:F2} {4,10:F5} {5:F5} {6:F5}", i, prof.Count, prof.TotalByteLength, prof.AverageByteLength, prof.TotalProcTime.TotalSeconds, prof.AverageProcTime.TotalSeconds, prof.PeakProcTime.TotalSeconds );
					}

					sw.WriteLine();
					sw.WriteLine();
				}
			}
			catch
			{
			}
		}

		[Usage( "TimerProfiles" )]
		[Description( "Generates a log file containing performance information pertaining to timers." )]
		public static void TimerProfiles_OnCommand( CommandEventArgs e )
		{
			try
			{
				using ( StreamWriter sw = new StreamWriter( "timerprofiles.log", true ) )
				{
					Dictionary<string, TimerProfile> profiles = Timer.Profiles;

					sw.WriteLine( "# Dump on {0:f}", DateTime.Now );
					sw.WriteLine( "# Core profiling for " + Core.ProfileTime );
					sw.WriteLine();

					foreach ( KeyValuePair<string, TimerProfile> kvp in profiles )
					{
						string name = kvp.Key;
						TimerProfile prof = kvp.Value;

						sw.WriteLine( "{6,-100}{0,-12}{1,12} {2,-12}{3,12} {4,-12:F5}{5:F5} {7:F5}", prof.Created, prof.Started, prof.Stopped, prof.Ticked, prof.TotalProcTime.TotalSeconds, prof.AverageProcTime.TotalSeconds, name, prof.PeakProcTime.TotalSeconds );
					}

					sw.WriteLine();
					sw.WriteLine();
				}
			}
			catch
			{
			}
		}

		[Usage( "SetProfiles [true | false]" )]
		[Description( "Enables, disables, or toggles the state of core packet and timer profiling." )]
		public static void SetProfiles_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
				Core.Profiling = e.GetBoolean( 0 );
			else
				Core.Profiling = !Core.Profiling;

			e.Mobile.SendMessage( "Profiling has been {0}.", Core.Profiling ? "enabled" : "disabled" );
		}

		[Usage( "DumpTimers" )]
		[Description( "Generates a log file of all currently executing timers. Used for tracing timer leaks." )]
		public static void DumpTimers_OnCommand( CommandEventArgs e )
		{
			try
			{
				using ( StreamWriter sw = new StreamWriter( "timerdump.log", true ) )
					Timer.DumpInfo( sw );
			}
			catch
			{
			}
		}

		private class CountSorter : IComparer
		{
			public int Compare( object x, object y )
			{
				DictionaryEntry a = (DictionaryEntry)x;
				DictionaryEntry b = (DictionaryEntry)y;

				int aCount = GetCount( a.Value );
				int bCount = GetCount( b.Value );

				int v = -aCount.CompareTo( bCount );

				if ( v == 0 )
				{
					Type aType = (Type)a.Key;
					Type bType = (Type)b.Key;

					v = aType.FullName.CompareTo( bType.FullName );
				}

				return v;
			}

			private int GetCount( object obj )
			{
				if ( obj is int )
					return (int) obj;

				if ( obj is int[] )
				{
					int[] list = (int[]) obj;

					int total = 0;

					for ( int i = 0; i < list.Length; ++i )
						total += list[i];

					return total;
				}

				return 0;
			}
		}

		[Usage( "CountObjects" )]
		[Description( "Generates a log file detailing all item and mobile types in the world." )]
		public static void CountObjects_OnCommand( CommandEventArgs e )
		{
			using ( StreamWriter op = new StreamWriter( "objects.log" ) )
			{
				Hashtable table = new Hashtable();

				foreach ( Item item in World.Items.Values )
				{
					Type type = item.GetType();

					object o = (object)table[type];

					if ( o == null )
						table[type] = 1;
					else
						table[type] = 1 + (int)o;
				}

				ArrayList items = new ArrayList( table );

				table.Clear();

				foreach ( Mobile m in World.Mobiles.Values )
				{
					Type type = m.GetType();

					object o = (object)table[type];

					if ( o == null )
						table[type] = 1;
					else
						table[type] = 1 + (int)o;
				}

				ArrayList mobiles = new ArrayList( table );

				items.Sort( new CountSorter() );
				mobiles.Sort( new CountSorter() );

				op.WriteLine( "# Object count table generated on {0}", DateTime.Now );
				op.WriteLine();
				op.WriteLine();

				op.WriteLine( "# Items:" );

				foreach ( DictionaryEntry de in items )
					op.WriteLine( "{0}\t{1:F2}%\t{2}", de.Value, (100 * (int)de.Value) / (double)World.Items.Count, de.Key );

				op.WriteLine();
				op.WriteLine();

				op.WriteLine( "#Mobiles:" );

				foreach ( DictionaryEntry de in mobiles )
					op.WriteLine( "{0}\t{1:F2}%\t{2}", de.Value, (100 * (int)de.Value) / (double)World.Mobiles.Count, de.Key );
			}

			e.Mobile.SendMessage( "Object table has been generated. See the file : <runuo root>/objects.log" );
		}

		[Usage( "TraceExpanded" )]
		[Description( "Generates a log file describing all items using expanded memory." )]
		public static void TraceExpanded_OnCommand( CommandEventArgs e )
		{
			Hashtable typeTable = new Hashtable();

			foreach ( Item item in World.Items.Values )
			{
				ExpandFlag flags = item.GetExpandFlags();

				if ( ( flags & ~(ExpandFlag.TempFlag | ExpandFlag.SaveFlag) ) == 0 )
					continue;

				Type itemType = item.GetType();

				do
				{
					int[] countTable = typeTable[itemType] as int[];

					if ( countTable == null )
						typeTable[itemType] = countTable = new int[8];

					if ( ( flags & ExpandFlag.Name ) != 0 )
						++countTable[0];

					if ( ( flags & ExpandFlag.Items ) != 0 )
						++countTable[1];

					if ( ( flags & ExpandFlag.Bounce ) != 0 )
						++countTable[2];

					if ( ( flags & ExpandFlag.Holder ) != 0 )
						++countTable[3];

					if ( ( flags & ExpandFlag.Blessed ) != 0 )
						++countTable[4];

					/*if ( ( flags & ExpandFlag.TempFlag ) != 0 )
						++countTable[5];

					if ( ( flags & ExpandFlag.SaveFlag ) != 0 )
						++countTable[6];*/

					if ( ( flags & ExpandFlag.Weight ) != 0 )
						++countTable[7];

					itemType = itemType.BaseType;
				} while ( itemType != typeof( object ) );
			}

			try
			{
				using ( StreamWriter op = new StreamWriter( "expandedItems.log", true ) )
				{
					string[] names = new string[]
					{
						"Name",
						"Items",
						"Bounce",
						"Holder",
						"Blessed",
						"TempFlag",
						"SaveFlag",
						"Weight"
					};

					ArrayList list = new ArrayList( typeTable );

					list.Sort( new CountSorter() );

					foreach ( DictionaryEntry de in list )
					{
						Type itemType = de.Key as Type;
						int[] countTable = de.Value as int[];

						op.WriteLine( "# {0}", itemType.FullName );

						for ( int i = 0; i < countTable.Length; ++i )
						{
							if ( countTable[i] > 0 )
								op.WriteLine( "{0}\t{1:N0}", names[i], countTable[i] );
						}

						op.WriteLine();
					}
				}
			}
			catch
			{
			}
		}

		[Usage( "TraceInternal" )]
		[Description( "Generates a log file describing all items in the 'internal' map." )]
		public static void TraceInternal_OnCommand( CommandEventArgs e )
		{
			int totalCount = 0;
			Hashtable table = new Hashtable();

			foreach ( Item item in World.Items.Values )
			{
				if ( item.Parent != null || item.Map != Map.Internal )
					continue;

				++totalCount;

				Type type = item.GetType();
				int[] parms = (int[])table[type];

				if ( parms == null )
					table[type] = parms = new int[]{ 0, 0 };

				parms[0]++;
				parms[1] += item.Amount;
			}

			using ( StreamWriter op = new StreamWriter( "internal.log" ) )
			{
				op.WriteLine( "# {0} items found", totalCount );
				op.WriteLine( "# {0} different types", table.Count );
				op.WriteLine();
				op.WriteLine();
				op.WriteLine( "Type\t\tCount\t\tAmount\t\tAvg. Amount" );

				foreach ( DictionaryEntry de in table )
				{
					Type type = (Type)de.Key;
					int[] parms = (int[])de.Value;

					op.WriteLine( "{0}\t\t{1}\t\t{2}\t\t{3:F2}", type.Name, parms[0], parms[1], (double)parms[1] / parms[0] );
				}
			}
		}

		[Usage( "ProfileWorld" )]
		[Description( "Prints the amount of data serialized for every object type in your world file." )]
		public static void ProfileWorld_OnCommand( CommandEventArgs e )
		{
			ProfileWorld( "items", "worldprofile_items.log" );
			ProfileWorld( "mobiles", "worldprofile_mobiles.log" );
		}

		public static void ProfileWorld( string type, string opFile )
		{
			try
			{
				ArrayList types = new ArrayList();

				using ( BinaryReader bin = new BinaryReader( new FileStream( String.Format( "Saves/{0}/{0}.tdb", type ), FileMode.Open, FileAccess.Read, FileShare.Read ) ) )
				{
					int count = bin.ReadInt32();

					for ( int i = 0; i < count; ++i )
						types.Add( ScriptCompiler.FindTypeByFullName( bin.ReadString() ) );
				}

				long total = 0;

				Hashtable table = new Hashtable();

				using ( BinaryReader bin = new BinaryReader( new FileStream( String.Format( "Saves/{0}/{0}.idx", type ), FileMode.Open, FileAccess.Read, FileShare.Read ) ) )
				{
					int count = bin.ReadInt32();

					for ( int i = 0; i < count; ++i )
					{
						int typeID = bin.ReadInt32();
						int serial = bin.ReadInt32();
						long pos = bin.ReadInt64();
						int length = bin.ReadInt32();
						Type objType = (Type)types[typeID];

						while ( objType != null && objType != typeof( object ) )
						{
							object obj = table[objType];

							if ( obj == null )
								table[objType] = length;
							else
								table[objType] = length + (int)obj;

							objType = objType.BaseType;
							total += length;
						}
					}
				}

				ArrayList list = new ArrayList( table );

				list.Sort( new CountSorter() );

				using ( StreamWriter op = new StreamWriter( opFile ) )
				{
					op.WriteLine( "# Profile of world {0}", type );
					op.WriteLine( "# Generated on {0}", DateTime.Now );
					op.WriteLine();
					op.WriteLine();

					foreach ( DictionaryEntry de in list )
						op.WriteLine( "{0}\t{1:F2}%\t{2}", de.Value, (100 * (int)de.Value) / (double)total, de.Key );
				}
			}
			catch
			{
			}
		}

		[Usage( "ViewEquip" )]
		[Description( "Lists equipment of a targeted mobile. From the list you can move, delete, or open props." )]
		public static void ViewEquip_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new ViewEqTarget();
		}

		[Usage( "Sound <index> [toAll=true]" )]
		[Description( "Plays a sound to players within 12 tiles of you. The (toAll) argument specifies to everyone, or just those who can see you." )]
		public static void Sound_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
				PlaySound( e.Mobile, e.GetInt32( 0 ), true );
			else if ( e.Length == 2 )
				PlaySound( e.Mobile, e.GetInt32( 0 ), e.GetBoolean( 1 ) );
			else
				e.Mobile.SendMessage( "Format: Sound <index> [toAll]" );
		}

		private static void PlaySound( Mobile m, int index, bool toAll )
		{
			Map map = m.Map;

			if ( map == null )
				return;

			CommandLogging.WriteLine( m, "{0} {1} playing sound {2} (toAll={3})", m.AccessLevel, CommandLogging.Format( m ), index, toAll );

			Packet p = new PlaySound( index, m.Location );

			p.Acquire();

			foreach ( NetState state in m.GetClientsInRange( 12 ) )
			{
				if ( toAll || state.Mobile.CanSee( m ) )
					state.Send( p );
			}

			p.Release();
		}

		private class BankTarget : Target
		{
			public BankTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					Mobile m = (Mobile)targeted;

					BankBox box = ( m.Player ? m.BankBox : m.FindBankNoCreate() );

					if ( box != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} opening bank box of {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targeted ) );

						if ( from == targeted )
							box.Open();
						else
							box.DisplayTo( from );
					}
					else
					{
						from.SendMessage( "They have no bank box." );
					}
				}
			}
		}

		[Usage( "Echo <text>" )]
		[Description( "Relays (text) as a system message." )]
		public static void Echo_OnCommand( CommandEventArgs e )
		{
			string toEcho = e.ArgString.Trim();

			if ( toEcho.Length > 0 )
				e.Mobile.SendMessage( toEcho );
			else
				e.Mobile.SendMessage( "Format: Echo \"<text>\"" );
		}

		[Usage( "Bank" )]
		[Description( "Opens the bank box of a given target." )]
		public static void Bank_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new BankTarget();
		}

		private class DismountTarget : Target
		{
			public DismountTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					CommandLogging.WriteLine( from, "{0} {1} dismounting {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targeted ) );

					Mobile targ = (Mobile)targeted;

					for ( int i = 0; i < targ.Items.Count; ++i )
					{
						Item item = targ.Items[i];

						if ( item is IMountItem )
						{
							IMount mount = ((IMountItem)item).Mount;

							if ( mount != null )
								mount.Rider = null;

							if ( targ.Items.IndexOf( item ) == -1 )
								--i;
						}
					}

					for ( int i = 0; i < targ.Items.Count; ++i )
					{
						Item item = targ.Items[i];

						if ( item.Layer == Layer.Mount )
						{
							item.Delete();
							--i;
						}
					}
				}
			}
		}

		private class ClientTarget : Target
		{
			public ClientTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					Mobile targ = (Mobile)targeted;

					if ( targ.NetState != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} opening client menu of {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targeted ) );
						from.SendGump( new ClientGump( from, targ.NetState ) );
					}
				}
			}
		}

		[Usage( "Client" )]
		[Description( "Opens the client gump menu for a given player." )]
		private static void Client_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new ClientTarget();
		}

		[Usage( "Move" )]
		[Description( "Repositions a targeted item or mobile." )]
		private static void Move_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new PickMoveTarget();
		}

		private class FirewallTarget : Target
		{
			public FirewallTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					Mobile targ = (Mobile)targeted;

					NetState state = targ.NetState;

					if ( state != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} firewalling {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targeted ) );

						try
						{
							Firewall.Add( ((IPEndPoint)state.Socket.RemoteEndPoint).Address );
						}
						catch
						{
						}
					}
				}
			}
		}

		[Usage( "Save" )]
		[Description( "Saves the world." )]
		private static void Save_OnCommand( CommandEventArgs e )
		{
			Misc.AutoSave.Save();
		}

		private static bool FixMap( ref Map map, ref Point3D loc, Item item )
		{
			if ( map == null || map == Map.Internal )
			{
				Mobile m = item.RootParent as Mobile;

				return ( m != null && FixMap( ref map, ref loc, m ) );
			}

			return true;
		}

		private static bool FixMap( ref Map map, ref Point3D loc, Mobile m )
		{
			if ( map == null || map == Map.Internal )
			{
				map = m.LogoutMap;
				loc = m.LogoutLocation;
			}

			return ( map != null && map != Map.Internal );
		}

		[Usage( "Go [name | serial | (x y [z]) | (deg min (N | S) deg min (E | W))]" )]
		[Description( "With no arguments, this command brings up the go menu. With one argument, (name), you are moved to that regions \"go location.\" Or, if a numerical value is specified for one argument, (serial), you are moved to that object. Two or three arguments, (x y [z]), will move your character to that location. When six arguments are specified, (deg min (N | S) deg min (E | W)), your character will go to an approximate of those sextant coordinates." )]
		private static void Go_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Length == 0 )
			{
				GoGump.DisplayTo( from );
				return;
			}

			if ( GoGump.MoveToLocation( from, e.ArgString ) )
					return;


			if ( e.Length == 1 )
			{
				try
				{
					int ser = e.GetInt32( 0 );

					IEntity ent = World.FindEntity( ser );

					if ( ent is Item )
					{
						Item item = (Item)ent;

						Map map = item.Map;
						Point3D loc = item.GetWorldLocation();

						Mobile owner = item.RootParent as Mobile;

						if( owner != null && (owner.Map != null && owner.Map != Map.Internal) && !BaseCommand.IsAccessible( from, owner ) /* !from.CanSee( owner )*/ )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( owner != null && (owner.Map == null || owner.Map == Map.Internal) && owner.Hidden && owner.AccessLevel >= from.AccessLevel )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( !FixMap( ref map, ref loc, item ) )
						{
							from.SendMessage( "That is an internal item and you cannot go to it." );
							return;
						}

						from.MoveToWorld( loc, map );

						return;
					}
					else if ( ent is Mobile )
					{
						Mobile m = (Mobile)ent;

						Map map = m.Map;
						Point3D loc = m.Location;

						Mobile owner = m;

						if ( owner != null && (owner.Map != null && owner.Map != Map.Internal) && !BaseCommand.IsAccessible( from, owner ) /* !from.CanSee( owner )*/ )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( owner != null && (owner.Map == null || owner.Map == Map.Internal) && owner.Hidden && owner.AccessLevel >= from.AccessLevel )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( !FixMap( ref map, ref loc, m ) )
						{
							from.SendMessage( "That is an internal mobile and you cannot go to it." );
							return;
						}

						from.MoveToWorld( loc, map );

						return;
					}
					else
					{
						string name = e.GetString( 0 );
						Map map;

						for ( int i = 0; i < Map.AllMaps.Count; ++i )
						{
							map = Map.AllMaps[i];

							if ( map.MapIndex == 0x7F || map.MapIndex == 0xFF )
								continue;

							if ( Insensitive.Equals( name, map.Name ) )
							{
								from.Map = map;
								return;
							}
						}

						Dictionary<string, Region> list = from.Map.Regions;

						foreach( KeyValuePair<string, Region> kvp in list )
						{
							Region r = kvp.Value;

							if ( Insensitive.Equals( r.Name, name ) )
							{
								from.Location = new Point3D( r.GoLocation );
								return;
							}
						}

						for( int i = 0; i < Map.AllMaps.Count; ++i )
						{
							Map m = Map.AllMaps[i];

							if( m.MapIndex == 0x7F || m.MapIndex == 0xFF || from.Map == m )
								continue;

							foreach( Region r in m.Regions.Values )
							{
								if( Insensitive.Equals( r.Name, name ) )
								{
									from.MoveToWorld( r.GoLocation, m );
									return;
								}
							}
						}



						if ( ser != 0 )
							from.SendMessage( "No object with that serial was found." );
						else
							from.SendMessage( "No region with that name was found." );

						return;
					}
				}
				catch
				{
				}

				from.SendMessage( "Region name not found" );
			}
			else if ( e.Length == 2 || e.Length == 3 )
			{
				Map map = from.Map;

				if ( map != null )
				{
					try
					{
						/*
						 * This to avoid being teleported to (0,0) if trying to teleport
						 * to a region with spaces in its name.
						 */
						int x = int.Parse( e.GetString( 0 ) );
						int y = int.Parse( e.GetString( 1 ) );
						int z = (e.Length == 3 ) ? int.Parse( e.GetString( 2 ) ) : map.GetAverageZ( x, y );

						from.Location = new Point3D( x, y, z );
					}
					catch
					{
						from.SendMessage( "Region name not found." );
					}
				}
			}
			else if ( e.Length == 6 )
			{
				Map map = from.Map;

				if ( map != null )
				{
					Point3D p = Sextant.ReverseLookup( map, e.GetInt32( 3 ), e.GetInt32( 0 ), e.GetInt32( 4 ), e.GetInt32( 1 ), Insensitive.Equals( e.GetString( 5 ), "E" ), Insensitive.Equals( e.GetString( 2 ), "S" ) );

					if ( p != Point3D.Zero )
						from.Location = p;
					else
						from.SendMessage( "Sextant reverse lookup failed." );
				}
			}
			else
			{
				from.SendMessage( "Format: Go [name | serial | (x y [z]) | (deg min (N | S) deg min (E | W)]" );
			}
		}

		/*[Usage( "Help" )]
		[Description( "Lists all available commands." )]
		public static void Help_OnCommand( CommandEventArgs e )
		{
			Mobile m = e.Mobile;

			List<CommandEntry> list = new List<CommandEntry>();

			foreach ( CommandEntry entry in CommandSystem.Entries.Values )
				if ( m.AccessLevel >= entry.AccessLevel )
					list.Add( entry );

			list.Sort();

			StringBuilder sb = new StringBuilder();

			if ( list.Count > 0 )
				sb.Append( list[0].Command );

			for ( int i = 1; i < list.Count; ++i )
			{
				string v = list[i].Command;

				if ( (sb.Length + 1 + v.Length) >= 256 )
				{
					m.SendAsciiMessage( 0x482, sb.ToString() );
					sb = new StringBuilder();
					sb.Append( v );
				}
				else
				{
					sb.Append( ' ' );
					sb.Append( v );
				}
			}

			if ( sb.Length > 0 )
				m.SendAsciiMessage( 0x482, sb.ToString() );
		}*/

		[Usage( "SMsg <text>" )]
		[Aliases( "S", "SM" )]
		[Description( "Broadcasts a message to all online staff." )]
		public static void StaffMessage_OnCommand( CommandEventArgs e )
		{
			BroadcastMessage( AccessLevel.Counselor, e.Mobile.SpeechHue, String.Format( "[{0}] {1}", e.Mobile.Name, e.ArgString ) );
		}

		[Usage( "BCast <text>" )]
		[Aliases( "B", "BC" )]
		[Description( "Broadcasts a message to everyone online." )]
		public static void BroadcastMessage_OnCommand( CommandEventArgs e )
		{
			BroadcastMessage( AccessLevel.Player, 0x482, String.Format( "Staff message from {0}:", e.Mobile.Name ) );
			BroadcastMessage( AccessLevel.Player, 0x482, e.ArgString );
		}

		public static void BroadcastMessage ( AccessLevel ac, int hue, string message ) 
		{ 
			foreach ( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if ( m != null && m.AccessLevel >= ac )
					m.SendMessage( hue, message );
			}
		}

		private class DeleteItemByLayerTarget : Target
		{
			private Layer m_Layer;

			public DeleteItemByLayerTarget( Layer layer ) : base( -1, false, TargetFlags.None )
			{
				m_Layer = layer;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					Item item = ((Mobile)targeted).FindItemOnLayer( m_Layer );

					if ( item != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} deleting item on layer {2} of {3}", from.AccessLevel, CommandLogging.Format( from ), m_Layer, CommandLogging.Format( targeted ) );
						item.Delete();
					}
				}
				else
				{
					from.SendMessage( "Target a mobile." );
				}
			}
		}

		[Usage( "AutoPageNotify" )]
		[Aliases( "APN" )]
		[Description( "Toggles your auto-page-notify status." )]
		public static void APN_OnCommand( CommandEventArgs e )
		{
			Mobile m = e.Mobile;

			m.AutoPageNotify = !m.AutoPageNotify;

			m.SendMessage( "Your auto-page-notify has been turned {0}.", m.AutoPageNotify ? "on" : "off" );
		}

		[Usage( "Animate <action> <frameCount> <repeatCount> <forward> <repeat> <delay>" )]
		[Description( "Makes your character do a specified animation." )]
		public static void Animate_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 6 )
			{
				e.Mobile.Animate( e.GetInt32( 0 ), e.GetInt32( 1 ), e.GetInt32( 2 ), e.GetBoolean( 3 ), e.GetBoolean( 4 ), e.GetInt32( 5 ) );
			}
			else
			{
				e.Mobile.SendMessage( "Format: Animate <action> <frameCount> <repeatCount> <forward> <repeat> <delay>" );
			}
		}

		[Usage( "Cast <name>" )]
		[Description( "Casts a spell by name." )]
		public static void Cast_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
			{
				if ( !Multis.DesignContext.Check( e.Mobile ) )
					return; // They are customizing

				Spell spell = SpellRegistry.NewSpell( e.GetString( 0 ), e.Mobile, null );

				if ( spell != null )
					spell.Cast();
				else
					e.Mobile.SendMessage( "That spell was not found." );
			}
			else
			{
				e.Mobile.SendMessage( "Format: Cast <name>" );
			}
		}

		private class StuckMenuTarget : Target
		{
			public StuckMenuTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					if( ((Mobile)targeted).AccessLevel >= from.AccessLevel && targeted != from )
						from.SendMessage( "You can't do that to someone with higher Accesslevel than you!" );
					else
						from.SendGump( new StuckMenu( from, (Mobile) targeted, false ) );
				}
			}
		}

		[Usage( "Stuck" )]
		[Description( "Opens a menu of towns, used for teleporting stuck mobiles." )]
		public static void Stuck_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new StuckMenuTarget();
		}

		[Usage( "Light <level>")]
		[Description( "Set your local lightlevel." )]
		public static void Light_OnCommand( CommandEventArgs e )
		{
			e.Mobile.LightLevel = e.GetInt32( 0 );
		}

		[Usage( "Stats")]
		[Description( "View some stats about the server." )]
		public static void Stats_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Open Connections: {0}", Network.NetState.Instances.Count );
			e.Mobile.SendMessage( "Mobiles: {0}", World.Mobiles.Count );
			e.Mobile.SendMessage( "Items: {0}", World.Items.Count );
		}
	}
}
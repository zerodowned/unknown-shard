using System;
using System.Collections;
using System.Xml;
using Server;
using System.Collections.Generic;

namespace Server.Regions
{
	public enum SpawnZLevel
	{
		Lowest,
		Highest,
		Random
	}

	public class BaseRegion : Region
	{
		public static void Configure()
		{
			Region.DefaultRegionType = typeof( BaseRegion );
		}

		private string m_RuneName;
		private bool m_NoLogoutDelay;

		private SpawnEntry[] m_Spawns;
		private SpawnZLevel m_SpawnZLevel;
		private bool m_ExcludeFromParentSpawns;

		public string RuneName{ get{ return m_RuneName; } set{ m_RuneName = value; } }

		public bool NoLogoutDelay{ get{ return m_NoLogoutDelay; } set{ m_NoLogoutDelay = value; } }

		public SpawnEntry[] Spawns
		{
			get{ return m_Spawns; }
			set
			{
				if ( m_Spawns != null )
				{
					for ( int i = 0; i < m_Spawns.Length; i++ )
						m_Spawns[i].Delete();
				}

				m_Spawns = value;
			}
		}

		public SpawnZLevel SpawnZLevel{ get{ return m_SpawnZLevel; } set{ m_SpawnZLevel = value; } }

		public bool ExcludeFromParentSpawns{ get{ return m_ExcludeFromParentSpawns; } set{ m_ExcludeFromParentSpawns = value; } }

		public override void OnUnregister()
		{
			base.OnUnregister();

			this.Spawns = null;
		}

		public static string GetRuneNameFor( Region region )
		{
			while ( region != null )
			{
				BaseRegion br = region as BaseRegion;

				if ( br != null && br.m_RuneName != null )
					return br.m_RuneName;

				region = region.Parent;
			}

			return null;
		}

		public override TimeSpan GetLogoutDelay( Mobile m )
		{
			if ( m_NoLogoutDelay )
			{
				if ( m.Aggressors.Count == 0 && m.Aggressed.Count == 0 && !m.Criminal )
					return TimeSpan.Zero;
			}

			return base.GetLogoutDelay( m );
		}

		public static bool CanSpawn( Region region, params Type[] types )
		{
			while ( region != null )
			{
				if ( !region.AllowSpawn() )
					return false;

				BaseRegion br = region as BaseRegion;

				if ( br != null )
				{
					if ( br.Spawns != null )
					{
						for ( int i = 0; i < br.Spawns.Length; i++ )
						{
							SpawnEntry entry = br.Spawns[i];

							if ( entry.Definition.CanSpawn( types ) )
								return true;
						}
					}

					if ( br.ExcludeFromParentSpawns )
						return false;
				}

				region = region.Parent;
			}

			return false;
		}

		public override bool AcceptsSpawnsFrom( Region region )
		{
			if ( region == this || !m_ExcludeFromParentSpawns )
				return base.AcceptsSpawnsFrom( region );

			return false;
		}

		private Rectangle3D[] m_Rectangles;
		private int[] m_RectangleWeights;
		private int m_TotalWeight;

		private static ArrayList m_Buffer1 = new ArrayList();
		private static ArrayList m_Buffer2 = new ArrayList();

		private void InitRectangles()
		{
			if ( m_Rectangles != null )
				return;

			// Test if area rectangles are overlapping, and in that case break them into smaller non overlapping rectangles
			for ( int i = 0; i < this.Area.Length; i++ )
			{
				m_Buffer2.Add( this.Area[i] );

				for ( int j = 0; j < m_Buffer1.Count && m_Buffer2.Count > 0; j++ )
				{
					Rectangle3D comp = (Rectangle3D) m_Buffer1[j];

					for ( int k = m_Buffer2.Count - 1; k >= 0; k-- )
					{
						Rectangle3D rect = (Rectangle3D) m_Buffer2[k];

						int l1 = rect.Start.X, r1 = rect.End.X, t1 = rect.Start.Y, b1 = rect.End.Y;
						int l2 = comp.Start.X, r2 = comp.End.X, t2 = comp.Start.Y, b2 = comp.End.Y;

						if ( l1 < r2 && r1 > l2 && t1 < b2 && b1 > t2 )
						{
							m_Buffer2.RemoveAt( k );

							int sz = rect.Start.Z;
							int ez = rect.End.X;

							if ( l1 < l2 )
							{
								m_Buffer2.Add( new Rectangle3D( new Point3D( l1, t1, sz ), new Point3D( l2, b1, ez ) ) );
							}

							if ( r1 > r2 )
							{
								m_Buffer2.Add( new Rectangle3D( new Point3D( r2, t1, sz ), new Point3D( r1, b1, ez ) ) );
							}

							if ( t1 < t2 )
							{
								m_Buffer2.Add( new Rectangle3D( new Point3D( Math.Max( l1, l2 ), t1, sz ), new Point3D( Math.Min( r1, r2 ), t2, ez ) ) );
							}

							if ( b1 > b2 )
							{
								m_Buffer2.Add( new Rectangle3D( new Point3D( Math.Max( l1, l2 ), b2, sz ), new Point3D( Math.Min( r1, r2 ), b1, ez ) ) );
							}
						}
					}
				}

				m_Buffer1.AddRange( m_Buffer2 );
				m_Buffer2.Clear();
			}

			m_Rectangles = (Rectangle3D[]) m_Buffer1.ToArray( typeof( Rectangle3D ) );
			m_Buffer1.Clear();

			m_RectangleWeights = new int[m_Rectangles.Length];
			for ( int i = 0; i < m_Rectangles.Length; i++ )
			{
				Rectangle3D rect = m_Rectangles[i];
				int weight = rect.Width * rect.Height;

				m_RectangleWeights[i] = weight;
				m_TotalWeight += weight;
			}
		}

		public Point3D RandomSpawnLocation( int spawnHeight, bool land, bool water, Point3D home, int range )
		{
			Map map = this.Map;

			if ( map == Map.Internal )
				return Point3D.Zero;

			InitRectangles();

			if ( m_TotalWeight <= 0 )
				return Point3D.Zero;

			for ( int i = 0; i < 10; i++ ) // Try 10 times
			{
				int x, y, minZ, maxZ;

				if ( home == Point3D.Zero )
				{
					int rand = Utility.Random( m_TotalWeight );

					x = int.MinValue; y = int.MinValue;
					minZ = int.MaxValue; maxZ = int.MinValue;
					for ( int j = 0; j < m_RectangleWeights.Length; j++ )
					{
						int curWeight = m_RectangleWeights[j];

						if ( rand < curWeight )
						{
							Rectangle3D rect = m_Rectangles[j];

							x = rect.Start.X + rand % rect.Width;
							y = rect.Start.Y + rand / rect.Width;

							minZ = rect.Start.Z;
							maxZ = rect.End.Z;

							break;
						}

						rand -= curWeight;
					}
				}
				else
				{
					x = Utility.RandomMinMax( home.X - range, home.X + range );
					y = Utility.RandomMinMax( home.Y - range, home.Y + range );

					minZ = int.MaxValue; maxZ = int.MinValue;
					for ( int j = 0; j < this.Area.Length; j++ )
					{
						Rectangle3D rect = this.Area[j];

						if ( x >= rect.Start.X && x < rect.End.X && y >= rect.Start.Y && y < rect.End.Y )
						{
							minZ = rect.Start.Z;
							maxZ = rect.End.Z;
							break;
						}
					}

					if ( minZ == int.MaxValue )
						continue;
				}

				if ( x < 0 || y < 0 || x >= map.Width || y >= map.Height )
					continue;


				Tile lt = map.Tiles.GetLandTile( x, y );

				int ltLowZ = 0, ltAvgZ = 0, ltTopZ = 0;
				map.GetAverageZ( x, y, ref ltLowZ, ref ltAvgZ, ref ltTopZ );

				TileFlag ltFlags = TileData.LandTable[lt.ID & 0x3FFF].Flags;
				bool ltImpassable = ( (ltFlags & TileFlag.Impassable) != 0 );

				if ( !lt.Ignored && ltAvgZ >= minZ && ltAvgZ < maxZ )
				{
					if ( (ltFlags & TileFlag.Wet) != 0 )
					{
						if ( water )
							m_Buffer1.Add( ltAvgZ );
					}
					else if ( land && !ltImpassable )
					{
						m_Buffer1.Add( ltAvgZ );
					}
				}


				Tile[] staticTiles = map.Tiles.GetStaticTiles( x, y, true );

				for ( int j = 0; j < staticTiles.Length; j++ )
				{
					Tile tile = staticTiles[j];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];
					int tileZ = tile.Z + id.CalcHeight;

					if ( tileZ >= minZ && tileZ < maxZ )
					{
						if ( (id.Flags & TileFlag.Wet) != 0 )
						{
							if ( water )
								m_Buffer1.Add( tileZ );
						}
						else if ( land && id.Surface && !id.Impassable )
						{
							m_Buffer1.Add( tileZ );
						}
					}
				}


				Sector sector = map.GetSector( x, y );

				for ( int j = 0; j < sector.Items.Count; j++ )
				{
					Item item = sector.Items[j];

					if ( item.ItemID < 0x4000 && item.AtWorldPoint( x, y ) )
					{
						m_Buffer2.Add( item );

						if ( !item.Movable )
						{
							ItemData id = item.ItemData;
							int itemZ = item.Z + id.CalcHeight;

							if ( itemZ >= minZ && itemZ < maxZ )
							{
								if ( (id.Flags & TileFlag.Wet) != 0 )
								{
									if ( water )
										m_Buffer1.Add( itemZ );
								}
								else if ( land && id.Surface && !id.Impassable )
								{
									m_Buffer1.Add( itemZ );
								}
							}
						}
					}
				}


				if ( m_Buffer1.Count == 0 )
				{
					m_Buffer1.Clear();
					m_Buffer2.Clear();
					continue;
				}

				int z;
				switch ( m_SpawnZLevel )
				{
					case SpawnZLevel.Lowest:
					{
						z = int.MaxValue;

						for ( int j = 0; j < m_Buffer1.Count; j++ )
						{
							int l = (int) m_Buffer1[j];

							if ( l < z )
								z = l;
						}

						break;
					}
					case SpawnZLevel.Highest:
					{
						z = int.MinValue;

						for ( int j = 0; j < m_Buffer1.Count; j++ )
						{
							int l = (int) m_Buffer1[j];

							if ( l > z )
								z = l;
						}

						break;
					}
					default: // SpawnZLevel.Random
					{
						int index = Utility.Random( m_Buffer1.Count );
						z = (int) m_Buffer1[index];

						break;
					}
				}

				m_Buffer1.Clear();


				if ( !Region.Find( new Point3D( x, y, z ), map ).AcceptsSpawnsFrom( this ) )
				{
					m_Buffer2.Clear();
					continue;
				}


				int top = z + spawnHeight;

				bool ok = true;
				for ( int j = 0; j < m_Buffer2.Count; j++ )
				{
					Item item = (Item) m_Buffer2[j];
					ItemData id = item.ItemData;

					if ( ( id.Surface || id.Impassable ) && item.Z + id.CalcHeight > z && item.Z < top )
					{
						ok = false;
						break;
					}
				}

				m_Buffer2.Clear();

				if ( !ok )
					continue;


				if ( ltImpassable && ltAvgZ > z && ltLowZ < top )
					continue;


				for ( int j = 0; j < staticTiles.Length; j++ )
				{
					Tile tile = staticTiles[j];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					if ( ( id.Surface || id.Impassable ) && tile.Z + id.CalcHeight > z && tile.Z < top )
					{
						ok = false;
						break;
					}
				}

				if ( !ok )
					continue;


				for ( int j = 0; j < sector.Mobiles.Count; j++ )
				{
					Mobile m = (Mobile) sector.Mobiles[j];

					if ( m.X == x && m.Y == y && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
					{
						if ( m.Z + 16 > z && m.Z < top )
						{
							ok = false;
							break;
						}
					}
				}


				if ( ok )
					return new Point3D( x, y, z );
			}

			return Point3D.Zero;
		}

		public override string ToString()
		{
			if ( this.Name != null )
				return this.Name;
			else if ( this.RuneName != null )
				return this.RuneName;
			else
				return this.GetType().Name;
		}

		public BaseRegion( string name, Map map, int priority, params Rectangle2D[] area ) : base( name, map, priority, area )
		{
		}

		public BaseRegion( string name, Map map, int priority, params Rectangle3D[] area ) : base( name, map, priority, area )
		{
		}

		public BaseRegion( string name, Map map, Region parent, params Rectangle2D[] area ) : base( name, map, parent, area )
		{
		}

		public BaseRegion( string name, Map map, Region parent, params Rectangle3D[] area ) : base( name, map, parent, area )
		{
		}

		public BaseRegion( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
			ReadString( xml["rune"], "name", ref m_RuneName, false );

			bool logoutDelayActive = true;
			ReadBoolean( xml["logoutDelay"], "active", ref logoutDelayActive, false );
			m_NoLogoutDelay = !logoutDelayActive;


			XmlElement spawning = xml["spawning"];
			if ( spawning != null )
			{
				ReadBoolean( spawning, "excludeFromParent", ref m_ExcludeFromParentSpawns, false );

				object zLevel = SpawnZLevel.Lowest;
				ReadEnum( spawning, "zLevel", typeof( SpawnZLevel ), ref zLevel, false );
				m_SpawnZLevel = (SpawnZLevel) zLevel;


				List<SpawnEntry> list = new List<SpawnEntry>();

				foreach ( XmlNode node in spawning.ChildNodes )
				{
					XmlElement el = node as XmlElement;

					if ( el != null )
					{
						SpawnDefinition def = SpawnDefinition.GetSpawnDefinition( el );
						if ( def == null )
							continue;

						int id = 0;
						if ( !ReadInt32( el, "id", ref id, true ) )
							continue;

						int amount = 0;
						if ( !ReadInt32( el, "amount", ref amount, true ) )
							continue;

						TimeSpan minSpawnTime = SpawnEntry.DefaultMinSpawnTime;
						ReadTimeSpan( el, "minSpawnTime", ref minSpawnTime, false );

						TimeSpan maxSpawnTime = SpawnEntry.DefaultMaxSpawnTime;
						ReadTimeSpan( el, "maxSpawnTime", ref maxSpawnTime, false );

						Point3D home = Point3D.Zero;
						int range = 0;

						XmlElement homeEl = el["home"];
						if ( ReadPoint3D( homeEl, map, ref home, false ) )
							ReadInt32( homeEl, "range", ref range, false );

						object oDir = SpawnEntry.InvalidDirection;
						ReadEnum( el["direction"], "value" , typeof( Direction ), ref oDir, false );
						Direction dir = (Direction) oDir;

						SpawnEntry entry = new SpawnEntry( id, this, home, range, dir, def, amount, minSpawnTime, maxSpawnTime );
						list.Add( entry );
					}
				}

				if ( list.Count > 0 )
				{
					m_Spawns = list.ToArray();
				}
			}
		}
	}
}
using System;
using Server.Engines.XmlSpawner2;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum GemType
	{
		None,
		StarSapphire,
		Emerald,
		Sapphire,
		Ruby,
		Citrine,
		Amethyst,
		Tourmaline,
		Amber,
		Diamond
	}

	public abstract class BaseJewel : Item, ICraftable
	{
		private bool m_Identified;
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Identified
		{
			get{ return m_Identified; }
			set{ m_Identified = value; InvalidateProperties(); }
		}

		private AosAttributes m_AosAttributes;
		private AosElementAttributes m_AosResistances;
		private AosSkillBonuses m_AosSkillBonuses;
		private CraftResource m_Resource;
		private GemType m_GemType;

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get{ return m_AosAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosElementAttributes Resistances
		{
			get{ return m_AosResistances; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosSkillBonuses SkillBonuses
		{
			get{ return m_AosSkillBonuses; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; Hue = CraftResources.GetHue( m_Resource ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public GemType GemType
		{
			get{ return m_GemType; }
			set{ m_GemType = value; InvalidateProperties(); }
		}

		public override int PhysicalResistance{ get{ return m_AosResistances.Physical; } }
		public override int FireResistance{ get{ return m_AosResistances.Fire; } }
		public override int ColdResistance{ get{ return m_AosResistances.Cold; } }
		public override int PoisonResistance{ get{ return m_AosResistances.Poison; } }
		public override int EnergyResistance{ get{ return m_AosResistances.Energy; } }
		public virtual int BaseGemTypeNumber{ get{ return 0; } }

		public override int LabelNumber
		{
			get
			{
				if ( m_GemType == GemType.None )
					return base.LabelNumber;

				return BaseGemTypeNumber + (int)m_GemType - 1;
			}
		}

		public virtual int ArtifactRarity{ get{ return 0; } }

		public BaseJewel( int itemID, Layer layer ) : base( itemID )
		{
			m_AosAttributes = new AosAttributes( this );
			m_AosResistances = new AosElementAttributes( this );
			m_AosSkillBonuses = new AosSkillBonuses( this );
			m_Resource = CraftResource.Iron;
			m_GemType = GemType.None;

            Layer = layer;
            // mod to randomly add sockets and socketability features to weapons. These settings will yield
            // 2% drop rate of socketed/socketable items
            // 0.1% chance of 5 sockets
            // 0.5% of 4 sockets
            // 3% chance of 3 sockets
            // 15% chance of 2 sockets
            // 50% chance of 1 socket
            // the remainder will be 0 socket (31.4% in this case)
            // uncomment the next line to prevent artifacts from being socketed
            // if(ArtifactRarity == 0)
            XmlSockets.ConfigureRandom(this, 2.0, 0.1, 0.5, 3.0, 15.0, 50.0);
		}

		#region ItemID_Mods
       		public override bool CanEquip(Mobile from)
     		{
       		if (from.AccessLevel < AccessLevel.GameMaster)
       	    	{

                if (m_Identified == false)
                {
                from.SendMessage("You are hesitant to put on something that is unknown to you");
                return false;
                }

            	}
            	return base.CanEquip(from);
        	}
        				#endregion

		public override void OnAdded( object parent )
		{
			if ( Core.AOS && parent is Mobile )
			{
				Mobile from = (Mobile)parent;

				m_AosSkillBonuses.AddTo( from );

				int strBonus = m_AosAttributes.BonusStr;
				int dexBonus = m_AosAttributes.BonusDex;
				int intBonus = m_AosAttributes.BonusInt;

				if ( strBonus != 0 || dexBonus != 0 || intBonus != 0 )
				{
					string modName = this.Serial.ToString();

					if ( strBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

					if ( dexBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

					if ( intBonus != 0 )
						from.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
				}

			from.CheckStatTimers();
		}

    		// XmlAttachment check for OnEquip and CanEquip
		if(parent is Mobile)
    		{
        		if(Server.Engines.XmlSpawner2.XmlAttach.CheckCanEquip(this, (Mobile)parent))
        		{
        			Server.Engines.XmlSpawner2.XmlAttach.CheckOnEquip(this, (Mobile)parent);
        		} else
        		{
        			((Mobile)parent).AddToBackpack(this);
        		}
    		}
	}
		public override void OnRemoved( object parent )
		{
			if ( Core.AOS && parent is Mobile )
			{
				Mobile from = (Mobile)parent;

				m_AosSkillBonuses.Remove();

				string modName = this.Serial.ToString();

				from.RemoveStatMod( modName + "Str" );
				from.RemoveStatMod( modName + "Dex" );
				from.RemoveStatMod( modName + "Int" );

			from.CheckStatTimers();
		}

		// XmlAttachment check for OnRemoved
		Server.Engines.XmlSpawner2.XmlAttach.CheckOnRemoved(this, parent);
	}

		public BaseJewel( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			m_AosSkillBonuses.GetProperties( list );

			int prop;
#region ItemID_Mods
           					 if ( m_Identified )
            					 {

			if ( (prop = ArtifactRarity) > 0 )
				list.Add( 1061078, prop.ToString() ); // artifact rarity ~1_val~

			if ( (prop = m_AosAttributes.WeaponDamage) != 0 )
				list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

			if ( (prop = m_AosAttributes.DefendChance) != 0 )
				list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusDex) != 0 )
				list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

			if ( (prop = m_AosAttributes.EnhancePotions) != 0 )
				list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

			if ( (prop = m_AosAttributes.CastRecovery) != 0 )
				list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~

			if ( (prop = m_AosAttributes.CastSpeed) != 0 )
				list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

			if ( (prop = m_AosAttributes.AttackChance) != 0 )
				list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusHits) != 0 )
				list.Add( 1060431, prop.ToString() ); // hit point increase ~1_val~

			if ( (prop = m_AosAttributes.BonusInt) != 0 )
				list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

			if ( (prop = m_AosAttributes.LowerManaCost) != 0 )
				list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

			if ( (prop = m_AosAttributes.LowerRegCost) != 0 )
				list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

			if ( (prop = m_AosAttributes.Luck) != 0 )
				list.Add( 1060436, prop.ToString() ); // luck ~1_val~

			if ( (prop = m_AosAttributes.BonusMana) != 0 )
				list.Add( 1060439, prop.ToString() ); // mana increase ~1_val~

			if ( (prop = m_AosAttributes.RegenMana) != 0 )
				list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~

			if ( (prop = m_AosAttributes.NightSight) != 0 )
				list.Add( 1060441 ); // night sight

			if ( (prop = m_AosAttributes.ReflectPhysical) != 0 )
				list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

			if ( (prop = m_AosAttributes.RegenStam) != 0 )
				list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

			if ( (prop = m_AosAttributes.RegenHits) != 0 )
				list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~

			if ( (prop = m_AosAttributes.SpellChanneling) != 0 )
				list.Add( 1060482 ); // spell channeling

			if ( (prop = m_AosAttributes.SpellDamage) != 0 )
				list.Add( 1060483, prop.ToString() ); // spell damage increase ~1_val~%

			if ( (prop = m_AosAttributes.BonusStam) != 0 )
				list.Add( 1060484, prop.ToString() ); // stamina increase ~1_val~

			if ( (prop = m_AosAttributes.BonusStr) != 0 )
				list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

			if ( (prop = m_AosAttributes.WeaponSpeed) != 0 )
				list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%

				base.AddResistanceProperties( list );
						}
            					else if ((ArtifactRarity) > 0 || (m_AosAttributes.WeaponDamage) != 0 || (m_AosAttributes.DefendChance) != 0 ||
                    					(m_AosAttributes.BonusDex) != 0 || (m_AosAttributes.EnhancePotions) != 0 || (m_AosAttributes.CastRecovery) != 0 ||
                    					(m_AosAttributes.CastSpeed) != 0 || (m_AosAttributes.AttackChance) != 0 || (m_AosAttributes.BonusHits) != 0 ||
                    					(m_AosAttributes.BonusInt) != 0 || (m_AosAttributes.LowerManaCost) != 0 || (m_AosAttributes.LowerRegCost) != 0 ||
                    					(m_AosAttributes.BonusMana) != 0 || (m_AosAttributes.RegenMana) != 0 || (m_AosAttributes.NightSight) != 0 ||
                    					(m_AosAttributes.ReflectPhysical) != 0 || (m_AosAttributes.RegenStam) != 0 || (m_AosAttributes.RegenHits) != 0 ||
                    					(m_AosAttributes.SpellChanneling) != 0 || (m_AosAttributes.SpellDamage) != 0 ||
                    					(m_AosAttributes.BonusStam) != 0 || (m_AosAttributes.BonusStr) != 0 || (m_AosAttributes.WeaponSpeed) != 0)
                          list.Add(1038000); // Unidentified
					#endregion
			
        // mod to display attachment properties
        Server.Engines.XmlSpawner2.XmlAttach.AddAttachmentProperties(this, list);
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			#region ItemID_Mods
          		writer.Write( (bool)m_Identified );
			#endregion

			writer.WriteEncodedInt( (int) m_Resource );
			writer.WriteEncodedInt( (int) m_GemType );

			m_AosAttributes.Serialize( writer );
			m_AosResistances.Serialize( writer );
			m_AosSkillBonuses.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				#region ItemID_Mods
				case (3):
				{
				m_Identified = reader.ReadBool();
				goto case 2;
				}
				#endregion
				case 2:
				{
					m_Resource = (CraftResource)reader.ReadEncodedInt();
					m_GemType = (GemType)reader.ReadEncodedInt();

					goto case 1;
				}
				case 1:
				{
					m_AosAttributes = new AosAttributes( this, reader );
					m_AosResistances = new AosElementAttributes( this, reader );
					m_AosSkillBonuses = new AosSkillBonuses( this, reader );

					if ( Core.AOS && Parent is Mobile )
						m_AosSkillBonuses.AddTo( (Mobile)Parent );

					int strBonus = m_AosAttributes.BonusStr;
					int dexBonus = m_AosAttributes.BonusDex;
					int intBonus = m_AosAttributes.BonusInt;

					if ( Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
					{
						Mobile m = (Mobile)Parent;

						string modName = Serial.ToString();

						if ( strBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Str, modName + "Str", strBonus, TimeSpan.Zero ) );

						if ( dexBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero ) );

						if ( intBonus != 0 )
							m.AddStatMod( new StatMod( StatType.Int, modName + "Int", intBonus, TimeSpan.Zero ) );
					}

					if ( Parent is Mobile )
						((Mobile)Parent).CheckStatTimers();

					break;
				}
				case 0:
				{
					m_AosAttributes = new AosAttributes( this );
					m_AosResistances = new AosElementAttributes( this );
					m_AosSkillBonuses = new AosSkillBonuses( this );

					break;
				}
			}

			if ( version < 2 )
			{
				m_Resource = CraftResource.Iron;
				m_GemType = GemType.None;
			}
		}
		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			Resource = CraftResources.GetFromType( resourceType );

			CraftContext context = craftSystem.GetContext( from );

			if ( context != null && context.DoNotColor )
				Hue = 0;

			if ( 1 < craftItem.Ressources.Count )
			{
				resourceType = craftItem.Ressources.GetAt( 1 ).ItemType;

				if ( resourceType == typeof( StarSapphire ) )
					GemType = GemType.StarSapphire;
				else if ( resourceType == typeof( Emerald ) )
					GemType = GemType.Emerald;
				else if ( resourceType == typeof( Sapphire ) )
					GemType = GemType.Sapphire;
				else if ( resourceType == typeof( Ruby ) )
					GemType = GemType.Ruby;
				else if ( resourceType == typeof( Citrine ) )
					GemType = GemType.Citrine;
				else if ( resourceType == typeof( Amethyst ) )
					GemType = GemType.Amethyst;
				else if ( resourceType == typeof( Tourmaline ) )
					GemType = GemType.Tourmaline;
				else if ( resourceType == typeof( Amber ) )
					GemType = GemType.Amber;
				else if ( resourceType == typeof( Diamond ) )
					GemType = GemType.Diamond;
			}

			return 1;
		}

		#endregion
	}
}
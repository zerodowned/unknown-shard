using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PotionKeg : Item
	{
		private PotionEffect m_Type;
		private int m_Held;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Held
		{
			get
			{
				return m_Held;
			}
			set
			{
				if ( m_Held != value )
				{
					m_Held = value;
					UpdateWeight();
					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public PotionEffect Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public PotionKeg() : base( 0x1940 )
		{
			UpdateWeight();
		}

		public virtual void UpdateWeight()
		{
			int held = Math.Max( 0, Math.Min( m_Held, 100 ) );

			this.Weight = 20 + ((held * 80) / 100);
		}

		public PotionKeg( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Type );
			writer.Write( (int) m_Held );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				case 0:
				{
					m_Type = (PotionEffect)reader.ReadInt();
					m_Held = reader.ReadInt();

					break;
				}
			}

			if ( version < 1 )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( UpdateWeight ) );
		}

		#region FS:ATS Edits
		public override void AddNameProperty(ObjectPropertyList list)
		{
			if ( m_Held > 0 )
			{
				if ( m_Type == PotionEffect.PetResurrect )
				{
					list.Add( "a keg of pet resurrection potions" );
				}
				else if ( m_Type == PotionEffect.PetShrink )
				{
					list.Add( "a keg of shrink potions" );
				}
				else if ( m_Type == PotionEffect.PetHeal )
				{
					list.Add( "a keg of pet heal potions" );
				}
				else if ( m_Type == PotionEffect.PetGreaterHeal )
				{
					list.Add( "a keg of pet greater heal potions" );
				}
				else if ( m_Type == PotionEffect.PetCure )
				{
					list.Add( "a keg of pet cure potions" );
				}
				else if ( m_Type == PotionEffect.PetGreaterCure )
				{
					list.Add( "a keg of pet greater cure potions" );
				}
				else
				{
					list.Add( 1041620 + (int)m_Type );
				}
			}
			else
			{
				list.Add( "an empty potion keg" );
			}
		}

		//public override int LabelNumber{ get{ return (m_Held > 0 ? 1041620 + (int)m_Type : 1041641); } }
		#endregion

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			int number;

			if ( m_Held <= 0 )
				number = 502246; // The keg is empty.
			else if ( m_Held < 5 )
				number = 502248; // The keg is nearly empty.
			else if ( m_Held < 20 )
				number = 502249; // The keg is not very full.
			else if ( m_Held < 30 )
				number = 502250; // The keg is about one quarter full.
			else if ( m_Held < 40 )
				number = 502251; // The keg is about one third full.
			else if ( m_Held < 47 )
				number = 502252; // The keg is almost half full.
			else if ( m_Held < 54 )
				number = 502254; // The keg is approximately half full.
			else if ( m_Held < 70 )
				number = 502253; // The keg is more than half full.
			else if ( m_Held < 80 )
				number = 502255; // The keg is about three quarters full.
			else if ( m_Held < 96 )
				number = 502256; // The keg is very full.
			else if ( m_Held < 100 )
				number = 502257; // The liquid is almost to the top of the keg.
			else
				number = 502258; // The keg is completely full.

			list.Add( number );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			int number;

			if ( m_Held <= 0 )
				number = 502246; // The keg is empty.
			else if ( m_Held < 5 )
				number = 502248; // The keg is nearly empty.
			else if ( m_Held < 20 )
				number = 502249; // The keg is not very full.
			else if ( m_Held < 30 )
				number = 502250; // The keg is about one quarter full.
			else if ( m_Held < 40 )
				number = 502251; // The keg is about one third full.
			else if ( m_Held < 47 )
				number = 502252; // The keg is almost half full.
			else if ( m_Held < 54 )
				number = 502254; // The keg is approximately half full.
			else if ( m_Held < 70 )
				number = 502253; // The keg is more than half full.
			else if ( m_Held < 80 )
				number = 502255; // The keg is about three quarters full.
			else if ( m_Held < 96 )
				number = 502256; // The keg is very full.
			else if ( m_Held < 100 )
				number = 502257; // The liquid is almost to the top of the keg.
			else
				number = 502258; // The keg is completely full.

			this.LabelTo( from, number );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), 2 ) )
			{
				if ( m_Held > 0 )
				{
					Container pack = from.Backpack;

					if ( pack != null && pack.ConsumeTotal( typeof( Bottle ), 1 ) )
					{
						from.SendLocalizedMessage( 502242 ); // You pour some of the keg's contents into an empty bottle...

						BasePotion pot = FillBottle();

						if ( pack.TryDropItem( from, pot, false ) )
						{
							from.SendLocalizedMessage( 502243 ); // ...and place it into your backpack.
							from.PlaySound( 0x240 );

							if ( --Held == 0 )
								from.SendLocalizedMessage( 502245 ); // The keg is now empty.
						}
						else
						{
							from.SendLocalizedMessage( 502244 ); // ...but there is no room for the bottle in your backpack.
							pot.Delete();
						}
					}
					else
					{
						// TODO: Target a bottle
					}
				}
				else
				{
					from.SendLocalizedMessage( 502246 ); // The keg is empty.
				}
			}
			else
			{
				from.LocalOverheadMessage( Network.MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			if ( item is BasePotion )
			{
				BasePotion pot = (BasePotion)item;

				if ( m_Held == 0 )
				{
					if ( GiveBottle( from ) )
					{
						m_Type = pot.PotionEffect;
						Held = 1;

						from.PlaySound( 0x240 );

						from.SendLocalizedMessage( 502237 ); // You place the empty bottle in your backpack.

						item.Delete();
						return true;
					}
					else
					{
						from.SendLocalizedMessage( 502238 ); // You don't have room for the empty bottle in your backpack.
						return false;
					}
				}
				else if ( pot.PotionEffect != m_Type )
				{
					from.SendLocalizedMessage( 502236 ); // You decide that it would be a bad idea to mix different types of potions.
					return false;
				}
				else if ( m_Held >= 100 )
				{
					from.SendLocalizedMessage( 502233 ); // The keg will not hold any more!
					return false;
				}
				else
				{
					if ( GiveBottle( from ) )
					{
						++Held;
						item.Delete();

						from.PlaySound( 0x240 );

						from.SendLocalizedMessage( 502237 ); // You place the empty bottle in your backpack.

						item.Delete();
						return true;
					}
					else
					{
						from.SendLocalizedMessage( 502238 ); // You don't have room for the empty bottle in your backpack.
						return false;
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 502232 ); // The keg is not designed to hold that type of object.
				return false;
			}
		}

		public bool GiveBottle( Mobile m )
		{
			Container pack = m.Backpack;

			Bottle bottle = new Bottle();

			if ( pack == null || !pack.TryDropItem( m, bottle, false ) )
			{
				bottle.Delete();
				return false;
			}

			return true;
		}

		public BasePotion FillBottle()
		{
			switch ( m_Type )
			{
				default:
				case PotionEffect.Nightsight:		return new NightSightPotion();

				case PotionEffect.CureLesser:		return new LesserCurePotion();
				case PotionEffect.Cure:				return new CurePotion();
				case PotionEffect.CureGreater:		return new GreaterCurePotion();

				case PotionEffect.Agility:			return new AgilityPotion();
				case PotionEffect.AgilityGreater:	return new GreaterAgilityPotion();

				case PotionEffect.Strength:			return new StrengthPotion();
				case PotionEffect.StrengthGreater:	return new GreaterStrengthPotion();

				case PotionEffect.PoisonLesser:		return new LesserPoisonPotion();
				case PotionEffect.Poison:			return new PoisonPotion();
				case PotionEffect.PoisonGreater:	return new GreaterPoisonPotion();
				case PotionEffect.PoisonDeadly:		return new DeadlyPoisonPotion();

				case PotionEffect.Refresh:			return new RefreshPotion();
				case PotionEffect.RefreshTotal:		return new TotalRefreshPotion();

				case PotionEffect.HealLesser:		return new LesserHealPotion();
				case PotionEffect.Heal:				return new HealPotion();
				case PotionEffect.HealGreater:		return new GreaterHealPotion();

				case PotionEffect.ExplosionLesser:	return new LesserExplosionPotion();
				case PotionEffect.Explosion:		return new ExplosionPotion();
				case PotionEffect.ExplosionGreater:	return new GreaterExplosionPotion();

				case PotionEffect.PetResurrect:		return new PetResurrectPotion();
				case PotionEffect.PetShrink:		return new PetShrinkPotion();
				case PotionEffect.PetHeal:		return new HealPotionPet();
				case PotionEffect.PetGreaterHeal:	return new GreaterHealPotionPet();
				case PotionEffect.PetCure:		return new CurePotionPet();
				case PotionEffect.PetGreaterCure:	return new GreaterCurePotionPet();
			}
		}

		public static void Initialize()
		{
			TileData.ItemTable[0x1940].Height = 4;
		}
	}
}
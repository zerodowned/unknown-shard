//Please Leave Credit XSlayerX aka XLacaestX
using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{	
	public class DeathCoinCheckBook : Item 
	{
		private int m_Token;
		private Mobile m_Owner;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int Token { get { return m_Token; } set { m_Token = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}
		
		[Constructable]
		public DeathCoinCheckBook() : base( 0xff0 )
		{
			Movable = true;
			Weight = 0;
			Name = "Death Coin Check book";
			LootType = LootType.Blessed;
			Hue = 1175;			
		}
		
		[Constructable]
		public DeathCoinCheckBook( Mobile m ) : base( 0xff0 )
		{
			Movable = true;
			Weight = 0;
			Name = "Death Coin Check book";
			LootType = LootType.Blessed;
			m_Owner = m;			
		}

		public override bool DisplayLootType{ get{ return false; } }		

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Owner == null )
			{
				Mobile mobile = (Mobile)from;
				PlayerMobile pm = (PlayerMobile)from;

				Owner = pm;
			}
			if ( from != m_Owner )
			{
				from.SendMessage( "This is not your check book, return it to it's owner." ); 
			}
			else if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from is PlayerMobile )
			{
				from.SendGump( new DeathCoinCheckBookGump( (PlayerMobile)from, this ) );
			}
		}

		public void BeginCombine( Mobile from )
		{
			from.Target = new DeathCoinCheckBookTarget( this );
		}

		public void EndCombine( Mobile from, object o )
		{
			if ( o is Item && ((Item)o).IsChildOf( from.Backpack ) )
			{
				if (!( o is Deathcoin ) && !( o is DeathCoinBankCheck ) )
				{
					from.SendMessage( "That is not an item you can put in here." );
				}
				if ( o is Deathcoin )
				{
                    //nessary?
					if ( Token >= 200000000 )
					from.SendMessage( "This check book is too full." );
					else
					{
						Item curItem = o as Item;
						Token += curItem.Amount;
						curItem.Delete();
						from.SendGump( new DeathCoinCheckBookGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}

				if ( o is DeathCoinBankCheck )
				{

					if ( Token >= 200000000 )
					from.SendMessage( "This check book is too full." );
					else
					{
						DeathCoinBankCheck DeathCoinBankCheck = o as DeathCoinBankCheck;
						Token += DeathCoinBankCheck.Worth;
						DeathCoinBankCheck.Delete();
						from.SendGump( new DeathCoinCheckBookGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 1045158 ); // You must have the item in your backpack to target it.
			}
		}

		public DeathCoinCheckBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) m_Token);
			writer.Write( ( Mobile ) m_Owner);
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			m_Token = reader.ReadInt();
			m_Owner = reader.ReadMobile();
			
		}
//
//		public override bool OnDragDrop( Mobile from, Item dropped )
//		{
//			PlayerMobile pm = (PlayerMobile)from;
//
//			if ( !base.OnDragDrop( from, dropped ) )
//				return false;
//
//			if( dropped is Tokens || dropped is TokenCheck )
//			{
//				if( dropped is Tokens )
//				{
//					m_Token += dropped.Amount;
//					from.SendMessage("You have deposited " + dropped.Amount.ToString() + " tokens.");
//				}
//
//				if( dropped is TokenCheck )
//				{
//					TokenCheck check = (TokenCheck)dropped;
//					m_Token += check.Worth;
//					from.SendMessage("You have deposited a check worth " + check.Worth.ToString() + " tokens.");
//				}
//			
//				dropped.Delete();
//				return true;
//			}
//	
//			else
//			{
//				from.SendMessage("You may only deposit tokens or a token check into a check book.");
//				return false;
//			}
//
//
//		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			
			list.Add( 1060658, "Death Coin Check book balance\t{0}", m_Token );
			
			if( m_Owner != null )
			list.Add( 1060659, "Death Coin Check book owner\t{0}", m_Owner.Name );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
		}
	}
}

namespace Server.Gumps
{
	
	public class DeathCoinCheckBookGump : Gump
	{
	
		private PlayerMobile m_From;
		private DeathCoinCheckBook m_Book;	
      	   
		public DeathCoinCheckBookGump( PlayerMobile from, Item item ): base( 0, 0 )
		{
		
			m_From = from;			
			if (!(item is DeathCoinCheckBook))
				return;
			DeathCoinCheckBook book = item as DeathCoinCheckBook;
			m_Book = book;

			m_From.CloseGump( typeof( DeathCoinCheckBookGump ) );
			
//			this.Closable=true;
//			this.Disposable=true;
//			this.Dragable=true;
//			this.Resizable=false;
//			this.AddPage(0);
//			this.AddImageTiled(53, 94, 511, 247, 3504);
//			this.AddImage(560, 338, 3508);
//			this.AddImage(31, 73, 3500);
//			this.AddImage(31, 338, 3506);
//			this.AddImageTiled(56, 73, 507, 24, 3501);
//			this.AddImageTiled(57, 338, 504, 24, 3507);
//			this.AddImageTiled(31, 98, 25, 245, 3503);
//			this.AddImageTiled(560, 95, 25, 245, 3505);
//			this.AddLabel(259, 89, 0, @"Bank of HyperCube");
//			this.AddImage(537, 176, 3502);
//			this.AddImage(537, 218, 3508);
//			this.AddImage(381, 218, 3506);
//			this.AddImage(381, 176, 3500);
//			this.AddImageTiled(406, 176, 135, 24, 3501);
//			this.AddImageTiled(406, 218, 132, 24, 3507);
//			this.AddImageTiled(381, 200, 25, 20, 3503);
//			this.AddImageTiled(537, 201, 25, 17, 3505);
//			this.AddImage(560, 73, 3502);
//			this.AddImageTiled(65, 234, 302, 5, 3007);
//			this.AddImageTiled(59, 283, 203, 5, 3007);
//			this.AddImageTiled(320, 283, 230, 5, 3007);
//			this.AddLabel(64, 265, 0, @"Note:");
//			this.AddLabel(325, 262, 0, @"Signature:");
//			this.AddImageTiled(65, 184, 302, 5, 3007);
//			this.AddLabel(510, 89, 0, @"# 001");
//			this.AddLabel(70, 164, 0, @"Pay to:");
//			this.AddLabel(318, 215, 0, @"Tokens");
//			this.AddLabel(59, 321, 0, @":1943859762: 113 || 089193 || 0009");
//			this.AddLabel(398, 262, 0, from.Name.ToString());
//			this.AddLabel(462, 310, 0, @"Make a deposit");
//			this.AddLabel(462, 333, 0, @"Write check");
//			this.AddLabel(462, 288, 0, @"Make a withdraw");
//			this.AddButton(445, 295, 2103, 2104, 2, GumpButtonType.Reply, 0); // Deposit
//			this.AddButton(445, 315, 2103, 2104, 1, GumpButtonType.Reply, 0); // withdraw
//			this.AddButton(445, 336, 2103, 2104, 3, GumpButtonType.Reply, 0); // Check
//			this.AddLabel(399, 200, 0, @"Balance:");
//			this.AddLabel(450, 200, 0, book.Token.ToString());
//			AddTextEntry(68, 216, 242, 20, 0, 1, "0");
			//*************************************************************************
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImageTiled(0, 0, 26, 324, 10464);
			this.AddImageTiled(27, 12, 200, 300, 9394);
			this.AddImageTiled(228, 0, 26, 324, 10464);
			this.AddLabel(114, 164, 0, @"Deposit");
			this.AddLabel(114, 200, 0, @"Withdraw");
			this.AddLabel(114, 235, 0, @"Write Check");
			this.AddButton(73, 164, 5224, 5231, 1, GumpButtonType.Reply, 0);
			this.AddButton(73, 199, 5224, 5231, 2, GumpButtonType.Reply, 0);
            this.AddLabel(259, 89, 0, @"Bank of MoonScape");
            this.AddButton(73, 235, 5224, 5231, 3, GumpButtonType.Reply, 0);
			this.AddLabel(73, 275, 0, @"____________");
			this.AddTextEntry(73, 272, 150, 20, 0, 1, "0");
			this.AddLabel(120, 21, 0, @"World Bank");
			this.AddLabel(120, 88, 0, book.Token.ToString());
			this.AddLabel(120, 63, 0, @"Balance :");
			this.AddImageTiled(0, 0, 96, 142, 5536);

			//*************************************************************************
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Book.Deleted )
			return;
			
			if ( info.ButtonID == 1)
			{
				m_From.SendGump( new DeathCoinCheckBookGump( m_From, m_Book ) );
				m_Book.BeginCombine( m_From );
			}
			
			if ( info.ButtonID == 2 )
			{
				TextRelay tr_TokenAmount = info.GetTextEntry( 1 );
				if(tr_TokenAmount != null)
				{
					int i_MaxAmount = 0;
					try
					{
						i_MaxAmount = Convert.ToInt32(tr_TokenAmount.Text,10);
					} 
					catch
					{
						m_From.SendMessage(1161, "Please make sure you write only numbers.");
					}
					if(i_MaxAmount > 0) 
					{
						if (i_MaxAmount <= ((DeathCoinCheckBook)m_Book ).Token)
						{				
							if (i_MaxAmount <= 60000 )
							{

								m_From.AddToBackpack(new Deathcoin(i_MaxAmount));
								m_From.SendMessage(1161, "You extracted {0} Death Coins from your check book.", i_MaxAmount);
								((DeathCoinCheckBook)m_Book ).Token = (((DeathCoinCheckBook)m_Book ).Token - i_MaxAmount);

							}
							else
								m_From.SendMessage(1161, "You can't extract more then 60,000 Death Coins at one time.");
						}
						else
							m_From.SendMessage(1173, "You don't have that many Death Coins in your book.");
					}
					m_From.SendGump( new DeathCoinCheckBookGump( m_From, m_Book ) );
				}
			}

			if ( info.ButtonID == 3 )
			{
				TextRelay tr_TokenAmount = info.GetTextEntry( 1 );
				if(tr_TokenAmount != null)
				{
					int i_MaxAmount = 0;
					try
					{
						i_MaxAmount = Convert.ToInt32(tr_TokenAmount.Text,10);
					} 
					catch
					{
						m_From.SendMessage(1161, "Please make sure you write only numbers.");
					}
					if(i_MaxAmount > 0) 
					{
						if (i_MaxAmount <= ((DeathCoinCheckBook)m_Book ).Token)
						{				
							if (i_MaxAmount <= 1000000)
							{
								m_From.AddToBackpack(new DeathCoinBankCheck(i_MaxAmount));
								m_From.SendMessage(1161, "You extracted {0} Death Coins from your check book.", i_MaxAmount);
								((DeathCoinCheckBook)m_Book ).Token = (((DeathCoinCheckBook)m_Book ).Token - i_MaxAmount);
							}
							else
								m_From.SendMessage(1161, "You cannnot write checks for more then 1,000,000 Death Coins at one time.");
						}
						else
							m_From.SendMessage(1173, "You don't have that much Death Coins in your check book.");
					}
					m_From.SendGump( new DeathCoinCheckBookGump( m_From, m_Book ) );
				}
			}
		}
	}
}

namespace Server.Items
{
	public class DeathCoinCheckBookTarget : Target
	{
		private DeathCoinCheckBook m_Book;

		public DeathCoinCheckBookTarget( DeathCoinCheckBook book ) : base( 18, false, TargetFlags.None )
		{
			m_Book = book;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_Book.Deleted )
			return;

			m_Book.EndCombine( from, targeted );
		}
	}
}

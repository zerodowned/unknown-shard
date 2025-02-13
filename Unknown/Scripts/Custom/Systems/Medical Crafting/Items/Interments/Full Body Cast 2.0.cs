using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;
using System.Collections.Generic;

namespace Server.Items
{
	public class FullBodyCast : Item, IDyable
	{
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public FullBodyCast() : this( 1 )
		{
		}

		[Constructable]
		public FullBodyCast( int amount ) : base( 0xEC4 )
		{
			Name = "a full body cast";
			Stackable = true;
			Amount = amount;
		}

		public FullBodyCast( Serial serial ) : base( serial )
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), Core.AOS ? 2 : 1 ) )
			{
				from.RevealingAction();

				from.SendLocalizedMessage( 500948 ); // Who will you use the FullBodyCasts on?

				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
			}
		}
		private class InternalTarget : Target
		{
			private FullBodyCast m_FullBodyCast;

			public InternalTarget( FullBodyCast FullBodyCast ) : base( 1, false, TargetFlags.Beneficial )
			{
				m_FullBodyCast = FullBodyCast;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_FullBodyCast.Deleted )
					return;

				if ( targeted is Mobile )
				{
					if ( from.InRange( m_FullBodyCast.GetWorldLocation(), Core.AOS ? 2 : 1 ) )
					{
						if ( FullBodyCastContext.BeginHeal( from, (Mobile) targeted ) != null )
						{
							m_FullBodyCast.Consume();
						}
					}
					else
					{
						from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
					}
				}
				else
				{
					from.SendLocalizedMessage( 500970 ); // FullBodyCasts can not be used on that.
				}
			}
		}
	}

	public class FullBodyCastContext
	{
		private Mobile m_Healer;
		private Mobile m_Patient;
		private int m_Slips;
		private Timer m_Timer;

		public Mobile Healer{ get{ return m_Healer; } }
		public Mobile Patient{ get{ return m_Patient; } }
		public int Slips{ get{ return m_Slips; } set{ m_Slips = value; } }
		public Timer Timer{ get{ return m_Timer; } }

		public void Slip()
		{
			m_Healer.SendLocalizedMessage( 500961 ); // Your fingers slip!
			++m_Slips;
		}

		public FullBodyCastContext( Mobile healer, Mobile patient, TimeSpan delay )
		{
			m_Healer = healer;
			m_Patient = patient;

			m_Timer = new InternalTimer( this, delay );
			m_Timer.Start();
		}

		public void StopHeal()
		{
			m_Table.Remove( m_Healer );

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;
		}

		private static Hashtable m_Table = new Hashtable();

		public static FullBodyCastContext GetContext( Mobile healer )
		{
			return (FullBodyCastContext)m_Table[healer];
		}

		public static SkillName GetPrimarySkill( Mobile m )
		{
			if ( !m.Player && (m.Body.IsMonster || m.Body.IsAnimal) )
				return SkillName.Veterinary;
			else
				return SkillName.Healing;
		}

		public static SkillName GetSecondarySkill( Mobile m )
		{
			if ( !m.Player && (m.Body.IsMonster || m.Body.IsAnimal) )
				return SkillName.AnimalLore;
			else
				return SkillName.Anatomy;
		}

		public void EndHeal()
		{
			StopHeal();

			int healerNumber = -1, patientNumber = -1;
			bool playSound = true;
			bool checkSkills = false;

			SkillName primarySkill = GetPrimarySkill( m_Patient );
			SkillName secondarySkill = GetSecondarySkill( m_Patient );

			BaseCreature petPatient = m_Patient as BaseCreature;

			if ( !m_Healer.Alive )
			{
				healerNumber = 500962; // You were unable to finish your work before you died.
				patientNumber = -1;
				playSound = false;
			}
			else if ( !m_Healer.InRange( m_Patient, Core.AOS ? 2 : 1 ) )
			{
				healerNumber = 500963; // You did not stay close enough to heal your target.
				patientNumber = -1;
				playSound = false;
			}
			else if ( !m_Patient.Alive || (petPatient != null && petPatient.IsDeadPet) )
			{
				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ((healing - 68.0) / 50.0) - (m_Slips * 0.02);

				if (( (checkSkills = (healing >= 80.0 && anatomy >= 80.0)) && chance > Utility.RandomDouble() )
				      || ( Core.SE && petPatient is Factions.FactionWarHorse && petPatient.ControlMaster == m_Healer) )	//TODO: Dbl check doesn't check for faction of the horse here?
				{
					if ( m_Patient.Map == null || !m_Patient.Map.CanFit( m_Patient.Location, 16, false, false ) )
					{
						healerNumber = 501042; // Target can not be resurrected at that location.
						patientNumber = 502391; // Thou can not be resurrected there!
					}
					else if ( m_Patient.Region != null && m_Patient.Region.IsPartOf( "Khaldun" ) )
					{
						healerNumber = 1010395; // The veil of death in this area is too strong and resists thy efforts to restore life.
						patientNumber = -1;
					}
					else
					{
						healerNumber = 500965; // You are able to resurrect your patient.
						patientNumber = -1;

						m_Patient.PlaySound( 0x214 );
						m_Patient.FixedEffect( 0x376A, 10, 16 );

						if ( petPatient != null && petPatient.IsDeadPet )
						{
							Mobile master = petPatient.ControlMaster;

							if ( master != null && master.InRange( petPatient, 3 ) )
							{
								healerNumber = 503255; // You are able to resurrect the creature.

								master.CloseGump( typeof( PetResurrectGump ) );
								master.SendGump( new PetResurrectGump( m_Healer, petPatient ) );
							}
							else
							{
								bool found = false;

								List<Mobile> friends = petPatient.Friends;

								for ( int i = 0; friends != null && i < friends.Count; ++i )
								{
									Mobile friend = friends[i];

									if ( friend.InRange( petPatient, 3 ) )
									{
										healerNumber = 503255; // You are able to resurrect the creature.

										friend.CloseGump( typeof( PetResurrectGump ) );
										friend.SendGump( new PetResurrectGump( m_Healer, petPatient ) );

										found = true;
										break;
									}
								}

								if ( !found )
									healerNumber = 1049670; // The pet's owner must be nearby to attempt resurrection.
							}
						}
						else
						{
							m_Patient.CloseGump( typeof( ResurrectGump ) );
							m_Patient.SendGump( new ResurrectGump( m_Patient, m_Healer ) );
						}
					}
				}
				else
				{
					if ( petPatient != null && petPatient.IsDeadPet )
						healerNumber = 503256; // You fail to resurrect the creature.
					else
						healerNumber = 500966; // You are unable to resurrect your patient.

					patientNumber = -1;
				}
			}
			else if ( m_Patient.Poisoned )
			{
				m_Healer.SendLocalizedMessage( 500969 ); // You finish applying the FullBodyCasts.

				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ((healing - 30.0) / 50.0) - (m_Patient.Poison.Level * 0.1) - (m_Slips * 0.02);

				if ( (checkSkills = (healing >= 60.0 && anatomy >= 60.0)) && chance > Utility.RandomDouble() )
				{
					if ( m_Patient.CurePoison( m_Healer ) )
					{
						healerNumber = (m_Healer == m_Patient) ? -1 : 1010058; // You have cured the target of all poisons.
						patientNumber = 1010059; // You have been cured of all poisons.
					}
					else
					{
						healerNumber = -1;
						patientNumber = -1;
					}
				}
				else
				{
					healerNumber = 1010060; // You have failed to cure your target!
					patientNumber = -1;
				}
			}
			else if ( BleedAttack.IsBleeding( m_Patient ) )
			{
				healerNumber = -1;
				patientNumber = 1060167; // The bleeding wounds have healed, you are no longer bleeding!

				BleedAttack.EndBleed( m_Patient, true );
			}
			else if ( MortalStrike.IsWounded( m_Patient ) )
			{
				healerNumber = ( m_Healer == m_Patient ? 1005000 : 1010398 );
				patientNumber = -1;
				playSound = false;
			}
			else if ( m_Patient.Hits == m_Patient.HitsMax )
			{
				healerNumber = 500967; // You heal what little damage your patient had.
				patientNumber = -1;
			}
			else
			{
				checkSkills = true;
				patientNumber = -1;

				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ((healing + 10.0) / 100.0) - (m_Slips * 0.02);

				if ( chance > Utility.RandomDouble() )
				{
					healerNumber = 500969; // You finish applying the FullBodyCasts.

					double min, max;

					if ( Core.AOS )
					{
						min = (anatomy / 2.0) + (healing / 2.0) + 2.0;
						max = (anatomy / 1.5) + (healing / 1.5) + 1.0;
					}
					else
					{
						min = (anatomy / 3.0) + (healing / 2.0) + 3.0;
						max = (anatomy / 2.0) + (healing / 2.0) + 10.0;
					}

					double toHeal = min + (Utility.RandomDouble() * (max - min));

					if ( m_Patient.Body.IsMonster || m_Patient.Body.IsAnimal )
						toHeal += m_Patient.HitsMax / 800;

					if ( Core.AOS )
						toHeal -= toHeal * m_Slips * 0.35; // TODO: Verify algorithm
					else
						toHeal -= m_Slips * 4;

					if ( toHeal < 1 )
					{
						toHeal = 1;
						healerNumber = 500968; // You apply the FullBodyCasts, but they barely help.
					}

					m_Patient.Heal( (int) toHeal, false );
				}
				else
				{
					healerNumber = 500968; // You apply the FullBodyCasts, but they barely help.
					playSound = false;
				}
			}

			if ( healerNumber != -1 )
				m_Healer.SendLocalizedMessage( healerNumber );

			if ( patientNumber != -1 )
				m_Patient.SendLocalizedMessage( patientNumber );

			if ( playSound )
				m_Patient.PlaySound( 0x57 );

			if ( checkSkills )
			{
				m_Healer.CheckSkill( secondarySkill, 0.0, 120.0 );
				m_Healer.CheckSkill( primarySkill, 0.0, 120.0 );
			}
		}

		private class InternalTimer : Timer
		{
			private FullBodyCastContext m_Context;

			public InternalTimer( FullBodyCastContext context, TimeSpan delay ) : base( delay )
			{
				m_Context = context;
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				m_Context.EndHeal();
			}
		}

		public static FullBodyCastContext BeginHeal( Mobile healer, Mobile patient )
		{
			bool isDeadPet = ( patient is BaseCreature && ((BaseCreature)patient).IsDeadPet );

			if ( patient is Golem )
			{
				healer.SendLocalizedMessage( 500970 ); // FullBodyCasts cannot be used on that.
			}
			else if ( patient is BaseCreature && ((BaseCreature)patient).IsAnimatedDead )
			{
				healer.SendLocalizedMessage( 500951 ); // You cannot heal that.
			}
			else if ( !patient.Poisoned && patient.Hits == patient.HitsMax && !BleedAttack.IsBleeding( patient ) && !isDeadPet )
			{
				healer.SendLocalizedMessage( 500955 ); // That being is not damaged!
			}
			else if ( !patient.Alive && (patient.Map == null || !patient.Map.CanFit( patient.Location, 16, false, false )) )
			{
				healer.SendLocalizedMessage( 501042 ); // Target cannot be resurrected at that location.
			}
			else if ( healer.CanBeBeneficial( patient, true, true ) )
			{
				healer.DoBeneficial( patient );

				bool onSelf = ( healer == patient );
				int dex = healer.Dex;

				double seconds;
				double resDelay = ( patient.Alive ? 0.0 : 5.0 );

				if ( onSelf )
				{
					if ( Core.AOS )
						seconds = 5.0 + (0.5 * ((double)(120 - dex) / 10)); // TODO: Verify algorithm
					else
						seconds = 9.4 + (0.6 * ((double)(120 - dex) / 10));
				}
				else
				{
					if ( Core.AOS && GetPrimarySkill( patient ) == SkillName.Veterinary )
					{
						//if ( dex >= 40 )
							seconds = 2.0;
						//else
						//	seconds = 3.0;
					}
					else
					{
						if ( dex >= 100 )
							seconds = 3.0 + resDelay;
						else if ( dex >= 40 )
							seconds = 4.0 + resDelay;
						else
							seconds = 5.0 + resDelay;
					}
				}

				FullBodyCastContext context = GetContext( healer );

				if ( context != null )
					context.StopHeal();

				context = new FullBodyCastContext( healer, patient, TimeSpan.FromSeconds( seconds ) );

				m_Table[healer] = context;

				if ( !onSelf )
					patient.SendLocalizedMessage( 1008078, false, healer.Name ); //  : Attempting to heal you.

				healer.SendLocalizedMessage( 500956 ); // You begin applying the FullBodyCasts.
				return context;
			}

			return null;
		}
	}
}
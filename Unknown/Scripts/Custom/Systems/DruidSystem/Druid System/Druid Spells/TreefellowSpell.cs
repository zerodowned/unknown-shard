using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Druid
{
	public class TreefellowSpell : DruidicSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Treefellow", "Kes En Sepa Ohm",
				SpellCircle.Eighth,
				269,
				9020,
				false,
				Reagent.BlackPearl,
            		Reagent.Bloodmoss,
            		Reagent.PetrafiedWood
			);

		public TreefellowSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}
		public override double CastDelay{ get{ return 1.0; } }
      	public override double RequiredSkill{ get{ return 80.0; } }
      	public override int RequiredMana{ get{ return 50; } }

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( (Caster.Followers + 2) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				if ( Core.AOS )
					SpellHelper.Summon( new SummonedTreefellow(), Caster, 0x217, TimeSpan.FromSeconds( 4.0 * Caster.Skills[SkillName.AnimalLore].Value ), false, false );
				else
					SpellHelper.Summon( new Treefellow(), Caster, 0x217, TimeSpan.FromSeconds( 4.0 * Caster.Skills[SkillName.AnimalLore].Value ), false, false );
			}

			FinishSequence();
		}
	}
}

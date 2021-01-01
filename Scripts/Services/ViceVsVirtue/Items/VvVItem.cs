namespace Server.Items
{
    public interface IVvVItem
    {
        bool IsVvVItem { get; set; }
    }

    public static class VvVEquipment
    {
        public static void CheckProperties(Item item)
        {
            if (item is PrimerOnArmsTalisman && ((PrimerOnArmsTalisman)item).Attributes.AttackChance != 10)
            {
                ((PrimerOnArmsTalisman)item).Attributes.AttackChance = 10;
            }

            if (item is ClaininsSpellbook && ((ClaininsSpellbook)item).Attributes.LowerManaCost != 10)
            {
                ((ClaininsSpellbook)item).Attributes.LowerManaCost = 10;
            }

            if (item is CrimsonCincture && ((CrimsonCincture)item).Attributes.BonusDex != 10)
            {
                ((CrimsonCincture)item).Attributes.BonusDex = 10;
            }

            if (item is CrystallineRing && ((CrystallineRing)item).Attributes.CastRecovery != 3)
            {
                ((CrystallineRing)item).Attributes.CastRecovery = 3;
            }

            if (item is FoldedSteelGlasses && ((FoldedSteelGlasses)item).Attributes.DefendChance != 25)
            {
                ((FoldedSteelGlasses)item).Attributes.DefendChance = 25;
            }

            if (item is HuntersHeaddress)
            {
                HuntersHeaddress hunters = (HuntersHeaddress)item;

                if (hunters.Resistances.Physical != 8)
                    hunters.Resistances.Physical = 8;

                if (hunters.Resistances.Fire != 4)
                    hunters.Resistances.Fire = 4;

                if (hunters.Resistances.Cold != -8)
                    hunters.Resistances.Cold = -8;

                if (hunters.Resistances.Poison != 9)
                    hunters.Resistances.Poison = 9;

                if (hunters.Resistances.Energy != 3)
                    hunters.Resistances.Energy = 3;
            }

            if (item is KasaOfTheRajin && ((KasaOfTheRajin)item).Attributes.DefendChance != 10)
            {
                ((KasaOfTheRajin)item).Attributes.DefendChance = 10;
            }

            if (item is MaceAndShieldGlasses && ((MaceAndShieldGlasses)item).Attributes.WeaponDamage != 10)
            {
                ((MaceAndShieldGlasses)item).Attributes.WeaponDamage = 10;
            }

            if (item is OrnamentOfTheMagician && ((OrnamentOfTheMagician)item).Attributes.RegenMana != 3)
            {
                ((OrnamentOfTheMagician)item).Attributes.RegenMana = 3;
            }

            if (item is RingOfTheVile && ((RingOfTheVile)item).Attributes.AttackChance != 25)
            {
                ((RingOfTheVile)item).Attributes.AttackChance = 25;
            }

            if (item is RuneBeetleCarapace)
            {
                RuneBeetleCarapace carapace = (RuneBeetleCarapace)item;

                if (carapace.PhysicalBonus != 3)
                    carapace.PhysicalBonus = 3;

                if (carapace.FireBonus != 3)
                    carapace.FireBonus = 3;

                if (carapace.ColdBonus != 3)
                    carapace.ColdBonus = 3;

                if (carapace.PoisonBonus != 3)
                    carapace.PoisonBonus = 3;

                if (carapace.EnergyBonus != 3)
                    carapace.EnergyBonus = 3;
            }

            if (item is SpiritOfTheTotem)
            {
                SpiritOfTheTotem totem = (SpiritOfTheTotem)item;

                if (totem.Resistances.Fire != 7)
                    totem.Resistances.Fire = 7;

                if (totem.Resistances.Cold != 2)
                    totem.Resistances.Cold = 2;

                if (totem.Resistances.Poison != 6)
                    totem.Resistances.Poison = 6;

                if (totem.Resistances.Energy != 6)
                    totem.Resistances.Energy = 6;
            }

            if (item is Stormgrip && ((Stormgrip)item).Attributes.AttackChance != 10)
            {
                ((Stormgrip)item).Attributes.AttackChance = 10;
            }

            if (item is TomeOfLostKnowledge && ((TomeOfLostKnowledge)item).Attributes.RegenMana != 3)
            {
                ((TomeOfLostKnowledge)item).Attributes.RegenMana = 3;
            }

            if (item is WizardsCrystalGlasses)
            {
                WizardsCrystalGlasses glasses = (WizardsCrystalGlasses)item;

                if (glasses.PhysicalBonus != 5)
                    glasses.PhysicalBonus = 5;

                if (glasses.FireBonus != 5)
                    glasses.FireBonus = 5;

                if (glasses.ColdBonus != 5)
                    glasses.ColdBonus = 5;

                if (glasses.PoisonBonus != 5)
                    glasses.PoisonBonus = 5;

                if (glasses.EnergyBonus != 5)
                    glasses.EnergyBonus = 5;
            }
        }
    }
}

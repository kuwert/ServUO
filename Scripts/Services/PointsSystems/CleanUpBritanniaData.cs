using Server.Accounting;
using Server.Engines.Craft;
using Server.Engines.Quests.Doom;
using Server.Items;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Engines.Points
{
    public class CleanUpBritanniaData : PointsSystem
    {
        public override PointsType Loyalty => PointsType.CleanUpBritannia;
        public override TextDefinition Name => m_Name;
        public override bool AutoAdd => true;
        public override double MaxPoints => double.MaxValue;
        public override bool ShowOnLoyaltyGump => false;

        private readonly TextDefinition m_Name = null;

        public static bool Enabled { get; set; }

        public CleanUpBritanniaData()
        {
            Enabled = true;

            if (Enabled)
            {
                InitializeEntries();
                PointsExchange = new Dictionary<string, double>();
            }
        }

        public static double GetPoints(Item item)
        {
            if (item is IVvVItem && ((IVvVItem)item).IsVvVItem)
                return 0;

            double points = 0;

            Type type = item.GetType();

            if (Entries.ContainsKey(type))
            {
                points = Entries[type];

                // Kind of ametuar, but if this arrizes more, we'll make a seperate function
                if (item is SOS && ((SOS)item).IsAncient)
                    points = 2500;

                if (item.Stackable)
                    points = points * item.Amount;

                return points;
            }
            else
            {
                if (item is RunicHammer)
                {
                    RunicHammer hammer = (RunicHammer)item;

                    if (hammer.Resource == CraftResource.DullCopper)
                        points = 5 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.ShadowIron)
                        points = 10 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.Copper)
                        points = 25 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.Bronze)
                        points = 100 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.Gold)
                        points = 250 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.Agapite)
                        points = 1000 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.Verite)
                        points = 4000 * hammer.UsesRemaining;
                    else if (hammer.Resource == CraftResource.Valorite)
                        points = 8000 * hammer.UsesRemaining;
                }
                else if (item is RunicSewingKit)
                {
                    RunicSewingKit sewing = (RunicSewingKit)item;

                    if (sewing.Resource == CraftResource.SpinedLeather)
                        points = 10 * sewing.UsesRemaining;
                    else if (sewing.Resource == CraftResource.HornedLeather)
                        points = 100 * sewing.UsesRemaining;
                    else if (sewing.Resource == CraftResource.BarbedLeather)
                        points = 400 * sewing.UsesRemaining;
                }
                else if (item is PowerScroll)
                {
                    PowerScroll ps = (PowerScroll)item;

                    if (ps.Value == 105)
                        points = 50;
                    else if (ps.Value == 110)
                        points = 100;
                    else if (ps.Value == 115)
                        points = 500;
                    else if (ps.Value == 120)
                        points = 2500;
                }
                else if (item is ScrollOfTranscendence)
                {
                    SpecialScroll sot = (SpecialScroll)item;

                    points = sot.Value / 0.1 * 2;
                }
                else if (item is Bait)
                {
                    Bait bait = (Bait)item;

                    points = 10 * bait.UsesRemaining;
                }
                else if (item is TreasureMap)
                {
                    TreasureMap tmap = (TreasureMap)item;

                    switch (tmap.Level)
                    {
                        default:
                        case 0: return 50;
                        case 1: return 100;
                        case 2: return 250;
                        case 3: return 750;
                        case 4: return 1000;
                    }
                }
                else if (item is MonsterStatuette)
                {
                    MonsterStatuette ms = (MonsterStatuette)item;

                    if (ms.Type == MonsterStatuetteType.Slime)
                        points = 5000;
                }
                else if (item is PigmentsOfTokuno || item is LesserPigmentsOfTokuno)
                {
                    BasePigmentsOfTokuno pigments = (BasePigmentsOfTokuno)item;
                    points = 500 * pigments.UsesRemaining;
                }
                else if (item is ICombatEquipment)
                {
                    points = GetPointsForEquipment(item);
                }

                if (item.LootType != LootType.Blessed && points < 100 && item is IShipwreckedItem && ((IShipwreckedItem)item).IsShipwreckedItem)
                {
                    points = 100;
                }

                return points;
            }
        }

        public override void SendMessage(PlayerMobile from, double old, double points, bool quest)
        {
            from.SendLocalizedMessage(1151281, CleanUpBritannia.GetPoints(from).ToString()); // Your Clean Up Britannia point total is now ~1_VALUE~!
        }

        public static Dictionary<Type, double> Entries;

        public void InitializeEntries()
        {
            Entries = new Dictionary<Type, double>();
        }

        public static int GetPointsForEquipment(Item item)
        {
            foreach (CraftSystem system in CraftSystem.Systems)
            {
                CraftItem crItem = null;

                if (system != null && system.CraftItems != null)
                {
                    Type type = item.GetType();

                    if (type == typeof(SilverRing))
                    {
                        type = typeof(GoldRing);
                    }
                    else if (type == typeof(SilverBracelet))
                    {
                        type = typeof(GoldBracelet);
                    }

                    crItem = system.CraftItems.SearchFor(type);

                    if (crItem != null && crItem.Resources != null)
                    {
                        CraftRes craftRes = crItem.Resources.GetAt(0);
                        double amount = 1;

                        if (craftRes != null)
                        {
                            amount = craftRes.Amount;
                        }

                        double award = 1;

                        if (item is IResource)
                        {
                            switch (((IResource)item).Resource)
                            {
                                default: award = amount * .1; break;
                                case CraftResource.DullCopper: award = amount * .47; break;
                                case CraftResource.ShadowIron: award = amount * .73; break;
                                case CraftResource.Copper: award = amount * 1.0; break;
                                case CraftResource.Bronze: award = amount * 1.47; break;
                                case CraftResource.Gold: award = amount * 2.5; break;
                                case CraftResource.Agapite: award = amount * 5.0; break;
                                case CraftResource.Verite: award = amount * 8.5; break;
                                case CraftResource.Valorite: award = amount * 10; break;
                                case CraftResource.SpinedLeather: award = amount * 0.5; break;
                                case CraftResource.HornedLeather: award = amount * 1.0; break;
                                case CraftResource.BarbedLeather: award = amount * 2.0; break;
                                case CraftResource.OakWood: award = amount * .17; break;
                                case CraftResource.AshWood: award = amount * .33; break;
                                case CraftResource.YewWood: award = amount * .67; break;
                                case CraftResource.Heartwood: award = amount * 1.0; break;
                                case CraftResource.Bloodwood: award = amount * 2.17; break;
                                case CraftResource.Frostwood: award = amount * 3.17; break;
                            }
                        }

                        int weight = item is BaseWeapon && !((BaseWeapon)item).DImodded ? Imbuing.GetTotalWeight(item, 12, false, true) : Imbuing.GetTotalWeight(item, -1, false, true);

                        if (weight > 0)
                        {
                            award += weight / 30;
                        }

                        return (int)award;
                    }
                }
            }

            return 0;
        }

        #region Points Exchange

        public Dictionary<string, double> PointsExchange { get; private set; }

        public double GetPointsFromExchange(Mobile m)
        {
            Account a = m.Account as Account;

            if (a != null && !PointsExchange.ContainsKey(a.Username))
            {
                PointsExchange[a.Username] = 0.0;
            }

            return a == null ? 0.0 : PointsExchange[a.Username];
        }

        public bool AddPointsToExchange(Mobile m)
        {
            Account a = m.Account as Account;

            if (a == null)
            {
                return false;
            }

            double points = GetPoints(m);

            if (points <= 0)
            {
                m.SendLocalizedMessage(1158451); // This account has no points to deposit.
            }
            else if (DeductPoints(m, points))
            {
                if (!PointsExchange.ContainsKey(a.Username))
                {
                    PointsExchange[a.Username] = points;
                }
                else
                {
                    PointsExchange[a.Username] += points;
                }

                m.SendLocalizedMessage(1158452, points.ToString("N0")); // You have deposited ~1_VALUE~ Cleanup Britannia Points.
                return true;
            }

            return false;
        }

        public bool RemovePointsFromExchange(Mobile m)
        {
            Account a = m.Account as Account;

            if (a == null)
            {
                return false;
            }

            double points = GetPointsFromExchange(m);

            if (points <= 0)
            {
                m.SendLocalizedMessage(1158457); // This account has no points to withdraw.
            }
            else if (PointsExchange.ContainsKey(a.Username))
            {
                PointsExchange[a.Username] = 0;
                AwardPoints(m, points, false, false);

                m.SendLocalizedMessage(1158453, string.Format("{0}\t{1}", points.ToString("N0"), ((int)GetPoints(m)).ToString("N0"))); // You have withdrawn ~1_VALUE~ Cleanup Britannia Points.  You now have ~2_VALUE~ points.
                return true;
            }

            return false;
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(PointsExchange == null ? 0 : PointsExchange.Count);

            if (PointsExchange != null)
            {
                foreach (KeyValuePair<string, double> kvp in PointsExchange)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            if (Version >= 2)
            {
                int version = reader.ReadInt();

                int count = reader.ReadInt();

                for (int i = 0; i < count; i++)
                {
                    string accountName = reader.ReadString();
                    double points = reader.ReadDouble();

                    PointsExchange[accountName] = points;
                }
            }
        }
    }

    public class AppraiseforCleanupTarget : Target
    {
        private readonly Mobile m_Mobile;

        public AppraiseforCleanupTarget(Mobile from) : base(-1, true, TargetFlags.None)
        {
            m_Mobile = from;
        }

        protected override void OnTarget(Mobile m, object targeted)
        {
            if (targeted is Item)
            {
                Item item = (Item)targeted;

                if (!item.IsChildOf(m_Mobile))
                    return;

                double points = CleanUpBritanniaData.GetPoints(item);

                if (points == 0)
                    m_Mobile.SendLocalizedMessage(1151271); // This item has no turn-in value for Clean Up Britannia.
                else if (points < 1)
                    m_Mobile.SendLocalizedMessage(1151272); // This item is worth less than one point for Clean Up Britannia.
                else if (points == 1)
                    m_Mobile.SendLocalizedMessage(1151273); // This item is worth approximately one point for Clean Up Britannia.
                else
                    m_Mobile.SendLocalizedMessage(1151274, points.ToString()); //This item is worth approximately ~1_VALUE~ points for Clean Up Britannia.

                m_Mobile.Target = new AppraiseforCleanupTarget(m_Mobile);
            }
            else
            {
                m_Mobile.SendLocalizedMessage(1151271); // This item has no turn-in value for Clean Up Britannia.
                m_Mobile.Target = new AppraiseforCleanupTarget(m_Mobile);
            }
        }
    }
}

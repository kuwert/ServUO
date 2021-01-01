using Server.Commands;
using Server.Engines.Points;
using Server.Engines.VendorSearching;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Engines.UOStore
{
    public enum StoreCategory
    {
        None,
        Featured,
        Character,
        Equipment,
        Decorations,
        Mounts,
        Misc,
        Cart
    }

    public enum SortBy
    {
        Name,
        PriceLower,
        PriceHigher,
        Newest,
        Oldest
    }

    public static class UltimaStore
    {
        public static readonly string FilePath = Path.Combine("Saves/Misc", "UltimaStore.bin");

        public static bool Enabled { get { return Configuration.Enabled; } set { Configuration.Enabled = value; } }

        public static List<StoreEntry> Entries { get; private set; }
        public static Dictionary<Mobile, List<Item>> PendingItems { get; private set; }

        private static UltimaStoreContainer _UltimaStoreContainer;

        public static UltimaStoreContainer UltimaStoreContainer
        {
            get
            {
                if (_UltimaStoreContainer != null && _UltimaStoreContainer.Deleted)
                {
                    _UltimaStoreContainer = null;
                }

                return _UltimaStoreContainer ?? (_UltimaStoreContainer = new UltimaStoreContainer());
            }
        }

        static UltimaStore()
        {
            Entries = new List<StoreEntry>();
            PendingItems = new Dictionary<Mobile, List<Item>>();
            PlayerProfiles = new Dictionary<Mobile, PlayerProfile>();
        }

        public static void Configure()
        {
            PacketHandlers.Register(0xFA, 1, true, UOStoreRequest);

            CommandSystem.Register("Store", AccessLevel.Player, e => OpenStore(e.Mobile as PlayerMobile));

            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        public static void Initialize()
        {
        }

        public static void Register<T>(TextDefinition name, int tooltip, int itemID, int gumpID, int hue,
            int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null) where T : Item
        {
            Register(typeof(T), name, tooltip, itemID, gumpID, hue, cost, cat, constructor);
        }

        public static void Register(Type itemType, TextDefinition name, int tooltip, int itemID, int gumpID,
            int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null)
        {
            Register(new StoreEntry(itemType, name, tooltip, itemID, gumpID, hue, cost, cat, constructor));
        }

        public static void Register<T>(TextDefinition[] name, int tooltip, int itemID, int gumpID, int hue,
            int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null) where T : Item
        {
            Register(typeof(T), name, tooltip, itemID, gumpID, hue, cost, cat, constructor);
        }

        public static void Register(Type itemType, TextDefinition[] name, int tooltip, int itemID, int gumpID,
            int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null)
        {
            Register(new StoreEntry(itemType, name, tooltip, itemID, gumpID, hue, cost, cat, constructor));
        }

        public static StoreEntry GetEntry(Type t)
        {
            return Entries.FirstOrDefault(e => e.ItemType == t);
        }

        public static void Register(StoreEntry entry)
        {
            Entries.Add(entry);
        }

        public static bool CanSearch(Mobile m)
        {
            return m != null && m.Region.GetLogoutDelay(m) <= TimeSpan.Zero;
        }

        public static void UOStoreRequest(NetState state, PacketReader pvSrc)
        {
            OpenStore(state.Mobile as PlayerMobile);
        }

        public static void OpenStore(PlayerMobile user, StoreEntry forcedEntry = null)
        {
            if (user == null || user.NetState == null)
            {
                return;
            }

            if (!Enabled)
            {
                // The promo code redemption system is currently unavailable. Please try again later.
                user.SendLocalizedMessage(1062904);
                return;
            }

            if (Configuration.CurrencyImpl == CurrencyType.None)
            {
                // The promo code redemption system is currently unavailable. Please try again later.
                user.SendLocalizedMessage(1062904);
                return;
            }

            if (user.AccessLevel < AccessLevel.Counselor && !CanSearch(user))
            {
                // Before using the in game store, you must be in a safe log-out location
                // such as an inn or a house which has you on its Owner, Co-owner, or Friends list.
                user.SendLocalizedMessage(1156586);
                return;
            }

            if (!user.HasGump(typeof(UltimaStoreGump)))
            {
                BaseGump.SendGump(new UltimaStoreGump(user, forcedEntry));
            }
        }

        #region Constructors
        public static Item ConstructHairDye(Mobile m, StoreEntry entry)
        {
            NaturalHairDye.HairDyeInfo info = NaturalHairDye.Table.FirstOrDefault(x => x.Localization == entry.Name[1].Number);

            if (info != null)
            {
                return new NaturalHairDye(info.Type);
            }

            return null;
        }

        public static Item ConstructHaochisPigment(Mobile m, StoreEntry entry)
        {
            HaochisPigment.HoachisPigmentInfo info = HaochisPigment.Table.FirstOrDefault(x => x.Localization == entry.Name[1].Number);

            if (info != null)
            {
                return new HaochisPigment(info.Type, 50);
            }

            return null;
        }

        public static Item ConstructPigments(Mobile m, StoreEntry entry)
        {
            PigmentType type = PigmentType.None;

            for (int i = 0; i < PigmentsOfTokuno.Table.Length; i++)
            {
                if (PigmentsOfTokuno.Table[i][1] == entry.Name[1].Number)
                {
                    type = (PigmentType)i;
                    break;
                }
            }

            if (type != PigmentType.None)
            {
                return new PigmentsOfTokuno(type, 50);
            }

            return null;
        }

        public static Item ConstructEarrings(Mobile m, StoreEntry entry)
        {
            AosElementAttribute ele = AosElementAttribute.Physical;

            switch (entry.Name[0].Number)
            {
                case 1071092: ele = AosElementAttribute.Fire; break;
                case 1071093: ele = AosElementAttribute.Cold; break;
                case 1071094: ele = AosElementAttribute.Poison; break;
                case 1071095: ele = AosElementAttribute.Energy; break;
            }

            return new EarringsOfProtection(ele);
        }

        public static Item ConstructRobe(Mobile m, StoreEntry entry)
        {
            return new HoodedBritanniaRobe(entry.ItemID);
        }

        public static Item ConstructMiniHouseDeed(Mobile m, StoreEntry entry)
        {
            int label = entry.Name[1].Number;

            switch (label)
            {
                default:
                    for (int i = 0; i < MiniHouseInfo.Info.Length; i++)
                    {
                        if (MiniHouseInfo.Info[i].LabelNumber == entry.Name[1].Number)
                        {
                            MiniHouseType type = (MiniHouseType)i;

                            return new MiniHouseDeed(type);
                        }
                    }
                    return null;
                case 1157015: return new MiniHouseDeed(MiniHouseType.TwoStoryWoodAndPlaster);
                case 1157014: return new MiniHouseDeed(MiniHouseType.TwoStoryStoneAndPlaster);
            }
        }

        public static Item ConstructRaisedGarden(Mobile m, StoreEntry entry)
        {
            Bag bag = new Bag();

            bag.DropItem(new RaisedGardenDeed());
            bag.DropItem(new RaisedGardenDeed());
            bag.DropItem(new RaisedGardenDeed());

            return bag;
        }

        public static Item ConstructLampPost(Mobile m, StoreEntry entry)
        {
            LampPost2 item = new LampPost2
            {
                Movable = true,
                LootType = LootType.Blessed
            };

            return item;
        }

        public static Item ConstructForgedMetal(Mobile m, StoreEntry entry)
        {
            switch (entry.Name[1].Number)
            {
                case 1156686: return new ForgedMetalOfArtifacts(10);
                case 1156687: return new ForgedMetalOfArtifacts(5);
            }

            return null;
        }

        public static Item ConstructSoulstone(Mobile m, StoreEntry entry)
        {
            switch (entry.Name[0].Number)
            {
                case 1078835: return new SoulstoneToken(SoulstoneType.Blue);
                case 1078834: return new SoulstoneToken(SoulstoneType.Green);
                case 1158404: return new SoulstoneToken(SoulstoneType.Violet);
                case 1158869: return new SoulstoneToken(SoulstoneType.Orange);
                case 1158870: return new SoulstoneToken(SoulstoneType.Yellow);
                case 1158868: return new SoulstoneToken(SoulstoneType.White);
                case 1158867: return new SoulstoneToken(SoulstoneType.Black);
            }

            return null;
        }

        public static Item ConstructMerchantsTrinket(Mobile m, StoreEntry entry)
        {
            switch (entry.Name[0].Number)
            {
                case 1156827: return new MerchantsTrinket(false);
                case 1156828: return new MerchantsTrinket(true);
            }

            return null;
        }

        public static Item ConstructBOBCoverOne(Mobile m, StoreEntry entry)
        {
            return new BagOfBulkOrderCovers(12, 25);
        }

        public static Item ConstructBOBCoverTwo(Mobile m, StoreEntry entry)
        {
            return new BagOfBulkOrderCovers(1, 11);
        }

        public static Item ConstructHitchingPost(Mobile m, StoreEntry entry)
        {
            return new HitchingPost(false);
        }
        #endregion

        public static void AddPendingItem(Mobile m, Item item)
        {
            if (!PendingItems.TryGetValue(m, out List<Item> list))
            {
                PendingItems[m] = list = new List<Item>();
            }

            if (!list.Contains(item))
            {
                list.Add(item);
            }

            UltimaStoreContainer.DropItem(item);
        }

        public static bool HasPendingItem(PlayerMobile pm)
        {
            return PendingItems.ContainsKey(pm);
        }

        public static void CheckPendingItem(Mobile m)
        {
            if (PendingItems.TryGetValue(m, out List<Item> list))
            {
                int index = list.Count;

                while (--index >= 0)
                {
                    if (index >= list.Count)
                    {
                        continue;
                    }

                    Item item = list[index];

                    if (item != null)
                    {
                        if (m.Backpack != null && m.Alive && m.Backpack.TryDropItem(m, item, false))
                        {
                            if (item is IPromotionalToken && ((IPromotionalToken)item).ItemName != null)
                            {
                                // A token has been placed in your backpack. Double-click it to redeem your ~1_PROMO~.
                                m.SendLocalizedMessage(1075248, ((IPromotionalToken)item).ItemName.ToString());
                            }
                            else if (item.LabelNumber > 0 || item.Name != null)
                            {
                                string name = item.LabelNumber > 0 ? ("#" + item.LabelNumber) : item.Name;

                                // Your purchase of ~1_ITEM~ has been placed in your backpack.
                                m.SendLocalizedMessage(1156844, name);
                            }
                            else
                            {
                                // Your purchased item has been placed in your backpack.
                                m.SendLocalizedMessage(1156843);
                            }

                            list.RemoveAt(index);
                        }
                    }
                    else
                    {
                        list.RemoveAt(index);
                    }
                }

                if (list.Count == 0 && PendingItems.Remove(m))
                {
                    list.TrimExcess();
                }
            }
        }

        public static List<StoreEntry> GetSortedList(string searchString)
        {
            List<StoreEntry> list = new List<StoreEntry>();

            list.AddRange(Entries.Where(e => Insensitive.Contains(GetStringName(e.Name), searchString)));

            return list;
        }

        public static string GetStringName(TextDefinition[] text)
        {
            string str = string.Empty;

            foreach (TextDefinition td in text)
            {
                if (td.Number > 0 && VendorSearch.StringList != null)
                {
                    str += string.Format("{0} ", VendorSearch.StringList.GetString(td.Number));
                }
                else if (!string.IsNullOrWhiteSpace(td.String))
                {
                    str += string.Format("{0} ", td.String);
                }
            }

            return str;
        }

        public static string GetStringName(TextDefinition text)
        {
            string str = text.String;

            if (text.Number > 0 && VendorSearch.StringList != null)
            {
                str = VendorSearch.StringList.GetString(text.Number);
            }

            return str ?? string.Empty;
        }

        public static List<StoreEntry> GetList(StoreCategory cat, StoreEntry forcedEntry = null)
        {
            if (forcedEntry != null)
            {
                return new List<StoreEntry>() { forcedEntry };
            }

            return Entries.Where(e => e.Category == cat).ToList();
        }

        public static void SortList(List<StoreEntry> list, SortBy sort)
        {
            switch (sort)
            {
                case SortBy.Name:
                    list.Sort((a, b) => string.CompareOrdinal(GetStringName(a.Name), GetStringName(b.Name)));
                    break;
                case SortBy.PriceLower:
                    list.Sort((a, b) => a.Price.CompareTo(b.Price));
                    break;
                case SortBy.PriceHigher:
                    list.Sort((a, b) => b.Price.CompareTo(a.Price));
                    break;
                case SortBy.Newest:
                    break;
                case SortBy.Oldest:
                    list.Reverse();
                    break;
            }
        }

        public static int CartCount(Mobile m)
        {
            PlayerProfile profile = GetProfile(m, false);

            if (profile != null)
            {
                return profile.Cart.Count;
            }

            return 0;
        }

        public static int GetSubTotal(Dictionary<StoreEntry, int> cart)
        {
            if (cart == null || cart.Count == 0)
            {
                return 0;
            }

            double sub = 0.0;

            foreach (KeyValuePair<StoreEntry, int> kvp in cart)
            {
                sub += kvp.Key.Cost * kvp.Value;
            }

            return (int)sub;
        }

        public static int GetCurrency(Mobile m, bool sendMessage = false)
        {
            switch (Configuration.CurrencyImpl)
            {
                case CurrencyType.Sovereigns:
                    {
                        if (m is PlayerMobile)
                        {
                            return ((PlayerMobile)m).AccountSovereigns;
                        }
                    }
                    break;
                case CurrencyType.Gold:
                    return Banker.GetBalance(m);
                case CurrencyType.PointsSystem:
                    {
                        PointsSystem sys = PointsSystem.GetSystemInstance(Configuration.PointsImpl);

                        if (sys != null)
                        {
                            return (int)Math.Min(int.MaxValue, sys.GetPoints(m));
                        }
                    }
                    break;
                case CurrencyType.Custom:
                    return Configuration.GetCustomCurrency(m);
            }

            return 0;
        }

        public static void TryPurchase(Mobile m)
        {
            Dictionary<StoreEntry, int> cart = GetCart(m);
            int total = GetSubTotal(cart);

            if (cart == null || cart.Count == 0 || total == 0)
            {
                // Purchase failed due to your cart being empty.
                m.SendLocalizedMessage(1156842);
            }
            else if (total > GetCurrency(m, true))
            {
                if (m is PlayerMobile)
                {
                    BaseGump.SendGump(new NoFundsGump((PlayerMobile)m));
                }
            }
            else
            {
                int subtotal = 0;
                bool fail = false;

                List<StoreEntry> remove = new List<StoreEntry>();

                foreach (KeyValuePair<StoreEntry, int> entry in cart)
                {
                    for (int i = 0; i < entry.Value; i++)
                    {
                        if (!entry.Key.Construct(m))
                        {
                            fail = true;

                            try
                            {
                                using (StreamWriter op = File.AppendText("UltimaStoreError.log"))
                                {
                                    op.WriteLine("Bad Constructor: {0}", entry.Key.ItemType.Name);

                                    Utility.WriteConsoleColor(ConsoleColor.Red, "[Ultima Store]: Bad Constructor: {0}", entry.Key.ItemType.Name);
                                }
                            }
                            catch (Exception e)
                            {
                                Diagnostics.ExceptionLogging.LogException(e);
                            }
                        }
                        else
                        {
                            remove.Add(entry.Key);

                            subtotal += entry.Key.Cost;
                        }
                    }
                }

                if (subtotal > 0)
                {
                    DeductCurrency(m, subtotal);
                }

                PlayerProfile profile = GetProfile(m);

                foreach (StoreEntry entry in remove)
                {
                    profile.RemoveFromCart(entry);
                }

                if (fail)
                {
                    // Failed to process one of your items. Please check your cart and try again.
                    m.SendLocalizedMessage(1156853);
                }
            }
        }

        /// <summary>
        /// Should have already passed GetCurrency
        /// </summary>
        /// <param name="m"></param>
        /// <param name="amount"></param>
        public static int DeductCurrency(Mobile m, int amount)
        {
            switch (Configuration.CurrencyImpl)
            {
                case CurrencyType.Sovereigns:
                    {
                        if (m is PlayerMobile && ((PlayerMobile)m).WithdrawSovereigns(amount))
                        {
                            return amount;
                        }
                    }
                    break;
                case CurrencyType.Gold:
                    {
                        if (Banker.Withdraw(m, amount, true))
                        {
                            return amount;
                        }
                    }
                    break;
                case CurrencyType.PointsSystem:
                    {
                        PointsSystem sys = PointsSystem.GetSystemInstance(Configuration.PointsImpl);

                        if (sys != null && sys.DeductPoints(m, amount, true))
                        {
                            return amount;
                        }
                    }
                    break;
                case CurrencyType.Custom:
                    return Configuration.DeductCustomCurrecy(m, amount);
            }

            return 0;
        }

        #region Player Persistence
        public static Dictionary<Mobile, PlayerProfile> PlayerProfiles { get; private set; }

        public static PlayerProfile GetProfile(Mobile m, bool create = true)
        {
            PlayerProfile profile;

            if ((!PlayerProfiles.TryGetValue(m, out profile) || profile == null) && create)
            {
                PlayerProfiles[m] = profile = new PlayerProfile(m);
            }

            return profile;
        }

        public static Dictionary<StoreEntry, int> GetCart(Mobile m)
        {
            PlayerProfile profile = GetProfile(m, false);

            if (profile != null)
            {
                return profile.Cart;
            }

            return null;
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, Serialize);
        }

        public static void OnLoad()
        {
            Persistence.Deserialize(FilePath, Deserialize);
        }

        private static void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(_UltimaStoreContainer);

            writer.Write(PendingItems.Count);

            foreach (KeyValuePair<Mobile, List<Item>> kvp in PendingItems)
            {
                writer.Write(kvp.Key);
                writer.WriteItemList(kvp.Value, true);
            }

            writer.Write(PlayerProfiles.Count);

            foreach (KeyValuePair<Mobile, PlayerProfile> pe in PlayerProfiles)
            {
                pe.Value.Serialize(writer);
            }
        }

        private static void Deserialize(GenericReader reader)
        {
            reader.ReadInt();

            _UltimaStoreContainer = reader.ReadItem<UltimaStoreContainer>();

            int count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                Mobile m = reader.ReadMobile();
                List<Item> list = reader.ReadStrongItemList<Item>();

                if (m != null && list.Count > 0)
                {
                    PendingItems[m] = list;
                }
            }

            count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                PlayerProfile pe = new PlayerProfile(reader);

                if (pe.Player != null)
                {
                    PlayerProfiles[pe.Player] = pe;
                }
            }
        }
        #endregion
    }

    [DeleteConfirm("This is the Ultima Store item display container. You should not delete this.")]
    public sealed class UltimaStoreContainer : Container
    {
        private static readonly List<Item> _DisplayItems = new List<Item>();

        public override bool Decays => false;

        public override string DefaultName => "Ultima Store Display Container";

        public UltimaStoreContainer()
            : base(0) // No Draw
        {
            Movable = false;
            Visible = false;

            Internalize();
        }

        public UltimaStoreContainer(Serial serial)
            : base(serial)
        { }

        public void AddDisplayItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (!_DisplayItems.Contains(item))
            {
                _DisplayItems.Add(item);
            }

            DropItem(item);
        }

        public Item FindDisplayItem(Type t)
        {
            Item item = GetDisplayItem(t);

            if (item == null)
            {
                item = Loot.Construct(t);

                if (item != null)
                {
                    AddDisplayItem(item);
                }
            }

            return item;
        }

        public Item GetDisplayItem(Type t)
        {
            return _DisplayItems.FirstOrDefault(x => x.GetType() == t);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.WriteItemList(_DisplayItems, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();

            List<Item> list = reader.ReadStrongItemList();

            if (list.Count > 0)
            {
                Timer.DelayCall(o => o.ForEach(AddDisplayItem), list);
            }
        }
    }
}

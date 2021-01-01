using System.Collections.Generic;

namespace Server.Items
{
    public class VesperDonationBox : BaseCollectionItem
    {
        [Constructable]
        public VesperDonationBox()
            : base(0xE7D)
        {
            Hue = 0x48D;
            StartTier = 10000000;
            NextTier = 5000000;
            DailyDecay = 100000;
        }

        public VesperDonationBox(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1073407;// Please Contribute to the public Museum of Vesper.
        public override Collection CollectionID => Collection.VesperMuseum;
        public override int MaxTier => 12;
        public override void Init()
        {
            base.Init();

            Donations.Add(new CollectionItem(typeof(Gold), 0xEEF, 1073116, 0x0, 0.06666));
            Donations.Add(new CollectionItem(typeof(Board), 0x1BD7, 1015101, 0, 1));
            Donations.Add(new CollectionItem(typeof(OakBoard), 0x1BD7, 1075052, 0x7DA, 3));
            Donations.Add(new CollectionItem(typeof(AshBoard), 0x1BD7, 1075053, 0x4A7, 6));
            Donations.Add(new CollectionItem(typeof(YewBoard), 0x1BD7, 1075054, 0x4A8, 9));
            Donations.Add(new CollectionItem(typeof(HeartwoodBoard), 0x1BD7, 1075062, 0x4A9, 12));
            Donations.Add(new CollectionItem(typeof(BloodwoodBoard), 0x1BD7, 1075055, 0x4AA, 24));
            Donations.Add(new CollectionItem(typeof(FrostwoodBoard), 0x1BD7, 1075056, 0x47F, 48));
            Donations.Add(new CollectionItem(typeof(Hinge), 0x1055, 1044172, 0x0, 2));
            Donations.Add(new CollectionItem(typeof(Scorp), 0x10E7, 1075057, 0x0, 2));
            Donations.Add(new CollectionItem(typeof(DrawKnife), 0x10E4, 1075058, 0x0, 2));
            Donations.Add(new CollectionItem(typeof(JointingPlane), 0x1030, 1075059, 0x0, 4));
            Donations.Add(new CollectionItem(typeof(MouldingPlane), 0x102C, 1075060, 0x0, 4));
            Donations.Add(new CollectionItem(typeof(SmoothingPlane), 0x1032, 1075061, 0x0, 4));
        }

        public override void IncreaseTier()
        {
            base.IncreaseTier();

            List<object> list = new List<object>();
            Item c;

            // don't know names above lev 6
            switch (Tier)
            {
                case 1:
                    c = new BookOfChivalry();
                    c.MoveToWorld(new Point3D(2924, 979, -18), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Longsword();
                    c.MoveToWorld(new Point3D(2923, 980, -18), Map);
                    c.Movable = false;
                    c.ItemID = 0x26CF;
                    list.Add(c);

                    c = new Shirt();
                    c.MoveToWorld(new Point3D(2924, 978, -18), Map);
                    c.Movable = false;
                    c.ItemID = 0x2662;
                    c.Name = "Crisp White Shirt";
                    list.Add(c);
                    break;
                case 2:
                    c = new GraveDust();
                    c.MoveToWorld(new Point3D(2921, 972, -17), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new NoxCrystal();
                    c.MoveToWorld(new Point3D(2921, 972, -17), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Static(0xF91);
                    c.MoveToWorld(new Point3D(2921, 972, -17), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new NecromancerSpellbook();
                    c.MoveToWorld(new Point3D(2922, 972, -18), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new AnimateDeadScroll();
                    c.MoveToWorld(new Point3D(2923, 972, -18), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new HorrificBeastScroll();
                    c.MoveToWorld(new Point3D(2923, 972, -18), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new VampiricEmbraceScroll();
                    c.MoveToWorld(new Point3D(2923, 971, -20), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Static(0xFDD);
                    c.MoveToWorld(new Point3D(2922, 971, -21), Map);
                    list.Add(c);

                    c = new Static(0xFDE);
                    c.MoveToWorld(new Point3D(2923, 971, -21), Map);
                    list.Add(c);

                    break;
                case 3:
                    c = new JesterSuit();
                    c.MoveToWorld(new Point3D(2919, 985, -16), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new LocalizedStatic(0xE74, 1073424);
                    c.MoveToWorld(new Point3D(2919, 984, -11), Map);
                    c.Movable = false;
                    c.Weight = 50.0;
                    c.Hue = 0x113;
                    list.Add(c);

                    c = new JesterHat();
                    c.MoveToWorld(new Point3D(2919, 983, -13), Map);
                    c.Movable = false;
                    c.Hue = 0x113;
                    list.Add(c);

                    break;
                case 4:
                    c = new Static(0xD25);
                    c.MoveToWorld(new Point3D(2916, 984, -13), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Static(0x20D9);
                    c.MoveToWorld(new Point3D(2916, 982, -12), Map);
                    c.Name = "Gargoyle";
                    list.Add(c);

                    c = new Static(0x2132);
                    c.MoveToWorld(new Point3D(2914, 982, -9), Map);
                    list.Add(c);

                    c = new Static(0x25B6);
                    c.MoveToWorld(new Point3D(2913, 982, -13), Map);
                    list.Add(c);

                    c = new Static(0x25B6);
                    c.MoveToWorld(new Point3D(2913, 982, -13), Map);
                    list.Add(c);

                    c = new Static(0x222E);
                    c.MoveToWorld(new Point3D(2915, 983, -14), Map);
                    list.Add(c);

                    c = new Static(0x2211);
                    c.MoveToWorld(new Point3D(2914, 984, -13), Map);
                    list.Add(c);

                    break;
                case 5:
                    c = new LocalizedStatic(0xE30, 1073421);
                    c.MoveToWorld(new Point3D(2911, 983, -12), Map);
                    c.Weight = 10.0;
                    list.Add(c);

                    c = new LocalizedStatic(0x2937, 1073422);
                    c.MoveToWorld(new Point3D(2911, 984, -13), Map);
                    list.Add(c);

                    c = new LocalizedStatic(0x12AA, 1073423);
                    c.MoveToWorld(new Point3D(2911, 985, -14), Map);
                    list.Add(c);

                    c = new Static(0xEAF);
                    c.MoveToWorld(new Point3D(2910, 985, -21), Map);
                    c.Weight = 5.0;
                    list.Add(c);

                    c = new Static(0xEAE);
                    c.MoveToWorld(new Point3D(2910, 986, -21), Map);
                    c.Weight = 5.0;
                    list.Add(c);

                    break;
                case 6:
                    c = new Tessen();
                    c.MoveToWorld(new Point3D(2910, 966, -17), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Shuriken();
                    c.MoveToWorld(new Point3D(2910, 965, -17), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Static(0x2855);
                    c.MoveToWorld(new Point3D(2910, 964, -16), Map);
                    c.Weight = 5.0;
                    list.Add(c);

                    c = new Static(0x241D);
                    c.MoveToWorld(new Point3D(2910, 963, -20), Map);
                    c.Weight = 5.0;
                    list.Add(c);

                    c = new Static(0x2409);
                    c.MoveToWorld(new Point3D(2910, 963, -17), Map);
                    list.Add(c);

                    c = new Static(0x2416);
                    c.MoveToWorld(new Point3D(2909, 965, -17), Map);
                    list.Add(c);

                    break;
                case 7:
                    c = new Static(0x3069);
                    c.MoveToWorld(new Point3D(2914, 964, -21), Map);
                    list.Add(c);

                    c = new Static(0x306A);
                    c.MoveToWorld(new Point3D(2913, 964, -21), Map);
                    list.Add(c);

                    c = new Static(0x306B);
                    c.MoveToWorld(new Point3D(2912, 964, -21), Map);
                    list.Add(c);

                    c = new ElvenLoveseatEastAddon();
                    c.MoveToWorld(new Point3D(2913, 966, -21), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Static(0x2CFC);
                    c.MoveToWorld(new Point3D(2912, 963, -21), Map);
                    list.Add(c);

                    c = new LocalizedStatic(0x2D74, 1073425);
                    c.MoveToWorld(new Point3D(2914, 963, -21), Map);
                    list.Add(c);

                    break;
                case 8:
                    c = new Static(0x2);
                    c.MoveToWorld(new Point3D(2905, 970, -15), Map);
                    list.Add(c);

                    c = new Static(0x3);
                    c.MoveToWorld(new Point3D(2905, 969, -15), Map);
                    list.Add(c);

                    c = new OrderShield();
                    c.MoveToWorld(new Point3D(2905, 971, -17), Map);
                    c.Movable = false;
                    list.Add(c);

                    c = new Static(0x1579);
                    c.MoveToWorld(new Point3D(2904, 971, -21), Map);
                    list.Add(c);

                    c = new Static(0x1613);
                    c.MoveToWorld(new Point3D(2908, 969, -21), Map);
                    list.Add(c);

                    c = new Static(0x1614);
                    c.MoveToWorld(new Point3D(2908, 968, -21), Map);
                    list.Add(c);

                    break;
                case 9:
                    c = new Static(0x1526);
                    c.MoveToWorld(new Point3D(2905, 976, -15), Map);
                    list.Add(c);

                    c = new Static(0x1527);
                    c.MoveToWorld(new Point3D(2905, 975, -15), Map);
                    list.Add(c);

                    c = new Static(0x151A);
                    c.MoveToWorld(new Point3D(2905, 972, -21), Map);
                    list.Add(c);

                    c = new Static(0x151A);
                    c.MoveToWorld(new Point3D(2905, 977, -21), Map);
                    list.Add(c);

                    c = new Static(0x151A);
                    c.MoveToWorld(new Point3D(2908, 972, -21), Map);
                    list.Add(c);

                    c = new Static(0x151A);
                    c.MoveToWorld(new Point3D(2908, 977, -21), Map);
                    list.Add(c);

                    c = new Static(0x1514);
                    c.MoveToWorld(new Point3D(2904, 975, -17), Map);
                    list.Add(c);

                    break;
                case 10:
                    c = new Static(0x15C5);
                    c.MoveToWorld(new Point3D(2904, 982, -21), Map);
                    list.Add(c);

                    c = new Static(0x15C5);
                    c.MoveToWorld(new Point3D(2904, 979, -21), Map);
                    list.Add(c);

                    c = new Static(0x157B);
                    c.MoveToWorld(new Point3D(2904, 981, -21), Map);
                    list.Add(c);

                    c = new Static(0x14E3);
                    c.MoveToWorld(new Point3D(2905, 980, -21), Map);
                    list.Add(c);

                    c = new Static(0x14E4);
                    c.MoveToWorld(new Point3D(2905, 981, -21), Map);
                    list.Add(c);

                    c = new Static(0x14E5);
                    c.MoveToWorld(new Point3D(2906, 981, -21), Map);
                    list.Add(c);

                    c = new Static(0x14E6);
                    c.MoveToWorld(new Point3D(2906, 980, -21), Map);
                    list.Add(c);

                    c = new ChaosShield();
                    c.MoveToWorld(new Point3D(2905, 978, -19), Map);
                    c.Movable = false;
                    list.Add(c);

                    break;
                case 11:
                    c = new FemaleStuddedChest();
                    c.MoveToWorld(new Point3D(2912, 976, -16), Map);
                    c.Movable = false;
                    c.Hue = 0x497;
                    list.Add(c);

                    c = new Static(0x1EA8);
                    c.MoveToWorld(new Point3D(2913, 973, -13), Map);
                    c.Hue = 0x497;
                    list.Add(c);

                    c = new Static(0x20F8);
                    c.MoveToWorld(new Point3D(2913, 975, -11), Map);
                    c.Hue = 0x113;
                    list.Add(c);

                    c = new Static(0x20E9);
                    c.MoveToWorld(new Point3D(2912, 974, -11), Map);
                    c.Name = "Troll";
                    list.Add(c);

                    c = new Static(0x2607);
                    c.MoveToWorld(new Point3D(2913, 974, -11), Map);
                    list.Add(c);

                    c = new Static(0x25F9);
                    c.MoveToWorld(new Point3D(2912, 975, -11), Map);
                    list.Add(c);

                    break;
                case 12:
                    c = new Static(0x1D8A);
                    c.MoveToWorld(new Point3D(2915, 976, -13), Map);
                    list.Add(c);

                    c = new Static(0x1D8B);
                    c.MoveToWorld(new Point3D(2916, 976, -13), Map);
                    list.Add(c);

                    c = new Static(0x234D);
                    c.MoveToWorld(new Point3D(2915, 975, -10), Map);
                    list.Add(c);

                    c = new WizardsHat();
                    c.MoveToWorld(new Point3D(2915, 974, -13), Map);
                    c.Movable = false;
                    list.Add(c);
                    break;
            }

            if (list.Count > 0)
                Tiers.Add(list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

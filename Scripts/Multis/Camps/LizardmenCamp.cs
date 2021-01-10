using Server.Engines.Quests;
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Multis
{
    public class LizardmenCamp : BaseCamp
    {
        [Constructable]
        public LizardmenCamp()
            : base(0x1F6D)
        {
        }

        public LizardmenCamp(Serial serial)
            : base(serial)
        {
        }

        public virtual Mobile Lizardmen => new Lizardman();

        [CommandProperty(AccessLevel.GameMaster)]
        public override TimeSpan DecayDelay => TimeSpan.FromMinutes(5.0);

        public override void AddComponents()
        {
            AddItem(new Static(0x10ee), 0, 0, 0);
            AddItem(new Static(0xfac), 0, 7, 0);

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        AddItem(new Item(0xDE3), 0, 7, 0); // Campfire
                        AddItem(new Item(0x974), 0, 7, 1); // Cauldron
                        break;
                    }
                case 1:
                    {
                        AddItem(new Item(0x1E95), 0, 7, 1); // Rabbit on a spit
                        break;
                    }
                default:
                    {
                        AddItem(new Item(0x1E94), 0, 7, 1); // Chicken on a spit
                        break;
                    }
            }
            AddItem(new Item(0x41F), 4, 4, 0); // Gruesome Standart South

            AddCampChests();

            for (int i = 0; i < 4; i++)
            {
                AddMobile(Lizardmen, Utility.RandomMinMax(-7, 7), Utility.RandomMinMax(-7, 7), 0);
            }
        }

        // Don't refresh decay timer
        public override void OnEnter(Mobile m)
        {
        }

        public override void AddItem(Item item, int xOffset, int yOffset, int zOffset)
        {
            if (item != null)
                item.Movable = false;

            base.AddItem(item, xOffset, yOffset, zOffset);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    break;
                case 0:
                    {
                        Prisoner = reader.ReadMobile() as BaseCreature;
                        break;
                    }
            }
        }
    }
}

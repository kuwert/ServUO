using Server.Engines.Quests;
using Server.Items;
using Server.Mobiles;

namespace Server.Multis
{
    public class PrisonerCamp : BaseCamp
    {
        private BaseDoor m_Gate;

        [Constructable]
        public PrisonerCamp() : base(0x1D4C)
        {
        }

        public override void AddComponents()
        {
            IronGate gate = new IronGate(DoorFacing.EastCCW);
            m_Gate = gate;

            gate.KeyValue = Key.RandomValue();
            gate.Locked = true;

            AddItem(gate, -2, 1, 0);
            AddCampChests();

            switch (Utility.Random(4))
            {
                case 0:
                    {
                        AddMobile(new Orc(), 0, -2, 0);
                        AddMobile(new OrcishMage(), 0, 1, 0);
                        AddMobile(new OrcishLord(), 0, -2, 0);
                        AddMobile(new OrcCaptain(), 0, 1, 0);
                        AddMobile(new Orc(), 0, -1, 0);
                        AddMobile(new OrcChopper(), 0, -2, 0);
                    }
                    break;

                case 1:
                    {
                        AddMobile(new Ratman(), 0, -2, 0);
                        AddMobile(new Ratman(), 0, 1, 0);
                        AddMobile(new RatmanMage(), 0, -2, 0);
                        AddMobile(new Ratman(), 0, 1, 0);
                        AddMobile(new RatmanArcher(), 0, -1, 0);
                        AddMobile(new Ratman(), 0, -2, 0);
                    }
                    break;

                case 2:
                    {
                        AddMobile(new Lizardman(), 0, -2, 0);
                        AddMobile(new Lizardman(), 0, 1, 0);
                        AddMobile(new Lizardman(), 0, -2, 0);
                        AddMobile(new Lizardman(), 0, 1, 0);
                        AddMobile(new Lizardman(), 0, -1, 0);
                        AddMobile(new Lizardman(), 0, -2, 0);
                    }
                    break;

                case 3:
                    {
                        AddMobile(new Brigand(), 0, -2, 0);
                        AddMobile(new Brigand(), 0, 1, 0);
                        AddMobile(new Brigand(), 0, -2, 0);
                        AddMobile(new Brigand(), 0, 1, 0);
                        AddMobile(new Brigand(), 0, -1, 0);
                        AddMobile(new Brigand(), 0, -2, 0);
                    }
                    break;
            }
        }

        public PrisonerCamp(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version

            writer.Write(m_Gate);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_Gate = reader.ReadItem() as BaseDoor;
                    break;
                case 0:
                    {
                        Prisoner = reader.ReadMobile() as BaseCreature;
                        m_Gate = reader.ReadItem() as BaseDoor;
                        break;
                    }
            }
        }
    }
}

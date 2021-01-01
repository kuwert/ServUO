using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Network.Packets
{
    public sealed class SetWarMode : Packet
    {
        public static readonly Packet InWarMode = SetStatic(new SetWarMode(true));
        public static readonly Packet InPeaceMode = SetStatic(new SetWarMode(false));

        public SetWarMode(bool mode)
            : base(0x72, 5)
        {
            m_Stream.Write(mode);
            m_Stream.Write((byte)0x00);
            m_Stream.Write((byte)0x32);
            m_Stream.Write((byte)0x00);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    public class Device : DataChunk
    {
        private List<uint> _uniqueID = new List<uint>();
        public List<uint> uniqueID { get { return _uniqueID; } set { _uniqueID = value; } }
        private int _flashSize;
        public int flashSize { get { return _flashSize; } set { _flashSize = value; } }
        private int _sectorValue;
        public int sectorValue { get { return _sectorValue; } private set { _sectorValue = value; } }
        private int _sectorPacketVal;
        public int sectorPacketVal { get { return _sectorPacketVal; } private set { _sectorPacketVal = value; } }
        private int _totalPacket;
        public int totalPacket { get { _totalPacket = sectorPacketVal * flashSize; return _totalPacket; } private set { _totalPacket = value; } }

        public Device()
        {
            this.sectorValue = 1024;    // 1kbyte.
            this.sectorPacketVal = this.sectorValue / 16;   // 1 sektörde 64 paket olmuş olur.
        }



    }
}

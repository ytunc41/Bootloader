using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    public class Device : DataChunk
    {
        private static Dictionary<int, List<byte>> _device = new Dictionary<int, List<byte>>();
        public Dictionary<int, List<byte>> datas { get { return _device; } private set { _device = value; } }

        private static uint[] _uniqueID = new uint[3];
        public uint[] uniqueID { get { return _uniqueID; } set { _uniqueID = value; } }
        private static uint _devID;
        public uint devID { get { return _devID; } set { _devID = value; } }
        private static uint _revID;
        public uint revID { get { return _revID; } set { _revID = value; } }

        private static int _flashSize;
        public int flashSize { get { return _flashSize; } set { _flashSize = value; } }
        private static int _sectorValue;
        public int sectorValue { get { _sectorValue = 1024; /*1kbyte*/ return _sectorValue; } private set { _sectorValue = value; } }
        private static int _sectorPacketVal;
        public int sectorPacketVal { get { _sectorPacketVal = sectorValue / 16; return _sectorPacketVal; } private set { _sectorPacketVal = value; } }
        private static int _totalPacket;
        public int totalPacket { get { _totalPacket = sectorPacketVal * flashSize; return _totalPacket; } private set { _totalPacket = value; } }

        private static int _addrMin;
        public int addrMin { get { _addrMin = this.datas.Keys.Min(); return _addrMin; } private set { _addrMin = value; } }
        private static int _addrMax;
        public int addrMax { get { _addrMax = datas.Keys.Max() + datas[datas.Keys.Max()].Count; return _addrMax; } private set { _addrMax = value; } }

        public void AddHT(int addr, List<byte> data)
        {
            datas.Add(addr, data);
        }

        public void ClearAll()
        {
            datas.Clear();
            this.baseAddr = 0;
            this.startAddr = 0;
            this.addrMax = 0;
            this.addrMin = 0;
            this.totalPacket = 0;
        }

    }
}

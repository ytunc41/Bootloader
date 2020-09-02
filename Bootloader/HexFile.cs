using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    public class HexFile : DataChunk
    {
        private static Dictionary<int, List<byte>> _file = new Dictionary<int, List<byte>>();
        public Dictionary<int, List<byte>> datas { get { return _file; } private set { _file = value; } }

        private static string _fileText;
        public string fileText { get { return _fileText; } set { _fileText = value; } }

        private static int _addrMin;
        public int addrMin { get { _addrMin = this.datas.Keys.Min(); return _addrMin; } private set { _addrMin = value; } }
        private static int _addrMax;
        public int addrMax { get { _addrMax = datas.Keys.Max() + datas[datas.Keys.Max()].Count; return _addrMax; } private set { _addrMax = value; } }

        private static uint _CRC32;
        public uint CRC32 { get { return _CRC32; } set { _CRC32 = value; } }

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
        }

    }
}

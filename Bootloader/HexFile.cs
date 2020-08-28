﻿using System;
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
            baseAddr = 0;
            startAddr = 0;
        }

    }
}

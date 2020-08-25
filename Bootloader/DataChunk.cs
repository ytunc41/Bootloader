using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    public class DataChunk
    {
        private int _baseAddr;
        public int baseAddr { get { return _baseAddr; } set { _baseAddr = value; } }
        private int _startAddr;
        public int startAddr { get { return _startAddr; } set { _startAddr = value; } }
        private int _memAddres;
        public int memAddres
        {
            get
            {
                _memAddres = (_baseAddr << 16) + _startAddr;
                return _memAddres;
            }
            private set
            {
                _memAddres = value;
            }
        }

        private Dictionary<int, List<byte>> _dataChunk = new Dictionary<int, List<byte>>();
        public Dictionary<int, List<byte>> datas { get { return _dataChunk; } private set { _dataChunk = value; } }

        public DataChunk(int _base = 0)
        {
            _baseAddr = _base;
        }

        public void AddByte(List<byte> data)
        {
            _dataChunk.Add(memAddres, data);
        }

        public void AddHT(int addr, List<byte> data)
        {
            _dataChunk.Add(addr, data);
        }

        public void ClearAll()
        {
            datas.Clear();
            baseAddr = 0;
            startAddr = 0;
            memAddres = 0;
        }


    }
}

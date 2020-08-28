using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    public abstract class DataChunk
    {
        private static int _baseAddr;
        public int baseAddr { get { return _baseAddr; } set { _baseAddr = value; } }
        private static int _startAddr;
        public int startAddr { get { return _startAddr; } set { _startAddr = value; } }
        private static int _memAddres;
        public int memAddres{
            get
            {
                _memAddres = (this.baseAddr << 16) + this.startAddr;
                return _memAddres;
            }
            private set { _memAddres = value; }
        }

        
    }
}

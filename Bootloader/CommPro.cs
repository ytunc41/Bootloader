using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UINT8 = System.Byte;
using INT8 = System.SByte;
using UINT16 = System.UInt16;
using INT16 = System.Int16;
using UINT32 = System.UInt32;
using INT32 = System.Int32;
using UINT64 = System.UInt64;
using FLOAT32 = System.Single;
using FLOAT64 = System.Double;

namespace Bootloader
{
    public class CommPro
    {
        public PACKET_STATUS    packet_status;
        public PACKET_TYPE      packet_type;

        public UINT8[] tx_buffer = new UINT8[255];
        public UINT8[] rx_buffer = new UINT8[255];


        public bool PAKET_HAZIR_FLAG = false;
    }

    public static class SendPacket
    {
        public static UINT8 sof1 = 58;
        public static UINT8 sof2 = 34;
        public static UINT8 packetType;
        public static UINT8 packetCounter;
        public static UINT8 dataSize;
        public static UINT8[] data = new UINT8[255];
        public static UINT8 crc1 = 41;
        public static UINT8 crc2 = 69;
    }

    public static class ReceivedPacket
    {
        public static UINT8 sof1;
        public static UINT8 sof2;
        public static UINT8 packetType;
        public static UINT8 packetCounter;
        public static UINT8 dataSize;
        public static UINT8[] data = new UINT8[255];
        public static UINT8 crc1;
        public static UINT8 crc2;
    }

    public enum PACKET_STATUS
    {
        SOF1 = 0,
        SOF2 = 1,
        PACKET_TYPE = 2,
        PACKET_COUNTER = 3,
        DATA_SIZE = 4,
        DATA = 5,
        CRC1 = 6,
        CRC2 = 7
    }

    public enum PACKET_TYPE
    {
        BAGLANTI = 0,
        PROGRAM,
        READ,
        ERASE
    }




}

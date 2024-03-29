﻿using System;
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
        
        public List<UINT8> txBuffer = new List<UINT8>();
        public List<UINT8> rxBuffer = new List<UINT8>();

        public bool PAKET_HAZIR_FLAG = false;
        public PACKET_TYPE_FLAG PACKET_TYPE_FLAG = new PACKET_TYPE_FLAG();
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

    public enum CHECK_STATUS
    {
        SOF1 = 58,
        SOF2 = 34,
        CRC1 = 41,
        CRC2 = 69
    }

    public enum PACKET_TYPE
    {
        BAGLANTI_REQUEST = 0,
        BAGLANTI_OK,
        PROGRAM_REQUEST,
        PROGRAM_OK,
        PROGRAM_ERROR,
        PROGRAM_RUN,
        MEMORY_DEVICE_REQUEST,
        MEMORY_DEVICE_OK,
        READ_OK,
        READ_ERROR,
        ERASE_REQUEST,
        ERASE_OK,
        ERASE_ERROR,
        CRC_REQUEST,
        CRC_OK
    }

    public class PACKET_TYPE_FLAG
    {
        public bool BAGLANTI_OK = false;
        public bool PROGRAM_OK = false;
        public bool READ_OK = false;
        public bool ERASE_OK = false;

        public void ClearAll()
        {
            this.BAGLANTI_OK = false;
            this.PROGRAM_OK = false;
            this.READ_OK = false;
            this.ERASE_OK = false;
        }
    }

    




}

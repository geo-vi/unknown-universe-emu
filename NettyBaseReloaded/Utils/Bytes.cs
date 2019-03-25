using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Utils
{
    class ByteArray
    {

        /// <summary>
        /// Credits to Yuuki for being a good dog and now he got awarded with a bone(r).
        /// </summary>

        public List<byte> Message;
        public bool NROL;
        public ByteArray(short ID, bool NeedReverseOrLength = true)
        {
            Message = new List<byte>();
            NROL = NeedReverseOrLength;
            Short(ID);
        }

        public ByteArray(bool NeedReverseOrLenght = false)
        {
            Message = new List<byte>();
            NROL = NeedReverseOrLenght;
        }

        public void setNROL(bool to)
        {
            NROL = to;
        }

        public void Integer(int Int32)
        {
            AddBytes(BitConverter.GetBytes(Int32), true);
        }

        public void Short(short Short)
        {
            AddBytes(BitConverter.GetBytes(Short), true);
        }
        public void Double(double num)
        {
            AddBytes(BitConverter.GetBytes(num), true);
        }

        public void Long(long num)
        {
            AddBytes(BitConverter.GetBytes(num));
        }

        public void Float(float num)
        {
            AddBytes(BitConverter.GetBytes(num), true);
        }
        public void UTF(string String)
        {
            Short((short)String.Length);
            AddBytes(Encoding.UTF8.GetBytes(String), false);
        }

        public void Boolean(bool Bool)
        {
            AddBytes(new byte[] { (Bool) ? (byte)1 : (byte)0 }, false);
        }

        public void AddBytes(byte[] Bytes, bool IsInt = false)
        {
            if (IsInt)
            {
                for (int i = Bytes.Length - 1; i > -1; i--)
                {
                    this.Message.Add(Bytes[i]);
                }
            }
            else
            {
                this.Message.AddRange(Bytes);
            }
        }

        public byte[] ToByteArray()
        {

            List<byte> NewMsg = new List<byte>();
            NewMsg.AddRange(BitConverter.GetBytes((short)Message.Count));
            NewMsg.Reverse();
            NewMsg.AddRange(Message);

            return NewMsg.ToArray();
        }
    }
    class ByteReader
    {
        public static int ReadInt(byte[] data)
        {
            int outputResult = 0;
            outputResult += data[0] << 24;
            outputResult += data[1] << 16;
            outputResult += data[2] << 8;
            outputResult += data[3];
            return outputResult;
        }

        public static int ReadShort(byte[] data)
        {
            int outputResult = 0;
            outputResult += data[0] << 8;
            outputResult += data[1];
            return outputResult;
        }
    }

    class ByteParser
    {

        /// <summary>
        /// Credits to E*PVP
        /// </summary>

        public byte[] byteArray { get; set; }
        public int byteCounter { get; set; }
        public List<byte> command { get; set; }

        public short Lenght;
        public short CMD_ID;

        public ByteParser(byte[] byteArray)
        {
            this.byteArray = byteArray;
            this.command = new List<byte>();
            byteCounter = 0;

            this.Lenght = readShort();
            this.CMD_ID = readShort();
        }

        //Reads the next short of the byteArray (2 bytes)
        public short readShort()
        {
            short value = BitConverter.ToInt16(new byte[] { byteArray[byteCounter + 1], byteArray[byteCounter] }, 0);
            byteCounter += 2;

            return value;
        }

        //Reads the next int of the byteArray (4 bytes)
        public int readInt()
        {
            int value = BitConverter.ToInt32(new byte[] { byteArray[byteCounter + 3], byteArray[byteCounter + 2], byteArray[byteCounter + 1], byteArray[byteCounter] }, 0);
            byteCounter += 4;

            return value;
        }

        //Reads the next long of the byteArray (8 bytes)
        public long readLong()
        {
            long value = BitConverter.ToInt64(new byte[] { byteArray[byteCounter + 7], byteArray[byteCounter + 6], byteArray[byteCounter + 5], byteArray[byteCounter + 4], byteArray[byteCounter + 3], byteArray[byteCounter + 2], byteArray[byteCounter + 1], byteArray[byteCounter] }, 0);
            byteCounter += 8;

            return value;
        }

        //Reads the next double of the byteArray using BitConverter from readLong
        public double readDouble()
        {
            return BitConverter.Int64BitsToDouble(readLong());
        }

        // Reads the next float from the byteArray (4 bytes)
        public double readFloat()
        {
            var value = BitConverter.ToSingle(new byte[] { byteArray[byteCounter + 3], byteArray[byteCounter + 2], byteArray[byteCounter + 1], byteArray[byteCounter] }, 0);
            byteCounter += 4;
            return value;
        }

        //Reads the next String of the byteArray. The length of the String is given by one short before it.
        public string readUTF()
        {
            short stringLength = readShort();
            string value = Encoding.UTF8.GetString(byteArray, byteCounter, stringLength);

            byteCounter += stringLength;

            return value;
        }

        //Reads the next boolean (1 byte)
        public bool readBool()
        {
            return byteArray[byteCounter++] == 1;
        }

    }
}

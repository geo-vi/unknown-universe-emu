using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedController.Utils
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

        public void Float(float num)
        {
            AddBytes(BitConverter.GetBytes(num), true);
        }
        public void UTF(string String)
        {
            Short((short)String.Length);
            AddBytes(Encoding.Default.GetBytes(String), false);
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

        private byte[] Body;
        private int Pointer = 0;

        public ByteParser(byte[] Packet)
        {
            Body = Packet;
            this.Lenght = Short();
            this.CMD_ID = Short();

        }

        public short Lenght;
        public short CMD_ID;

        public short Short()
        {
            return Convert.ToInt16(ByteReader.ReadShort(new byte[] { Body[Pointer++], Body[Pointer++] }));
        }

        public int Int()
        {
            return ByteReader.ReadInt(
                new byte[] { Body[Pointer++], Body[Pointer++], Body[Pointer++], Body[Pointer++], });
        }

        public ushort UShort()
        {
            return Convert.ToUInt16(ByteReader.ReadShort(new byte[] { Body[Pointer++], Body[Pointer++] }));
        }

        public uint UInt()
        {
            return
                Convert.ToUInt32(
                    ByteReader.ReadInt(new byte[] { Body[Pointer++], Body[Pointer++], Body[Pointer++], Body[Pointer++], }));
        }

        public string UTF()
        {
            try
            {
                int Lenght = Short();
                string data = Encoding.UTF8.GetString(Body, Pointer, Lenght);
                Pointer += Lenght;
                return data;
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public bool Boolean()
        {
            return Body[Pointer++] == 1;
        }

        public double Double()
        {
            return BitConverter.ToDouble(Body, Pointer);
        }
    }
}

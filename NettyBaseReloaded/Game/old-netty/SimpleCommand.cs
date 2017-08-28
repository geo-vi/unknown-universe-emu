using System;
using System.Collections.Generic;
using System.Text;

namespace NettyBaseReloaded.Game.netty
{
    class SimpleCommand
    {
        public byte[] byteArray { get; set; }
        public int byteCounter { get; set; }
        public List<byte> command { get; set; }

        public SimpleCommand(byte[] byteArray)
        {
            this.byteArray = byteArray;
            this.command = new List<byte>();
            byteCounter = 0;

            readShort(); //Used to read the packet length (Just to "remove" it from the byteArray)
        }

        /// <summary>
        /// Used by clientCommands (Read only)
        /// </summary>
        /// <param name="command"></param>
        public SimpleCommand(SimpleCommand command)
        {
            this.byteArray = command.byteArray;
            this.command = new List<byte>();
            this.byteCounter = command.byteCounter;
        }

        /// <summary>
        /// Used by serverCommands (Write only)
        /// </summary>
        public SimpleCommand()
        {
            this.byteArray = null;
            this.command = new List<byte>();
            this.byteCounter = 0;
        }


        /****************
         * READ METHODS *
         ****************/

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
            var value = BitConverter.ToSingle(new byte[] { byteArray[byteCounter + 3], byteArray[byteCounter + 2],byteArray[byteCounter + 1], byteArray[byteCounter] }, 0);
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

        /*****************
         * WRITE METHODS *
         *****************/

        public void writeShort(int value)
        {
            writeShort((short)value);
        }

        public void writeShort(short value)
        {
            byte[] dataToOuput = new byte[2];
            dataToOuput[0] = ((byte)(((uint)value >> 8) & 0xFF));
            dataToOuput[1] = ((byte)(((uint)value >> 0) & 0xFF));
            command.AddRange(dataToOuput);
        }

        public void writeInt(int value)
        {
            byte[] dataToOuput = new byte[4];
            dataToOuput[0] = ((byte)(((uint)value >> 24) & 0xFF));
            dataToOuput[1] = ((byte)(((uint)value >> 16) & 0xFF));
            dataToOuput[2] = ((byte)(((uint)value >> 8) & 0xFF));
            dataToOuput[3] = ((byte)(((uint)value >> 0) & 0xFF));
            command.AddRange(dataToOuput);
        }

        public void writeFloat(float value)
        {
            byte[] temp = BitConverter.GetBytes(value);
            Array.Reverse(temp);
            writeInt(BitConverter.ToInt32(temp, 0));
        }

        public void writeDouble(double value)
        {
            writeLong(BitConverter.DoubleToInt64Bits(value));
        }

        public void writeLong(long v)
        {
            byte[] WriteBuffer = new byte[8];
            WriteBuffer[0] = (byte)((ulong)v >> 56);
            WriteBuffer[1] = (byte)((ulong)v >> 48);
            WriteBuffer[2] = (byte)((ulong)v >> 40);
            WriteBuffer[3] = (byte)((ulong)v >> 32);
            WriteBuffer[4] = (byte)((ulong)v >> 24);
            WriteBuffer[5] = (byte)((ulong)v >> 16);
            WriteBuffer[6] = (byte)((ulong)v >> 8);
            WriteBuffer[7] = (byte)((ulong)v >> 0);
            command.AddRange(WriteBuffer);
        }

        public void writeUTF(string value)
        {
            if (value == null)
                value = "";

            byte[] stringBytes = Encoding.UTF8.GetBytes(value + (char)0x00);
            writeShort((short)stringBytes.Length);
            command.AddRange(stringBytes);
        }

        public void writeBoolean(bool value)
        {
            command.Add(Convert.ToByte(value));
        }

        public void writeBytes(byte[] value)
        {
            command.AddRange(value);
        }

        public byte[] getBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((short)command.Count));
            bytes.Reverse();
            bytes.AddRange(command);

            return bytes.ToArray();
        }

    }
}

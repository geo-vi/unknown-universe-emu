using System;
using System.Security.Cryptography;
using System.Text;

namespace Server.Utils
{
    class Cypher
    {
        public static string EncryptBase64(string toEncode)
        {
            var toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(toEncode);
            var returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(encodedDataAsBytes);
        }

        public static string EncryptMD5(string source)
        {
            using (MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                return GetMd5Hash(md5Hash, source);
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}

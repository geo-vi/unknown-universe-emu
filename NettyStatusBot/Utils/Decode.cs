using System;
using System.Text;

namespace NettyStatusBot.Utils
{
    class Decode
    {
        public static string DecodeFrom64(string encodedData)
        {
            encodedData = encodedData.Replace("\r\n", String.Empty);

            encodedData = encodedData.Replace(" ", String.Empty);

            encodedData = encodedData.Replace('-', '+');

            encodedData = encodedData.Replace('_', '/');

            encodedData = encodedData.Replace(@"\/", "/");
            var encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(encodedDataAsBytes);
        }
    }
}

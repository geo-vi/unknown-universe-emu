using System;
using System.Linq;
using System.Reflection;

namespace NettyBaseReloaded.Unused_Codes
{
    public class Looping_through_constants
    {
        /// <summary>
        /// This should get all the constants from the file
        ///
        /// Name => Name of constant
        /// GetValue(object?) => Constant value
        /// </summary>

        public static void Execute(Type targetClassType)
        {
            foreach (FieldInfo x in targetClassType.GetFields().Where(x => x.IsStatic && x.IsLiteral))
            {
                Console.WriteLine(x.Name + " " + x.GetValue(new object()));
            }
        }
    }
}
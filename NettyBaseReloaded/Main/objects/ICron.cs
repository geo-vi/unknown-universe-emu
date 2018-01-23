using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NettyBaseReloaded.Main.objects
{
    class ICron
    {
        public string Action { get; set; }

        public string Params { get; set; }

        public static ICron ParseCronjob(XmlDocument xml)
        {
            return null;
        }
    }
}

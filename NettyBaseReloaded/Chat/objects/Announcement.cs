using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Chat.objects
{
    class Announcement
    {
        public int Id { get; }

        public string Text { get; }

        public Announcement(int id, string txt)
        {
            Id = id;
            Text = txt;
        }
    }
}

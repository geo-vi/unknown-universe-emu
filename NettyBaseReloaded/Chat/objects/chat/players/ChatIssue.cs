using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Chat.objects.chat.players
{
    class ChatIssue
    {
        public int Id { get; set; }

        public ChatIssueTypes IssueType { get; set; } 

        public int IssuedBy { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime Expiry { get; set; }

        public string Reason { get; set; }

        public Moderator GetIssuer() => Chat.StorageManager.GetChatModerator(IssuedBy);

        public ChatIssue(int id, ChatIssueTypes type, int issuedBy, DateTime timeIssued, DateTime expiry, string reason)
        {
            Id = id;
            IssueType = type;
            IssuedBy = issuedBy;
            IssuedAt = timeIssued;
            Expiry = expiry;
            Reason = reason;
        }

        public bool CanLogin()
        {
            if (IssueType == ChatIssueTypes.BAN && Expiry > DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}

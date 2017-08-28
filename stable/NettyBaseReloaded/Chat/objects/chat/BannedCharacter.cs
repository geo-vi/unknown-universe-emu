using System;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class BannedCharacter
    {
        /// <summary>
        /// Banned character id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Database ban id
        /// </summary>
        private int BanId { get; }

        /// <summary>
        /// Ban expiration time
        /// </summary>
        public DateTime BanEndTime { get; set; }

        /// <summary>
        /// Ban reason
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Additional mod notes to the ban
        /// </summary>
        public string Note { get; set; }

        public BannedCharacter(int id, DateTime endTime, string reason, string note = "")
        {
            Id = id;
            BanEndTime = endTime;
            Reason = reason;
            Note = note;
        }
    }
}
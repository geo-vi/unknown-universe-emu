using System;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Banned : Character
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

        public Banned(int id, DateTime endTime, int banId, string reason, string note = "") : base(id, "", "", null)
        {
            BanId = banId;
            BanEndTime = endTime;
            Reason = reason;
            Note = note;
        }
    }
}
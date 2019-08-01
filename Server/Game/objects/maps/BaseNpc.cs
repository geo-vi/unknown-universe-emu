namespace Server.Game.objects.maps
{
    class BaseNpc
    {
        public int NpcId { get; set; }
        public int Count { get; set; }

        public BaseNpc(int NpcId, int Count)
        {
            this.NpcId = NpcId;
            this.Count = Count;
        }
    }
}
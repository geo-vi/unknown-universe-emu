using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class CooldownsAssembly
    {
        public ConcurrentDictionary<int, Cooldown> CooldownDictionary = new ConcurrentDictionary<int, Cooldown>();

        private Character Character;
        public CooldownsAssembly(Character character)
        {
            Character = character;
        }

        private int GetNextId()
        {
            var i = 0;
            while (true)
            {
                if (CooldownDictionary.ContainsKey(i))
                    i++;
                else return i;
            }
        }

        public void Add(Cooldown cooldown)
        {
            cooldown.Id = GetNextId();
            CooldownDictionary.TryAdd(cooldown.Id, cooldown);
        }

        public void Remove(Cooldown cooldown)
        {
            CooldownDictionary.TryRemove(cooldown.Id, out _);
            cooldown.Id = -1;
        }

        private DateTime LastTick = new DateTime();
        private bool _sendAll = false;
        private readonly object CooldownLock = new object();
        public void Tick()
        {
            if (LastTick.AddMilliseconds(100) > DateTime.Now) return;
            lock (CooldownLock)
            {
                var cooldowns = CooldownDictionary.Values.ToList();
                foreach (var cooldown in cooldowns)
                {
                    //if (_sendAll && Character is Player player)
                    //{
                    //    var session = player.GetGameSession();
                    //    if (session != null) cooldown.Send(session);
                    //}
                    if (cooldown.EndTime < DateTime.Now)
                    {
                        cooldown.OnFinish(Character);
                        Remove(cooldown);
                    }
                }
            }

            LastTick = DateTime.Now;
        }

        public void SendAll()
        {
            _sendAll = true;
        }
    }
}

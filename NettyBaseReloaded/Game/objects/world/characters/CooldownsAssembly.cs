using System;
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
        private List<Cooldown> PendingToBeAdded = new List<Cooldown>();
        private List<Cooldown> PendingToBeRemoved = new List<Cooldown>();

        public List<Cooldown> Cooldowns = new List<Cooldown>();
        private Character Character;
        public CooldownsAssembly(Character character)
        {
            Character = character;
        }

        public void Add(Cooldown cooldown)
        {
            PendingToBeAdded.Add(cooldown);
        }

        public void Remove(Cooldown cooldown)
        {
            PendingToBeRemoved.Add(cooldown);
        }

        private void Sync()
        {
            if (PendingToBeAdded.Count > 0)
            {
                Cooldowns.AddRange(PendingToBeAdded);
                PendingToBeAdded.Clear();
            }
            if (PendingToBeRemoved.Count > 0)
            {
                foreach (var tick in PendingToBeRemoved)
                    Cooldowns.Remove(tick);
                PendingToBeRemoved.Clear();
            }
        }

        public void Tick()
        {
            Sync();
                    Parallel.For(0, Cooldowns.Count, index =>
                    {
                        var cooldown = Cooldowns[index];
                        if (cooldown.EndTime < DateTime.Now)
                        {
                            cooldown.OnFinish(Character);
                            Remove(cooldown);
                        }
                    });
        }

        public void Update()
        {
            Task.Factory.StartNew(() =>
            {
                while (Character.EntityState == EntityStates.ALIVE)
                {
                    Sync();
                    Parallel.For(0, Cooldowns.Count, index =>
                    {
                        var cooldown = Cooldowns[index];
                        if (cooldown.EndTime < DateTime.Now)
                        {
                            cooldown.OnFinish(Character);
                            Remove(cooldown);
                        }
                    });
                    Thread.Sleep(150);
                }
            });
        }

        public bool Any(Predicate<Cooldown> source)
        {
            if (PendingToBeRemoved.Count == 0 && PendingToBeAdded.Count == 0) return Cooldowns.Exists(source);
            return (PendingToBeAdded.Exists(source) || Cooldowns.Exists(source));
        } 
    }
}

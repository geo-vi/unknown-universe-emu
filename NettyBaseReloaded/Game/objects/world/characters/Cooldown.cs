using System;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    abstract class Cooldown
    {
        public DateTime StartTime = new DateTime();

        public DateTime EndTime = new DateTime();

        public Cooldown(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public virtual void OnStart(Character character)
        {
            character.Ticked += Ticked;
        }

        private void Ticked(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Verified");
            if (DateTime.Now > EndTime)
            {
                Console.WriteLine("End");
                var character = sender as Character;
                character.Ticked -= Ticked;
                character.Cooldowns.Remove(this);
                OnFinish(character);
            }
        }

        public abstract void OnFinish(Character character);

        public abstract void Send(GameSession gameSession);

        public byte[] SetCooldown(string itemId, short state, double time, double totalTime, bool activatable)
        {
            var command = new SlotbarCategoryItemTimerModule(itemId, new TimerState(state), time, totalTime, activatable);
            return command.write2();
        }

    }
}
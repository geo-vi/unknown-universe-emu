using System;
using System.Text;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using Console = System.Console;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController : ITick
    {
        /// <summary>
        /// TICK ID
        /// </summary>
        protected int TickId { get; set; }
        
        public Character Character { get; }

        public Checkers Checkers { get; }

        public Attack Attack { get; }

        public Damage Damage { get; }

        public Heal Heal { get; }

        public Destruction Destruction { get; }

        public Effects Effects { get; }

        public bool StopController { get; set; }

        public bool Active { get; set; }

        public AbstractCharacterController(Character character)
        {
            Character = character;

            Checkers = new Checkers(this);
            Attack = new Attack(this);
            Damage = new Damage(this);
            Heal = new Heal(this);
            Destruction = new Destruction(this);
            Effects = new Effects(this);

            StopController = false;

        }

        public virtual void Initiate()
        {
            Active = true;
            StopController = false;
            var id = -1;
            Global.TickManager.Add(this, out id);
            if (id != -1)
                TickId = id;
        }

        public int GetId()
        {
            return TickId;
        }

        public void Tick()
        {
            try
            {
                if (StopController || Character.EntityState == EntityStates.DEAD)
                {
                    StopAll();
                    return;
                }

                TickClasses();

                if (this is PlayerController playerController)
                {
                    playerController.Tick();
                }
                else if (this is NpcController npcController)
                {
                    npcController.Tick();
                }
                else if (this is PetController petController)
                {
                    petController.Tick();
                }
            }
            catch (Exception e)
            {
                var builder = new StringBuilder();
                builder.AppendLine("ERROR::");
                builder.AppendLine(Character.Id + " " + Character.Name);
                builder.AppendLine(e.ToString());
                builder.AppendLine(e.StackTrace);
                builder.AppendLine("ENDERROR");
                Logger.Logger._instance.Enqueue("pact", builder.ToString());
                StopAll();
                if (Character is Player player)
                {
                    var session = player.GetGameSession();
                    if (session != null)
                    {
                        Packet.Builder.LegacyModule(session, "PLEASE RELOG, KICKING YOU OUT!\nWe found error in your account, please report to admins.");
                        session.Kick();
                    }
                } 
            }
        }


        public void TickClasses()
        {
            Character.Position = MovementController.ActualPosition(Character);
            Checkers?.Tick();
            Attack?.Tick();
            Damage?.Tick();
            Heal?.Tick();
            Destruction?.Tick();
            Effects?.Tick();
        }

        public void StopAll()
        {
            Global.TickManager.Remove(this);
            StopController = true;
            Active = false;
            Checkers.Stop();
            Attack.Stop();
        }
    }
}

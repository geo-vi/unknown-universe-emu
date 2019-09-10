using System;

namespace Server.Game.objects.implementable
{
    interface ITriggerable
    {
        event EventHandler OnTriggered;
        Vector Position { get; set; }
        void Trigger();
    }
}

using System;
using Server.Game.objects.entities;

namespace Server.Game.controllers.characters
{
    abstract class AbstractedSubController
    {
        public Character Character;

        public virtual void OnAdded()
        {
        }

        public virtual void OnOverwritten()
        {
        }

        public virtual void OnTick()
        {
        }

        public virtual void OnRemoved()
        {
        }
    }
}
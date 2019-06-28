using System;
using Server.Game.objects.entities;

namespace Server.Game.controllers.pet
{
    class GearController : PetSubController
    {        
        public GearController(Pet pet) : base(pet)
        {
        }
        
        /// <summary>
        /// Adding a new GEAR to the pet, causes window update
        /// </summary>
        public void AddGear()
        {

        }

        /// <summary>
        /// Removing a PET gear, causes (if current gear is the gear removed) gear cancellation.
        /// </summary>
        public void RemoveGear()
        {

        }

        /// <summary>
        /// Switching to a certain gear, cancels current one, activates the new one
        /// </summary>
        public void SwitchGear()
        {

        }

        /// <summary>
        /// Activates a gear
        /// </summary>
        public void ActivateGear()
        {

        }

        /// <summary>
        /// Deactivates a gear
        /// </summary>
        public void DeactivateGear()
        {

        }

        /// <summary>
        /// Gets the current gear
        /// </summary>
        /// <returns>Returns it</returns>
        public object GetCurrentGear()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using Server.Game.objects.entities;

namespace Server.Game.controllers.pet
{
    class FuelController : PetSubController
    {
        public FuelController(Pet pet) : base(pet)
        {
        }
        
        /// <summary>
        /// Reducing via getting calculation and checking if it causes a shortage,
        /// if it does call FuelShortage() method
        /// </summary>
        public void Reduce()
        {

        }

        /// <summary>
        /// Calculating the power PET currently uses according to Pet's state
        /// </summary>
        /// <returns></returns>
        public int CalculateRunningPower()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shutdown P.E.T due to insufficient pet fuel
        /// </summary>
        public void FuelShortage()
        {

        }
    }
}

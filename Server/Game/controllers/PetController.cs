using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects.entities;
using Server.Game.objects.entities.pets;
using Server.Game.objects.enums;

namespace Server.Game.controllers
{
    class PetController : AbstractCharacterController
    {
        private Pet Pet;
        
        public ConcurrentDictionary<PetGears, PetGear> PetGears = new ConcurrentDictionary<PetGears, PetGear>();

        protected PetController(Pet pet) : base(pet)
        {
            Pet = pet;
        }
        
        public void Run()
        {
            
        }
               
        public void ChangeGear(PetGears petGear)
        {
            
        }

        public void AddGear(List<PetGears> petGears)
        {
            
        }

        public void RemoveGears(List<PetGears> petGears)
        {
            
        }

        public void DeactivateGear(PetGears gear)
        {
            
        }
    }
}

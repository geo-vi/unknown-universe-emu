using Server.Game.controllers.characters;
using Server.Game.controllers.players;
using Server.Game.objects.entities;

namespace Server.Game.controllers
{
    class PlayerController : AbstractCharacterController
    {
        private Player _player;
        
        public PlayerController(Player player) : base(player)
        {
            _player = player;
        }

        protected override void CreateControllers()
        {
            base.CreateControllers();
            OverrideControlledInstance<ConfigurationController, PlayerConfigurationController>();
            OverrideControlledInstance<CharacterRangeController, PlayerRangeController>();
            OverrideControlledInstance<CharacterSelectionController, PlayerSelectionController>();
            OverrideControlledInstance<CharacterMovementController, PlayerMovementController>();
            OverrideControlledInstance<CharacterCombatController, PlayerCombatController>();
            OverrideControlledInstance<CharacterDamageController, PlayerDamageController>();
            CreateControlledInstance<AbilityController>();
            CreateControlledInstance<BoosterController>();
            CreateControlledInstance<CargoController>();
            CreateControlledInstance<CollectableController>();
            CreateControlledInstance<CpuController>();
            CreateControlledInstance<DroneController>();
            CreateControlledInstance<GalaxyGateController>();
            CreateControlledInstance<InformationController>();
            CreateControlledInstance<ItemController>();
            CreateControlledInstance<JumpingController>();
            CreateControlledInstance<LogoutController>();
            CreateControlledInstance<PlayerLevelController>();
            CreateControlledInstance<PlayerLogController>();
            CreateControlledInstance<RewardController>();
            CreateControlledInstance<SessionController>();
            CreateControlledInstance<SettingsController>();
            CreateControlledInstance<TechController>();
            CreateControlledInstance<WindowsController>();
        }
    }
}

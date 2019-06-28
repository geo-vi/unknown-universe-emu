using DotNetty.Buffers;
using Server.Game.netty.commands.old_client;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class BattleStationEquipHandler : IHandler
    {
        public void execute(GameSession session, IByteBuffer buffer)
        {
            if (session.Player.UsingNewClient)
            {
                return;
            }

            var cmd = new EquipModuleRequest();
            cmd.readCommand(buffer);
            if (session.Player.Range.Objects.FirstOrDefault(x => x.Value is Asteroid).Value is Asteroid asteroid)
            {
                //if (!session.Player.Equipment.Modules.ContainsKey(cmd.itemId))
                //{
                //    ThrowError(session);
                //    return;
                //}
                //var module = session.Player.Equipment.Modules[cmd.itemId];
                //if (session.Player.Equipment.ModuleEquipping || module == null)
                //{
                //    ThrowError(session);
                //    return;
                //}

                //var battleStationModule = BattleStationModule.Equip(session.Player, module, asteroid, cmd.slotId);
                //if (battleStationModule == null)
                //{
                //    ThrowError(session);
                //    return;
                //}
                //asteroid.EquippedModules.Add(cmd.itemId, battleStationModule);
                //foreach (var rangeSession in session.Player.Range.Entities.Where(x => x.Value is Player && x.Value.Range.Objects.ContainsKey(asteroid.Id)))
                //    Packet.Builder.BattleStationBuildingUiInitializationCommand(((Player)rangeSession.Value).GetGameSession(), asteroid);
                //Packet.Builder.BattleStationBuildingUiInitializationCommand(session, asteroid);
                //session.Player.Equipment.ModuleEquipping = true;
            }

        }

        public void ThrowError(GameSession session)
        {
            session.Client.Send(BattleStationErrorCommand.write(BattleStationErrorCommand.ITEM_NOT_OWNED).Bytes);
        }
    }
}

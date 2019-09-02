using System;
using System.Collections.Generic;
using System.Linq;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.handlers;
using NettyBaseReloaded.Networking;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.packet
{
    class Handler
    {
        public Dictionary<short, IHandler> OldClientCommands = new Dictionary<short, IHandler>();
        public Dictionary<short, IHandler> NewClientCommands = new Dictionary<short, IHandler>();
        public Dictionary<string, ILegacyHandler> LegacyCommands = new Dictionary<string, ILegacyHandler>();

        public void AddCommands()
        {
            NewClientCommands.Add(commands.new_client.requests.MoveRequest.ID, new MoveHandler());
            OldClientCommands.Add(commands.old_client.requests.MoveRequest.ID, new MoveHandler());
            NewClientCommands.Add(commands.new_client.requests.UIOpenRequest.ID, new UIOpenHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipSelectionRequest.ID, new ShipSelectionHandler());
            NewClientCommands.Add(commands.new_client.requests.ShipSelectionRequest.ID, new ShipSelectionHandler());
            NewClientCommands.Add(commands.new_client.requests.ItemSelectionRequest.ID, new ItemSelectionHandler());
            OldClientCommands.Add(commands.old_client.requests.SelectBatteryRequest.ID, new ItemSelectionHandler());
            OldClientCommands.Add(commands.old_client.requests.SelectRocketRequest.ID, new ItemSelectionHandler());
            NewClientCommands.Add(commands.new_client.requests.AttackLaserRequest.ID, new AttackLaserHandler());
            OldClientCommands.Add(commands.old_client.requests.AttackLaserRequest.ID, new AttackLaserHandler());
            NewClientCommands.Add(commands.new_client.requests.AttackAbortLaserRequest.ID, new AttackAbortLaserHandler());
            LegacyCommands.Add(ClientCommands.LASER_STOP, new AttackAbortLaserHandler());
            NewClientCommands.Add(commands.new_client.requests.PetRequest.ID, new PetHandler());
            OldClientCommands.Add(commands.old_client.requests.PetRequest.ID, new PetHandler());
            NewClientCommands.Add(commands.new_client.requests.AttackRocketRequest.ID, new AttackRocketHandler());
            NewClientCommands.Add(commands.old_client.requests.AttackRocketRequest.ID, new AttackRocketHandler());
            LegacyCommands.Add(ClientCommands.SELECT, new LegacySelectHandler());
            LegacyCommands.Add(ServerCommands.TECHS, new LegacySelectHandler());
            LegacyCommands.Add(ServerCommands.REQUEST_SHIP, new ForceInitHandler());
            NewClientCommands.Add(commands.new_client.requests.ClickableRequest.ID, new ClickableHandler());
            NewClientCommands.Add(commands.new_client.requests.commandHF.ID, new command42JHandler());
            OldClientCommands.Add(commands.old_client.requests.CollectBoxRequest.ID, new CollectBoxHandler());
            NewClientCommands.Add(commands.new_client.requests.CollectBoxRequest.ID, new CollectBoxHandler());
            LegacyCommands.Add(ClientCommands.PORTAL_JUMP, new JumpRequestHandler());
            LegacyCommands.Add(ServerCommands.ROCKET_ATTACK, new AttackRocketLegacyHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipWarpWindowRequest.ID, new ShipWarpWindowHandler()); 
            OldClientCommands.Add(commands.old_client.requests.PetGearActivationRequest.ID, new PetGearActivationHandler());
            OldClientCommands.Add(commands.old_client.requests.WindowSettingsRequest.ID, new WindowSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.HellstormLoadRequest.ID, new HellstormLoadHandler());
            OldClientCommands.Add(commands.old_client.requests.HellstormLaunchRequest.ID, new HellstormLaunchHandler());
            OldClientCommands.Add(commands.old_client.requests.HellstormSelectRocketRequest.ID, new HellstormSelectRocketHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipSettingsRequest.ID, new ShipSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.DisplaySettingsRequest.ID, new DisplaySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.QualitySettingsRequest.ID, new QualitySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.AudioSettingsRequest.ID, new AudioSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.GameplaySettingsRequest.ID, new GameplaySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.LogoutRequest.ID, new LogoutHandler());
            OldClientCommands.Add(commands.old_client.requests.ClientResolutionChangeRequest.ID, new ClientResolutionChangeHandler());
            LegacyCommands.Add(ClientCommands.GROUPSYSTEM, new GroupSystemHandler());
            OldClientCommands.Add(commands.old_client.requests.DroneFormationChangeRequest.ID, new DroneFormationChangeHandler());
            OldClientCommands.Add(commands.old_client.requests.KillScreenRepairRequest.ID, new KillScreenRepairRequestHandler());
            OldClientCommands.Add(commands.old_client.requests.EquipModuleRequest.ID, new BattleStationEquipHandler());
            OldClientCommands.Add(commands.old_client.requests.BuildStationRequest.ID, new BuildStationHandler());
            OldClientCommands.Add(commands.old_client.requests.AbilityLaunchRequest.ID, new AbilityLaunchHandler());
            OldClientCommands.Add(commands.old_client.requests.HarvestRequest.ID, new HarvestHandler());
            OldClientCommands.Add(commands.old_client.requests.QuestListRequest.ID, new QuestListHandler());
            OldClientCommands.Add(commands.old_client.requests.QuestInfoRequest.ID, new QuestInfoHandler());
            OldClientCommands.Add(commands.old_client.requests.QuestCancelRequest.ID, new QuestCancelHandler());
            OldClientCommands.Add(commands.old_client.requests.QuestAcceptRequest.ID, new QuestAcceptHandler());
            OldClientCommands.Add(commands.old_client.requests.LabRefinementRequest.ID, new LabRefinementHandler());
            OldClientCommands.Add(commands.old_client.requests.LabUpdateItemRequest.ID, new LabUpdateItemHandler());
            OldClientCommands.Add(commands.old_client.requests.TradeRequest.ID, new TradeHandler());
            OldClientCommands.Add(commands.old_client.requests.QuestPrivilegeRequest.ID, new QuestPrivilegeHandler());
            OldClientCommands.Add(commands.old_client.requests.TradeSellOreRequest.ID, new TradeSellOreHandler());
            LegacyCommands.Add(ServerCommands.ADVANCED_JUMP_CPU, new JumpCPUHandler());
            OldClientCommands.Add(commands.old_client.requests.ClanMemberInvitationRequest.ID, new ClanMemberInvitationHandler());
            LegacyCommands.Add(ServerCommands.EXCHANGE_PALLADIUM, new ExchangePalladiumHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipWarpRequest.ID, new ShipWarpHandler());
            OldClientCommands.Add(commands.old_client.requests.LabUpdateRequest.ID, new LabUpdateHandler());
            OldClientCommands.Add(commands.old_client.UserKeyBindingsUpdate.ID, new UserKeyBindingsUpdateHandler());
        }

        public void LookUp(IByteBuffer buffer, GameClient client)
        {
            try
            {
                var parser = new ByteParser(buffer.Copy());

                if (parser.CMD_ID == commands.old_client.requests.VersionRequest.ID)
                {
                    var cmd = new commands.old_client.requests.VersionRequest();
                    cmd.readCommand(buffer);
                    new ShipInitalizationHandler(client, cmd.playerId, cmd.sessionId);
                    return;
                }

                if (parser.CMD_ID == commands.new_client.requests.LoginRequest.ID)
                {
                    var cmd = new commands.new_client.requests.LoginRequest();
                    cmd.readCommand(buffer);
                    new ShipInitalizationHandler(client, cmd.playerId, cmd.sessionId, true);
                    return;
                }

                var gameSession = World.StorageManager.GetGameSession(client.UserId);

                if (gameSession != null)
                {
                    if (gameSession.Player.UsingNewClient)
                    {
                        if (NewClientCommands.ContainsKey(parser.CMD_ID))
                            NewClientCommands[parser.CMD_ID].execute(gameSession, buffer);
                        //else if (parser.CMD_ID != commands.new_client.LegacyModule.ID) Console.WriteLine("[3D] Received->{0} Instance: {1}", parser.CMD_ID, client.UserId);
                    }
                    else
                    {
                        if (OldClientCommands.ContainsKey(parser.CMD_ID))
                            OldClientCommands[parser.CMD_ID].execute(gameSession, buffer);
                        //else if (parser.CMD_ID != commands.old_client.LegacyModule.ID) Console.WriteLine("[7.5.3] Received->{0} Instance: {1}", parser.CMD_ID, client.UserId);
                    }

                    if (parser.CMD_ID == commands.new_client.LegacyModule.ID ||
                        parser.CMD_ID == commands.old_client.LegacyModule.ID)
                    {
                        var packet = parser.readUTF();

                        if (packet.Contains('|'))
                        {
                            var splittedPacket = packet.Split('|');
                            if (LegacyCommands.ContainsKey(splittedPacket[0]))
                                LegacyCommands[splittedPacket[0]].execute(gameSession, splittedPacket);
                            //else Console.WriteLine("Received->{0} Instance: {1}", packet, client.UserId);
                        }
                        else
                        {
                            if (LegacyCommands.ContainsKey(packet))
                                LegacyCommands[packet].execute(gameSession, new[] {packet});
                            //else Console.WriteLine("Received->{0} Instance: {1}", packet, client.UserId);

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception found.. Disconnecting user");
                Console.WriteLine("Exception: " + exception + ";" + exception.StackTrace + ";" + exception.Message);
                if (client.UserId != 0)
                {
                    // find session
                    var session = World.StorageManager.GetGameSession(client.UserId);
                    if (session != null)
                    {
                        session.Kick();
                    }
                    else client.Disconnect();
                }
                else client.Disconnect();
            }
        }

        public void DialogLookUp(string packet)
        {
            
        }
    }
}

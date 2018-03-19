﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.login
{
    class Regular : ILogin
    {
        public Regular(GameSession gameSession) : base(gameSession)
        {
        }

        public override void Execute()
        {
            InitiateEvents();
            SendSettings();
            Spawn();
            SendLegacy();
            //SendTestQuest();
            AddCBS();
        }

        private void SendTestQuest()
        {
            //if (GameSession.Player.UsingNewClient) return;
            //var testQuest = Quest.LoadQuest(0);

            //var elements = QuestElement.ParseElementsOld(testQuest.Root.Elements);
            //var reward = Quest.GetReward(testQuest.Reward);
            //var quest = new QuestDefinitionModule(testQuest.Id,
            //    new List<QuestTypeModule> { new QuestTypeModule((short)testQuest.QuestType) }, new QuestCaseModule(testQuest.Root.Id, testQuest.Root.Active,
            //        testQuest.Root.Mandatory, testQuest.Root.Ordered, testQuest.Root.MandatoryCount, elements), reward, new List<QuestIconModule> { new QuestIconModule((short)testQuest.Icon) });

            //GameSession.Client.Send(QuestInitializationCommand.write(quest).Bytes);
        }

        private void CoolExampleQuest()
        {
            int questId = 1;
            QuestTypes type = QuestTypes.MISSION;
            QuestIcons icon = QuestIcons.KILL;

            // mandatory => {value}/{targetnumber}
            // for flying to pos (QuestConditions.TRAVEL) currentvalue = x, targetValue = y

            var quest = new QuestDefinitionModule(questId,
                new List<QuestTypeModule> {new QuestTypeModule((short) type)}, new QuestCaseModule(questId * 100, false,
                    true, false, 0, new List<QuestElementModule>
                    {
                        new QuestElementModule(
                            new QuestCaseModule(0, true, true, false, 1, new List<QuestElementModule>()),
                            new QuestConditionModule(questId * 100 + 1, new List<int> {4}, 6, 6, 100, true,
                                new QuestConditionStateModule(0, true, false), new List<QuestConditionModule>())),
                        new QuestElementModule(
                            new QuestCaseModule(0, false, true, true, 2, new List<QuestElementModule>()),
                            new QuestConditionModule(questId * 100 + 2, new List<int>(), (short) QuestConditions.TRAVEL,
                                (short) QuestConditions.TRAVEL, 3, true, new QuestConditionStateModule(5, false, false),
                                new List<QuestConditionModule>()))
                    }), new List<LootModule>
                {
                    new LootModule("currency_uridium", 69)
                }, new List<QuestIconModule> {new QuestIconModule((short) icon)});



            GameSession.Client.Send(QuestInitializationCommand.write(quest).Bytes);
        }

        public void Spawn()
        {
            var player = GameSession.Player;
            if (!player.Spacemap.Entities.ContainsKey(player.Id))
            {
                player.Spacemap.AddEntity(player);
            }
            else
            {
                player.Range.Clear();
                player.Storage.Clean();
            }

            Packet.Builder.ShipInitializationCommand(GameSession);
            Packet.Builder.DronesCommand(GameSession, GameSession.Player);
        }

        public void AddCBS()
        {
            var center = new Vector(30100, 8900);
            var map = GameSession.Player.Spacemap;
            var m1Pos = new Vector(center.X - 413, center.Y - 98);
            var m2Pos = new Vector(center.X - 171, center.Y - 236);
            var m3Pos = new Vector(center.X + 170, center.Y + 236);
            var m4Pos = new Vector(center.X + 412, center.Y - 98);
            var m5Pos = new Vector(center.X + 412, center.Y + 97);
            var m6Pos = new Vector(center.X + 170, center.Y - 235);
            var m7Pos = new Vector(center.X - 171, center.Y + 235);
            var m8Pos = new Vector(center.X - 413, center.Y + 97);

            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111111, "Kureboec", AssetTypes.BATTLESTATION, Faction.VRU, Global.StorageManager.Clans[3], 2, 0, center, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111112, "Maikati-01", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 0, 0, m1Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111113, "Maikati-02", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 3, 0, m2Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111114, "Maikati-03", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 4, 0, m3Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111115, "Maikati-04", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 5, 0, m4Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111116, "Maikati-05", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 6, 0, m5Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111117, "Maikati-06", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 7, 0, m6Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111118, "Maikati-07", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 8, 0, m7Pos, map, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111119, "Maikati-08", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[3], 9, 0, m8Pos, map, false, false, true));
        }
    }
}

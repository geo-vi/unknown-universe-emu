using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Moderator : Character
    {
        public ModController Controller { get; set; }

        public ModeratorLevel Hierarchy { get; set; }

        public int CreatorId { get; set; }

        public Dictionary<int, Room> AssignedRooms { get; set; }

        public Dictionary<int, Room> FavoriteRooms { get; set; }

        public string ChatColor { get; set; }

        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }

        public bool Online { get; set; }

        public bool Custom { get; set; }

        public bool Invisible { get; set; }

        public string Forename { get; set; }

        public string Lastname { get; set; }

        public DateTime Birthday { get; set; }

        public Moderator(int id, string username, Clan clan, Dictionary<int, Room> connectedRooms, string lang,
            ModeratorLevel hierarchy, int creatorId, Dictionary<int, Room> assignedRooms, Dictionary<int, Room> favRooms, string chatColor, DateTime lastLogin, DateTime lastLogout, 
            bool online, bool custom, bool invisible, string forename, string lastname, DateTime birthday) : base(id, username, clan, lang)
        {
            Hierarchy = hierarchy;
            CreatorId = creatorId;
            AssignedRooms = assignedRooms;
            FavoriteRooms = favRooms;
            ChatColor = chatColor;
            LastLogin = lastLogin;
            LastLogout = lastLogout;
            Online = online;
            Custom = custom;
            Invisible = invisible;
            Forename = forename;
            Lastname = lastname;
            Birthday = birthday;
        }

        public string GetAssignedRooms()
        {
            string baseString = "";
            if (AssignedRooms.Count == 0) baseString = "-1";
            else
            {
                int i = 0;
                var max = AssignedRooms.Count;
                foreach (var room in AssignedRooms.Values)
                {
                    i++;
                    if (i == max)
                        baseString += room.Id;
                    else
                        baseString += room.Id + ",";
                }
            }
            return baseString;
        }

        public string GetFavoriteRooms()
        {
            string baseString = "";
            if (FavoriteRooms.Count == 0) baseString = "-1";
            else
            {
                int i = 0;
                var max = FavoriteRooms.Count;
                foreach (var room in FavoriteRooms.Values)
                {
                    i++;
                    if (i == max)
                        baseString += room.Id;
                    else
                        baseString += room.Id + ",";
                }
            }
            return baseString;
        }
    }
}

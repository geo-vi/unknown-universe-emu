using System;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using Server.Game.objects.maps.objects.assets.triggered;

namespace Server.Game.controllers.characters
{
    class ShipController : AbstractedSubController
    {
        public void Update()
        {
//            MovementController.Move(this, MovementController.ActualPosition(this));
//
//            var sessions =
//                World.StorageManager.GameSessions.Where(
//                    x => x.Value.Client != null && InRange(x.Value.Player));
//            foreach (var session in sessions)
//            {
//                if (session.Key != Id && session.Value != null)
//                    Packet.Builder.ShipCreateCommand(session.Value, this);
//            }
//
//            if (this is Player)
//                Packet.Builder.ShipInitializationCommand(World.StorageManager.GetGameSession(Id));
//
//            Updaters.Update();
        }

        public void SetPosition(Vector newPosition)
        {
//            Destination = targetPosition;
//            Position = targetPosition;
//            OldPosition = targetPosition;
//            Direction = targetPosition;
//            Moving = false;
//
//            MovementController.Move(this, MovementController.ActualPosition(this)); 
        }

        public void RefreshRangeView()
        {
//            foreach (var rangeCharacter in Range.Entities.Values.Where(x => x is Player))
//            {
//                var rangePlayer = rangeCharacter as Player;
//                var session = rangePlayer.GetGameSession();
//                if (session == null) return;
//                Packet.Builder.ShipRemoveCommand(session, this);
//                Packet.Builder.ShipCreateCommand(session, this);
//            }
//
//            if (this is Player p)
//            {
//                var session = p.GetGameSession();
//                if (session == null) return;
//
//                Packet.Builder.ShipRemoveCommand(session, this);
//                p.Refresh();
//            }
        }

        public void RefreshOwnView()
        {
            
        }

        public void MoveToMap(Vector newPosition, Spacemap newMap)
        {
            
        }

        public Station GetClosestStation()
        {
            throw new NotImplementedException();
        }
    }
}
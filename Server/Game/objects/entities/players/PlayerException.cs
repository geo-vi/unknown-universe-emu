using System;
using System.Runtime.Serialization;

namespace Server.Game.objects.entities.players
{
    class PlayerException : Exception
    {
        /// <summary>
        /// Player that exception links to
        /// </summary>
        private Player _player;

        /// <summary>
        /// Custom message
        /// </summary>
        public override string Message => "Player exception was thrown from Player " + _player.Id + " with name " + _player.Name;

        public PlayerException(Player player)
        {
            _player = player;
        }
    }
}
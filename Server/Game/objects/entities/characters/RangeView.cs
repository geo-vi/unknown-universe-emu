using System.Collections.Concurrent;

namespace Server.Game.objects.entities.characters
{
    class RangeView
    {
        public readonly ConcurrentDictionary<int, Character> CharactersInRenderRange = new ConcurrentDictionary<int, Character>();
    }
}
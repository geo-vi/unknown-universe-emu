using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class WreckAsset : Asset
    {
        public WreckAsset(Player destroyedPlayer) : base(destroyedPlayer.Spacemap.GetNextObjectId(), destroyedPlayer.Name, AssetTypes.WRECK, destroyedPlayer.FactionId, destroyedPlayer.Clan, 1, 1, destroyedPlayer.Position, destroyedPlayer.Spacemap, false, false, false)
        {
        }
    }
}

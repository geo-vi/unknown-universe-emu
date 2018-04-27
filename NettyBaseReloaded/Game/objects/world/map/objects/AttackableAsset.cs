using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class AttackableAsset : Asset
    {
        public AttackableAssetCore Core { get; set; }

        public AttackableAsset(int id, string name, AssetTypes type, Faction faction, Clan clan, int designId, int expansionStage, Vector position, Spacemap map, bool invisible, bool visibleOnWarnRadar, bool detectedByWarnRadar, int hp, int maxHp, int shd, int maxShd, int nano, int maxNano, double abs, double pen) : base(id, name, type, faction, clan, designId, expansionStage, position, map, invisible, visibleOnWarnRadar, detectedByWarnRadar)
        {
            Core = new AttackableAssetCore(id, this, hp, maxHp, shd, maxShd, nano, maxNano, abs, pen);
            Core.Destroyed += (sender, asset) => OnDestroyed();
        }
    }
}

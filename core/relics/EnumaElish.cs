using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Relics;

[Pool(typeof(RinRelicPool))]
public sealed class EnumaElish : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;

    public override string PackedIconPath => "FateNightOfTheGalacticRailway/images/relics/EnumaElish.png";
    protected override string PackedIconOutlinePath => "FateNightOfTheGalacticRailway/images/relics/EnumaElish_outline.png";
    protected override string BigIconPath => "FateNightOfTheGalacticRailway/images/relics/big/EnumaElish.png";
}

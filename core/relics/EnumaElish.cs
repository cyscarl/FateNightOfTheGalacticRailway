using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Cards;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Relics;

/// <summary>
/// 天地乖离·开辟之星 — Each time an extra card is drawn (not the initial
/// hand draw), add a KingTreasure to hand (or upgrade the one in hand).
/// </summary>
[Pool(typeof(RinRelicPool))]
public sealed class EnumaElish : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;

    public override string PackedIconPath => "FateNightOfTheGalacticRailway/images/relics/EnumaElish.png";
    protected override string PackedIconOutlinePath => "FateNightOfTheGalacticRailway/images/relics/EnumaElish_outline.png";
    protected override string BigIconPath => "FateNightOfTheGalacticRailway/images/relics/big/EnumaElish.png";

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (Owner == null) return;
        if (card.Owner != Owner) return;
        if (fromHandDraw) return; // initial hand draw is not "extra"

        await KingTreasure.AddToHand(Owner);
    }
}

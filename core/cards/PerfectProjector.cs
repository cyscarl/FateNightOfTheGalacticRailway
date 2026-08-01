using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 完美投影仪 — Copy the effect of the previous card played this turn.
/// </summary>
[Pool(typeof(RinCardPool))]
public class PerfectProjector : CustomCardModel
{
    public PerfectProjector() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { };

    public override string PortraitPath => "PerfectProjector.png".CardPortraitPath();
    public override string CustomPortraitPath => "PerfectProjector.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "PerfectProjector.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null || CombatState == null) return;

        var prev = CombatManager.Instance.History.CardPlaysFinished
            .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                             && e.CardPlay.Card != this
                             && e.HappenedThisTurn(CombatState));
        if (prev == null) return;

        var dupe = prev.CardPlay.Card.CreateDupe();
        await CardCmd.AutoPlay(choiceContext, dupe, null);
    }

    protected override void OnUpgrade() { }
}

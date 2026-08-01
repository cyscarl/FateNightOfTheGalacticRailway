using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace FateNightOfTheGalacticRailway.Core.Relics;

[Pool(typeof(SharedRelicPool))]
public sealed class Excalibur : CustomRelicModel
{
    private int _cardsPlayed;
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;
    public override string PackedIconPath => "FateNightOfTheGalacticRailway/images/relics/Excalibur.png";
    protected override string PackedIconOutlinePath => "FateNightOfTheGalacticRailway/images/relics/Excalibur_outline.png";
    protected override string BigIconPath => "FateNightOfTheGalacticRailway/images/relics/big/Excalibur.png";

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        _cardsPlayed++;
        if (_cardsPlayed >= 8 && Owner != null)
        {
            _cardsPlayed = 0;
            Flash();
            await PlayerCmd.GainEnergy(1m, Owner);
        }
    }
}

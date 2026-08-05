using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using FateNightOfTheGalacticRailway.Core.Powers;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Relics;

[Pool(typeof(RinRelicPool))]
public sealed class FlySafely : CustomRelicModel
{
    private int _cardsPlayed;
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override bool IsAllowed(IRunState runState) => true;
    public override bool ShouldReceiveCombatHooks => true;
    public override string PackedIconPath => "FateNightOfTheGalacticRailway/images/relics/FlySafely.png";
    protected override string PackedIconOutlinePath => "FateNightOfTheGalacticRailway/images/relics/FlySafely_outline.png";
    protected override string BigIconPath => "FateNightOfTheGalacticRailway/images/relics/big/FlySafely.png";

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        _cardsPlayed++;
        if (_cardsPlayed % 10 == 0 && Owner != null)
        {
            Flash();
            var targets = new List<Creature> { Owner.Creature };
            await PowerCmd.Apply<PowerAhaStrikeDamageUp>(choiceContext, targets, 1m, Owner.Creature, null);
        }
    }
}

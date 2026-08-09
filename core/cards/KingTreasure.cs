using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using FateNightOfTheGalacticRailway.Core.Characters;

namespace FateNightOfTheGalacticRailway.Core.Cards;

/// <summary>
/// 王之财宝！ — 1 cost. Retain. Exhaust. Deal 3 AOE.
/// Hand limit: only 1 copy. Any duplicate that would enter the hand merges into
/// the existing one instead, summing their damage (base + base per fresh dupe).
/// Derived card — not in starter pool, not in rewards.
/// </summary>
[Pool(typeof(RinCardPool))]
public class KingTreasure : CustomCardModel
{
    /// <summary>Damage of a freshly created 王之财宝 — also the amount a fresh duplicate merges in.</summary>
    public const decimal BaseDamage = 3m;

    public KingTreasure() : base(1, CardType.Attack, CardRarity.Event, TargetType.AllEnemies)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Retain,
        CardKeyword.Exhaust,
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(BaseDamage, ValueProp.Move)
    };

    public override string PortraitPath => "KingTreasure.png".CardPortraitPath();
    public override string CustomPortraitPath => "KingTreasure.png".BigCardPortraitPath();
    public override string BetaPortraitPath => "KingTreasure.png".CardPortraitPath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState!)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(BaseDamage);
    }

    private static ulong _lastMergeForgeTicks;
    private const ulong MergeForgeThrottleMs = 600;

    /// <summary>
    /// Feedback when a KingTreasure merges and its damage stacks up: a forge sfx plus the
    /// vanilla card-"smith" hit effect on the 王之财宝 card in hand (no model/animation is
    /// created — the smith VFX positions itself on the card and frees itself). Throttled so
    /// rapid batch merges (e.g. 凛的吊坠·财宝 x4) don't stack.
    /// </summary>
    private static void PlayMergeForgeVfx(Player owner, KingTreasure card)
    {
        if (owner == null || owner.Creature.CombatState == null) return;
        if (NCombatRoom.Instance == null) return;

        // Throttle rapid sequential merges to a single forge hit.
        ulong now = Time.GetTicksMsec();
        if (now - _lastMergeForgeTicks < MergeForgeThrottleMs) return;
        _lastMergeForgeTicks = now;

        // 锻造音效（君王之剑锻造参考）
        SfxCmd.Play("event:/sfx/characters/regent/regent_forge");

        // 卡牌上"敲一下"特效（原版卡牌升级锻造特效，自动定位到卡并自删）
        NCard? cardNode = NCombatRoom.Instance.Ui.Hand.GetCard(card);
        if (cardNode == null) return;
        NCardSmithVfx? vfx = NCardSmithVfx.Create(cardNode, playSfx: false);
        if (vfx == null) return;
        NRun.Instance?.GlobalUi.AboveTopBarVfxContainer.AddChildSafely(vfx);
    }

    /// <summary>
    /// Add a KingTreasure to hand. If one already exists, merge a fresh duplicate's
    /// damage into it (damage = sum of both) instead of adding a second copy.
    /// Uses CardCmd.Upgrade for proper UI refresh.
    /// </summary>
    public static async Task AddToHand(Player owner)
    {
        var state = owner.Creature.CombatState;
        if (state == null) return;

        // Re-query hand each call — essential for sequential batch generation
        var existing = PileType.Hand.GetPile(owner)?.Cards.OfType<KingTreasure>().FirstOrDefault();
        if (existing != null)
        {
            existing.DynamicVars.Damage.UpgradeValueBy(BaseDamage);
            PlayMergeForgeVfx(owner, existing);
        }
        else
        {
            var card = state.CreateCard<KingTreasure>(owner);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, owner);
        }
    }

    /// <summary>
    /// Safety net for the "only 1 in hand" stacking rule: whenever this card lands in
    /// the hand while another 王之财宝 is already there — through copy, draw, or any
    /// other means outside <see cref="AddToHand"/> — merge its damage into the one
    /// already in hand (damage = sum of both) and remove this duplicate.
    /// </summary>
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card != this) return;
        var hand = Pile;
        if (hand?.Type != PileType.Hand) return;

        var existing = hand.Cards.OfType<KingTreasure>().FirstOrDefault(t => t != this);
        if (existing == null) return;

        // Damage = sum of both cards.
        existing.DynamicVars.Damage.UpgradeValueBy(DynamicVars.Damage.BaseValue);
        PlayMergeForgeVfx(Owner, existing);
        await CardPileCmd.RemoveFromCombat(this);
    }
}

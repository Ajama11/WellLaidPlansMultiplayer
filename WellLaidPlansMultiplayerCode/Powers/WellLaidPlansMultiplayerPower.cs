using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using WellLaidPlansMultiplayer.WellLaidPlansMultiplayerCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using WellLaidPlansMultiplayer.WellLaidPlansMultiplayerCode.Keywords;

namespace WellLaidPlansMultiplayer.WellLaidPlansMultiplayerCode.Powers;

public class WellLaidPlansMultiplayerPower : CustomPowerModel
{
    public override string CustomPackedIconPath => ImageHelper.GetImagePath("atlases/power_atlas.sprites/well_laid_plans_power.tres");
    public override string CustomBigIconPath => ImageHelper.GetImagePath("powers/well_laid_plans_power.png");
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task BeforeFlushLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || !Hook.ShouldFlush(player.Creature.CombatState!, player)) return Task.CompletedTask;

        foreach (CardModel card in PileType.Hand.GetPile(player).Cards)
        {
            card.AddKeyword(TrackerKeyword.WellLaidPlansTracker);
        }

        return Task.CompletedTask;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player) return;
        
        List<CardModel> possibleCards = [..PileType.Draw.GetPile(player).Cards, ..PileType.Discard.GetPile(player).Cards];

        possibleCards = possibleCards
            .Where(c => c.Keywords.Contains(TrackerKeyword.WellLaidPlansTracker))
            .OrderBy(c => c.Rarity)
            .ThenBy(c => c.Id)
            .ToList();

        IEnumerable<CardModel> selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, possibleCards, player, new CardSelectorPrefs(SelectionScreenPrompt, 0, Amount));

        await CardPileCmd.Add(selectedCards, PileType.Hand);

        foreach (CardModel card in player.PlayerCombatState!.AllCards)
        {
            card.RemoveKeyword(TrackerKeyword.WellLaidPlansTracker);
        }
    }
}
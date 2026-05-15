using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using WellLaidPlansMultiplayer.WellLaidPlansMultiplayerCode.Powers;

namespace WellLaidPlansMultiplayer.WellLaidPlansMultiplayerCode.Cards;

[Pool(typeof(SilentCardPool))]
public class WellLaidPlansMultiplayer() :
    CustomCardModel(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string PortraitPath =>
        ImageHelper.GetImagePath("atlases/card_atlas.sprites/silent/well_laid_plans.tres");
    public override string CustomPortraitPath => PortraitPath;
    public override string BetaPortraitPath => PortraitPath;
    
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WellLaidPlansMultiplayerPower>(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        await CommonActions.ApplySelf<WellLaidPlansMultiplayerPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WellLaidPlansMultiplayerPower>().UpgradeValueBy(1);
    }
}
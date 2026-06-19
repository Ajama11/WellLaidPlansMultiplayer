using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Cards;

namespace WellLaidPlansMultiplayer.WellLaidPlansMultiplayerCode.Patches;

[HarmonyPatch(
    typeof(WellLaidPlans), 
    nameof(WellLaidPlans.MultiplayerConstraint), 
    MethodType.Getter)]
public static class WellLaidPlansMulitplayerConstraintPatch
{
    internal static void Postfix(ref CardMultiplayerConstraint __result)
    {
        __result = CardMultiplayerConstraint.None;
    }
}
using HarmonyLib;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace CombatFriends.Patches {
    [HarmonyPatch(typeof(DefaultSkillLevelingManager), nameof(DefaultSkillLevelingManager.OnGainRelation))]
    internal class RelationGainSkillLevelPatch {
        [HarmonyPrefix]
        private static bool Prefix() {
            if (!GlobalSettings<MCMConfig>.Instance.GiveCharmXP && CombatFriendsBehavior.Instance.RelationGainInProgress)
                return false;

            return true;
        }
    }
}

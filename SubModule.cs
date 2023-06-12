using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CombatFriends {
    public class SubModule : MBSubModuleBase {
        protected override void OnSubModuleLoad() {
            new Harmony("CombatFriends").PatchAll();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter) {
            if (game.GameType is Campaign) {
                CampaignGameStarter campaignStarter = (CampaignGameStarter)gameStarter;

                campaignStarter.AddBehavior(new CombatFriendsBehavior());
            }
        }

        public static void PrintMessage(string str) {
            InformationManager.DisplayMessage(new InformationMessage(str));
        }
    }
}
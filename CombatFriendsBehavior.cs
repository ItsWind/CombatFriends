using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CombatFriends {
    public class CombatFriendsBehavior : CampaignBehaviorBase {
        public static CombatFriendsBehavior Instance;

        public bool RelationGainInProgress = false;

        public CombatFriendsBehavior() {
            Instance = this;
        }

        public override void RegisterEvents() {
            CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, PlayerBattleEnded);
            CampaignEvents.TournamentFinished.AddNonSerializedListener(this, TournamentEnded);
        }

        public override void SyncData(IDataStore dataStore) {
            //
        }

        private void TournamentEnded(CharacterObject winner, List<CharacterObject> participants, Town town, ItemObject reward) {
            if (winner != Hero.MainHero.CharacterObject)
                return;

            // FOR CHARM XP GAIN DISABLING
            RelationGainInProgress = true;

            // Gain relation with all heroes in parties
            foreach (MobileParty party in town.Settlement.Parties) {
                TroopRoster allHeroesInParty = party.Party.MemberRoster.CloneRosterData();
                allHeroesInParty.RemoveIf(x => !x.Character.IsHero);

                foreach (TroopRosterElement troop in allHeroesInParty.GetTroopRoster()) {
                    Hero hero = troop.Character.HeroObject;
                    if (hero == Hero.MainHero)
                        continue;

                    ChangeRelationAction.ApplyPlayerRelation(hero, MCMConfig.Instance.RelationGainTournamentWon, false, MCMConfig.Instance.RelationChangeNotifications);
                }
            }

            // Gain relation with all heroes without a party
            foreach (Hero hero in town.Settlement.HeroesWithoutParty) {
                if (hero == Hero.MainHero)
                    continue;

                ChangeRelationAction.ApplyPlayerRelation(hero, MCMConfig.Instance.RelationGainTournamentWon, false, MCMConfig.Instance.RelationChangeNotifications);
            }

            // FOR CHARM XP GAIN REENABLING
            RelationGainInProgress = false;
        }

        private void PlayerBattleEnded(MapEvent mapEvent) {
            if (mapEvent.WinningSide != mapEvent.PlayerSide)
                return;

            float renownChange = 0f;
            float x = 0f;
            mapEvent.GetBattleRewards(PartyBase.MainParty, out renownChange, out x, out x, out x, out x);

            int relationChange = (int)Math.Round(renownChange * MCMConfig.Instance.RenownToRelationMult);

            // SET RELATION MIN/MAX
            if (relationChange < 0)
                relationChange = 0;
            else if (relationChange > MCMConfig.Instance.RelationChangeMax)
                relationChange = MCMConfig.Instance.RelationChangeMax;

            // FOR CHARM XP GAIN DISABLING
            RelationGainInProgress = true;

            foreach (MapEventParty mapEventParty in mapEvent.GetMapEventSide(mapEvent.PlayerSide).Parties) {
                TroopRoster allHeroesInParty = mapEventParty.Party.MemberRoster.CloneRosterData();
                allHeroesInParty.RemoveIf(x => !x.Character.IsHero);

                foreach (TroopRosterElement troop in allHeroesInParty.GetTroopRoster()) {
                    Hero hero = troop.Character.HeroObject;
                    if (hero == Hero.MainHero)
                        continue;

                    ChangeRelationAction.ApplyPlayerRelation(hero, relationChange, false, GlobalSettings<MCMConfig>.Instance.RelationChangeNotifications);
                }
            }

            // FOR CHARM XP GAIN REENABLING
            RelationGainInProgress = false;
        }
    }
}

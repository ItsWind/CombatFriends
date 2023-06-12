using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatFriends {
	internal sealed class MCMConfig : AttributeGlobalSettings<MCMConfig> {
		public override string Id => "CombatFriends";
		public override string DisplayName => "Combat Friends";
		public override string FolderName => "CombatFriends";
		public override string FormatType => "xml";

		[SettingPropertyBool("Relation Change Notifications", HintText = "Enable/disable relation gain notifications from this mod.", Order = 1, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public bool RelationChangeNotifications { get; set; } = true;

		[SettingPropertyBool("Give Charm XP", HintText = "Enable to reenable charm XP being gained if relation is gained with this mod.", Order = 2, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public bool GiveCharmXP { get; set; } = false;

		[SettingPropertyFloatingInteger("Renown To Relation Multiplier", 0f, 5f, HintText = "Set the multiplier for turning renown to relation.", Order = 3, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public float RenownToRelationMult { get; set; } = 1f;

		[SettingPropertyInteger("Relation Gain Cap", 0, 100, HintText = "Set a cap for the maximum amount of relation to gain.", Order = 4, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public int RelationChangeMax { get; set; } = 25;

		[SettingPropertyInteger("Relation Gain For Tournaments", 0, 100, HintText = "Set the relation gain for winning tournaments.", Order = 5, RequireRestart = false)]
		[SettingPropertyGroup("General")]
		public int RelationGainTournamentWon { get; set; } = 3;
	}
}

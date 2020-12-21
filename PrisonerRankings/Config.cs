using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Interfaces;

namespace PrisonerRankings
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("Also enable the rating system for chaos.")]
        public bool EnableForChaos { get; set; } = false;
        [Description("If set to true, Class-D with 0-star rating cannot be killed by NTF. Does not apply to Chaos Insurgency.")]
        public bool RatingProtection { get; set; } = false;
        [Description("Determines whether or not cuffing a class-d will reset their rating. Does not apply to Chaos Insurgency. If rating protection is also enabled, cuffed class-d cannot be shot.")]
        public bool ResetRatingOnCuff { get; set; } = true;
        [Description("Determines whether or not stars will be displayed above a player's name representing their rating.")]
        public bool DisplayOnCharacter { get; set; } = true;
        [Description("Determines the hint to show upon gaining rating. Set to none to disable.")]
        public string RatingGainHint { get; set; } = "You have gained {amount} rating by {action}.";
        [Description("The message to show to all alive MTF when a player reaches a 3-star rating. Set to none to disable.")]
        public string ThreeRatingBroadcast { get; set; } = "{name} has reached a 3-star rating!";
        [Description("The message to show to all alive MTF when a player reaches a 5-star rating. Set to none to disable.")]
        public string FiveRatingBroadcast { get; set; } = "{name} has reached a 5-star rating and must be terminated immediately!";
        [Description("Sets the message to announce when someone with a rating of 3 or higher is killed. Set to none to disable.")]
        public string TerminationBroadcast { get; set; } = "{name} has been terminated.";
        [Description("Determines the amount of rating that is gained for each of the specified actions.")]
        public float PickupKeycard { get; set; } = 0.2f;
        public float PickupWeapon { get; set; } = 1f;
        public float PickupScpItem { get; set; } = 0.2f;
        public float EnterHcz { get; set; } = 1f;
        public float EnterEz { get; set; } = 1f;
        [Description("Determines the amount of rating that is gained for killing users of certain classes. Add/remove classes to the list to include them (eg. SCPs).")]
        public Dictionary<RoleType, float> ClassRatings { get; set; } = new Dictionary<RoleType, float>
        {
            [RoleType.Scientist] = 0.5f,
            [RoleType.FacilityGuard] = 0.3f,
            [RoleType.NtfCadet] = 0.3f,
            [RoleType.NtfLieutenant] = 0.6f,
            [RoleType.NtfScientist] = 1f,
            [RoleType.NtfCommander] = 2f,
        };
        [Description("Translations for the {action} parameter in the bounty_gain_hint setting.")]
        public Dictionary<string, string> ActionTranslations { get; set; } = new Dictionary<string, string>
        {
            ["PickupKeycard"] = "picking up a keycard",
            ["PickupWeapon"] = "picking up a weapon",
            ["PickupScpItem"] = "picking up an SCP item",
            ["EnterHcz"] = "entering heavy containment zone",
            ["EnterEz"] = "entering entrance zone",
        };

        [Description("Translations for killing certain roles. If roles are added to the class_ratings setting, they must also be added here!")]
        public Dictionary<RoleType, string> KillTranslations { get; set; } = new Dictionary<RoleType, string>
        {
            [RoleType.Scientist] = "killing a Scientist",
            [RoleType.FacilityGuard] = "killing a Facility Guard",
            [RoleType.NtfCadet] = "killing an NTF Cadet",
            [RoleType.NtfLieutenant] = "killing an NTF Lieutenant",
            [RoleType.NtfScientist] = "killing an NTF Scientist",
            [RoleType.NtfCommander] = "killing an NTF Commander",
        };
    }
}

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
        public bool DisplayOnCharacter { get; set; } = true;
        public string ThreeRatingBroadcast { get; set; } = "{name} has reached a 3-star rating!";
        public string FiveRatingBroadcast { get; set; } = "{name} has reached a 5-star rating and must be terminated immediately!";
        [Description("Sets the message to announce when someone with a rating of 3 or higher is killed. Set to none to disable.")]
        public string TerminationBroadcast { get; set; } = "{name} has been terminated.";
        public float PickupKeycard { get; set; } = 0.2f;
        public float PickupWeapon { get; set; } = 1f;
        public float PickupScpItem { get; set; } = 0.2f;
        public float EnterHcz { get; set; } = 1f;
        public float EnterEz { get; set; } = 1f;
        public Dictionary<RoleType, float> ClassRatings { get; set; } = new Dictionary<RoleType, float>
        {
            [RoleType.Scientist] = 0.5f,
            [RoleType.FacilityGuard] = 0.3f,
            [RoleType.NtfCadet] = 0.3f,
            [RoleType.NtfLieutenant] = 0.6f,
            [RoleType.NtfScientist] = 1f,
            [RoleType.NtfCommander] = 2f,
        };
    }
}

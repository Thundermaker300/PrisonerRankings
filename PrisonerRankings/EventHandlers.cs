using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace PrisonerRankings
{
    public class EventHandlers
    {
        private static Dictionary<Player, float> Rating = new Dictionary<Player, float> { };
        public const string EmptyRating = "☆☆☆☆☆";

        public static string GetRatingString(float rating)
        {
            int fullStarCount = (int)Mathf.Floor(rating);
            int emptyStarCount = 5 - fullStarCount;
            string retString = new string('★', fullStarCount);
            if (rating - fullStarCount >= 0.5)
            {
                emptyStarCount--;
                retString += "⯪";
            }
            retString += new string('☆', emptyStarCount);
            return retString;
        }

        public static void RefreshRating(Player Ply, bool Clear = false)
        {
            if (Plugin.Singleton.Config.DisplayOnCharacter == false) return;
            if (Clear)
            {
                if (Rating.ContainsKey(Ply))
                {
                    Rating.Remove(Ply);
                }
                Ply.CustomPlayerInfo = EmptyRating;
            }
            else
            {
                if (!Rating.ContainsKey(Ply))
                {
                    Ply.CustomPlayerInfo = EmptyRating;
                    return;
                }
                Ply.CustomPlayerInfo = GetRatingString(Rating[Ply]);
            }
        }

        public static float GetRating(Player Ply)
        {
            if (!Rating.ContainsKey(Ply))
            {
                return 0f;
            }
            return Rating[Ply];
        }

        public static void SetRating(Player Ply, float Amount)
        {
            Amount = Mathf.Clamp(Amount, 0, 5);
            if (!CanHaveRating(Ply))
            {
                return;
            }
            if (!Rating.ContainsKey(Ply))
            {
                Rating.Add(Ply, Amount);
            }
            else
            {
                Rating[Ply] = Amount;
            }
            // Announcements
            if (Amount == 5 && Plugin.Singleton.Config.FiveRatingBroadcast != "none")
            {
                foreach (Player NtfPly in Player.Get(Team.MTF))
                {
                    NtfPly.Broadcast(5, Plugin.Singleton.Config.FiveRatingBroadcast.Replace("{name}", Ply.Nickname));
                }
            }
            if (Amount >= 3 && Plugin.Singleton.Config.ThreeRatingBroadcast != "none")
            {
                foreach (Player NtfPly in Player.Get(Team.MTF))
                {
                    NtfPly.Broadcast(5, Plugin.Singleton.Config.ThreeRatingBroadcast.Replace("{name}", Ply.Nickname));
                }
            }
            // Refresh
            RefreshRating(Ply);
        }

        public static void AddRating(Player Ply, float Amount, string Action)
        {
            if (Plugin.Singleton.Config.BountyGainHint != "none" && !string.IsNullOrEmpty(Action))
            {
                if (Plugin.Singleton.Config.ActionTranslations.TryGetValue(Action, out string Translation))
                {
                    Ply.ShowHint(Plugin.Singleton.Config.BountyGainHint.Replace("{bounty}", Amount.ToString()).Replace("{action}", Translation), 5);
                }
                else
                {
                    Ply.ShowHint(Plugin.Singleton.Config.BountyGainHint.Replace("{bounty}", Amount.ToString()).Replace("{action}", Action), 5);
                }
            }
            SetRating(Ply, GetRating(Ply) + Amount);
        }

        public static bool CanHaveRating(Player Ply)
            => Ply.Role == RoleType.ClassD || (Ply.Role == RoleType.ChaosInsurgency && Plugin.Singleton.Config.EnableForChaos);

        // Events
        public void ChangingRole(ChangingRoleEventArgs ev)
        {
            RefreshRating(ev.Player, true);
        }

        public void Died(DiedEventArgs ev)
        {
            if (GetRating(ev.Target) >= 3 && Plugin.Singleton.Config.TerminationBroadcast != "none")
            {
                foreach (Player Ply in Player.Get(Team.MTF))
                {
                    Ply.Broadcast(5, Plugin.Singleton.Config.TerminationBroadcast.Replace("{name}", ev.Target.Nickname));
                }
            }

            if (CanHaveRating(ev.Killer) && Plugin.Singleton.Config.ClassRatings.ContainsKey(ev.Target.Role))
            {
                if (!Plugin.Singleton.Config.KillTranslations.TryGetValue(ev.Target.Role, out string value))
                {
                    AddRating(ev.Killer, Plugin.Singleton.Config.ClassRatings[ev.Target.Role], "");
                }
                else
                {
                    AddRating(ev.Killer, Plugin.Singleton.Config.ClassRatings[ev.Target.Role], value);
                }
            }
        }

        public void Hurting(HurtingEventArgs ev)
        {
            if (Plugin.Singleton.Config.RatingProtection && ev.Target.Role == RoleType.ClassD && ev.Attacker.Team == Team.MTF)
            {
                ev.IsAllowed = false;
            }
        }

        public void Handcuffing(HandcuffingEventArgs ev)
        {
            if (Plugin.Singleton.Config.ResetRatingOnCuff && ev.Target.Role == RoleType.ClassD)
            {
                SetRating(ev.Target, 0);
            }
        }

        public void PickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.ItemId.IsWeapon())
            {
                AddRating(ev.Player, Plugin.Singleton.Config.PickupWeapon, "PickupWeapon");
            }
            else if (ev.Pickup.ItemId.IsKeycard())
            {
                AddRating(ev.Player, Plugin.Singleton.Config.PickupKeycard, "PickupKeycard");
            }
            else if (ev.Pickup.ItemId.IsScp())
            {
                AddRating(ev.Player, Plugin.Singleton.Config.PickupScpItem, "PickupScpItem");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace PrisonerRankings.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class Rating : ICommand
    {
        public string Command => "getrating";

        public string[] Aliases => new string[] { "rating" };

        public string Description => "Returns the rating of the targeted user, or sets the rating of a user.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count() < 2)
            {
                response = "Missing get/set and player argument.";
                return false;
            }
            if (arguments.At(0) == "get")
            {
                if (!((CommandSender)sender).CheckPermission("pr.view"))
                {
                    response = "Missing permission pr.view.";
                    return false;
                }
                Player Ply = Player.Get(arguments.At(1));
                if (Ply == null)
                {
                    response = "Invalid player";
                    return false;
                }
                response = $"Rating of {Ply.DisplayNickname ?? Ply.Nickname} is {EventHandlers.GetRating(Ply)}";
                return true;
            }
            else if (arguments.At(0) == "set")
            {
                if (!((CommandSender)sender).CheckPermission("pr.edit"))
                {
                    response = "Missing permission pr.edit.";
                    return false;
                }
                Player Ply = Player.Get(arguments.At(1));
                if (Ply == null)
                {
                    response = "Invalid player";
                    return false;
                }
                if (arguments.Count() < 3)
                {
                    response = "Missing amount argument";
                    return false;
                }
                if (!float.TryParse(arguments.At(2), out float Amount))
                {
                    response = "Invalid amount argument, must be a number.";
                    return false;
                }
                Amount = Mathf.Clamp(Amount, 0, 5);
                EventHandlers.SetRating(Ply, Amount);
                response = $"Set {Ply.DisplayNickname ?? Ply.Nickname} rating to {Amount}.";
                return true;
            }
            else
            {
                response = "First argument must be 'get' or 'set'.";
                return false;
            }
            
        }
    }
}

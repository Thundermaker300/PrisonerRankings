using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerHandler = Exiled.Events.Handlers.Player;

namespace PrisonerRankings
{
    public class Plugin : Plugin<Config>
    {
        private static Lazy<Plugin> _lazy = new Lazy<Plugin>(valueFactory: () => new Plugin());
        private EventHandlers _handler;
        public static Plugin Singleton = _lazy.Value;
        public override void OnEnabled()
        {
            _handler = new EventHandlers();

            // Events
            PlayerHandler.Spawning += _handler.Spawning;
            PlayerHandler.Died += _handler.Died;
            PlayerHandler.Handcuffing += _handler.Handcuffing;
            PlayerHandler.Hurting += _handler.Hurting;
            PlayerHandler.PickingUpItem += _handler.PickingUpItem;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            // Events
            PlayerHandler.Spawning -= _handler.Spawning;
            PlayerHandler.Died -= _handler.Died;
            PlayerHandler.Handcuffing -= _handler.Handcuffing;
            PlayerHandler.Hurting -= _handler.Hurting;
            PlayerHandler.PickingUpItem -= _handler.PickingUpItem;

            _handler = null;
            base.OnDisabled();
        }

        public override string Name => "PrisonerRankings";
        public override string Author => "Thunder";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(2, 1, 19);
        public override PluginPriority Priority => PluginPriority.High;
    }
}

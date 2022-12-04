using System;
using CommandSystem;
using PluginAPI.Core;

namespace SCPDiscord.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class VerboseCommand : ICommand
	{
		public string Command => "scpdiscord_verbose";
		public string[] Aliases => new string[] { "scpd_verbose" };
		public string Description => "Toggles verbose messages.";
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			/*if (sender is Player player)
			{
				if (!player.HasPermission("scpdiscord.validate"))
				{
					return new[] { "You don't have permission to use that command." };
				}
			}*/

			Config.ValidateConfig(SCPDiscord.plugin);
			Language.ValidateLanguageStrings();

			response = "Validation report posted in server console.";
			return true;
		}
	}
}
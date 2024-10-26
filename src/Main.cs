using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace TrickOrTreat;

public class TrickOrTreat : BasePlugin, IPluginConfig<TrickOrTreatConfig>
{
    public override string ModuleName => "CS2-TrickOrTreat";
    public override string ModuleDescription => "Small plugin for halloween";
    public override string ModuleAuthor => "verneri";
    public override string ModuleVersion => "1.0";

    private static readonly Dictionary<CCSPlayerController, DateTime> lastcmdUsage = new();

    public TrickOrTreatConfig Config { get; set; } = new();

    public void OnConfigParsed(TrickOrTreatConfig config)
	{
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        Logger.LogInformation($"Loaded (version {ModuleVersion})");
    }

    [ConsoleCommand("css_tot", "Trick or treat command")]
    [ConsoleCommand("css_treat", "Trick or treat command")]
    [ConsoleCommand("css_trick", "Trick or treat command")]
    [ConsoleCommand("css_trickortreat", "Trick or treat command")]
    public void OnCommandTot(CCSPlayerController? player, CommandInfo command)
    {
        if (!player.IsValid) return;

        if (lastcmdUsage.TryGetValue(player, out DateTime lastUsage))
        {
            TimeSpan timeSinceLastUsage = DateTime.Now - lastUsage;
            if (timeSinceLastUsage.TotalMinutes < Config.Cooldown)
            {
                int minutesRemaining = Config.Cooldown - (int)timeSinceLastUsage.TotalMinutes;
                command.ReplyToCommand(string.Format(Localizer["command.cooldown"], minutesRemaining));
                return;
            }
        }
        lastcmdUsage[player] = DateTime.Now;

        var playerPawn = player.PlayerPawn.Value;
        var money = player.InGameMoneyServices;

        if (player != null)
        {
            Random random = new Random();
            int action = random.Next(13);

            switch (action)
            {
                case 0:
                    player.GiveNamedItem("weapon_deagle");
                    command.ReplyToCommand($"{Localizer["treat.givedeagle"]}");
                    break;

                case 1:
                    player.CommitSuicide(true, false);
                    command.ReplyToCommand($"{Localizer["trick.suicide"]}");
                    break;

                case 2:
                    player.GiveNamedItem("weapon_hegrenade");
                    command.ReplyToCommand($"{Localizer["treat.grenade"]}");
                    break;

                case 3:
                    playerPawn.Health = 120;
                    playerPawn.MaxHealth = 120;
                    Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
                    command.ReplyToCommand($"{Localizer["treat.give20hp"]}");
                    break;

                case 4:
                    player.RemoveWeapons();
                    player.GiveNamedItem("weapon_knife");
                    command.ReplyToCommand($"{Localizer["trick.stripweaponsbutknife"]}");
                    break;

                case 5:
                    player.GiveNamedItem("weapon_ak47");
                    command.ReplyToCommand($"{Localizer["treat.giveak"]}");
                    break;

                case 6:
                    playerPawn.Health = 50;
                    Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
                    command.ReplyToCommand($"{Localizer["trick.take50hp"]}");
                    break;

                case 7:
                    playerPawn.Health = 1;
                    Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
                    command.ReplyToCommand($"{Localizer["trick.take99hp"]}");
                    break;

                case 8:
                    player.GiveNamedItem("weapon_healthshot");
                    command.ReplyToCommand($"{Localizer["treat.givehealthshot"]}");
                    break;

                case 9:
                    command.ReplyToCommand($"{Localizer["trick.nothing"]}");
                    break;

                case 10:
                    money.Account += 5000;
                    Utilities.SetStateChanged(player, "CCSPlayerController_InGameMoneyServices", "m_iAccount");
                    command.ReplyToCommand($"{Localizer["treat.money"]}");
                    break;

                case 11:
                    player.RemoveWeapons();
                    command.ReplyToCommand($"{Localizer["trick.stripweapons"]}");
                    break;

                case 12:
                    playerPawn.VelocityModifier = 1.15f;
                    Utilities.SetStateChanged(player, "CCSPlayerPawn", "m_flVelocityModifier");
                    command.ReplyToCommand($"{Localizer["treat.speed"]}");
                    break;

                default:
                    command.ReplyToCommand($"{Localizer["happy.halloween"]}");
                    break;
            }
        }
    }
}
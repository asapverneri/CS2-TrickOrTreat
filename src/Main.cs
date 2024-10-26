using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using System;

namespace TrickOrTreat;

public class TrickOrTreat : BasePlugin, IPluginConfig<TrickOrTreatConfig>
{
    public override string ModuleName => "CS2-TrickOrTreat";
    public override string ModuleDescription => "Small plugin for halloween";
    public override string ModuleAuthor => "verneri";
    public override string ModuleVersion => "1.1";

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


        if (player != null && playerPawn != null)
        {
            List<int> actions = new List<int>();

            if (Config.TreatDeagle) actions.Add(0); 
            if (Config.TrickSuicide) actions.Add(1);
            if (Config.TreatGrenade) actions.Add(2);
            if (Config.TreatHP) actions.Add(3);
            if (Config.TrickStripweapons) actions.Add(4);
            if (Config.TreatAK47) actions.Add(5);
            if (Config.Trick50HP) actions.Add(6);
            if (Config.Trick99HP) actions.Add(7);
            if (Config.TreatHealthShot) actions.Add(8);
            if (Config.TrickNothing) actions.Add(9);
            if (Config.TreatMoney) actions.Add(10);
            if (Config.TreatSpeed) actions.Add(11);

            Random random = new Random();
            int action = actions[random.Next(actions.Count)];
            //Random random = new Random();
            //int action = random.Next(12);

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
                    playerPawn.Health = Config.TreatHPValue;
                    playerPawn.MaxHealth = Config.TreatHPValue;
                    Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
                    command.ReplyToCommand($"{Localizer["treat.give20hp"]}");
                    break;

                case 4:
                    RemoveWeapons(player);
                    player.GiveNamedItem("weapon_knife");
                    command.ReplyToCommand($"{Localizer["trick.stripweapons"]}");
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
                    AddMoney(player);
                    command.ReplyToCommand($"{Localizer["treat.money", Config.TreatMoneyValue]}");
                    break;

                case 11:
                    playerPawn.VelocityModifier = Config.TreatSpeedValue;
                    Utilities.SetStateChanged(player, "CCSPlayerPawn", "m_flVelocityModifier");
                    command.ReplyToCommand($"{Localizer["treat.speed"]}");
                    break;

                default:
                    command.ReplyToCommand($"{Localizer["happy.halloween"]}");
                    break;
            }
        }
    }

    private static void RemoveWeapons(CCSPlayerController? player)
    {
        if (player == null) return;
        foreach (var weapon in player.PlayerPawn.Value!.WeaponServices!.MyWeapons)
        {
            if (weapon.Value != null)
            {
                if (weapon.Value.DesignerName == "weapon_c4")
                    continue;

                weapon.Value.Remove();
            }
        }
    }

    private void AddMoney(CCSPlayerController player)
    {
        var moneyServices = player.InGameMoneyServices;
        if (moneyServices == null) return;

        moneyServices.Account += Config.TreatMoneyValue;
        Utilities.SetStateChanged(player, "CCSPlayerController", "m_pInGameMoneyServices");
    }
}
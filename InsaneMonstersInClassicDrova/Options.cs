using Drova_Modding_API.Access;
using Drova_Modding_API.UI.Builder;
using Il2Cpp;
using Il2CppCustomFramework.Localization;

namespace InsaneMonstersInClassicDrova
{
    internal static class Options
    {
        private const string ModName = "InsaneMonstersInClassicDrova";
        private const string KeySpawnGroups = "InsaneMonsters_SpawnGroups";
        private const string KeyMovesets = "InsaneMonsters_Movesets";
        private const string KeyAttackTimings = "InsaneMonsters_AttackTimings";

        private static bool _loaded;
        private static bool _spawnGroups = true;
        private static bool _movesets = true;
        private static bool _attackTimings = true;

        // The co-op host's values while a session runs, since different switches make different monsters.
        private static bool? _hostSpawnGroups;
        private static bool? _hostMovesets;
        private static bool? _hostAttackTimings;

        internal static bool SpawnGroups
        {
            get
            {
                EnsureLoaded();
                return _hostSpawnGroups ?? _spawnGroups;
            }
        }

        internal static bool Movesets
        {
            get
            {
                EnsureLoaded();
                return _hostMovesets ?? _movesets;
            }
        }

        internal static bool AttackTimings
        {
            get
            {
                EnsureLoaded();
                return _hostAttackTimings ?? _attackTimings;
            }
        }

        internal static void ShareWithCoopHost()
        {
            CoopAccess.ShareHostSetting(KeySpawnGroups, () => Text(SpawnGroups), value => _hostSpawnGroups = Parse(value), needsReload: true);
            CoopAccess.ShareHostSetting(KeyMovesets, () => Text(Movesets), value => _hostMovesets = Parse(value), needsReload: true);
            CoopAccess.ShareHostSetting(KeyAttackTimings, () => Text(AttackTimings), value => _hostAttackTimings = Parse(value));
        }

        private static string Text(bool value) => value ? "1" : "0";

        private static bool? Parse(string? value) => value == null ? null : value == "1";

        internal static void BuildPanel()
        {
            OptionUIBuilder builder = OptionMenuAccess.Instance.GetBuilder("InsaneMonsters");
            if (builder == null)
            {
                return;
            }
            builder
                .CreateTitle(L("Title"))
                .CreateDisclaimer(L("Disclaimer"))
                .CreateSwitch(L("SpawnGroups"), L("On"), L("Off"), KeySpawnGroups, true)
                .CreateSwitch(L("Movesets"), L("On"), L("Off"), KeyMovesets, true)
                .CreateSwitch(L("AttackTimings"), L("On"), L("Off"), KeyAttackTimings, true)
                .Build();
        }

        internal static void Refresh()
        {
            if (ConfigAccessor.TryGetConfigValue(KeySpawnGroups, out bool spawnGroups))
            {
                _spawnGroups = spawnGroups;
            }
            if (ConfigAccessor.TryGetConfigValue(KeyMovesets, out bool movesets))
            {
                _movesets = movesets;
            }
            if (ConfigAccessor.TryGetConfigValue(KeyAttackTimings, out bool attackTimings))
            {
                _attackTimings = attackTimings;
            }
            _loaded = true;
        }

        internal static void CreateLocalization()
        {
            LocalizationAccess.CreateLocalizationEntries([
                new LocalizationAccess.LocalizationEntry("Title", "Insane Monsters in Classic", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("Disclaimer", "Spawn groups and movesets apply after loading a savegame, attack timings right away.", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("SpawnGroups", "Insane spawn groups", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("Movesets", "Insane movesets & skills", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("AttackTimings", "Insane attack timings", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("On", "On", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("Off", "Off", LocalizationDB.ELanguage.en),
                new LocalizationAccess.LocalizationEntry("Title", "Insane Monsters in Classic", LocalizationDB.ELanguage.de),
                new LocalizationAccess.LocalizationEntry("Disclaimer", "Spawngruppen und Movesets wirken nach dem Laden eines Spielstands, Angriffstimings sofort.", LocalizationDB.ELanguage.de),
                new LocalizationAccess.LocalizationEntry("SpawnGroups", "Insane-Spawngruppen", LocalizationDB.ELanguage.de),
                new LocalizationAccess.LocalizationEntry("Movesets", "Insane-Movesets & Fähigkeiten", LocalizationDB.ELanguage.de),
                new LocalizationAccess.LocalizationEntry("AttackTimings", "Insane-Angriffstimings", LocalizationDB.ELanguage.de),
                new LocalizationAccess.LocalizationEntry("On", "An", LocalizationDB.ELanguage.de),
                new LocalizationAccess.LocalizationEntry("Off", "Aus", LocalizationDB.ELanguage.de)
            ], ModName);
        }

        private static void EnsureLoaded()
        {
            if (!_loaded)
            {
                Refresh();
            }
        }

        private static LocalizedString L(string key)
        {
            return LocalizationAccess.GetLocalizedString(ModName, key);
        }
    }
}

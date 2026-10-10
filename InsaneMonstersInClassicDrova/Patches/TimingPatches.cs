using Il2CppDrova.ActorActions;
using Timing = Il2CppDrova.ActorActions.AA_AttackAction_Settings.Timing;

namespace InsaneMonstersInClassicDrova.Patches
{
    internal static class TimingPatches
    {
        private static readonly Dictionary<IntPtr, (Timing Base, List<Timing> Combos)> Originals = new();

        internal static void Install(HarmonyLib.Harmony harmony)
        {
            PatchInstaller.TryPatch(harmony, typeof(AA_AttackAction_Settings), nameof(AA_AttackAction_Settings.GetTimings), typeof(TimingPatches),
                prefix: nameof(GetTimingsPrefix));
            PatchInstaller.TryPatch(harmony, typeof(AA_AttackAction_Settings), nameof(AA_AttackAction_Settings.GetTiming), typeof(TimingPatches),
                prefix: nameof(GetTimingsPrefix));
            PatchInstaller.TryPatch(harmony, typeof(AA_AttackAction_Settings), nameof(AA_AttackAction_Settings.GetTimingOrNull), typeof(TimingPatches),
                prefix: nameof(GetTimingsPrefix));
            PatchInstaller.TryPatch(harmony, typeof(AA_ATrigger), nameof(AA_ATrigger.SetValuesFromAttackSettings), typeof(TimingPatches),
                prefix: nameof(SetValuesFromAttackSettingsPrefix));
        }

        private static void GetTimingsPrefix(AA_AttackAction_Settings __instance)
        {
            if (PatchInstaller.Is<AA_AttackAction_Settings>(__instance))
            {
                Apply(__instance);
            }
        }

        private static void SetValuesFromAttackSettingsPrefix(AA_AttackAction_Settings settings)
        {
            if (PatchInstaller.Is<AA_AttackAction_Settings>(settings))
            {
                Apply(settings);
            }
        }

        private static void Apply(AA_AttackAction_Settings settings)
        {
            IntPtr pointer = settings.Pointer;
            if (ModeGate.AttackTimingsActive())
            {
                if (Originals.ContainsKey(pointer))
                {
                    return;
                }
                Il2CppSystem.Collections.Generic.List<Timing> insaneTimings = settings._insaneTimings;
                if (insaneTimings == null || insaneTimings.Count == 0)
                {
                    return;
                }
                List<Timing> originalCombos = new();
                Il2CppSystem.Collections.Generic.List<Timing> combos = settings._comboTimings;
                if (combos != null)
                {
                    foreach (Timing timing in combos)
                    {
                        originalCombos.Add(timing);
                    }
                }
                Originals[pointer] = (settings._baseTimings, originalCombos);
                settings._baseTimings = insaneTimings[0];
                if (combos != null)
                {
                    for (int i = 0; i < combos.Count; i++)
                    {
                        combos[i] = insaneTimings[Math.Min(i + 1, insaneTimings.Count - 1)];
                    }
                }
            }
            else if (Originals.TryGetValue(pointer, out var original))
            {
                settings._baseTimings = original.Base;
                Il2CppSystem.Collections.Generic.List<Timing> combos = settings._comboTimings;
                if (combos != null)
                {
                    for (int i = 0; i < combos.Count && i < original.Combos.Count; i++)
                    {
                        combos[i] = original.Combos[i];
                    }
                }
                Originals.Remove(pointer);
            }
        }
    }
}

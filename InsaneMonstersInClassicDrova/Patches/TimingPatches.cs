using System;
using System.Collections.Generic;
using Il2CppDrova.ActorActions;

namespace InsaneMonstersInClassicDrova
{
    internal static class TimingPatches
    {
        private static readonly Dictionary<IntPtr, (AA_AttackAction_Settings.Timing Base, List<AA_AttackAction_Settings.Timing> Combos)> Originals = new();

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
                var insaneTimings = settings._insaneTimings;
                if (insaneTimings == null || insaneTimings.Count == 0)
                {
                    return;
                }
                List<AA_AttackAction_Settings.Timing> originalCombos = new();
                var combos = settings._comboTimings;
                if (combos != null)
                {
                    for (int i = 0; i < combos.Count; i++)
                    {
                        originalCombos.Add(combos[i]);
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
                var combos = settings._comboTimings;
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

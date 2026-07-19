using Il2CppDrova.ABI;
using Il2CppDrova.ActorActions;

namespace InsaneMonstersInClassicDrova
{
    internal static class AbilityPatches
    {
        internal static void Install(HarmonyLib.Harmony harmony)
        {
            PatchInstaller.TryPatch(harmony, typeof(AA_Trigger_Ability), nameof(AA_Trigger_Ability.TriggerInternal), typeof(AbilityPatches),
                prefix: nameof(TriggerAbilityPrefix));
            PatchInstaller.TryPatch(harmony, typeof(AA_Trigger_RandAbility), nameof(AA_Trigger_RandAbility.TriggerInternal), typeof(AbilityPatches),
                prefix: nameof(TriggerRandAbilityPrefix));
            PatchInstaller.TryPatch(harmony, typeof(AA_Trigger_RandAbility), nameof(AA_Trigger_RandAbility.TriggerAbilities), typeof(AbilityPatches),
                prefix: nameof(TriggerAbilitiesPrefix));
            PatchInstaller.TryPatch(harmony, typeof(ABI_Bhvr_TriggerAbility_OnEnd), nameof(ABI_Bhvr_TriggerAbility_OnEnd.SpawnEndAbility), typeof(AbilityPatches),
                prefix: nameof(SpawnEndAbilityPrefix), finalizer: nameof(SpawnEndAbilityFinalizer));
        }

        private static void TriggerAbilityPrefix(AA_Trigger_Ability __instance)
        {
            if (ModeGate.MovesetsActive() && __instance._insaneOnly)
            {
                __instance._insaneOnly = false;
            }
        }

        private static void TriggerRandAbilityPrefix(AA_Trigger_RandAbility __instance)
        {
            if (ModeGate.MovesetsActive() && __instance._insaneOnly)
            {
                __instance._insaneOnly = false;
            }
        }

        private static void TriggerAbilitiesPrefix(ref bool isInsane)
        {
            if (!isInsane && ModeGate.MovesetsActive())
            {
                isInsane = true;
            }
        }

        private static void SpawnEndAbilityPrefix(out bool __state)
        {
            __state = ModeGate.SpawnGroupsActive();
            if (__state)
            {
                GameModePatches.EnterSpoof();
            }
        }

        private static void SpawnEndAbilityFinalizer(bool __state)
        {
            if (__state)
            {
                GameModePatches.ExitSpoof();
            }
        }
    }
}

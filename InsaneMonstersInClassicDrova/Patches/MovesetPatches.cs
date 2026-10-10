using Il2CppDrova;
using Il2CppDrova.ActorActions;
using Il2CppDrova.AI.Graphs;
using Il2CppDrova.AI.GroupBhvrs;
using Il2CppDrova.AI.Modules;

namespace InsaneMonstersInClassicDrova.Patches
{
    internal static class MovesetPatches
    {
        internal static void Install(HarmonyLib.Harmony harmony)
        {
            PatchInstaller.TryPatch(harmony, typeof(AIGraphsBehaviours), nameof(AIGraphsBehaviours.Awake), typeof(MovesetPatches),
                prefix: nameof(AIGraphsAwakePrefix));
            PatchInstaller.TryPatch(harmony, typeof(InsaneModeCondition), nameof(InsaneModeCondition.OnCheck), typeof(MovesetPatches),
                postfix: nameof(InsaneConditionPostfix));
            PatchInstaller.TryPatch(harmony, typeof(AIGroupBhvr_Stalker), nameof(AIGroupBhvr_Stalker.Awake), typeof(MovesetPatches),
                prefix: nameof(StalkerAwakePrefix));
            PatchInstaller.TryPatch(harmony, typeof(ActorStance_PeriodicAbility), nameof(ActorStance_PeriodicAbility.Awake), typeof(MovesetPatches),
                prefix: nameof(PeriodicAbilityAwakePrefix));
            PatchInstaller.TryPatch(harmony, typeof(ActorABITrace), nameof(ActorABITrace.Start), typeof(MovesetPatches),
                prefix: nameof(AbiTraceStartPrefix));
            PatchInstaller.TryPatch(harmony, typeof(HealthBhvr_Bloodquito_Kamikaze), nameof(HealthBhvr_Bloodquito_Kamikaze.GetTargetPrefab), typeof(MovesetPatches),
                postfix: nameof(KamikazePrefabPostfix));
        }

        private static void AIGraphsAwakePrefix(AIGraphsBehaviours __instance)
        {
            if (!PatchInstaller.Is<AIGraphsBehaviours>(__instance) || !ModeGate.MovesetsActive())
            {
                return;
            }
            if (__instance._insaneCombatGraph)
            {
                __instance._overrideCombatGraph = __instance._insaneCombatGraph;
            }
            if (__instance._insanePreAggroGraph)
            {
                __instance._overridePreAggroGraph = __instance._insanePreAggroGraph;
            }
            if (__instance._insanePrepareCombatGraph)
            {
                __instance._overridePrepareCombatGraph = __instance._insanePrepareCombatGraph;
            }
            if (__instance._insaneIdleGraph)
            {
                __instance._overrideIdleGraph = __instance._insaneIdleGraph;
            }
        }

        private static void InsaneConditionPostfix(ref bool __result)
        {
            if (!__result && ModeGate.MovesetsActive())
            {
                __result = true;
            }
        }

        private static void StalkerAwakePrefix(AIGroupBhvr_Stalker __instance)
        {
            if (!PatchInstaller.Is<AIGroupBhvr_Stalker>(__instance) || !ModeGate.MovesetsActive() || !__instance._overrideValuesForInsane)
            {
                return;
            }
            __instance._attackTimer = __instance._attackTimerInsane;
            __instance._timeBetweenAttacks = __instance._timeBetweenAttacksInsane;
            __instance._attackAmount = __instance._attackAmountInsane;
            __instance._overrideValuesForInsane = false;
        }

        private static void PeriodicAbilityAwakePrefix(ActorStance_PeriodicAbility __instance)
        {
            if (PatchInstaller.Is<ActorStance_PeriodicAbility>(__instance) && ModeGate.MovesetsActive() && __instance._insaneOnly)
            {
                __instance._insaneOnly = false;
            }
        }

        private static void AbiTraceStartPrefix(ActorABITrace __instance)
        {
            if (PatchInstaller.Is<ActorABITrace>(__instance) && ModeGate.MovesetsActive() && __instance._onInsaneOnly)
            {
                __instance._onInsaneOnly = false;
            }
        }

        private static void KamikazePrefabPostfix(HealthBhvr_Bloodquito_Kamikaze __instance, ref AA_ABase __result)
        {
            if (PatchInstaller.Is<HealthBhvr_Bloodquito_Kamikaze>(__instance) && ModeGate.MovesetsActive() && __instance._hasInsaneVariant && __instance._insanePrefab)
            {
                __result = __instance._insanePrefab;
            }
        }
    }
}

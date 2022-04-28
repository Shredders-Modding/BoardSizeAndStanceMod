using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;
using MelonLoader;
using Lirp;

namespace BoardSizeAndStanceMod
{
    [HarmonyPatch(typeof(Lirp.SnowboardController), "Show")]
    class SnowboardControllerPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Lirp.SnowboardController __instance)
        {
            try
            {
                //MelonLogger.Msg($"Patch called: SnowboardSounds.OnLand\n__instance: {__instance.ToString()}\n");
                //MelonLogger.Msg($"OnlAnd hooked from script {__originalMethod}");
                //ModLogger.Log($"Hooked method {__originalMethod.Name}");
                if (__instance == ModManager.userSession.sc && !ModManager.instance.gameplayRider)
                {
                    ModManager.instance.gameplayRider = __instance.gameObject;
                    //MelonLogger.Msg("User rider registered");
                    if (ModManager.instance.GetGameBoardGameObjects())
                    {
                        ModManager.instance.InitBoardSizeValues();
                        if (!BindingsManager.instance.areGameplayBindingsRegistered)
                            BindingsManager.instance.SetupGameplayBindings();
                        BindingsManager.instance.InitBindings();
                        ModManager.instance.InitBoardOverTime(100);
                        //MelonLogger.Msg("Gameplay Objects registered");
                    }
                    //else MelonLogger.Msg("Can't register all gameplay Objects");
                }
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Lirp.PreviewCharacter), "Exit")]
    class PreviewCharacterPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Lirp.PreviewCharacter __instance)
        {
            try
            {
                if (ModManager.instance.areGameplayObjectsRegistered)
                {
                    ModLogger.Log("PreviewExit");
                    //ModManager.instance.ResetBoardBonesScale();
                    //ModManager.instance.InitBoardSizeValues();
                    BindingsManager.instance.InitBindings();
                    BindingsManager.instance.ResetBindingsUpdaters();
                }
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Lirp.PreviewCharacter), "SetRiderToConfig")]
    class PreviewCharacterPatcher2
    {
        [HarmonyPrefix]
        public static void Prefix(System.Reflection.MethodBase __originalMethod, Lirp.PreviewCharacter __instance)
        {
            try
            {
                ModLogger.Log("SetRiderToConfig");
                //ModManager.instance.ResetBoardBonesScale();
                //ModManager.instance.gameplayBoardEquipement.ToLocalScale = new Vector3(1, 1, 1);
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(EditCharacterGUI), "DoBack")]
    class EditCharacterGUIPatcher_DoBack
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Lirp.ActionGrab __instance)
        {
            try
            {
                ModLogger.Log($"Hooked method {__originalMethod.Name}");
                ModManager.instance.ReloadStructureOverTime(100);
                BindingsManager.instance.ResetBindingsUpdaters();
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(EditCharacterGUI), "OnShow")]
    class EditCharacterGUIPatcher_OnShow
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Lirp.ActionGrab __instance)
        {
            try
            {
                ModLogger.Log($"Hooked method {__originalMethod.Name}");
                //ModManager.instance.ReloadStructureOverTime(100);
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    /*
    [HarmonyPatch(typeof(PreviewCharacter), "Init")]
    class EditCharacterGUIPatcher_Init
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Lirp.ActionGrab __instance)
        {
            try
            {
                ModLogger.Log($"Hooked method {__originalMethod.Name}");
                //ModManager.instance.ResetBoardBonesScale();
                //ModManager.instance.ReloadStructureOverTime(100);
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }*/

    /*
    [HarmonyPatch(typeof(Lirp.SnowboardEquipment), "GenerateBones")]
    class SnowboardEquipmentGenerateBonesPatcher
    {
        [HarmonyPrefix]
        public static void Prefix(System.Reflection.MethodBase __originalMethod, Lirp.SnowboardController __instance)
        {
            try
            {
                ModLogger.Log("SnowboardEquipment GenerateBones");
                BindingsManager.instance.ResetBindingsUpdaters();
                ModManager.instance.InitBoardSizeValues();
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }
    */

    [HarmonyPatch(typeof(Lirp.SnowboardController), "StartNewRide")]
    class SnowboardEquipmentPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Lirp.SnowboardController __instance)
        {
            try
            {
                ModLogger.Log("SnowboardController StartNewRide");
                if (!BindingsManager.instance.areGameplayBindingsRegistered)
                    BindingsManager.instance.SetupGameplayBindings();
                BindingsManager.instance.InitBindings();
                BindingsManager.instance.ResetBindingsUpdaters();
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(PlayerRigReplayManager), "SetupReplays")]
    class ReplayManagerShowPlayerPatch
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, PlayerRigReplayManager __instance)
        {
            try
            {
                ModLogger.Log("Replay started");
                if (!ModManager.instance.replayRider)
                {
                    ModManager.instance.replayRider = PlayerRigReplayManager._instance._instantiatedRider.scGO;
                    if (ModManager.instance.GetReplayBoardGameObjects())
                    {
                        ModManager.instance.InitBoardOverTime(100);
                        BindingsManager.instance.InitBindings();
                        BindingsManager.instance.ResetBindingsUpdaters();
                    }
                }
                else if (ModManager.instance.areReplayObjectsRegistered)
                {
                    ModManager.instance.InitBoardOverTime(100);
                    BindingsManager.instance.InitBindings();
                    BindingsManager.instance.ResetBindingsUpdaters();
                }
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(PlayerRigReplayManager), "DisableReplays")]
    class ReplayerStartPatch
    {
        [HarmonyPostfix]
        public static void Postfix(System.Reflection.MethodBase __originalMethod, Replayer __instance)
        {
            try
            {
                //ModLogger.Log("TEST LAAAAAAAAA DisableReplays");
                BindingsManager.instance.ResetBindingsUpdaters();
            }
            catch (System.Exception ex)
            {
                //MelonLogger.Msg($"Exception in patch of SnowboardSounds.OnLand:\n{ex}");
            }
        }
    }
}

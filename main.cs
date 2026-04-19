using HarmonyLib;
using Il2CppRUMBLE.Managers;
using Il2CppRUMBLE.Players;
using Il2CppRUMBLE.Players.Subsystems;
using Il2CppRUMBLE.Poses;
using MelonLoader;
using UIFramework;

namespace MoveRestrictor
{
    public static class BuildInfo
    {
        public const string ModName = "MoveRestrictor";
        public const string ModVersion = "2.3.0";
        public const string Author = "UlvakSkillz";
    }
    public class Main : MelonMod
    {
        private static List<PoseInputSource> storedMoves = new List<PoseInputSource>();

        public override void OnInitializeMelon()
        {
            Preferences.InitPrefs();
            UI.Register((MelonBase)this, Preferences.SettingsCategory, Preferences.StructuresCategory, Preferences.ModifiersCategory, Preferences.MovementCategory).OnModSaved += Save;
        }

        public static void Save()
        {
            if (Preferences.AnyPrefsChanged())
            {
                ChangeAvailableMoveSets(PlayerManager.instance.localPlayer.Controller.PlayerPoseSystem);
                Preferences.StoreLastSavedPrefs();
            }
        }

        [HarmonyPatch(typeof(PlayerPoseSystem), nameof(PlayerPoseSystem.Initialize), new Type[] { typeof(PlayerController)})]
        public static class PlayerPoseSystemInitializePatch
        {
            private static void Postfix(ref PlayerPoseSystem __instance, PlayerController controller)
            {
                if (controller.controllerType == ControllerType.Local) { storedMoves.Clear(); ChangeAvailableMoveSets(__instance); }
            }
        }

        public static void ChangeAvailableMoveSets(PlayerPoseSystem __instance)
        {
            List<string> movesToKeep = new();
            if (!Preferences.PrefSprintBlocked.Value) { movesToKeep.Add("SprintingPoseSet"); }
            if (!Preferences.PrefDiscBlocked.Value) { movesToKeep.Add("PoseSetDisc"); }
            if (!Preferences.PrefPillarBlocked.Value) { movesToKeep.Add("PoseSetSpawnPillar"); }
            if (!Preferences.PrefStraightBlocked.Value) { movesToKeep.Add("PoseSetStraight"); }
            if (!Preferences.PrefBallBlocked.Value) { movesToKeep.Add("PoseSetBall"); }
            if (!Preferences.PrefKickBlocked.Value) { movesToKeep.Add("PoseSetKick"); }
            if (!Preferences.PrefStompBlocked.Value) { movesToKeep.Add("PoseSetStomp"); }
            if (!Preferences.PrefWallBlocked.Value) { movesToKeep.Add("PoseSetWall_Grounded"); }
            if (!Preferences.PrefJumpBlocked.Value) { movesToKeep.Add("PoseSetRockjump"); }
            if (!Preferences.PrefUppercutBlocked.Value) { movesToKeep.Add("PoseSetUppercut"); }
            if (!Preferences.PrefCubeBlocked.Value) { movesToKeep.Add("PoseSetSpawnCube"); }
            if (!Preferences.PrefDashBlocked.Value) { movesToKeep.Add("PoseSetDash"); }
            if (!Preferences.PrefParryBlocked.Value) { movesToKeep.Add("PoseSetParry"); }
            if (!Preferences.PrefHoldLeftBlocked.Value) { movesToKeep.Add("PoseSetHoldLeft"); }
            if (!Preferences.PrefHoldRightBlocked.Value) { movesToKeep.Add("PoseSetHoldRight"); }
            if (!Preferences.PrefExplodeBlocked.Value) { movesToKeep.Add("PoseSetExplode"); }
            if (!Preferences.PrefFlickBlocked.Value) { movesToKeep.Add("PoseSetFlick"); }
            Il2CppSystem.Collections.Generic.List<PoseInputSource> activePoses = __instance.currentInputPoses;
            string poseList = "";
            for (int i = 0; i < activePoses.Count; i++)
            {
                if (!movesToKeep.Contains(activePoses[i].poseSet.name))
                {
                    poseList += (poseList != "" ? ", " : "") + activePoses[i].poseSet.name;
                    storedMoves.Add(activePoses[i]);
                    activePoses.RemoveAt(i);
                    i--;
                    break;
                }
            }
            if (poseList != "") { Melon<Main>.Logger.Msg("Poses Removed: " + poseList); }
            poseList = "";
            for (int x = 0; x < storedMoves.Count; x++)
            {
                if (movesToKeep.Contains(storedMoves[x].poseSet.name))
                {
                    poseList += (poseList != "" ? ", " : "") + storedMoves[x].poseSet.name;
                    activePoses.Add(storedMoves[x]);
                    storedMoves.RemoveAt(x);
                    x--;
                    break;
                }
            }
            if (poseList != "") { Melon<Main>.Logger.Msg("Poses Added: " + poseList); }
        }
    }
}

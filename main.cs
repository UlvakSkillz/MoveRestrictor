using MelonLoader;
using RUMBLE.Managers;
using RUMBLE.Players.Subsystems;
using RUMBLE.Poses;
using System.Collections;
using UnityEngine;
using RumbleModUI;

namespace MoveRestrictor
{
    public class main : MelonMod
    {
        private bool sceneChanged = false;
        private string currentScene = "Loader";
        private int sceneChangeCount = 0;
        UI UI = RumbleModUIClass.UI_Obj;
        private Mod MoveRestrictor = new Mod();
        private bool movesSettable = false;
        private object checkPosesCoroutine;
        private bool timeToReadFile = true;

        public override void OnLateInitializeMelon()
        {
            MoveRestrictor.ModName = "MoveRestrictor";
            MoveRestrictor.ModVersion = "1.1.1";
            MoveRestrictor.SetFolder("MoveRestrictor");
            MoveRestrictor.AddToList("Description", ModSetting.AvailableTypes.Description, "", "Disables Specific Moves");
            MoveRestrictor.AddToList("Sprint", true, 0, "Grey Box Turns Off Sprint");
            MoveRestrictor.AddToList("Disc", true, 0, "Grey Box Turns Off Disc");
            MoveRestrictor.AddToList("Pillar", true, 0, "Grey Box Turns Off Pillar");
            MoveRestrictor.AddToList("Straight", true, 0, "Grey Box Turns Off Straight");
            MoveRestrictor.AddToList("Ball", true, 0, "Grey Box Turns Off Ball");
            MoveRestrictor.AddToList("Kick", true, 0, "Grey Box Turns Off Kick");
            MoveRestrictor.AddToList("Stomp", true, 0, "Grey Box Turns Off Stomp");
            MoveRestrictor.AddToList("Wall", true, 0, "Grey Box Turns Off Wall");
            MoveRestrictor.AddToList("Jump", true, 0, "Grey Turns Off Jump");
            MoveRestrictor.AddToList("Uppercut", true, 0, "Grey Box Turns Off Uppercut");
            MoveRestrictor.AddToList("Cube", true, 0, "Grey Box Turns Off Cube");
            MoveRestrictor.AddToList("Dash", true, 0, "Grey Box Turns Off Dash");
            MoveRestrictor.AddToList("Parry", true, 0, "Grey Box Turns Off Parry");
            MoveRestrictor.AddToList("Hold Left", true, 0, "Grey Box Turns Off Left Hand Hold");
            MoveRestrictor.AddToList("Hold Right", true, 0, "Grey Box Turns Off Right Hand Hold");
            MoveRestrictor.AddToList("Explode", true, 0, "Grey Box Turns Off Explode");
            MoveRestrictor.AddToList("Flick", true, 0, "Grey Box Turns Off Flick");
            MoveRestrictor.GetFromFile();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            sceneChanged = true;
            movesSettable = false;
            sceneChangeCount++;
        }

        public override void OnFixedUpdate()
        {
            if (UI.GetInit() && !MoveRestrictor.GetUIStatus())
            {
                UI.AddMod(MoveRestrictor);
            }
            if (sceneChanged)
            {
                if (currentScene != "Loader")
                {
                    checkPosesCoroutine = MelonCoroutines.Start(WaitThenCheckPoses(sceneChangeCount));
                }
                sceneChanged = false;
            }
            if (movesSettable && (MoveRestrictor.GetSaveStatus() || timeToReadFile))
            {
                MoveRestrictor.ConfirmSave();
                timeToReadFile = false;
                ChangeAvailableMoveSets();
            }
        }

        public IEnumerator WaitThenCheckPoses(int sceneCount)
        {
            for (int i = 0; i < 300; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            while (PlayerManager.instance.localPlayer.Controller == null)
            {
                yield return new WaitForFixedUpdate();
            }
            bool enabled = PlayerManager.instance.localPlayer.Controller.gameObject.transform.GetComponentInChildren<PlayerPoseSystem>().enabled;
            while (!enabled)
            {
                if (sceneCount != sceneChangeCount) { break; }
                enabled = PlayerManager.instance.localPlayer.Controller.gameObject.transform.GetComponentInChildren<PlayerPoseSystem>().enabled;
                yield return new WaitForFixedUpdate();
            }
            if (enabled)
            {
                movesSettable = true;
            }
            MoveRestrictor.GetFromFile();
            timeToReadFile = true;
            MelonCoroutines.Stop(checkPosesCoroutine);
        }

        public void ChangeAvailableMoveSets()
        {
            System.Collections.Generic.List<string> movesToKeep = new System.Collections.Generic.List<string>();
            for (int i = 1; i < MoveRestrictor.Settings.Count; i++)
            {
                if ((bool)MoveRestrictor.Settings[i].SavedValue)
                {
                    switch (i)
                    {
                        case 1:
                            movesToKeep.Add("SprintingPoseSet");
                            break;
                        case 2:
                            movesToKeep.Add("PoseSetDisc");
                            break;
                        case 3:
                            movesToKeep.Add("PoseSetSpawnPillar");
                            break;
                        case 4:
                            movesToKeep.Add("PoseSetStraight");
                            break;
                        case 5:
                            movesToKeep.Add("PoseSetBall");
                            break;
                        case 6:
                            movesToKeep.Add("PoseSetKick");
                            break;
                        case 7:
                            movesToKeep.Add("PoseSetStomp");
                            break;
                        case 8:
                            movesToKeep.Add("PoseSetWall_Grounded");
                            break;
                        case 9:
                            movesToKeep.Add("PoseSetRockjump");
                            break;
                        case 10:
                            movesToKeep.Add("PoseSetUppercut");
                            break;
                        case 11:
                            movesToKeep.Add("PoseSetSpawnCube");
                            break;
                        case 12:
                            movesToKeep.Add("PoseSetDash");
                            break;
                        case 13:
                            movesToKeep.Add("PoseSetParry");
                            break;
                        case 14:
                            movesToKeep.Add("PoseSetHoldLeft");
                            break;
                        case 15:
                            movesToKeep.Add("PoseSetHoldRight");
                            break;
                        case 16:
                            movesToKeep.Add("PoseSetExplode");
                            break;
                        case 17:
                            movesToKeep.Add("PoseSetFlick");
                            break;
                    }
                }
            }
            Il2CppSystem.Collections.Generic.List<PoseInputSource> activePoses = PlayerManager.instance.localPlayer.Controller.gameObject.transform.GetComponentInChildren<PlayerPoseSystem>().currentInputPoses;
            string poseList = "";
            for (int i = 0; i < activePoses.Count; i++)
            {
                bool poseFound = false;
                for (int x = 0; x < movesToKeep.Count; x++)
                {
                    if (activePoses[i].poseSet.name == movesToKeep[x])
                    {
                        poseFound = true;
                        break;
                    }
                }
                if (!poseFound)
                {
                    if (poseList != "")
                    {
                        poseList += ", ";
                    }
                    poseList += activePoses[i].poseSet.name;
                    activePoses.RemoveAt(i);
                    i--;
                }
            }
            if (poseList == "")
            {
                poseList = "None";
            }
            MelonLogger.Msg("Poses Removed: " + poseList);
        }
    }
}

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
        UI UI = UI.instance;
        private Mod MoveRestrictor = new Mod();
        private bool movesSettable = false;
        private object checkPosesCoroutine;
        private bool timeToReadFile = true;
        private Il2CppSystem.Collections.Generic.List<PoseInputSource> storedMoves = new Il2CppSystem.Collections.Generic.List<PoseInputSource>();

        public override void OnLateInitializeMelon()
        {
            MoveRestrictor.ModName = "MoveRestrictor";
            MoveRestrictor.ModVersion = "1.3.2";
            MoveRestrictor.SetFolder("MoveRestrictor");
            MoveRestrictor.AddDescription("Description", "Description", "Disables Specific Moves", new Tags { IsSummary = true });
            MoveRestrictor.AddToList("Sprint", true, 0, "Grey Box Turns Off Sprint", new Tags { });
            MoveRestrictor.AddToList("Disc", true, 0, "Grey Box Turns Off Disc", new Tags { });
            MoveRestrictor.AddToList("Pillar", true, 0, "Grey Box Turns Off Pillar", new Tags { });
            MoveRestrictor.AddToList("Straight", true, 0, "Grey Box Turns Off Straight", new Tags { });
            MoveRestrictor.AddToList("Ball", true, 0, "Grey Box Turns Off Ball", new Tags { });
            MoveRestrictor.AddToList("Kick", true, 0, "Grey Box Turns Off Kick", new Tags { });
            MoveRestrictor.AddToList("Stomp", true, 0, "Grey Box Turns Off Stomp", new Tags { });
            MoveRestrictor.AddToList("Wall", true, 0, "Grey Box Turns Off Wall", new Tags { });
            MoveRestrictor.AddToList("Jump", true, 0, "Grey Turns Off Jump", new Tags { });
            MoveRestrictor.AddToList("Uppercut", true, 0, "Grey Box Turns Off Uppercut", new Tags { });
            MoveRestrictor.AddToList("Cube", true, 0, "Grey Box Turns Off Cube", new Tags { });
            MoveRestrictor.AddToList("Dash", true, 0, "Grey Box Turns Off Dash", new Tags { });
            MoveRestrictor.AddToList("Parry", true, 0, "Grey Box Turns Off Parry", new Tags { });
            MoveRestrictor.AddToList("Hold Left", true, 0, "Grey Box Turns Off Left Hand Hold", new Tags { });
            MoveRestrictor.AddToList("Hold Right", true, 0, "Grey Box Turns Off Right Hand Hold", new Tags { });
            MoveRestrictor.AddToList("Explode", true, 0, "Grey Box Turns Off Explode", new Tags { });
            MoveRestrictor.AddToList("Flick", true, 0, "Grey Box Turns Off Flick", new Tags { });
            MoveRestrictor.GetFromFile();
            MoveRestrictor.ModSaved += Save;
            UI.instance.UI_Initialized += UIInit;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            sceneChanged = true;
            movesSettable = false;
            storedMoves.Clear();
            sceneChangeCount++;
        }

        public override void OnFixedUpdate()
        {
            if (sceneChanged)
            {
                if (currentScene != "Loader")
                {
                    checkPosesCoroutine = MelonCoroutines.Start(WaitThenCheckPoses(sceneChangeCount));
                }
                sceneChanged = false;
            }
            if (movesSettable && timeToReadFile)
            {
                Save();
            }
        }

        public void UIInit()
        {
            UI.AddMod(MoveRestrictor);
        }

        public void Save()
        {
            timeToReadFile = false;
            ChangeAvailableMoveSets();
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
                    storedMoves.Add(activePoses[i]);
                    activePoses.RemoveAt(i);
                    i--;
                }
            }
            if (poseList != "")
            {
                MelonLogger.Msg("Poses Removed: " + poseList);
            }
            poseList = "";
            for (int x = 0; x < storedMoves.Count; x++)
            {
                bool poseFound = false;
                for (int i = 0; i < movesToKeep.Count; i++)
                {
                    if (storedMoves[x].poseSet.name == movesToKeep[i])
                    {
                        poseFound = true;
                        break;
                    }
                }
                if (poseFound)
                {
                    if (poseList != "")
                    {
                        poseList += ", ";
                    }
                    poseList += storedMoves[x].poseSet.name;
                    activePoses.Add(storedMoves[x]);
                    storedMoves.RemoveAt(x);
                    x--;
                }
            }
            if (poseList != "")
            {
                MelonLogger.Msg("Poses Added: " + poseList);
            }
        }
    }
}

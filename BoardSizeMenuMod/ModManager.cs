using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes;

namespace BoardSizeAndStanceMod
{
    public class ModManager : MelonMod
    {
        public static ModManager instance;

        public static MelonPreferences_Category boardSizePrefCategory;
        public static MelonPreferences_Entry<string> boardLengthPref;
        public static MelonPreferences_Entry<string> boardWidthPref;
        public static MelonPreferences_Entry<bool> crazyModPref;

        public float boardLengthFactor;
        public float boardWidthFactor;
        public Vector3 currentBoardScale;

        public GameObject gameplayRider;
        public GameObject gameplayBoard;
        public bool areGameplayObjectsRegistered;
        public Il2CppLirp.SnowboardEquipment gameplayBoardEquipement;
        private Vector3[] defaultGameplayBoardBonesPos;

        public GameObject replayRider;
        public GameObject replayBoard;
        public bool areReplayObjectsRegistered;
        public Il2CppLirp.SnowboardEquipment replayBoardEquipement;
        private Vector3[] defaultReplayBoardBonesPos;

        public GameObject newBoardChildrenParent;

        //private GameObject[] boardColliders;
        public static Il2CppLirp.UserSession userSession;
        public static PlayerRigReplayManager replayManager;

        private AssetManager assetManager;

        public bool isDebugActive;

        //public Lirp.VisualCharacter visualCharacter;
        public Il2CppLirp.CharacterStructure characterStructure;
        public GameObject csGo;
        public Transform csTransform;
        public bool csBool;
        public bool csRetrieved;
        public bool lastSnowboarder2;

        public bool isMenuVisible;

        public static bool isCrazyModeActivated;
        public static float crazyFactor;

        public int updaterFrameCount;
        public int updaterTarget;
        public bool isUpdaterRunning;

        public bool isStructureInit;
        private bool isStructureReloaded;
        public float lengthFactorOnReload;
        public float widthFactorOnReload;
        public float previousWidthOnReload;

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<MenuBuilder>();
            ClassInjector.RegisterTypeInIl2Cpp<TransformUpdater>();
            ClassInjector.RegisterTypeInIl2Cpp<BindingsUpdater>();

            instance = this;
            isDebugActive = false;

            boardSizePrefCategory = MelonPreferences.CreateCategory("boardSizePrefCategory");

            boardLengthPref = boardSizePrefCategory.CreateEntry("boardLengthPref", 1f.ToString("F2"));
            boardWidthPref = boardSizePrefCategory.CreateEntry("boardWidthPref", 1f.ToString("F2"));

            crazyModPref = boardSizePrefCategory.CreateEntry("crazyModPref", false);

            boardLengthFactor = float.Parse(boardLengthPref.Value);
            boardWidthFactor = float.Parse(boardWidthPref.Value);

            isCrazyModeActivated = crazyModPref.Value;
            if (isCrazyModeActivated)
                crazyFactor = 2f;
            else
                crazyFactor = 1f;

            previousWidthOnReload = 1f;

            assetManager = new AssetManager();
            assetManager.Init();

            BindingsManager bindingsManager = new BindingsManager();
            bindingsManager.Init();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Loader")
            {
                assetManager.CreateMenu();
                MelonLogger.Msg("Board size and stance mod initialized.");
            }

            if (sceneName == "GameBase")
            {
                userSession = GameObject.Find("UserSession").GetComponent<Il2CppLirp.UserSession>();
                //MelonLogger.Msg("UserSession registered");
            }

            if (sceneName == "mountain01Logic")
            {
                replayManager = GameObject.Find("ReplayManager").GetComponent<PlayerRigReplayManager>();
                //MelonLogger.Msg("ReplayManager registered");
            }
        }

        public override void OnLateUpdate()
        {
            /*
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
                LengthAndWidthControl();*/

            BindingsManager.instance.LateUpdate();

            if (Input.GetKeyDown(KeyCode.Keypad5))
                InitBoardSizeValues();

                if (Input.GetKeyDown(KeyCode.S))
                if (assetManager.instantiatedMenu.active)
                {
                    assetManager.instantiatedMenu.SetActive(false);
                    isMenuVisible = false;
                }
                else
                {
                    assetManager.instantiatedMenu.SetActive(true);
                    isMenuVisible = true;
                }

            if (isUpdaterRunning)
            {
                if (updaterFrameCount < updaterTarget)
                {
                    updaterFrameCount++;
                    SetupNewHierarchy();
                    //ModLogger.Log("Init overtime");
                    if (updaterFrameCount == updaterTarget)
                    {
                        isStructureInit = true;
                        InitBoardSizeValues();
                        //UpdateBoardBonesScales();
                    }
                }
                else
                {
                    isUpdaterRunning = false;
                }
            }
        }

        private void LengthAndWidthControl()
        {
            /*
            if (Input.GetKeyDown(KeyCode.UpArrow))
                UpdateBoardSizeValues(boardLengthFactor + 0.01f, boardWidthFactor);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                UpdateBoardSizeValues(boardLengthFactor - 0.01f, boardWidthFactor);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                UpdateBoardSizeValues(boardLengthFactor, boardWidthFactor + 0.01f);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                UpdateBoardSizeValues(boardLengthFactor, boardWidthFactor - 0.01f);

            if (Input.GetKeyDown(KeyCode.Keypad0))
                UpdateBoardSizeValues(1f, 1f);

            if (Input.GetKeyDown(KeyCode.Keypad7))
                SetCrazyFactor(!isCrazyModeActivated);
            */
        }

        public bool GetGameBoardGameObjects()
        {
            gameplayBoard = gameplayRider.transform.FindChild("Root/characterbase(Clone)/Root/Snowboard").gameObject;
            if (!gameplayBoard)
                return false;

            newBoardChildrenParent = GameObject.Instantiate(new GameObject(), gameplayBoard.transform);
            newBoardChildrenParent.name = "NewParent";

            gameplayBoardEquipement = gameplayBoard.GetComponent<Il2CppLirp.SnowboardEquipment>();
            defaultGameplayBoardBonesPos = gameplayBoardEquipement.defaultBonePositions;
            //BindingsManager.instance.SetupGameplayBindings();

            //MelonLogger.Msg("Trying to find board collider");
            /*
            boardColliders = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "boardColliderContainer").ToArray();
            if (boardColliders.Length == 0)
                return false;
            */
            areGameplayObjectsRegistered = true;
            return true;
        }

        public void SetupNewHierarchy()
        {
            //ModLogger.Log("Setup new hierarchy");
            //gameplayBoardEquipement.ToLocalScale = new Vector3(1, 1, 1);
            for (int i = 0; i < gameplayBoard.transform.GetChildCount(); i++)
            {
                Transform childToCheck = gameplayBoard.transform.GetChild(i);
                if (childToCheck.gameObject != newBoardChildrenParent)
                {
                    childToCheck.SetParent(newBoardChildrenParent.transform, false);
                }
            }
            /*
            if (isStructureInit)
            {
                float widthFactorToApply = Mathf.Pow(boardWidthFactor, crazyFactor);
                float lengthFactorToApply = Mathf.Pow(boardLengthFactor, crazyFactor);
                Vector3 newScale = new Vector3(1 / widthFactorToApply, 1, 1 / lengthFactorToApply);
                gameplayBoard.transform.localScale = currentBoardScale;
                newBoardChildrenParent.transform.localScale = newScale;
            }
            */
        }

        public void ReloadStructureOverTime(int in_nbrOfFrames)
        {
            previousWidthOnReload = boardWidthFactor;
            isStructureReloaded = true;
            ModLogger.Log("Structure reloaded");
            InitBoardOverTime(in_nbrOfFrames);
        }

        public bool GetReplayBoardGameObjects()
        {
            replayBoard = replayRider.transform.FindChild("Root/characterbase(Clone)/Root/Snowboard").gameObject;
            if (!replayBoard)
                return false;

            replayBoardEquipement = replayBoard.GetComponent<Il2CppLirp.SnowboardEquipment>();
            defaultReplayBoardBonesPos = replayBoardEquipement.defaultBonePositions;

            BindingsManager.instance.SetupReplayBindings();
            areReplayObjectsRegistered = true;
            return true;
        }

        public void InitBoardSizeValues() => UpdateBoardSizeValues(float.Parse(boardLengthPref.Value), float.Parse(boardWidthPref.Value));

        public void InitBoardOverTime(int in_nbrOfFrames)
        {
            isUpdaterRunning = true;
            updaterFrameCount = 0;
            updaterTarget = in_nbrOfFrames;
            isStructureInit = false;
        }

        public void UpdateBoardBonesScales()
        {
            float lengthFactorToApply = Mathf.Pow(float.Parse(boardLengthPref.Value), crazyFactor);
            float widthFactorToApply = Mathf.Pow(float.Parse(boardWidthPref.Value), crazyFactor);

            //currentBoardScale = new Vector3(widthFactorToApply, 1, lengthFactorToApply);

            if (gameplayBoard)
            {
                for (int i = 0; i < gameplayBoardEquipement.defaultBonePositions.Length; i++)
                {
                    /*
                    Vector3 pos = defaultGameplayBoardBonesPos[i];
                    pos = new Vector3(pos.x, pos.y, pos.z * lengthFactorToApply);
                    gameplayBoardEquipement.defaultBonePositions[i] = pos;
                    */
                    Vector3 newScale;
                    if (isStructureReloaded)
                    {
                        newScale = new Vector3(widthFactorToApply / previousWidthOnReload, 1, 1); // * (1 / lengthFactorOnReload) + (1 - lengthFactorOnReload)
                        ModLogger.Log($"Reloaded scale with maxScale = {previousWidthOnReload} and final x scale = {widthFactorToApply / previousWidthOnReload}");
                    }
                    else
                        newScale = new Vector3(widthFactorToApply, 1, 1);
                    gameplayBoardEquipement.bones[i].localScale = newScale;
                }
                /*
                if (isStructureInit)
                {
                    gameplayBoard.transform.localScale = currentBoardScale;
                    Vector3 newScale = new Vector3(1 / widthFactorToApply, 1, 1 / lengthFactorToApply);
                    newBoardChildrenParent.transform.localScale = newScale;
                }
                */
            }

            /*
            if (isMenuVisible)
                assetManager.menuBuilder.UpdateSizeValues(boardLengthFactor, boardWidthFactor);
            */
        }

        public void ResetBoardBonesScale()
        {
            ModLogger.Log("ResetBoardBonesScale");
            for (int i = 0; i < gameplayBoardEquipement.defaultBonePositions.Length; i++)
            {
                Vector3 newScale = new Vector3(1, 1, 1);
                gameplayBoardEquipement.bones[i].localScale = newScale;
            }
        }

        public void UpdateBoardSizeValues(float in_length, float in_width)
        {
            //MelonLogger.Msg($"Updating board size with {in_boardLength} length factor and {in_boardWidth} width factor");
            
            boardLengthFactor = in_length;
            boardWidthFactor = in_width;

            boardLengthPref.Value = boardLengthFactor.ToString("F2");
            boardWidthPref.Value = boardWidthFactor.ToString("F2");

            float lengthFactorToApply = Mathf.Pow(boardLengthFactor, crazyFactor);
            float widthFactorToApply = Mathf.Pow(boardWidthFactor, crazyFactor);

            currentBoardScale = new Vector3(widthFactorToApply, 1, lengthFactorToApply);

            if (gameplayBoard)
            {
                for (int i = 0; i < gameplayBoardEquipement.defaultBonePositions.Length; i++)
                {
                    Vector3 pos = defaultGameplayBoardBonesPos[i];
                    pos = new Vector3(pos.x, pos.y, pos.z * lengthFactorToApply);
                    gameplayBoardEquipement.defaultBonePositions[i] = pos;
                    Vector3 newScale;
                    if (isStructureReloaded)
                    {
                        newScale = new Vector3(Mathf.Pow(widthFactorToApply / previousWidthOnReload, crazyFactor), 1, 1); // * (1 / lengthFactorOnReload) + (1 - lengthFactorOnReload)
                        ModLogger.Log($"Reloaded scale with previousWidthOnReload = {previousWidthOnReload} and final x scale = {widthFactorToApply / previousWidthOnReload}");
                    }
                    else
                        newScale = new Vector3(widthFactorToApply, 1, 1);
                    //newScale = new Vector3(widthFactorToApply, 1, 1);
                    gameplayBoardEquipement.bones[i].localScale = newScale;
                }
                if (isStructureInit)
                {
                    gameplayBoard.transform.localScale = currentBoardScale;
                    Vector3 newScale = new Vector3(1 / widthFactorToApply, 1, 1 / lengthFactorToApply);
                    newBoardChildrenParent.transform.localScale = newScale;
                }
            }
                //gameplayBoard.transform.localScale = newBoardScale;

            if (replayBoard)
            {
                for (int i = 0; i < replayBoardEquipement.defaultBonePositions.Length; i++)
                {
                    Vector3 pos = defaultReplayBoardBonesPos[i];
                    pos = new Vector3(pos.x, pos.y, pos.z * lengthFactorToApply);
                    replayBoardEquipement.defaultBonePositions[i] = pos;
                    replayBoardEquipement.bones[i].localScale = new Vector3(widthFactorToApply, 1, 1);
                }
            }
                //replayBoard.transform.localScale = newBoardScale;

            //BindingsManager.instance.UpdateBindingsScale(newBindingScale);

            //MelonLogger.Msg("Updating board collider scale.");
            /*
            foreach (GameObject go in boardColliders)
                go.transform.localScale = currentBoardScale;
            */
            //MelonLogger.Msg("End of updating board size");

            if (isMenuVisible)
                assetManager.menuBuilder.UpdateSizeValues(boardLengthFactor, boardWidthFactor);
        }

        public void SetCrazyFactor(bool in_isCrazyFactorActivated)
        {
            isCrazyModeActivated = in_isCrazyFactorActivated;
            crazyModPref.Value = isCrazyModeActivated;
            if (in_isCrazyFactorActivated)
                crazyFactor = 2f;
            else
                crazyFactor = 1f;
            UpdateBoardSizeValues(boardLengthFactor, boardWidthFactor);
            BindingsManager.instance.InitBindings();
        }

        public void SolveHandIKObjectsScale()
        {
            /*
            if (gameplayBoard)
            {
                gameplayHandsIKObjects.Clear();
                ModLogger.Log("Grab Start");
                gameplayHandsIKObjects.Add(gameplayBoard.transform.FindChild("LeftHandObjectIKTarget").gameObject);
                gameplayHandsIKObjects.Add(gameplayBoard.transform.FindChild("RightHandObjectIKTarget").gameObject);
                ModLogger.Log($"Solving IK for {gameplayHandsIKObjects.Count} found.");
                foreach (GameObject gameObject in gameplayHandsIKObjects)
                {
                    gameObject.transform.SetParent(newBoardChildrenParent.transform, false);
                    ModLogger.Log($"Set new parent for for {gameObject.name}.");
                }
                newBoardChildrenParent.transform.localScale = currentBoardScale;
            }
            if (replayBoard)
            {
                foreach (GameObject gameObject in replayHandsIKObjects)
                    gameObject.transform.localScale = currentBoardScale;
            }
            */
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;

namespace BoardSizeAndStanceMod
{
    public class BindingsManager
    {
        public static MelonPreferences_Entry<float> stanceWidthPref;
        public static MelonPreferences_Entry<float> leftBindingAnglePref;
        public static MelonPreferences_Entry<float> rightBindingAnglePref;
        public static MelonPreferences_Entry<bool> hideBindingsPref;

        public static BindingsManager instance;

        public float currentBindingsWidthFactor;
        public float defaultBindingsWidthFactor;
        public float leftBindingDistanceOffset;
        public float leftBindingAngleOffset;
        public float rightBindingDistanceOffset;
        public float rightBindingAngleOffset;

        public Vector3 defaultLeftBindingDistance;
        public Quaternion defaultLeftBindingAngle;
        public Vector3 defaultRightBindingDistance;
        public Quaternion defaultRightBindingAngle;

        public GameObject originalGameplayBoardLeftBinding;
        public GameObject newGameplayBoardLeftBinding;
        public GameObject originalGameplayBoardRightBinding;
        public GameObject newGameplayBoardRightBinding;
        private BindingsUpdater gameplayLeftBindingUpdater;
        private BindingsUpdater gameplayRightBindingUpdater;
        private MeshRenderer gameplayLeftBindingMeshRenderer;
        private MeshRenderer gameplayRightBindingMeshRenderer;

        public GameObject originalReplayBoardLeftBinding;
        public GameObject newReplayBoardLeftBinding;
        public GameObject originalReplayBoardRightBinding;
        public GameObject newReplayBoardRightBinding;
        private BindingsUpdater replayLeftBindingUpdater;
        private BindingsUpdater replayRightBindingUpdater;
        private MeshRenderer replayLeftBindingMeshRenderer;
        private MeshRenderer replayRightBindingMeshRenderer;

        public GameObject menuBoardLeftBinding;

        public bool areBindingsHidden;

        //public bool hideOriginalBindings;
        //public bool areOriginalBindingsHidden;
        public bool areGameplayBindingsRegistered;
        public bool areReplayBindingsRegistered;

        public void Init()
        {
            instance = this;
            defaultBindingsWidthFactor = 0;

            stanceWidthPref = ModManager.boardSizePrefCategory.CreateEntry("stanceWidthPref", 0f);
            leftBindingAnglePref = ModManager.boardSizePrefCategory.CreateEntry("leftBindingAnglePref", 0f);
            rightBindingAnglePref = ModManager.boardSizePrefCategory.CreateEntry("rightBindingAnglePref", 0f);
            hideBindingsPref = ModManager.boardSizePrefCategory.CreateEntry("bindingsVisibilityPref", false);

            currentBindingsWidthFactor = stanceWidthPref.Value;
            leftBindingAngleOffset = leftBindingAnglePref.Value;
            rightBindingAngleOffset = rightBindingAnglePref.Value;
            areBindingsHidden = hideBindingsPref.Value;

            SetBindingsVisibility(areBindingsHidden);
        }

        public void LateUpdate()
        {
            /*
            if (Input.GetKey(KeyCode.LeftControl))
                LeftBindingControl();
            if (Input.GetKey(KeyCode.RightControl))
                RightBindingControl();
            */
        }

        public void SetupGameplayBindings()
        {
            //Left binding
            originalGameplayBoardLeftBinding = ModManager.instance.gameplayBoard.transform.FindChild("BindingLeft").gameObject;
            if (!originalGameplayBoardLeftBinding)
                return;

            newGameplayBoardLeftBinding = GameObject.Instantiate(originalGameplayBoardLeftBinding, originalGameplayBoardLeftBinding.transform);

            newGameplayBoardLeftBinding.name = "NewBindingVisual";
            newGameplayBoardLeftBinding.transform.localPosition = new Vector3(0, 0, 0);
            //newGameplayBoardLeftBinding.transform.localRotation = newGameplayBoardLeftBinding.transform.parent.transform.localRotation;
            gameplayLeftBindingMeshRenderer = newGameplayBoardLeftBinding.GetComponent<MeshRenderer>();
            gameplayLeftBindingMeshRenderer.enabled = !areBindingsHidden;

            GameObject childToDestroy = newGameplayBoardLeftBinding.transform.GetChild(0).gameObject;
            ModLogger.Log($"Child found with name = {childToDestroy.name}");
            GameObject.Destroy(childToDestroy);

            //newGameplayBoardLeftBinding = newGameplayBoardLeftBinding.transform.parent.gameObject;
            defaultLeftBindingDistance = newGameplayBoardLeftBinding.transform.localPosition;
            defaultLeftBindingAngle = newGameplayBoardLeftBinding.transform.localRotation;

            gameplayLeftBindingUpdater = originalGameplayBoardLeftBinding.AddComponent<BindingsUpdater>();
            gameplayLeftBindingUpdater.Init(newGameplayBoardLeftBinding, false); 
            //originalGameplayBoardLeftBinding.GetComponent<MeshRenderer>().enabled = false;

            //Right binding
            originalGameplayBoardRightBinding = ModManager.instance.gameplayBoard.transform.FindChild("BindingRight").gameObject;
            if (!originalGameplayBoardRightBinding)
                return;

            newGameplayBoardRightBinding = GameObject.Instantiate(originalGameplayBoardRightBinding, originalGameplayBoardRightBinding.transform);

            newGameplayBoardRightBinding.name = "NewBindingVisual";
            newGameplayBoardRightBinding.transform.localPosition = new Vector3(0, 0, 0);
            //newGameplayBoardRightBinding.transform.localRotation = newGameplayBoardRightBinding.transform.parent.transform.localRotation;
            gameplayRightBindingMeshRenderer = newGameplayBoardRightBinding.GetComponent<MeshRenderer>();
            gameplayRightBindingMeshRenderer.enabled = !areBindingsHidden;

            childToDestroy = newGameplayBoardRightBinding.transform.GetChild(0).gameObject;
            ModLogger.Log($"Child found with name = {childToDestroy.name}");
            GameObject.Destroy(childToDestroy);

            //newGameplayBoardRightBinding = newGameplayBoardRightBinding.transform.parent.gameObject;
            defaultRightBindingDistance = newGameplayBoardRightBinding.transform.localPosition;
            defaultRightBindingAngle = newGameplayBoardRightBinding.transform.localRotation;

            gameplayRightBindingUpdater = originalGameplayBoardRightBinding.AddComponent<BindingsUpdater>();
            gameplayRightBindingUpdater.Init(newGameplayBoardRightBinding, false);

            areGameplayBindingsRegistered = true;
        }

        public void SetupReplayBindings()
        {
            //LeftBinding
            originalReplayBoardLeftBinding = ModManager.instance.replayBoard.transform.FindChild("BindingLeft").gameObject;
            if (!originalReplayBoardLeftBinding)
                return;

            newReplayBoardLeftBinding = GameObject.Instantiate(originalReplayBoardLeftBinding, originalReplayBoardLeftBinding.transform);

            newReplayBoardLeftBinding.name = "NewBindingVisual";
            newReplayBoardLeftBinding.transform.localPosition = new Vector3(0, 0, 0);
            //newReplayBoardLeftBinding.transform.localRotation = newReplayBoardLeftBinding.transform.parent.transform.localRotation;
            replayLeftBindingMeshRenderer = newReplayBoardLeftBinding.GetComponent<MeshRenderer>();
            replayLeftBindingMeshRenderer.enabled = !areBindingsHidden;

            GameObject childToDestroy = newReplayBoardLeftBinding.transform.GetChild(0).gameObject;
            ModLogger.Log($"Child found with name = {childToDestroy.name}");
            GameObject.Destroy(childToDestroy);

            //newReplayBoardLeftBinding.GetComponent<MeshFilter>().mesh = originalReplayBoardLeftBinding.GetComponent<MeshFilter>().mesh;

            //newReplayBoardLeftBinding = newReplayBoardLeftBinding.transform.parent.gameObject;
            defaultLeftBindingDistance = newReplayBoardLeftBinding.transform.localPosition;
            defaultLeftBindingAngle = newReplayBoardLeftBinding.transform.localRotation;

            replayLeftBindingUpdater = originalReplayBoardLeftBinding.AddComponent<BindingsUpdater>();
            replayLeftBindingUpdater.Init(newReplayBoardLeftBinding, true);

            //Right binding
            originalReplayBoardRightBinding = ModManager.instance.replayBoard.transform.FindChild("BindingRight").gameObject;
            if (!originalReplayBoardRightBinding)
                return;

            newReplayBoardRightBinding = GameObject.Instantiate(originalReplayBoardRightBinding, originalReplayBoardRightBinding.transform);

            newReplayBoardRightBinding.name = "NewBindingVisual";
            newReplayBoardRightBinding.transform.localPosition = new Vector3(0, 0, 0);
            //newReplayBoardRightBinding.transform.localRotation = newReplayBoardRightBinding.transform.parent.transform.localRotation;
            replayRightBindingMeshRenderer = newReplayBoardRightBinding.GetComponent<MeshRenderer>();
            replayRightBindingMeshRenderer.enabled = !areBindingsHidden;

            //newReplayBoardRightBinding.GetComponent<MeshFilter>().mesh = originalReplayBoardRightBinding.GetComponent<MeshFilter>().mesh;

            childToDestroy = newReplayBoardRightBinding.transform.GetChild(0).gameObject;
            ModLogger.Log($"Child found with name = {childToDestroy.name}");
            GameObject.Destroy(childToDestroy);

            //newReplayBoardRightBinding = newReplayBoardRightBinding.transform.parent.gameObject;
            defaultRightBindingDistance = newReplayBoardRightBinding.transform.localPosition;
            defaultRightBindingAngle = newReplayBoardRightBinding.transform.localRotation;

            replayRightBindingUpdater = originalReplayBoardRightBinding.AddComponent<BindingsUpdater>();
            replayRightBindingUpdater.Init(newReplayBoardRightBinding, true);
        }

        //INPUTS
        private void LeftBindingControl()
        {
            /*
            if (Input.GetKeyDown(KeyCode.UpArrow))
                UpdateBindingsWidth(0.01f);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                UpdateBindingsWidth(-0.01f);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                UpdateLeftBindingAngle(-1f);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                UpdateLeftBindingAngle(1f);

            if (Input.GetKeyDown(KeyCode.Keypad0))
                ResetBindingsWidth();

            if (Input.GetKeyDown(KeyCode.Keypad1))
                ResetLeftBindingAngle();
            */
        }

        private void RightBindingControl()
        {
            /*
            if (Input.GetKeyDown(KeyCode.UpArrow))
                UpdateBindingsWidth(0.01f);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                UpdateBindingsWidth(-0.01f);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                UpdateRightBindingAngle(-1f);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                UpdateRightBindingAngle(1f);

            if (Input.GetKeyDown(KeyCode.Keypad0))
                ResetBindingsWidth();

            if (Input.GetKeyDown(KeyCode.Keypad1))
                ResetRightBindingAngle();
            */
        }

        public void InitBindings()
        {
            UpdateBindingsWidth(0f);
            UpdateLeftBindingAngle(0);
            UpdateRightBindingAngle(0);
        }

        //WIDTH
        private void UpdateBindingsWidth(float in_widthOffset)
        {
            currentBindingsWidthFactor += in_widthOffset;
            Vector3 newLeftPos = new Vector3(0, 0, currentBindingsWidthFactor);
            Vector3 newRightPos = new Vector3(0, 0, -currentBindingsWidthFactor);
            if (ModManager.instance.areGameplayObjectsRegistered)
            {
                gameplayLeftBindingUpdater.SetPosition(newLeftPos);
                gameplayRightBindingUpdater.SetPosition(newRightPos);
            }
            if (ModManager.instance.areReplayObjectsRegistered)
            {
                replayLeftBindingUpdater.SetPosition(newLeftPos);
                replayRightBindingUpdater.SetPosition(newRightPos);
            }
            stanceWidthPref.Value = currentBindingsWidthFactor;
            //ModLogger.Log($"Width updated with offset = {currentBindingsWidthFactor}");
        }

        public void SetNewBindingsWidth(float in_value)
        {
            currentBindingsWidthFactor = in_value;
            UpdateBindingsWidth(0);
        }

        public void ResetBindingsWidth()
        {
            currentBindingsWidthFactor = defaultBindingsWidthFactor;
            stanceWidthPref.Value = currentBindingsWidthFactor;
            Vector3 newLeftPos = defaultLeftBindingDistance;
            Vector3 newRightPos = defaultRightBindingDistance;
            if (ModManager.instance.areGameplayObjectsRegistered)
            {
                gameplayLeftBindingUpdater.SetPosition(newLeftPos);
                gameplayRightBindingUpdater.SetPosition(newRightPos);
            }

            if (ModManager.instance.areReplayObjectsRegistered)
            {
                replayLeftBindingUpdater.SetPosition(newLeftPos);
                replayRightBindingUpdater.SetPosition(newRightPos);
            }
        }

        //ANGLES
        private void UpdateLeftBindingAngle(float in_angle)
        {
            leftBindingAngleOffset += in_angle;
            Quaternion newAngle = Quaternion.Euler(defaultLeftBindingAngle.eulerAngles.x, leftBindingAngleOffset + defaultLeftBindingAngle.eulerAngles.y, defaultLeftBindingAngle.eulerAngles.z);
            if (ModManager.instance.areGameplayObjectsRegistered)
                gameplayLeftBindingUpdater.SetRotation(newAngle);
            
            if (ModManager.instance.areReplayObjectsRegistered)
                replayLeftBindingUpdater.SetRotation(newAngle);
            leftBindingAnglePref.Value = leftBindingAngleOffset;
            //ModLogger.Log($"Left binding angle updated with offset = {leftBindingAngleOffset}");
        }

        public void SetLeftBindingAngle(float in_value)
        {
            leftBindingAngleOffset = in_value;
            UpdateLeftBindingAngle(0);
        }

        public void ResetLeftBindingAngle()
        {
            leftBindingAngleOffset = 0;
            leftBindingAnglePref.Value = leftBindingAngleOffset;
            if (ModManager.instance.areGameplayObjectsRegistered)
                gameplayLeftBindingUpdater.SetRotation(defaultLeftBindingAngle);

            if (ModManager.instance.areReplayObjectsRegistered)
                replayLeftBindingUpdater.SetRotation(defaultLeftBindingAngle);
            UpdateLeftBindingAngle(0);
        }

        //RIGHT BINDINGS
        private void UpdateRightBindingAngle(float in_angle)
        {
            rightBindingAngleOffset += in_angle;
            Quaternion newAngle = Quaternion.Euler(defaultRightBindingAngle.eulerAngles.x, rightBindingAngleOffset + defaultRightBindingAngle.eulerAngles.y, defaultRightBindingAngle.eulerAngles.z);
            if (ModManager.instance.areGameplayObjectsRegistered)
                gameplayRightBindingUpdater.SetRotation(newAngle);

            if (ModManager.instance.areReplayObjectsRegistered)
                replayRightBindingUpdater.SetRotation(newAngle);
            rightBindingAnglePref.Value = rightBindingAngleOffset;
            //ModLogger.Log($"Right binding angle updated with offset = {rightBindingAngleOffset}");
        }

        public void SetRightBindingAngle(float in_value)
        {
            rightBindingAngleOffset = in_value;
            UpdateRightBindingAngle(0);
        }

        public void ResetRightBindingAngle()
        {
            rightBindingAngleOffset = 0;
            rightBindingAnglePref.Value = rightBindingAngleOffset;
            if (ModManager.instance.areGameplayObjectsRegistered)
                gameplayLeftBindingUpdater.SetRotation(defaultRightBindingAngle);

            if (ModManager.instance.areReplayObjectsRegistered)
                replayLeftBindingUpdater.SetRotation(defaultRightBindingAngle);
            UpdateRightBindingAngle(0);
        }


        //UTILS
        public void UpdateBindingsScale(Vector3 in_bindingScale)
        {
            if (ModManager.instance.areGameplayObjectsRegistered)
            {
                newGameplayBoardLeftBinding.transform.localScale = in_bindingScale;
                newGameplayBoardRightBinding.transform.localScale = in_bindingScale;
            }

            if (ModManager.instance.areReplayObjectsRegistered)
            {
                newReplayBoardLeftBinding.transform.localScale = in_bindingScale;
                newReplayBoardRightBinding.transform.localScale = in_bindingScale;
            }
        }

        public void ResetBindingsUpdaters()
        {
            gameplayLeftBindingUpdater.ResetTimer();
            gameplayRightBindingUpdater.ResetTimer();
            replayLeftBindingUpdater.ResetTimer();
            replayRightBindingUpdater.ResetTimer();
        }

        public void SetBindingsVisibility(bool in_areBindingsHidden)
        {
            areBindingsHidden = in_areBindingsHidden;
            hideBindingsPref.Value = areBindingsHidden;
            if (ModManager.instance.areGameplayObjectsRegistered)
            {
                gameplayLeftBindingMeshRenderer.enabled = !areBindingsHidden;
                gameplayRightBindingMeshRenderer.enabled = !areBindingsHidden;
            }
            if (ModManager.instance.areReplayObjectsRegistered)
            {
                replayLeftBindingMeshRenderer.enabled = !areBindingsHidden;
                replayRightBindingMeshRenderer.enabled = !areBindingsHidden;
            }
        }

        //Trash
        private void UpdateLeftBindingDistance(float in_distanceOffset)
        {
            leftBindingDistanceOffset += in_distanceOffset;
            if (originalGameplayBoardLeftBinding)
            {
                Vector3 newPos = defaultLeftBindingDistance + new Vector3(0, 0, leftBindingDistanceOffset);
                originalGameplayBoardLeftBinding.transform.localPosition = newPos;
                if (originalReplayBoardLeftBinding)
                    originalReplayBoardLeftBinding.transform.localPosition = newPos;
                ModLogger.Log($"Left binding distance updated with offset = {leftBindingDistanceOffset}");
            }
        }

        private void UpdateRightBindingDistance(float in_distanceOffset)
        {
            rightBindingDistanceOffset += in_distanceOffset;
            if (originalGameplayBoardRightBinding)
            {
                Vector3 newPos;
                newPos = defaultRightBindingDistance + new Vector3(0, 0, rightBindingDistanceOffset);
                originalGameplayBoardRightBinding.transform.localPosition = newPos;
                if (originalReplayBoardRightBinding)
                    originalReplayBoardRightBinding.transform.localPosition = newPos;
                ModLogger.Log($"Left binding distance updated with offset = {rightBindingDistanceOffset}");
            }
        }
    }
}

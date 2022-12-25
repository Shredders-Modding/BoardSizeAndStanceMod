using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardSizeAndStanceMod
{
    class MenuBuilder : MonoBehaviour
    {
        public MenuBuilder(IntPtr ptr) : base(ptr) { }

        private GameObject lengthParent;
        private Slider lengthSlider;
        private TMP_InputField lengthInputFieldTMPro;
        private Button lengthResetButton;

        private GameObject widthParent;
        private Slider widthSlider;
        private TMP_InputField widthInputFieldTMPro;
        private Button widthResetButton;

        private GameObject stanceWidthParent;
        private Slider stanceWidthSlider;
        private TMP_InputField stanceWidthInputFieldTMPro;
        private Button stanceWidthResetButton;

        private GameObject leftBindingAngleParent;
        private Slider leftBindingAngleSlider;
        private TMP_InputField leftBindingAngleInputFieldTMPro;
        private Button leftBindingAngleResetButton;

        private GameObject rightBindingAngleParent;
        private Slider rightBindingAngleSlider;
        private TMP_InputField rightBindingAngleInputFieldTMPro;
        private Button rightBindingAngleResetButton;

        private Toggle goNutsToggle;
        private Toggle bindingsVisibilityToggle;

        private void Start()
        {
            InitMenu();
        }

        private void InitMenu()
        {
            //MelonLogger.Msg("Menu builder init started");

            BoardSizeSetup();
            StanceWidthSetup();
            LeftBindingSetup();
            RigthBindingSetup();
            TogglesSetup();

            ModLogger.Log("Menu builder initalized");
        }

        private void TogglesSetup()
        {
            goNutsToggle = gameObject.transform.Find("GoNuts_Toggle").gameObject.GetComponent<Toggle>();
            bindingsVisibilityToggle = gameObject.transform.Find("BindingsVisibility_Toggle").gameObject.GetComponent<Toggle>();

            goNutsToggle.isOn = ModManager.isCrazyModeActivated;
            bindingsVisibilityToggle.isOn = BindingsManager.instance.areBindingsHidden;

            goNutsToggle.onValueChanged.AddListener(new Action<bool>(OnGoNutsToggleChanged));
            bindingsVisibilityToggle.onValueChanged.AddListener(new Action<bool>(OnBindingsVisibilityChanged));
        }

        private void RigthBindingSetup()
        {
            rightBindingAngleParent = gameObject.transform.Find("RightBindingAngle_Parent").gameObject;
            rightBindingAngleSlider = rightBindingAngleParent.transform.Find("RightBindingAngle_Slider").GetComponent<Slider>();
            rightBindingAngleInputFieldTMPro = rightBindingAngleParent.transform.Find("RightBindingAngle_InputField").GetComponent<TMP_InputField>();
            rightBindingAngleResetButton = rightBindingAngleParent.transform.Find("RightBindingAngle_ResetButton").GetComponent<Button>();

            rightBindingAngleSlider.value = BindingsManager.rightBindingAnglePref.Value;
            rightBindingAngleInputFieldTMPro.text = BindingsManager.rightBindingAnglePref.Value.ToString("F2");

            rightBindingAngleSlider.onValueChanged.AddListener(new Action<float>(OnRightBindingAngleChange));
            rightBindingAngleInputFieldTMPro.onSubmit.AddListener(new Action<string>(OnRightBindingAngleSubmit));
            rightBindingAngleResetButton.onClick.AddListener(new Action(OnRightBindingAngleReset));
        }

        private void LeftBindingSetup()
        {
            leftBindingAngleParent = gameObject.transform.Find("LeftBindingAngle_Parent").gameObject;
            leftBindingAngleSlider = leftBindingAngleParent.transform.Find("LeftBindingAngle_Slider").GetComponent<Slider>();
            leftBindingAngleInputFieldTMPro = leftBindingAngleParent.transform.Find("LeftBindingAngle_InputField").GetComponent<TMP_InputField>();
            leftBindingAngleResetButton = leftBindingAngleParent.transform.Find("LeftBindingAngle_ResetButton").GetComponent<Button>();

            leftBindingAngleSlider.value = BindingsManager.leftBindingAnglePref.Value;
            leftBindingAngleInputFieldTMPro.text = BindingsManager.leftBindingAnglePref.Value.ToString("F2");

            leftBindingAngleSlider.onValueChanged.AddListener(new Action<float>(OnLeftBindingAngleChange));
            leftBindingAngleInputFieldTMPro.onSubmit.AddListener(new Action<string>(OnLeftBindingAngleSubmit));
            leftBindingAngleResetButton.onClick.AddListener(new Action(OnLeftBindingAngleReset));
        }

        private void StanceWidthSetup()
        {
            stanceWidthParent = gameObject.transform.Find("StanceWidth_Parent").gameObject;
            stanceWidthSlider = stanceWidthParent.transform.Find("StanceWidth_Slider").GetComponent<Slider>();
            stanceWidthInputFieldTMPro = stanceWidthParent.transform.Find("StanceWidth_InputField").GetComponent<TMP_InputField>();
            stanceWidthResetButton = stanceWidthParent.transform.Find("StanceWidth_ResetButton").GetComponent<Button>();

            stanceWidthSlider.value = BindingsManager.instance.currentBindingsWidthFactor;
            stanceWidthInputFieldTMPro.text = BindingsManager.instance.currentBindingsWidthFactor.ToString("F2");

            stanceWidthSlider.onValueChanged.AddListener(new Action<float>(OnStanceWidthChange));
            stanceWidthInputFieldTMPro.onSubmit.AddListener(new Action<string>(OnStanceWidthSubmit));
            stanceWidthResetButton.onClick.AddListener(new Action(OnStanceWidthReset));
        }

        private void BoardSizeSetup()
        {
            lengthParent = gameObject.transform.Find("Length_Parent").gameObject;
            lengthSlider = lengthParent.transform.Find("Length_Slider").GetComponent<Slider>();
            lengthInputFieldTMPro = lengthParent.transform.Find("Length_InputField").GetComponent<TMP_InputField>();
            lengthResetButton = lengthParent.transform.Find("Length_ResetButton").gameObject.GetComponent<Button>();

            widthParent = gameObject.transform.Find("Width_Parent").gameObject;
            widthSlider = widthParent.transform.Find("Width_Slider").GetComponent<Slider>();
            widthInputFieldTMPro = widthParent.transform.Find("Width_InputField").GetComponent<TMP_InputField>();
            widthResetButton = widthParent.transform.Find("Width_ResetButton").gameObject.GetComponent<Button>();

            lengthSlider.value = ModManager.instance.boardLengthFactor;
            lengthInputFieldTMPro.text = ModManager.instance.boardLengthFactor.ToString("F2");

            widthSlider.value = ModManager.instance.boardWidthFactor;
            widthInputFieldTMPro.text = ModManager.instance.boardWidthFactor.ToString("F2");

            lengthSlider.onValueChanged.AddListener(new Action<float>(OnBoardLengthChange));
            lengthInputFieldTMPro.onSubmit.AddListener(new Action<string>(OnBoardLengthSubmit));
            lengthResetButton.onClick.AddListener(new Action(OnBoardLengthReset));

            widthSlider.onValueChanged.AddListener(new Action<float>(OnBoardWidthChange));
            widthInputFieldTMPro.onSubmit.AddListener(new Action<string>(OnBoardWidthSubmit));
            widthResetButton.onClick.AddListener(new Action(OnBoardWidthReset));
        }

        private void OnBoardLengthChange(float value)
        {
            //Change board size here
            lengthInputFieldTMPro.text = value.ToString("F2");
            ModManager.instance.UpdateBoardSizeValues(value, ModManager.instance.boardWidthFactor);
        }

        public void OnBoardLengthSubmit(string text)
        {
            //Change board size here
            lengthSlider.value = float.Parse(text);
            ModManager.instance.UpdateBoardSizeValues(lengthSlider.value, ModManager.instance.boardWidthFactor);
        }

        public void OnBoardWidthChange(float value)
        {
            //Change board size here
            widthInputFieldTMPro.text = value.ToString("F2");
            ModManager.instance.UpdateBoardSizeValues(ModManager.instance.boardLengthFactor, value);
        }

        public void OnBoardWidthSubmit(string text)
        {
            //Change board size here
            widthSlider.value = float.Parse(text);
            ModManager.instance.UpdateBoardSizeValues(ModManager.instance.boardLengthFactor, widthSlider.value);
        }

        public void OnBoardLengthReset()
        {
            lengthSlider.value = 1f;
            lengthInputFieldTMPro.text = "1.00";
            //Change board size here
            ModManager.instance.UpdateBoardSizeValues(1f, ModManager.instance.boardWidthFactor);
        }

        public void OnBoardWidthReset()
        {
            widthSlider.value = 1f;
            widthInputFieldTMPro.text = "1.00";
            //Change board size here
            ModManager.instance.UpdateBoardSizeValues(ModManager.instance.boardLengthFactor, 1f);
        }

        public void UpdateSizeValues(float in_length, float in_width)
        {
            lengthInputFieldTMPro.text = in_length.ToString("F2");
            lengthSlider.value = in_length;

            widthInputFieldTMPro.text = in_width.ToString("F2");
            widthSlider.value = in_width;
        }


        //STANCE WIDTH
        public void OnStanceWidthChange(float value)
        {
            stanceWidthInputFieldTMPro.text = value.ToString("F2");
            BindingsManager.instance.SetNewBindingsWidth(value);
        }

        public void OnStanceWidthSubmit(string text)
        {
            stanceWidthSlider.value = float.Parse(text);
            BindingsManager.instance.SetNewBindingsWidth(stanceWidthSlider.value);
        }

        public void UpdateStanceWidthValue(float value)
        {
            stanceWidthSlider.value = value;
            stanceWidthInputFieldTMPro.text = value.ToString("F2");
        }

        public void OnStanceWidthReset()
        {
            BindingsManager.instance.ResetBindingsWidth();
            stanceWidthInputFieldTMPro.text = "0";
            UpdateStanceWidthValue(BindingsManager.instance.currentBindingsWidthFactor);
        }


        //LEFT BINDING ANGLE
        public void OnLeftBindingAngleChange(float value)
        {
            leftBindingAngleInputFieldTMPro.text = value.ToString("F2");
            BindingsManager.instance.SetLeftBindingAngle(value);
        }

        public void OnLeftBindingAngleSubmit(string text)
        {
            leftBindingAngleSlider.value = float.Parse(text);
            BindingsManager.instance.SetLeftBindingAngle(leftBindingAngleSlider.value);
        }

        public void UpdateLeftBindingAngleValue(float value)
        {
            leftBindingAngleSlider.value = value;
            leftBindingAngleInputFieldTMPro.text = value.ToString("F2");
        }

        public void OnLeftBindingAngleReset()
        {
            BindingsManager.instance.ResetLeftBindingAngle();
            leftBindingAngleInputFieldTMPro.text = "0";
            UpdateLeftBindingAngleValue(BindingsManager.instance.leftBindingAngleOffset);
        }


        //RIGHT BINDING ANGLE
        public void OnRightBindingAngleChange(float value)
        {
            rightBindingAngleInputFieldTMPro.text = value.ToString("F2");
            BindingsManager.instance.SetRightBindingAngle(value);
        }

        public void OnRightBindingAngleSubmit(string text)
        {
            rightBindingAngleSlider.value = float.Parse(text);
            BindingsManager.instance.SetRightBindingAngle(rightBindingAngleSlider.value);
        }

        public void UpdateRightBindingAngleValue(float value)
        {
            rightBindingAngleSlider.value = value;
            rightBindingAngleInputFieldTMPro.text = value.ToString("F2");
        }

        public void OnRightBindingAngleReset()
        {
            BindingsManager.instance.ResetRightBindingAngle();
            rightBindingAngleInputFieldTMPro.text = "0";
            UpdateRightBindingAngleValue(BindingsManager.instance.rightBindingAngleOffset);
        }

        //TOGGLES
        public void OnGoNutsToggleChanged(bool isOn)
        {
            ModLogger.Log("POUET");
            ModManager.instance.SetCrazyFactor(isOn);
        }

        public void OnBindingsVisibilityChanged(bool isOn)
        {
            BindingsManager.instance.SetBindingsVisibility(isOn);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;

namespace BoardSizeAndStanceMod
{
    class BindingsUpdater : MonoBehaviour
    {
        private float currentTime;
        private float endTime;
        private bool isTimerRunning;
        private MeshRenderer meshRenderer;
        private MeshFilter originalMeshFilter;
        public MeshFilter newMeshFilter;
        public bool isReplayBinding;
        private GameObject visualObject;
        private List<Transform> childrens = new List<Transform>();
        private Vector3 bindingPos;
        private Quaternion bindingRot;
            
        public BindingsUpdater(IntPtr ptr) : base(ptr) { }

        public void Init(GameObject in_visualObject, bool in_isReplayBinding)
        {
            isTimerRunning = true;
            meshRenderer = GetComponent<MeshRenderer>();
            originalMeshFilter = GetComponent<MeshFilter>();

            visualObject = in_visualObject;
            bindingPos = in_visualObject.transform.localPosition;
            bindingRot = in_visualObject.transform.localRotation;

            for (int i = 0; i < transform.GetChildCount(); i++)
                childrens.Add(transform.GetChild(i));

            isReplayBinding = in_isReplayBinding;

            newMeshFilter = in_visualObject.GetComponent<MeshFilter>();
        }

        void LateUpdate()
        {
            if (isTimerRunning)
            {
                currentTime += Time.deltaTime;
                if (currentTime < 0.1f)
                {
                    BindingsManager.instance.InitBindings();
                    if (newMeshFilter.mesh != originalMeshFilter.mesh && isReplayBinding)
                    {
                        newMeshFilter.mesh = originalMeshFilter.mesh;
                        newMeshFilter.GetComponent<MeshRenderer>().material = originalMeshFilter.GetComponent<MeshRenderer>().material;
                        ModLogger.Log($"Replacing binding meshes with mesh = {originalMeshFilter.mesh.name}");
                    }
                    SetPositionAndRotation();
                }
            }
            
            if (meshRenderer.enabled)
            {
                meshRenderer.enabled = false;
                //ModLogger.Log("Binding hidden");
            }
        }

        private void SetPositionAndRotation()
        {
            foreach (Transform transform in childrens)
            {
                transform.localPosition = bindingPos;
                transform.localRotation = bindingRot;
                Quaternion rotForIK = Quaternion.Euler(new Vector3(bindingRot.eulerAngles.x, bindingRot.eulerAngles.y + 180, bindingRot.eulerAngles.z + 90));
                if (transform.gameObject.name != "NewBindingVisual")
                    transform.localRotation = rotForIK;
            }
        }

        public void SetPosition(Vector3 in_bindingPos)
        {
            bindingPos = in_bindingPos;
            foreach (Transform transform in childrens)
                transform.localPosition = bindingPos;
        }

        public void SetRotation(Quaternion in_bindingRot)
        {
            bindingRot = in_bindingRot;
            Quaternion rotForIK = Quaternion.Euler(new Vector3(bindingRot.eulerAngles.x, bindingRot.eulerAngles.y + 180, bindingRot.eulerAngles.z + 90));
            foreach (Transform transform in childrens)
            {
                transform.localRotation = bindingRot;
                if (transform.gameObject.name != "NewBindingVisual")
                    transform.localRotation = rotForIK;
            }
        }

        public void ResetTimer()
        {
            currentTime = 0;
            isTimerRunning = true;

            childrens.Clear();
            for (int i = 0; i < transform.GetChildCount(); i++)
                childrens.Add(transform.GetChild(i));

            //ModLogger.Log("BindingUpdater reseted");
        }
    }
}

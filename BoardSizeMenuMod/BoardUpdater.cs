using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;

namespace BoardSizeAndStanceMod
{
    class BoardUpdater : MonoBehaviour
    {
        private float currentTime;
        private float endTime;
        private bool isTimerRunning;

        public BoardUpdater(IntPtr ptr) : base(ptr) { }

        /*
        void Start()
        {
            isTimerRunning = true;
            meshRenderer = GetComponent<MeshRenderer>();
            originalMeshFilter = GetComponent<MeshFilter>();
        }

        void LateUpdate()
        {
            if (isTimerRunning)
            {
                currentTime += Time.deltaTime;
                if (currentTime < 2f)
                {
                    
                }
            }
            if (meshRenderer.enabled)
            {
                meshRenderer.enabled = false;
                //ModLogger.Log("Binding hidden");
            }
        }

        public void ResetTimer()
        {
            currentTime = 0;
            isTimerRunning = true;
            ModLogger.Log("BindingUpdater reseted");
        }
        */
    }
}

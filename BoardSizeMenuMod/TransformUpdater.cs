using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoardSizeAndStanceMod
{
    class TransformUpdater : MonoBehaviour
    {
        public TransformUpdater(IntPtr ptr) : base(ptr) { }

        public Vector3 posOffset;
        public Quaternion angleOffset;
        public bool isPositionFixed;
        private Vector3 forcedPos;

        void Awake()
        {
            forcedPos = transform.localPosition;
        }

        void LateUpdate()
        {
            if (isPositionFixed)
            {
                Vector3 newPos = forcedPos;
                newPos += posOffset;
                transform.localPosition = newPos;
            }
            else
                transform.localPosition += posOffset;
            Quaternion newAngle = Quaternion.Euler(transform.localRotation.eulerAngles.x + angleOffset.eulerAngles.x, transform.localRotation.eulerAngles.y + angleOffset.eulerAngles.y, transform.localRotation.eulerAngles.z + angleOffset.eulerAngles.z);
            transform.localRotation = newAngle;
        }
    }
}

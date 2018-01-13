using Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Ai
{
    namespace Combat
    {
        public enum EKungFuStateAttack
        {
            IDLE,
            SELECTION,
            RUNNING
        }

        public class SlotData
        {
            public Vector3 firstPos;
            public Vector3 secondPos;
        }

        [System.Serializable]
        public abstract class KungFuCircle
            : MonoBehaviour
        {
            public EKungFuStateAttack state = EKungFuStateAttack.IDLE;

            [Range (60f, 360f)]
            public float degrees = 360f;

            [Range (0.5f, 10f)]
            public float radius;
            [Tooltip("key - available weight slots;\nval - capacity weight slots")]
            [SerializeField]
            public PairFloat slotsWeight;
            [SerializeField]
            public PairFloat timer;

            public int maximumSlots = 7;


            protected Vector3 ComputePosition(float degrees, float radius)
            {
                return new Vector3
                    (Mathf.Cos (Mathf.Deg2Rad * degrees) * radius
                    ,0f
                    ,Mathf.Sin (Mathf.Deg2Rad * degrees) * radius);
            }

            public float GetAvailableSlotsWeight ()
            {
                return slotsWeight.key;
            }

            public float GetCapacitySlotsWeight ()
            {
                return slotsWeight.val;
            }

            public bool UseAvailSlotsWeight (float weight)
            {
                if (slotsWeight.key < weight)
                {
                    return false;
                }

                slotsWeight.key -= weight;
                return true;
            }

            public void SetAvailableSlotsWeight (float weight)
            {
                slotsWeight.key = Mathf.Clamp (weight, 0f, slotsWeight.val);
            }

            public void ResetAvailableSlotsWeight ()
            {
                slotsWeight.key = slotsWeight.val;
            }

            public abstract bool Register (Subject enemy);
            public abstract bool Unregister (Subject enemy);
            public abstract bool IsContains (Subject enemy);
            public abstract ECircles GetCircleType ();

        }
    }
}

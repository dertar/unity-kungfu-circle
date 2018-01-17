using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ai
{
    namespace Combat
    {
        [Serializable]
        public class MeleeCircle
            : KungFuCircle
        {
            [SerializeField]
            private Vector3 []localPositions;

            private Dictionary<int, Pair<Subject,int>> engagedSlots; 

            public float GIZMO_RADIUS_POINT = 0.5f;
            public Color GIZMO_COLOR_POINT = Color.blue;
            public bool gizmo_draw = false;

            public List<Subject> debug = new List<Subject>();

            private void Awake ()
            {
                localPositions = new Vector3[maximumSlots];
                var arc = degrees / maximumSlots;
                for (int i = 0; i < maximumSlots; i++)
                {
                    localPositions[i] = ComputePosition (arc * i, radius);
                }

                engagedSlots = new Dictionary<int, Pair<Subject, int>> ();
            }

            private void Draw ()
            {
                for (int i = 0; i < maximumSlots; i++)
                {
                    DebugDrawing.DrawCircle 
                        (GetGlobalPosition (i)
                        ,GIZMO_RADIUS_POINT, GIZMO_COLOR_POINT);
                }
            }

            public Vector3 GetGlobalPosition (int slot)
            {
                return localPositions[slot] + transform.position;
            }

            public Vector3 GetGlobalPosition (Subject enemy)
            {
                int slot = engagedSlots[enemy.gameObject.GetInstanceID ()].Second;
                return GetGlobalPosition (slot);
            }

            public Vector3 GetGlobalPositionByInstance (int instance)
            {
                int slot = engagedSlots[instance].Second;
                return GetGlobalPosition (slot);
            }

            public override bool Register (Subject enemy)
            {
                int instance = enemy.gameObject.GetInstanceID ();

                if (engagedSlots.ContainsKey (instance))
                    return false;

                if (engagedSlots.Count >= maximumSlots)
                    return false;

                float availWeight = GetAvailableSlotsWeight ();
                float enemyWeight = enemy.GetSlotWeight ();

                if (availWeight < enemyWeight)
                    return false;

                engagedSlots.Add (instance
                    , new Pair<Subject, int>(enemy,0));

                SetAvailableSlotsWeight (availWeight - enemyWeight);
                debug.Add (enemy);
                FindNearestSlotToEnemies ();
                return true;
            }

            public override bool IsContains (Subject enemy)
            {
                return engagedSlots.ContainsKey (enemy.gameObject.GetInstanceID());
            }

            public override bool Unregister (Subject enemy)
            {
                bool isContains = IsContains (enemy);
                int instance = enemy.gameObject.GetInstanceID ();
                if (isContains)
                {
                    SetAvailableSlotsWeight (slotsWeight.key + enemy.GetSlotWeight ());
                    engagedSlots.Remove (instance);
                    debug.Remove (enemy);
                }

                return isContains;
            }

            public void Update ()
            {
                if(gizmo_draw)
                    Draw ();

                FindNearestSlotToEnemies ();
            }

            public void FindNearestSlotToEnemies ()
            {
                var center = transform.position;
                var freePositions = GetGlobalPositions ();
                var nonFreeSlots = new List<int> ();
                foreach (var val in engagedSlots.Values)
                {
                    var enemy = val.First;
                    
                    int slot = FindNearestSlotToEnemy (enemy, freePositions, nonFreeSlots);
                    nonFreeSlots.Add (slot);
                    engagedSlots[enemy.gameObject.GetInstanceID ()].Second = slot;
                }
            }

            private int FindNearestSlotToEnemy (Subject enemy
                                   , List<Vector3> globalPositions
                                   , List<int> nonFreeSlots)
            {
                var pos = enemy.transform.position;
                int imin = 0;
                float minDist = Vector3.Distance (pos, globalPositions[0]);

                for (int i = 1; i < globalPositions.Count; i++)
                {
                    if (!nonFreeSlots.Contains (i))
                    {
                        float distance = Vector3.Distance (pos, globalPositions[i]);

                        if (distance < minDist)
                        {
                            minDist = distance;
                            imin = i;
                        }
                    }
                }

                return imin;
            }

            private List<Vector3> GetGlobalPositions ()
            {
                var center = transform.position;
                List<Vector3> ret = new List<Vector3> ();

                for (int i = 0; i < localPositions.Length; i++)
                {
                    ret.Add (localPositions[i] + center);
                }

                return ret;
            }

            private List<Vector3> GetGlobalFreePositions ()
            {
                var center = transform.position;
                List<Vector3> ret = new List<Vector3> ();
                var engaged = engagedSlots.Values.ToList ().ConvertAll (x => x.Second);

                for (int i = 0; i < localPositions.Length; i++)
                {
                    if (!engaged.Contains (i))
                    {
                        ret.Add (localPositions[i] + center);
                    }
                }

                return ret;
            }



            public override ECircles GetCircleType ()
            {
                return ECircles.Melee;
            }


        }
    }
}

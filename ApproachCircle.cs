using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Ai
{
    namespace Combat
    {
        public class ApproachCircle 
            : KungFuCircle
        {
            private List<Pair<Vector3, Vector3>> localPositions;

            public Color GIZMO_RADIUS_START= Color.green;
            public Color GIZMO_RADIUS_END = Color.blue;

            public float offsetBetweenPoints = 5f;

            private Dictionary<int, Pair<Subject, int>> engagedSlots;
            public bool gizmo_draw = false;
            public float GIZMO_RADIUS_POINT = 0.5f;

            public List<Subject> debug = new List<Subject> ();

            public void Draw ()
            {
                
                foreach (var x in GetGlobalPosition ())
                {
                    DebugDrawing.DrawCircle (x.First, GIZMO_RADIUS_POINT, GIZMO_RADIUS_START);
                    DebugDrawing.DrawCircle (x.Second, GIZMO_RADIUS_POINT, GIZMO_RADIUS_END);
                }
            }

            private void Awake ()
            {
                localPositions = new List<Pair<Vector3, Vector3>> ();


                var arc = degrees / maximumSlots;
                for (int i = 0; i < maximumSlots; i++)
                {
                    localPositions.Add
                        (new Pair<Vector3,Vector3>(ComputePosition (arc * i, radius)
                        , ComputePosition ((arc * (i + 1)) - offsetBetweenPoints, radius)));
                }

                engagedSlots = new Dictionary<int, Pair<Subject, int>> ();
            }

            public override ECircles GetCircleType ()
            {
                return ECircles.Approach;
            }

            public override bool IsContains (Subject enemy)
            {
                return engagedSlots.ContainsKey (enemy.gameObject.GetInstanceID());
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
                    , new Pair<Subject, int> (enemy, 0));

                SetAvailableSlotsWeight (availWeight - enemyWeight);
                debug.Add (enemy);
                FindNearestSlotToEnemies ();
                return true;
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

            public void FindNearestSlotToEnemies ()
            {
                var center = transform.position;
                var globalPositions = GetGlobalPosition ();

                foreach (var val in engagedSlots.Values)
                {
                    var enemy = val.First;
                    int slot = FindNearestSlotToEnemy (enemy, globalPositions);
                    globalPositions.RemoveAt (slot);

                    engagedSlots[enemy.gameObject.GetInstanceID ()].Second = slot;
                }
            }

            private int FindNearestSlotToEnemy (Subject enemy
                                   , List<Pair<Vector3,Vector3>> globalPositions)
            {
                var pos = enemy.transform.position;
                int imin = 0;
                float minDist = Vector3.Distance (pos, globalPositions[0].First);

                for (int i = 1; i < globalPositions.Count; i++)
                {
                    float distance = Vector3.Distance (pos, globalPositions[i].First);

                    if (distance < minDist)
                    {
                        minDist = distance;
                        imin = i;
                    }
                }

                return imin;
            }

            public Pair<Vector3,Vector3> GetGlobalPosition (int slot)
            {
                return new Pair<Vector3, Vector3>
                    (localPositions[slot].First + transform.position
                    , localPositions[slot].Second + transform.position);
            }

            public Pair<Vector3, Vector3> GetGlobalPosition (Subject enemy)
            {
                int slot = engagedSlots[enemy.gameObject.GetInstanceID ()].Second;
                return GetGlobalPosition (slot);
            }

            public Pair<Vector3, Vector3> GetGlobalPositionByInstance (int instance)
            {
                int slot = engagedSlots[instance].Second;
                return GetGlobalPosition (slot);
            }

            private List<Pair<Vector3,Vector3>> GetGlobalPosition ()
            {
                var center = transform.position;
                var ret = new List<Pair<Vector3, Vector3>>();

                for (int i = 0; i < localPositions.Count; i++)
                {
                    ret.Add 
                        (new Pair<Vector3, Vector3>
                        (localPositions[i].First + center
                        ,localPositions[i].Second + center));
                }

                return ret;
            }


            public void Update ()
            {
                if(gizmo_draw) Draw ();
            }
        }
    }
}

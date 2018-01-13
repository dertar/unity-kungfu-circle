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

        public class CombatManager
            : MonoBehaviour
        {
            private Dictionary<int, FightingCircle> circles;

            public FightingCircle[] debug;
            public bool isDebug = false;

            private void Awake ()
            {
                circles = new Dictionary<int, FightingCircle> ();
            }

            public bool RegisterFightingCircle (FightingCircle circle)
            {
                int instance = circle.gameObject.GetInstanceID ();
                if (circles.ContainsKey (instance))
                {
                    return false;
                }

                circles.Add (instance, circle);
                if (isDebug) debug = circles.Values.ToArray ();

                return true;
            }

            public bool MoveBetweenCircle (Subject attacker, Subject defender, ECircles to)
            {
                int instanceDefender = defender.gameObject.GetInstanceID ();

                var circle = circles[instanceDefender];
                return circle.Move (attacker, to);
            }

            public Vector3 GetSlotPositionFromMeleeCircle
                (Subject attacker, Subject defender)
            {
                int instanceDefender = defender.gameObject.GetInstanceID ();

                if (!circles.ContainsKey (instanceDefender))
                    return Vector3.zero;

                var circle = circles[instanceDefender];

                return circle.GetSlotPositionFromMeleeCircle (attacker);
            }

            public Pair<Vector3, Vector3> GetSlotPositionFromApproachCircle
                            (Subject attacker, Subject defender)
            {
                int instanceDefender = defender.gameObject.GetInstanceID ();

                if (!circles.ContainsKey (instanceDefender))
                    return null;

                var circle = circles[instanceDefender];

                return circle.GetSlotPositionFromApproachCircle (attacker);
            }

            //public bool RequestActionCombat (Subject attacker, Subject defender)
            //{
            //    int instanceDefender = defender.gameObject.GetInstanceID ();

            //    return circles[instanceDefender].RequestActionAttack (attacker);
            //}

            //public bool CloseActionCombat (Subject attacker, Subject defender)
            //{
            //    int instaceDefender = defender.gameObject.GetInstanceID ();

            //    return circles[instaceDefender].CloseActionAttack (attacker);
            //}

            public bool RegisterActionAttack (Subject attacker, Subject defender, float weight)
            {
                int instanceDefender = defender.gameObject.GetInstanceID ();

                return circles[instanceDefender].RegisterAction (attacker, weight);
            }

            public bool UnregisterActionAttack (Subject attacker, Subject defender, float weight)
            {
                int instanceDefender = defender.gameObject.GetInstanceID ();

                return circles[instanceDefender].UnregisterAction (attacker, weight);
            }

            public bool RegisterToCircle (Subject attacker, Subject defender, ECircles type)
            {
                int instanceDefender = defender.gameObject.GetInstanceID ();

                return circles[instanceDefender].Register (attacker, type);
            }

            //public bool IsAvailableAction (Subject attacker, Subject defender)
            //{
            //    int instanceDefender = defender.gameObject.GetInstanceID ();

            //    if (!circles.ContainsKey (instanceDefender))
            //        return false;

            //    var circle = circles[instanceDefender];
            //    return circle.IsAvailableAction (attacker);
            //}

            //public ActionCombat GetAction (Subject attacker, Subject defender)
            //{
            //    int instanceDefender = defender.gameObject.GetInstanceID ();


            //    if (!circles.ContainsKey (instanceDefender))
            //        return null;

            //    var circle = circles[instanceDefender];
            //    return circle.GetActionCombat (attacker);
            //}

            //public Vector3 GetSlotPosition (Subject attacker, Subject defender)
            //{
            //    int instanceDefender = defender.gameObject.GetInstanceID ();

            //    return circles[instanceDefender].GetSlotPosition (attacker);
            //}
        }
    }
}

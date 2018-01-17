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
        public enum ECircles
        {
            Melee,
            Approach,
            Range
        }


        public class KungFuData
        {
            //public ActionsManager actionManager;

            public Subject subject;
            public ECircles type;

            public KungFuData (Subject subject, ECircles type)
            {
                this.subject = subject;
                this.type = type;
                //this.actionManager = subject.GetActionsManager ();
            }


        }

        [RequireComponent (typeof (MeleeCircle))]
        [RequireComponent (typeof (ApproachCircle))]
        [RequireComponent (typeof (RangeCircle))]

        public class FightingCircle :
           MonoBehaviour
        {
            private Dictionary<int, KungFuData> enemies;

            private Dictionary<ECircles, KungFuCircle> circles;

            [SerializeField]
            [Tooltip ("First - available attacks\nSecond - max attacks")]
            public PairFloat attacksWeight;

            private void Awake ()
            {
                circles = new Dictionary<ECircles, KungFuCircle> ();

                var dataCircles = GetComponents<KungFuCircle> ();

                for (int i = 0; i < dataCircles.Length; i++)
                {
                    circles.Add (dataCircles[i].GetCircleType ()
                                , dataCircles[i]);
                }
                enemies = new Dictionary<int, KungFuData> ();
            }

            public bool Register (Subject enemy, ECircles type)
            {
                if (IsContains (enemy))
                {
                    return Move (enemy, type);
                }

                var circle = circles[type];
                bool isRegistered = circle.Register (enemy);

                if (isRegistered)
                {
                    enemies.Add (enemy.gameObject.GetInstanceID ()
                        , new KungFuData (enemy, type));

                }

                return isRegistered;
            }

            public bool Move (Subject enemy, ECircles to)
            {
                int instance = enemy.gameObject.GetInstanceID ();

                var data = enemies[instance];

                if (to.Equals (data.type))
                    return true;

                var fromCircle = circles[data.type];
                var toCircle = circles[to];

                fromCircle.Unregister (enemy);
                bool isSwapped = toCircle.Register (enemy);

                if (isSwapped)
                {
                    data.type = to;
                }

                return isSwapped;
            }

            public bool Unregister (Subject enemy)
            {
                int instance = enemy.gameObject.GetInstanceID ();

                if (!enemies.ContainsKey (instance))
                    return false;

                var type = enemies[instance].type;
                var circle = circles[type];

                bool isUnregistered = circle.Unregister (enemy);

                if (isUnregistered)
                {
                    enemies.Remove (instance);
                }

                return isUnregistered;
            }

            public bool IsContains (Subject enemy)
            {
                return enemies.ContainsKey (enemy.gameObject.GetInstanceID ());
            }

            public MeleeCircle GetMeleeCircle ()
            {
                return circles[ECircles.Melee] as MeleeCircle;
            }

            public Vector3 GetSlotPositionFromMeleeCircle (Subject enemy)
            {
                var melee = GetMeleeCircle ();
                int instance = enemy.gameObject.GetInstanceID ();

                var type = melee.GetGlobalPosition (enemy);

                return melee.GetGlobalPositionByInstance (instance);
            }


            public Pair<Vector3, Vector3> GetSlotPositionFromApproachCircle (Subject enemy)
            {
                var approach = GetApproachCircle ();
                int instance = enemy.gameObject.GetInstanceID ();

                var type = approach.GetGlobalPosition (enemy);

                return approach.GetGlobalPositionByInstance (instance);
            }

            private ApproachCircle GetApproachCircle ()
            {
                return circles[ECircles.Approach] as ApproachCircle;
            }

            public bool RegisterAction (Subject enemy, float weight)
            {
                int instance = enemy.gameObject.GetInstanceID ();

                if (!enemies.ContainsKey (instance))
                    return false;

                if (attacksWeight.key < weight)
                    return false;

                attacksWeight.key -= weight;

                return true;
            }

            public bool UnregisterAction (Subject enemy, float weight)
            {
                int instance = enemy.gameObject.GetInstanceID ();

                if (!enemies.ContainsKey (instance))
                    return false;

                attacksWeight.key = Mathf.Clamp (attacksWeight.key + weight, 0f, attacksWeight.val);

                return true;
            }

            //public bool IsAvailableAction (Subject enemy)
            //{
            //    int instance = enemy.gameObject.GetInstanceID ();

            //    return enemies[instance].actionManager.HasCurrentAction ();
            //}

            //public ActionCombat GetActionCombat (Subject enemy)
            //{
            //    int instance = enemy.gameObject.GetInstanceID ();

            //    return enemies[instance].actionManager.GetCurrentAction ();
            //}

            //public bool RequestActionAttack (Subject enemy)
            //{
            //    int instance = enemy.gameObject.GetInstanceID ();

            //    if (!enemies.ContainsKey (instance))
            //        return false;

            //    DistributeActionAttack (enemies[instance]);

            //    return true;
            //}

            //public bool CloseActionAttack (Subject enemy)
            //{
            //    int instance = enemy.gameObject.GetInstanceID ();

            //    if (!enemies.ContainsKey (instance))
            //        return false;

            //    var data = enemies[instance];

            //    if (data.actionManager.GetCurrentAction().Is (EActionState.RUNNING))
            //    {
            //        return false;
            //    }

            //    IncWeight (data.actionManager.GetCurrentAction ().weight);

            //    return true;
            //}

            //public bool RequestActionDefence(Subject enemy)
            //{
            //    int instance = enemy.gameObject.GetInstanceID ();

            //    if (!enemies.ContainsKey (instance))
            //        return false;

            //    DistributeActionAttack (enemies[instance]);

            //    return true;
            //}

            //public bool CloseActionDefence (Subject enemy)
            //{
            //    int instance = enemy.gameObject.GetInstanceID ();

            //    if (!enemies.ContainsKey (instance))
            //        return false;

            //    var data = enemies[instance];

            //    if (data.actionManager.GetCurrentAction ().Is (EActionState.RUNNING))
            //    {
            //        return false;
            //    }

            //    IncWeight (data.actionManager.GetCurrentAction ().weight);

            //    return true;
            //}


            //private ActionCombat FindMaximumWeightInActions (List<ActionAttack> actions, float maxWeight)
            //{
            //    if (actions.Count <= 0)
            //        return null;

            //    int max = 0;


            //    for (int i = 1; i < actions.Count; i++)
            //    {
            //        if (CheckFit(actions[i], maxWeight)
            //            && actions[i].weight <= attacksWeight.key)
            //            max = i;
            //    }

            //    return CheckFit (actions[max], maxWeight)
            //        ? actions[max]
            //        : null;
            //}

            //private bool CheckFit (ActionCombat action, float maxWeight)
            //{
            //    return !action.cooldown
            //        && action.weight < maxWeight;
            //}

            //private void DistributeActionAttack (KungFuData enemy)
            //{
            //    if (enemy.actionManager.GetCurrentAction() == null)
            //    {
            //        var action = FindMaximumWeightInActions (enemy.actionManager.GetActionsAttack(), attacksWeight.key);

            //        if (action != null)
            //        {
            //            action.Idle ();

            //            DecWeight (action.weight);

            //            enemy.actionManager.SetCurrentAction (action);
            //        }
            //    }

            //}


            //private void IncWeight (float weight)
            //{
            //    attacksWeight.key = Mathf.Clamp(attacksWeight.key + weight, 0f, attacksWeight.val);
            //}

            //private void DecWeight (float weight)
            //{
            //    attacksWeight.key = Mathf.Clamp (attacksWeight.key - weight, 0f, attacksWeight.val);
            //}



            private void Update()
            {

            }
        }
    }
}

using Ai.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(FightingCircle))]
    public abstract class Subject : MonoBehaviour
    {
        Animator anim;

        protected ActionsManager actionsManager;

        protected ActionCombat currentAction = null;

        public float slotWeight = 1.0f;

        private bool TriggerAction (string key)
        {
            var ret = ACTIONS[key] (this);

            return ret;
        }

        public bool TriggerAction (ActionCombat attack)
        {
            if (attack == null)
                return false;

            currentAction = attack;

            return TriggerAction (attack.keyAction);
        }

        public void SetAnimationTrigger (string trigger)
        {
            anim.SetTrigger (trigger);
        }

        public float GetSlotWeight ()
        {
            return slotWeight;
        }

        protected static Dictionary<string, Func<Subject, bool>> ACTIONS =
            new Dictionary<string, Func<Subject, bool>> ()
            {
                    {"Attack",
                    new Func<Subject, bool>(
                        (s) => {
                            //if (s.OnGround())
                            {
                                s.SetAnimationTrigger("Attack");
                                return true;
                            }
                            //return false;

                        })
                    },
                    {"Death",
                    new Func<Subject, bool>(
                        (s) => {
                            //if (s.OnGround())
                            {
                                s.SetAnimationTrigger("Death");
                                return true;
                            }
                            //return false;
                        })
                    }
            };
    }
}

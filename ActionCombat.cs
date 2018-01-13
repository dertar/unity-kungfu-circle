using Character;
using System.Collections;
using UnityEngine;
using System;

namespace Ai
{
    namespace Combat
    {
        public enum EActionState
        {
            IDLE,
            RUNNING,
            FINISHED
        }

        public abstract class ActionCombat 
            : MonoBehaviour
        {
            [SerializeField]
            private EActionState state;
            protected EActionType action;

            public string keyAction;

            public float weight;
            [Tooltip("key - cd\nval - time")]
            public PairFloat timer;

            
            public bool cooldown = true;

            public string nameAction;

            protected Subject me;
            public float distance = 3f;

            private void Awake ()
            {
                me = GetComponent<Subject>();
                state = EActionState.IDLE;
            }

            public EActionType GetActionType()
            {
                return action;
            }

            public bool GetCooldown ()
            {
                return cooldown;
            }

            public virtual void Execute ()
            {
                me.TriggerAction (this);
                timer.key = 0f;
                cooldown = true;

                state = EActionState.RUNNING;
            }

            public bool IsRunning ()
            {
                return state == EActionState.RUNNING;
            }

            public void Stop ()
            {
                state = EActionState.FINISHED;
            }

            public EActionState GetState ()
            {
                return state;
            }

            public void Idle ()
            {
                state = EActionState.IDLE;
            }

            public bool Is (EActionState state)
            {
                return this.state == state;
            }

            public void Update ()
            {
                if (cooldown)
                {
                    timer.key += Time.deltaTime;
                    if (timer.key >= timer.val)
                    {
                        cooldown = false;
                    }
                }
            }

            public abstract bool Next ();
        }
    }
}

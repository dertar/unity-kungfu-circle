using Ai.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Character
{

    public class ActionsManager
        : MonoBehaviour
    {
        //private Dictionary<string, ActionAttack> actionsAttack;
        //private Dictionary<string, ActionDefence> actionsDefence;
        //private Dictionary<string, ActionSupport> actionsSupport;
        private Dictionary<string, ActionCombat> actions;
        private ActionCombat currentAction = null;
        //private List<ActionCombat> queue;

        private void Awake ()
        {
            //actionsAttack = GetComponents<ActionAttack> ().Select (x => new { x.nameAction, x }).ToDictionary (k => k.nameAction, v => v.x);
            //actionsDefence = GetComponents<ActionDefence> ().Select (x => new { x.nameAction, x }).ToDictionary (k => k.nameAction, v => v.x);
            //actionsSupport = GetComponents<ActionSupport> ().Select (x => new { x.nameAction, x }).ToDictionary (k => k.nameAction, v => v.x);
            actions = GetComponents<ActionCombat> ().Select (x => new { x.nameAction, x }).ToDictionary (k => k.nameAction, v => v.x);
            //queue = new List<ActionCombat> ();
        }


        //public ActionCombat GetAction (string key)
        //{
        //    if (actionsAttack.ContainsKey (key))
        //        return actionsAttack[key];
        //    else if (actionsDefence.ContainsKey (key))
        //        return actionsDefence[key];
        //    else if (actionsSupport.ContainsKey (key))
        //        return actionsSupport[key];
        //    return null;
        //}

        public ActionCombat GetAction (string key)
        {
            return actions[key];
        }

        void Start ()
        {

        }

        void Update ()
        {

        }

        public void SetCurrentAction (ActionCombat action)
        {
            currentAction = action;
        }

        public ActionCombat GetCurrentAction ()
        {
            return currentAction;
        }

        public bool HasCurrentAction ()
        {
            return currentAction != null;
        }

        //public void PushAction (ActionCombat actionCombat)
        //{
        //    queue.Add (actionCombat);
        //}

        //public ActionCombat PopAction ()
        //{
        //    if (queue.Count > 0)
        //    {
        //        currentAction = queue[0];
        //        queue.RemoveAt (0);
        //    }
        //    else
        //    {
        //        currentAction = null;
        //    }

        //    return currentAction;
        //}
    }
}

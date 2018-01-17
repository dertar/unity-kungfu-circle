using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    namespace Combat
    {
        public class ActionDefence 
            : ActionCombat
        {
            public int lastHits;
            public Vector3 side = Vector3.forward;

            public ActionDefence ()
            {
                this.action = EActionType.BLOCK;
            }

            public override bool Next ()
            {
                return false;
            }

        }

    }
}

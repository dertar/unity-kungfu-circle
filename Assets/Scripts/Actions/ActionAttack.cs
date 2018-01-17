using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    namespace Combat
    {
        //[Serializable]
        //public class ActionStrike
        //{
        //    public string keyAction;
        //    public float damage;
        //    public float fear;
        //    public EAttackType type;
        //}

        public class ActionAttack 
            : ActionCombat
        {
            [Tooltip("key-min\nval-max")]
            public PairFloat damage;
            [Tooltip ("key-min\nval-max")]
            public PairFloat fear;
            public EAttackType type;

            public int combo = 1;

            public int iCombo = 0;
            //public AttackType attackType;

            public ActionAttack()
            {
                base.action = EActionType.ATTACK;
            }

            public override void Execute ()
            {
                base.Execute ();
                
                iCombo = 0;
            }

            public override bool Next ()
            {
                return ++iCombo < combo;
            }


        }
    }
}
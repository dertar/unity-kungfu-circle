using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
namespace Ai
{
    namespace Combat
    {
        public class RangeCircle 
            : KungFuCircle
        {
            public List<Vector3> positions = new List<Vector3>();

            public float maxRadius;

            public Color GIZMO_RADIUS_MIN = Color.green;
            public Color GIZMO_RADIUS_MAX = Color.green;

            public void Draw ()
            {
                var center = transform.position;
                DebugDrawing.DrawCircle (center, radius, GIZMO_RADIUS_MIN);
                DebugDrawing.DrawCircle (center, maxRadius, GIZMO_RADIUS_MAX);
            }

            public override ECircles GetCircleType ()
            {
                return ECircles.Range;
            }

            public override bool IsContains (Subject enemy)
            {
                throw new NotImplementedException ();
            }

            public override bool Register (Subject enemy)
            {
                throw new NotImplementedException ();
            }

            public override bool Unregister (Subject enemy)
            {
                throw new NotImplementedException ();
            }

            public void Update ()
            {
                Draw ();
            }
        }
    }
}


using UnityEngine;
using System.Collections;

public static class DebugDrawing {

    public static void DrawLine(float time, params Vector3[] points) {
#if UNITY_EDITOR
        DrawLine(Color.white, time, points);
#endif
    }
    public static void DrawLine(Color color, float time, params Vector3[] points) {
#if UNITY_EDITOR
        for (int i = 0; i < points.Length - 1; i++) {
            if (time == 0f)
                Debug.DrawLine(points[i], points[i + 1], color);
            else
                Debug.DrawLine(points[i], points[i + 1], color, time);
        }
#endif
    }

    public static void DrawElipse(Vector3 position, float radiusX, float radiusY, float time = 0f) {
#if UNITY_EDITOR
        DrawElipse(position, radiusX, radiusY, Color.white, time);
#endif
    }
    public static void DrawElipse(Vector3 position, float radiusX, float radiusY, Color color, float time = 0f) {
#if UNITY_EDITOR
        int step = 1;
        for (int angle = 0; angle <= 360; angle += step) {
            var radAngl_0 = angle * Mathf.Deg2Rad;
            var radAngl_1 = (angle + step) * Mathf.Deg2Rad;

            var point_0 = position + GetElipsePoint(radAngl_0, radiusX, radiusY);//new Vector3(Mathf.Cos(radAngl_0), 0f, Mathf.Sin(radAngl_0)) * radius;
            var point_1 = position + GetElipsePoint(radAngl_1, radiusX, radiusY);//new Vector3(Mathf.Cos(radAngl_1), 0f, Mathf.Sin(radAngl_1)) * radius;

            if (time == 0f)
                Debug.DrawLine(point_0, point_1, color);
            else
                Debug.DrawLine(point_0, point_1, color, time);
        }
#endif
    }
    static Vector3 GetElipsePoint(float t, float radiusX, float radiusY) {
        return new Vector3(radiusX * Mathf.Cos(t), 0f, radiusY * Mathf.Sin(t));
    }
    public static void DrawCircle(Vector3 position, float radius, float time = 0f) {
#if UNITY_EDITOR
        DrawCircle(position, radius, Color.white, time);
#endif
    }
    public static void DrawCircle(Vector3 position, float radius, Color color, float time = 0f) {
#if UNITY_EDITOR
        int step = 1;
        for (int angle = 0; angle <= 360; angle += step) {
            var radAngl_0 = angle * Mathf.Deg2Rad;
            var radAngl_1 = (angle + step) * Mathf.Deg2Rad;

            var point_0 = position + new Vector3(Mathf.Cos(radAngl_0), 0f, Mathf.Sin(radAngl_0)) * radius;
            var point_1 = position + new Vector3(Mathf.Cos(radAngl_1), 0f, Mathf.Sin(radAngl_1)) * radius;

            if (time == 0f)
                Debug.DrawLine(point_0, point_1, color);
            else
                Debug.DrawLine(point_0, point_1, color, time);
        }
#endif
    }
}

// Ignore Spelling: Collider

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class LineGenerator : MonoBehaviour
{
    LineRenderer lineRenderer;
    PolygonCollider2D polyCollider;
    [SerializeField] float tSegmentVisualLength;

    public LineRenderer LineRenderer
    {
        get
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            return lineRenderer;
        }
    }

    public PolygonCollider2D PolygonCollider
    {
        get
        {
            if (polyCollider == null)
            {
                polyCollider = GetComponent<PolygonCollider2D>();
            }

            return polyCollider;
        }
    }

    private void Update()
    {
        if (Application.isPlaying) return;
        if (LineRenderer == null) return;
        if (transform.childCount < 2) return;

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 p1 = transform.GetChild(i).localPosition;

            if (i == 0 /*|| i == transform.childCount - 1*/)
            {
                points.Add(p1);
                continue;
            }


            float t = 0;
            if (tSegmentVisualLength <= 0) tSegmentVisualLength = 0.1f;
            while (t < 1)
            {
                points.Add(HermitePoint(t, transform.GetChild(i - 1), transform.GetChild(i)));
                t += tSegmentVisualLength;
            }

            points.Add(transform.GetChild(i).localPosition);
        }

        LineRenderer.positionCount = points.Count;
        LineRenderer.SetPositions(points.ToArray());

        if (PolygonCollider == null) return;

        Vector2[] colliderPoints = new Vector2[points.Count * 2];

        Vector2 dir = Vector2.right;
        for (int i = 0; i < points.Count; i++)
        {
            if (i + 1 < points.Count)
            {
                dir = (points[i + 1] - points[i]).normalized;
                dir = new Vector2(-dir.y, dir.x);
            }
            colliderPoints[i] = (Vector2)points[i] + dir * LineRenderer.widthMultiplier / 2;
            colliderPoints[colliderPoints.Length - 1 - i] = (Vector2)points[i] - dir * LineRenderer.widthMultiplier / 2;
        }

        PolygonCollider.points = colliderPoints;
    }

    Vector3 HermitePoint(float t, Transform point1, Transform point2)
    {
        Vector3 v0 = point1.right * point1.localScale.x;
        Vector3 v1 = point2.right * point2.localScale.x;
        Vector3 p0 = point1.localPosition;
        Vector3 p1 = point2.localPosition;

        Vector3 a = v0 + 2 * p0 - 2 * p1 + v1;
        Vector3 b = -v1 - 2 * v0 - 3 * p0 + 3 * p1;
        Vector3 c = v0;
        Vector3 d = p0;
        Vector3 point = (a * Mathf.Pow(t, 3)) + (b * Mathf.Pow(t, 2)) + (c * t) + d;
        Vector3 forward = Vector3.Lerp(v0, v1, t).normalized;
        Vector3 right = new Vector3(-forward.y, forward.x, forward.z);
        //Debug.DrawRay(transform.position + point, forward, Color.red);
        //Debug.DrawRay(transform.position + point, right, Color.green);
        return point;
    }
}


public static class Vector2Extensions
{
    public static Vector2[] Convert(this Vector3[] points)
    {
        Vector2[] vector2s = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            vector2s[i] = points[i];
        }

        return vector2s;
    }
}

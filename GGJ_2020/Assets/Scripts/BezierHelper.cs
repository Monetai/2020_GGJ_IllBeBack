using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierHelper : MonoBehaviour
{
    // https://stackoverflow.com/questions/6711707/draw-a-quadratic-b%C3%A9zier-curve-through-three-given-points
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 point = p0 * t * t + p1 * 2 * t * (1 - t) + p2 * (1 - t) * (1 - t); //P0 * t ^ 2 + P1 * 2 * t * (1 - t) + P2 * (1 - t) ^ 2
        return point;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Def
{
    public static bool isPointInsideSphere(Vector3 point, Vector3 sphere, float radius)
    {
        // we are using multiplications because is faster than calling Math.pow
        var distance = Mathf.Sqrt((point.x - sphere.x) * (point.x - sphere.x) +
                                 (point.y - sphere.y) * (point.y - sphere.y) +
                                 (point.z - sphere.z) * (point.z - sphere.z));
        return distance < radius;
    }
}
using UnityEngine;

public static class VectorHelper
{
    //Checks if two vector are somewhat similar
    public static bool ApproximatelyEqual(Vector3 a, Vector3 b, float threshold)
    {
        return Mathf.Abs(a.x - b.x) <= threshold &&
               Mathf.Abs(a.y - b.y) <= threshold &&
               Mathf.Abs(a.z - b.z) <= threshold;
    }
}
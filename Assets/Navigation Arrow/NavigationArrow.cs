using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NavigationArrow : MonoBehaviour
{
    public List<GameObject> navigation_points;
    [FormerlySerializedAs("hand")] public GameObject hand_rotation_parent;

    public GameObject closest_point;
    public float closest_distance;
    public bool navigating = true;

    private void Awake()
    {
        if (navigation_points.Count > 0)
        {
            closest_point = navigation_points[0];
        }
    }

    private void Update()
    {
        if (navigation_points.Count > 0 && navigating)
        {
            determineClosestPoint();
            rotateArrow();
        }
        else
        {
            // If nothing to point at, point forward.
            hand_rotation_parent.transform.localRotation = Quaternion.identity;
        }
    }

    private void rotateArrow()
    {
        Vector3 direction = closest_point.transform.position - hand_rotation_parent.transform.position;
        direction = direction.normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);
        hand_rotation_parent.transform.rotation = rotation;
        hand_rotation_parent.transform.localRotation = Quaternion.Euler(0.0f, hand_rotation_parent.transform.localRotation.eulerAngles.y, 0.0f);
    }

    private void determineClosestPoint()
    {
        foreach (var point in navigation_points)
        {
            closest_distance = Vector3.Distance(hand_rotation_parent.transform.position, closest_point.transform.position);
            float hand_point_distance = Vector3.Distance(hand_rotation_parent.transform.position, point.transform.position);

            if (Mathf.Abs(hand_point_distance) < Mathf.Abs(closest_distance))
            {
                closest_point = point;
            }
        }
    }
}

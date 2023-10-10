using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/DebrisTimeline")]
[System.Serializable]
public class DebrisTimeline : ScriptableObject
{
    public List<Pair<float, DebrisTimelineElement>> timeline;
}

[System.Serializable]
public class DebrisTimelineElement
{
    public DebrisTimelineElement(int _building_id, int _debris_index, float _force, DebrisHandler.DebrisType _type)
    {
        building_id = _building_id;
        type = _type;
        force = _force;
        debris_index = _debris_index;
    }
    
    public int building_id;
    public float force;
    public DebrisHandler.DebrisType type;
    public int debris_index;
}
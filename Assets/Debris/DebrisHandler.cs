using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebrisHandler : MonoBehaviour
{
    public enum DebrisType
    {
        BRICK,
        CONCRETE_SLAB,
        CONCRETE_CHUNK,
        BRICKWORK_CHUNK,
        CINDER_BLOCK,
        
        COUNT
    }

    public List<Triple<DebrisType, GameObject, int>> debris_prefabs;
    private Dictionary<DebrisType, List<Pair<GameObject, DebrisScript>>> debris_pool;

    public float debris_lifetime = 0;
    public Vector3 debris_min_scale;

    public SpawnPointsMap spawn_point_mesh_map;
    public float distance_from_player_cutoff = 20;

    private void Awake()
    {
        debris_pool = new Dictionary<DebrisType, List<Pair<GameObject, DebrisScript>>>();

        foreach (var triple in debris_prefabs)
        {
            for (int i = 0; i < triple.third; i++)
            {
                var debris_object = Instantiate(triple.second, transform, true);
                var debris_script = debris_object.GetComponent<DebrisScript>();

                if (!debris_pool.ContainsKey(triple.first))
                {
                    debris_pool.Add(triple.first, new List<Pair<GameObject, DebrisScript>>());
                }

                debris_pool[triple.first].Add(new Pair<GameObject, DebrisScript>(debris_object, debris_script));
            }
        }
    }

    public void triggerDebris(DebrisTimelineElement _debris_data)
    {
        if (!debris_pool.ContainsKey(_debris_data.type))
        {
            Debug.Log("Debris pool dictionary does not contain key " + _debris_data.type);
            return;
        }
        
        foreach (var debris in debris_pool[_debris_data.type])
        {
            if (!debris.first.activeSelf)
            {
                var building = GameManager.Instance.BuildingManager.getBuilding(_debris_data.building_id);
                var spawn_point_data = spawn_point_mesh_map.map[building.third.type][building.third.state][_debris_data.debris_index];

                if (spawn_point_data.IsUnityNull()) return;
                
                Vector3 point = building.second.transform.TransformPoint(spawn_point_data.spawn_point);

                if (Vector3.Distance(GameManager.Instance.Player.transform.position, point) >
                    distance_from_player_cutoff) return;
                
                Vector3 direction = building.second.transform.TransformDirection(spawn_point_data.direction);
                
                debris.first.SetActive(true);
                debris.first.transform.position = point;
                debris.first.GetComponent<Rigidbody>().AddForce(direction * _debris_data.force, ForceMode.Impulse);
                
                return;
            }
        }
        
        //Debug.Log("No available " + _debris_data.type + " debris in the pool - consider increasing the pool");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/DebrisEditorData")]
public class DebrisEditorData : ScriptableObject
{
    public string safe_risk_tag = "Black";
    public string low_risk_tag = "Beige";
    public string mid_risk_tag = "Yellow";
    public string high_risk_tag = "Red";

    public int safe_risk_debris_max;
    public int low_risk_debris_max;
    public int mid_risk_debris_max;
    public int high_risk_debris_max;

    public int safe_risk_debris_min;
    public int low_risk_debris_min;
    public int mid_risk_debris_min;
    public int high_risk_debris_min;

    public int points_per_mesh;

    public float direction_max_horizontal_rotation;
    public float direction_min_horizontal_rotation;
    public float direction_max_vertical_rotation;
    public float direction_min_vertical_rotation;
    
    public float max_force;
    public float min_force;

    public float debris_timeline_start;
    public float debris_timeline_end;

    public DebrisTimeline timeline;
    public SpawnPointsMap mesh_spawn_points_map;
    public List<MeshBuildingStateMap> building_maps;

    public bool timeline_follows_earthquake_intensity_curve;
    
    public GameObject debug_prefab;
    public GameObject debug_parent_prefab;
    
    public BuildingManager.BuildingState debug_view_state;
}

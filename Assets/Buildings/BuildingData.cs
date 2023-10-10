using System;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public bool overRideData = true;
    public int id;
    public MeshBuildingStateMap building_map;

    public BuildingManager.BuildingType type = BuildingManager.BuildingType.BASIC_SET;
    public BuildingManager.BuildingState state = BuildingManager.BuildingState.NO_DAMAGE;
    public Vector3 original_position;

    public MeshCollider building_collider;
    public MeshFilter mesh_filter;
    public MeshRenderer mesh_renderer;

    public SetCompositeBuildingProperties build_prop;

    private void Awake()
    {
        original_position = transform.position;
        if (!overRideData) return;
        
        building_collider = GetComponent<MeshCollider>();
        mesh_renderer = GetComponent<MeshRenderer>();
        mesh_filter = GetComponent<MeshFilter>();
    }

    public void DamageBuilding()
    {
        build_prop.LoadNextState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/BuildingStateMeshMap")]
public class MeshBuildingStateMap : ScriptableObject
{
    // This is painful and I just want a dictionary, but dictionaries won't show in the inspector, so the list will have to do. 
    // As this is a list, please take care to add meshes in order from most damaged to least. 
    public List<Triple<BuildingManager.BuildingType, BuildingManager.BuildingState, Mesh>> states;
}

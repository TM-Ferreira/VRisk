using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SetCompositeBuildingProperties : MonoBehaviour
{
    public DebrisSelector debrisSelector;
    
    [SerializeField] private List<GameObject> buildingGroup;
    [SerializeField] private int nOfElements = 4;

    public enum BuildingState : int
    {
        BASE = 0,
        GRADE1 = 1,
        GRADE2 = 2,
        GRADE3 = 3
    }

    //State of destruction
    private BuildingState currentState = BuildingState.BASE;

    public void SetState(BuildingState state)
    {
        currentState = state;
    }

    public BuildingState GetState()
    {
        return currentState;
    }

    //Color of the building
    private SetNewMaterial.MaterialSelection color = SetNewMaterial.MaterialSelection.CYAN;

    public void SetColor(SetNewMaterial.MaterialSelection newColor)
    {
        color = newColor;
    }

    public SetNewMaterial.MaterialSelection GetColor()
    {
        return color;
    }

    //Pavement properties
    private bool pavement = true;

    public void SetPavement(bool visible)
    {
        pavement = visible;
    }

    public bool GetPavement()
    {
        return pavement;
    }

    public void SetMaxDebrisLevel(DebrisSelector.DebrisLevel level)
    {
        debrisSelector.maxDebrisLevel = level;
    }

    public DebrisSelector.DebrisLevel GetMaxDebrisLevel()
    {
        return debrisSelector.maxDebrisLevel;
    }

    public void SetCurrentDebrisLevel(DebrisSelector.DebrisLevel level)
    {
        debrisSelector.currentDebrisLevel = level;
    }

    public DebrisSelector.DebrisLevel GetCurrentDebrisLevel()
    {
        return debrisSelector.currentDebrisLevel;
    }

    public void UpdateDebrisLevel()
    {
        debrisSelector.SetDebrisLevel();
    }

    public void IncreaseDebrisLevel()
    {
        debrisSelector.IncreaseDebrisAmount();
    }

    private void Start()
    {
        GroupBuildings();
    }

    //Groups all the buildings inside the list
    private void GroupBuildings()
    {
        // Does not group if already grouped
        if (buildingGroup.Count == nOfElements) return;
        buildingGroup.Clear();

        foreach (Transform building in transform)
        {
            if (!building.gameObject.CompareTag("Ignore"))
            {
                //if child is a building
                buildingGroup.Add(building.gameObject);
            }
        }
    }

    // Sets color and checks if the pavement should be present or not
    public void ChangeProperties()
    {
        GroupBuildings();

        foreach (GameObject building in buildingGroup)
        {
            SetNewMaterial script = building.GetComponent<SetNewMaterial>();
            script.setMat(color);
            script.setPavement(pavement);
            script.ChangeMaterial();
        }
    }

    //Mainly meant for the editor, sets the correct building state
    public void SetCurrentState()
    {
        GroupBuildings();
        DamageBuild((int)currentState);
    }

    //Ticks building health down
    public void LoadNextState()
    {
        if (currentState >= BuildingState.GRADE3) return;
        
        currentState++;
        DamageBuild((int)currentState);
        debrisSelector.IncreaseDebrisAmount();
    }

    public void DamageBuild(int newState)
    {
        foreach (GameObject building in buildingGroup)
        {
            building.SetActive(false);
        }
        buildingGroup[newState].SetActive(true);
    }

}

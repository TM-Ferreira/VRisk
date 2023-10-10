using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class IDSetter : EditorWindow
    {
        private string not_affected_tag = "White";
        private string safe_risk_tag = "Black";
        private string low_risk_tag = "Beige";
        private string mid_risk_tag = "Yellow";
        private string high_risk_tag = "Red";

        private SetCompositeBuildingProperties.BuildingState currentSelected = SetCompositeBuildingProperties.BuildingState.BASE;
        private DebrisSelector.DebrisLevel levelSelected = DebrisSelector.DebrisLevel.NONE;

        [MenuItem("Window/ID Setter")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            IDSetter window = (IDSetter)EditorWindow.GetWindow(typeof(IDSetter));
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Risk Tags", EditorStyles.boldLabel);

            EditorGUILayout.Space(10);

            EditorGUILayout.TextField("Unaffected tag: ", not_affected_tag);
            EditorGUILayout.TextField("Safe tag: ", safe_risk_tag);
            EditorGUILayout.TextField("Low risk tag: ", low_risk_tag);
            EditorGUILayout.TextField("Mid risk tag: ", mid_risk_tag);
            EditorGUILayout.TextField("High risk tag: ", high_risk_tag);

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Give Buildings IDs")) SetIDs(false);
            if (GUILayout.Button("Reset All IDs")) SetIDs(true);
            
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Disable Pavement")) TogglePavement(false);
            if (GUILayout.Button("Enable Pavement")) TogglePavement(true);
            
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Randomize Colors")) AssignRandomColors();
            
            EditorGUILayout.Space(10);
            
            currentSelected = (SetCompositeBuildingProperties.BuildingState)EditorGUILayout.EnumPopup("Grade Selected: ", currentSelected);
            if (GUILayout.Button("Set Grade")) SetAllBuildingsDamage(currentSelected);
            
            EditorGUILayout.Space(10);

            levelSelected = (DebrisSelector.DebrisLevel)EditorGUILayout.EnumPopup("Max Grade Allowed", levelSelected);
            if (GUILayout.Button("Set Max level")) MaxDebrisAllowed(levelSelected);
            
            if (GUILayout.Button("Remove Base Debris")) RemoveDebris();
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Optimize Tiles")) RemoveExcessTiles();
        }

        private GameObject[][] ReturnBuildings()
        {
            GameObject[][] buildings = new GameObject[5][];
            
            buildings[0] = GameObject.FindGameObjectsWithTag(not_affected_tag);
            buildings[1] = GameObject.FindGameObjectsWithTag(safe_risk_tag);
            buildings[2] = GameObject.FindGameObjectsWithTag(low_risk_tag);
            buildings[3] = GameObject.FindGameObjectsWithTag(mid_risk_tag);
            buildings[4] = GameObject.FindGameObjectsWithTag(high_risk_tag);

            return buildings;
        }

        private void SetIDs(bool reset)
        {
            GameObject[][] buildings = ReturnBuildings();
            int current_id = 0;
            
            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var building_data = building.GetComponent<BuildingData>();
                
                    switch (i)
                    {
                        case 0:
                            building.name = "UnaffectedBuilding(" + current_id + ")";
                            break;
                        case 1:
                            building.name = "SafeBuilding(" + current_id + ")";
                            break;
                        case 2:
                            building.name = "LowRiskBuilding(" + current_id + ")";
                            break;
                        case 3:
                            building.name = "MidRiskBuilding(" + current_id + ")";
                            break;
                        case 4:
                            building.name = "HighRiskBuilding(" + current_id + ")";
                            break;
                    }

                    building_data.id = reset ? 0 : current_id;
                    current_id++;
                }
            }
        }

        private void TogglePavement(bool active)
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var build_prop = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    build_prop.SetPavement(active);
                    build_prop.ChangeProperties();
                }
            }
        }

        private void AssignRandomColors()
        {
            GameObject[][] buildings = ReturnBuildings();
            
            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var build_prop = building.GetComponent<SetCompositeBuildingProperties>();

                    build_prop.SetColor((SetNewMaterial.MaterialSelection)Random.Range(0, 5));
                    build_prop.ChangeProperties();
                }
            }
        }

        private void SetAllBuildingsDamage(SetCompositeBuildingProperties.BuildingState grade)
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var build_prop = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    build_prop.SetState(grade);
                    build_prop.SetCurrentState();
                    build_prop.ChangeProperties();
                }
            }
        }

        private void MaxDebrisAllowed(DebrisSelector.DebrisLevel level)
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var build_prop = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    build_prop.SetMaxDebrisLevel(level);
                }
            }
        }
        
        private void RemoveDebris()
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var build_prop = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    build_prop.SetCurrentDebrisLevel(DebrisSelector.DebrisLevel.NONE);
                    build_prop.UpdateDebrisLevel();
                }
            }
        }

        private void RemoveExcessTiles()
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    foreach (Transform state in building.transform)
                    {
                        if (state.gameObject.CompareTag("Ignore")) continue;

                        foreach (Transform component in state)
                        {
                            if (component.name == "mat4")
                            {
                                foreach (Transform materialSection in component)
                                {
                                    //Excessive tiles
                                    if (materialSection.name is "mat4_top_tilesrow1" or "mat4_top_tilesrow2")
                                    {
                                        //BOOM!
                                        DestroyImmediate(materialSection.gameObject);
                                        Debug.Log("Caboom!");
                                        
                                        //materialSection.gameObject.SetActive(false);
                                    }
                                    //Top-overlapped tiles
                                    else if (materialSection.name == "mat4_end_tiles")
                                    {
                                        foreach (Transform group in materialSection)
                                        {
                                            if (group.name is "mat4_mat2_group41")
                                            {
                                                group.gameObject.SetActive(true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

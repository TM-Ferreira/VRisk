using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class IDSetter : EditorWindow
    {
        [SerializeField] private Material unaffectedMat;
        [SerializeField] private Material safeMat;
        [SerializeField] private Material affectedMat;
        [SerializeField] private Material damagedMat;
        [SerializeField] private Material destroyedMat;

        private readonly string _notAffectedTag = "White";
        private readonly string _safeRiskTag = "Black";
        private readonly string _lowRiskTag = "Beige";
        private readonly string _midRiskTag = "Yellow";
        private readonly string _highRiskTag = "Red";

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

            EditorGUILayout.TextField("Unaffected tag: ", _notAffectedTag);
            EditorGUILayout.TextField("Safe tag: ", _safeRiskTag);
            EditorGUILayout.TextField("Low risk tag: ", _lowRiskTag);
            EditorGUILayout.TextField("Mid risk tag: ", _midRiskTag);
            EditorGUILayout.TextField("High risk tag: ", _highRiskTag);

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
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space(10);

            GUILayout.Label("Do this only in ScrubTrough scene!", EditorStyles.largeLabel);
            
            if (GUILayout.Button("Color accordingly to danger level")) ColorAccordingly();
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Box Out Mesh Cube")) BoxOutBuildings();
        }

        private GameObject[][] ReturnBuildings()
        {
            GameObject[][] buildings = new GameObject[5][];
            
            buildings[0] = GameObject.FindGameObjectsWithTag(_notAffectedTag);
            buildings[1] = GameObject.FindGameObjectsWithTag(_safeRiskTag);
            buildings[2] = GameObject.FindGameObjectsWithTag(_lowRiskTag);
            buildings[3] = GameObject.FindGameObjectsWithTag(_midRiskTag);
            buildings[4] = GameObject.FindGameObjectsWithTag(_highRiskTag);

            return buildings;
        }

        private void SetIDs(bool reset)
        {
            GameObject[][] buildings = ReturnBuildings();
            int currentID = 0;
            
            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    var buildingData = building.GetComponent<BuildingData>();
                
                    switch (i)
                    {
                        case 0:
                            building.name = "UnaffectedBuilding(" + currentID + ")";
                            break;
                        case 1:
                            building.name = "SafeBuilding(" + currentID + ")";
                            break;
                        case 2:
                            building.name = "LowRiskBuilding(" + currentID + ")";
                            break;
                        case 3:
                            building.name = "MidRiskBuilding(" + currentID + ")";
                            break;
                        case 4:
                            building.name = "HighRiskBuilding(" + currentID + ")";
                            break;
                    }

                    buildingData.id = reset ? 0 : currentID;
                    currentID++;
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
                    var buildProp = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    buildProp.SetPavement(active);
                    buildProp.ChangeProperties();
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
                    var buildProp = building.GetComponent<SetCompositeBuildingProperties>();

                    buildProp.SetColor((SetNewMaterial.MaterialSelection)Random.Range(0, 5));
                    buildProp.ChangeProperties();
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
                    var buildProp = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    buildProp.SetState(grade);
                    buildProp.SetCurrentState();
                    buildProp.ChangeProperties();
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
                    var buildProp = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    buildProp.SetMaxDebrisLevel(level);
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
                    var buildProp = building.GetComponent<SetCompositeBuildingProperties>();
                    
                    buildProp.SetCurrentDebrisLevel(DebrisSelector.DebrisLevel.NONE);
                    buildProp.UpdateDebrisLevel();
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

        private void ColorAccordingly()
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    foreach (Transform state in building.transform)
                    {
                        if (state.gameObject.CompareTag("Ignore")) continue;

                        var buildMatProp = state.gameObject.GetComponent<SetNewMaterial>();

                        if (building.CompareTag(_notAffectedTag)) buildMatProp.ChangeMaterial(unaffectedMat);
                        else if (building.CompareTag(_safeRiskTag)) buildMatProp.ChangeMaterial(safeMat);
                        else if (building.CompareTag(_lowRiskTag)) buildMatProp.ChangeMaterial(affectedMat);
                        else if (building.CompareTag(_midRiskTag)) buildMatProp.ChangeMaterial(damagedMat);
                        else if (building.CompareTag(_highRiskTag)) buildMatProp.ChangeMaterial(destroyedMat);
                    }
                }
            }
        }
        
        private void BoxOutBuildings()
        {
            GameObject[][] buildings = ReturnBuildings();

            for (int i = 0; i < 5; i++)
            {
                foreach (var building in buildings[i])
                {
                    foreach (Transform state in building.transform)
                    {
                
                        Debug.Log(state.gameObject.name);
                        
                        if (state.gameObject.name is "MeshCube")
                        {
                            Material newMat = safeMat;
                            if (building.CompareTag(_notAffectedTag)) newMat = unaffectedMat;
                            else if (building.CompareTag(_safeRiskTag)) newMat = safeMat;
                            else if (building.CompareTag(_lowRiskTag)) newMat = affectedMat;
                            else if (building.CompareTag(_midRiskTag)) newMat = damagedMat;
                            else if (building.CompareTag(_highRiskTag)) newMat = destroyedMat;
                            
                            state.gameObject.GetComponent<MeshRenderer>().enabled = true;
                            state.gameObject.GetComponent<MeshRenderer>().material = newMat;
                        }
                        else
                        {
                            state.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}

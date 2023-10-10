using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Editor
{
    public class TimelineGenerator : EditorWindow
    {
        private float duration = 0;
        
        private string not_affected_tag = "White";
        private string safe_risk_tag = "Black";
        private string low_risk_tag = "Beige";
        private string mid_risk_tag = "Yellow";
        private string high_risk_tag = "Red";
        
        [MenuItem("Window/Timeline Generator")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            TimelineGenerator window = (TimelineGenerator)EditorWindow.GetWindow(typeof(TimelineGenerator));
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Timeline", EditorStyles.boldLabel);
            duration = EditorGUILayout.FloatField("Duration", duration);
            if (GUILayout.Button("Generate")) GenerateTimeline();
            if (GUILayout.Button("Clear")) ClearTimeline();
        }

        private void GenerateTimeline()
        {
            var lowRiskBuildings = GameObject.FindGameObjectsWithTag(low_risk_tag);
            var midRiskBuildings = GameObject.FindGameObjectsWithTag(mid_risk_tag);
            var highRiskBuildings = GameObject.FindGameObjectsWithTag(high_risk_tag);

            foreach (var building in lowRiskBuildings)
            {
                GenerateBuildingData(building);
            }
            
            foreach (var building in midRiskBuildings)
            {
                for (int i = 0; i < 2; i++)
                {
                    GenerateBuildingData(building);
                }
            }
            
            foreach (var building in highRiskBuildings)
            {
                for (int i = 0; i < 3; i++)
                {
                    GenerateBuildingData(building);
                }
            }
        }

        private void GenerateBuildingData(GameObject building)
        {
            int ID = building.GetComponent<BuildingData>().id;
            float randomTriggerTime = Random.Range(0.0f, 30.0f);
            float triggerTime = Mathf.Round(randomTriggerTime * 100.0f) / 100.0f;
            WriteToCSV(ID, triggerTime, 0.03f, 0.077f);
        }

        private void WriteToCSV(int ID, float triggerTime, float intensity, float repositionInterval)
        {
            TextWriter tw = new StreamWriter("Assets/Timeline/VRiskTimelineCSV.csv", true);
            tw.WriteLine(ID + "," + triggerTime + "," + intensity + "," + repositionInterval + "," + ",");
            tw.Close();
        }
        
        private void ClearTimeline()
        {
            TextWriter tw = new StreamWriter("Assets/Timeline/VRiskTimelineCSV.csv", false);
            tw.WriteLine("Siren Start Time,Quake Start Time,Global shake intensity,Global Reposition Interval,Global Duration,Siren Enabled");
            tw.WriteLine("17,20,0.05,0.05,30,0");
            tw.WriteLine(",,,,,");
            tw.WriteLine("ID,Trigger Time,Intensity,Reposition Interval,,");
            tw.Close();
        }
    }
}

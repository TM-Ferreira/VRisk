using System;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;

namespace DataVisualiser
{
    public class UIHandler : MonoBehaviour
    {
        public PathVisualiser path_visualiser;
        public GameObject playthrough_button_prefab;
        public RectTransform playthrough_button_container;
        public float tab_gap = 5;
        public float tab_height = 40;
        public float tab_width = 140;

        public float UI_open_slider_right = 150f;
        public float UI_closed_slider_right = 0f;
        public RectTransform scroll_bar_panel;

        private List<Pair<GameObject, PlaythroughDataScript>> playthroughs;

        public GameObject menu_open;
        public GameObject menu_closed;

        private void Awake()
        {
            playthroughs = new List<Pair<GameObject, PlaythroughDataScript>>();
        }

        public void toggleMenu(bool _activate)
        {
            if (_activate)
            {
                menu_open.SetActive(true);
                menu_closed.SetActive(false);

                Vector2 offsetMax = scroll_bar_panel.offsetMax;
                offsetMax.x = UI_open_slider_right;
                scroll_bar_panel.offsetMax = offsetMax;
            }
            else
            {
                menu_open.SetActive(false);
                menu_closed.SetActive(true);
                
                Vector2 offsetMax = scroll_bar_panel.offsetMax;
                offsetMax.x = UI_closed_slider_right;
                scroll_bar_panel.offsetMax = offsetMax;
            }
        }

        public void importData()
        {
            loadCSVData(selectFolder());
        }

        private string selectFolder()
        {
            string folder_path = "";
            var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);

            if (paths.Length > 0)
            {
                folder_path = paths[0]; 
            }

            return folder_path;
        }

        private void loadCSVData(string _folder_path)
        {
            string[] csv_files = Directory.GetFiles(_folder_path, "*.csv");

            foreach (var file in csv_files)
            {
                // Grab the file contents and parse the csv.
                string contents = FileManager.getFileContents(new[] {file});
                string[][] csv_contents = FileManager.parseCSV(contents);
                
                // Create playthrough tab set.
                var playthrough_data = createTab();
                
                // Set bool for whether or not player lived. 
                playthrough_data.survived = csv_contents[csv_contents.Length-2][6] == "Survived\r";
                playthrough_data.completion_time = float.Parse(csv_contents[csv_contents.Length - 2][0]);
                
                // Display data (username, date, time, completion time, completion_condition).
                setTabDisplayData(playthrough_data, file, playthrough_data.completion_time.ToString(), csv_contents[csv_contents.Length - 2][6]);

                // Store timeline points.
                for (int line_index = 0; line_index < csv_contents.Length-1; line_index++)
                {
                    string[] line = csv_contents[line_index];
                    
                    float time = float.Parse(line[0]);
                    Vector2 grid_cell = new Vector2(float.Parse(line[1]), float.Parse(line[2]));
                    Vector3 facing_direction = new Vector3(float.Parse(line[3]), float.Parse(line[4]), float.Parse(line[5]));
                    
                    playthrough_data.timeline.Add(new TimelineElement(time, grid_cell, facing_direction));
                }
            }
        }

        private PlaythroughDataScript createTab()
        {
            // Expand container.
            playthrough_button_container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (tab_height + tab_gap) * (playthroughs.Count + 1) + tab_gap);
            
            // Create new tab.
            GameObject playthrough_tab = Instantiate(playthrough_button_prefab, playthrough_button_container);
            PlaythroughDataScript playthrough_data = playthrough_tab.GetComponent<PlaythroughDataScript>();
            
            // Add button functionality for utilising path visualiser.
            playthrough_data.display_data_button.onClick.AddListener(() => path_visualiser.setData(playthrough_data));

            // Add tab to list. 
            playthroughs.Add(new Pair<GameObject, PlaythroughDataScript>(playthrough_tab, playthrough_data));
            
            return playthrough_data;
        }

        private void setTabDisplayData(PlaythroughDataScript _script_data, string _file, string _completion_time, string _completion_state)
        {
            string filename = Path.GetFileNameWithoutExtension(_file);
                
            string[] parts = filename.Split(new string[] { " - " }, StringSplitOptions.None);
            string dateAndTimePart = parts[0];
            _script_data.save_name.text = parts.Length > 1 ? parts[1] : "";
                
            string[] dateAndTimeParts = dateAndTimePart.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            _script_data.save_date.text = dateAndTimeParts.Length > 0 ? dateAndTimeParts[0].Trim() : "";
            _script_data.save_time.text = dateAndTimeParts.Length > 1 ? dateAndTimeParts[1].Trim() : "";

            _script_data.finish_state.text = _completion_state;
            _script_data.finish_state.color = _script_data.survived ? Color.green : Color.red;

            float completion_in_seconds = float.Parse(_completion_time);
            int milliseconds = (int)Math.Round(completion_in_seconds * 1000) % 1000;
            int seconds = (int)Math.Round(completion_in_seconds % 60);
            int minutes = (int)Math.Round(completion_in_seconds / 60);
            _script_data.time_to_completion.text = "Time: " + minutes.ToString("D2") + "m : " + seconds.ToString("D2") + "s : " + milliseconds.ToString("D3") + "ms";
        }
        
        public void unloadData()
        {
            for (int i = playthroughs.Count-1; i > -1; --i)
            {
                Destroy(playthroughs[i].first);
            }
            
            playthroughs.Clear();
            playthrough_button_container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (tab_height + tab_gap) * (playthroughs.Count + 1) + tab_gap);

        }
        
        public void setSelectionForAll(bool _select)
        {
            foreach (var playthrough in playthroughs)
            {
                playthrough.second.selected.isOn = _select;
            }
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DataVisualiser
{
    public class PlaythroughDataScript : MonoBehaviour
    {
        public Toggle selected;
        
        public TMP_Text save_name;
        public TMP_Text save_date;
        public TMP_Text save_time;
        public TMP_Text finish_state;
        public TMP_Text time_to_completion;
        public Button display_data_button;

        public List<TimelineElement> timeline;
        public bool survived;
        public float completion_time;
    }

    [System.Serializable]
    public class TimelineElement
    {
        public float time;
        public Vector2 grid_cell;
        public Vector3 facing_direction;

        public TimelineElement(float _time, Vector2 _grid_cell, Vector3 _facing_direction)
        {
            time = _time;
            grid_cell = _grid_cell;
            facing_direction = _facing_direction;
        }
    }
}
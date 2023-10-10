using System.Collections.Generic;
using DataVisualiser;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualiser : MonoBehaviour
{
    public PlaythroughDataScript data;
    public List<Pair<float, GameObject>> locations;
    
    public float height_offset = 0;
    public Transform grid_origin;
    public Vector2 grid_cell_size;
    public GameObject location_marker_prefab;
        
    public Slider timeline_slider;
    public TMP_Text time_display;
    public GameObject end_marker;

    private LineRenderer path_line_renderer;
    public LineRenderer forward_line_renderer;

    private void Start()
    {
        grid_cell_size = new Vector2(GameManager.Instance.Data.grid_cell_size_x,
            GameManager.Instance.Data.grid_cell_size_y);

        path_line_renderer = GetComponent<LineRenderer>();
    }

    public void setData(PlaythroughDataScript _data)
    {
        clearPathVisualiserData();

        data = _data;
        data.display_data_button.interactable = false;
        
        foreach (var element in data.timeline)
        {
            var location_marker = Instantiate(location_marker_prefab);

            var tile_location = element.grid_cell;
            var local_location = new Vector3(tile_location.x * grid_cell_size.x, height_offset, tile_location.y * grid_cell_size.y);


            var world_position = grid_origin.TransformPoint(local_location);
            location_marker.transform.position = world_position;
            locations.Add(new Pair<float, GameObject>(element.time, location_marker));
        }
        
        updateSliderText(0.0f, data.completion_time);
        end_marker.transform.position = locations[0].second.transform.position;
    }

    private void Update()
    {
        if (data != null)
        {
            float current_time = timeline_slider.value * data.completion_time;
            
            updatePathVisuals(current_time);
            updateSliderText(current_time, data.completion_time);
        }
    }

    public void updateSliderText(float _current_seconds_from_start, float _total_time_from_start)
    {
        int total_minutes = (int)(_total_time_from_start / 60);
        int total_seconds = (int)(_total_time_from_start % 60);
        int total_milliseconds = (int)(_total_time_from_start * 1000) % 1000;
        string total_time = total_minutes.ToString("D2") + "m : " + total_seconds.ToString("D2") + "s : " + total_milliseconds.ToString("D3") + "ms";

        int current_minutes = (int)(_current_seconds_from_start / 60);
        int current_seconds = (int)(_current_seconds_from_start % 60);
        int current_milliseconds = (int)(_current_seconds_from_start * 1000) % 1000;
        string current_time = current_minutes.ToString("D2") + "m : " + current_seconds.ToString("D2") + "s : " + current_milliseconds.ToString("D3") + "ms";

        time_display.text = current_time + " / " + total_time;
    }

    public void updatePathVisuals(float _time)
    {
        // Remove interpolated line.
        if (path_line_renderer.positionCount > 0 && path_line_renderer.positionCount <= locations.Count)
        {
            path_line_renderer.positionCount -= 1;
        }
        
        // Cycle through locations and find one that's timeframe is just below that point.
        int last_line_point = 0;
        for (int i = locations.Count - 1; i >= 0; --i)
        {
            if (locations[i].first <= _time)
            {
                // Offset as indexing starts at zero so that we can add the first point.
                last_line_point = i + 1; 
                break;
            }
        }
        
        // Calculate how many points need to be added or removed from the line renderer.
        int line_renderer_points_required = last_line_point - path_line_renderer.positionCount;

        //Line renderer needs more points. 
        if (line_renderer_points_required > 0)
        {
            for (int i = line_renderer_points_required; i > 0; --i)
            {
                path_line_renderer.positionCount += 1;
                path_line_renderer.SetPosition(path_line_renderer.positionCount - 1, locations[last_line_point - i].second.transform.position);
            }
        }
        // Line renderer needs less points
        else if (line_renderer_points_required < 0)
        {
            path_line_renderer.positionCount += line_renderer_points_required;
        }

        // handle any half point / interpolation.

        if (last_line_point > 0 && last_line_point < locations.Count)
        {
            float time_between_points = mapRange(_time, locations[last_line_point - 1].first,
                locations[last_line_point].first, 0, 1);

            Vector3 interpolated_point = Vector3.Lerp(locations[last_line_point - 1].second.transform.position,
                locations[last_line_point].second.transform.position,
                time_between_points);

            path_line_renderer.positionCount += 1;
            path_line_renderer.SetPosition(path_line_renderer.positionCount - 1, interpolated_point);
        }
        
        setEndMarker(last_line_point);
    }

    private void setEndMarker(int _last_line_point)
    {
        _last_line_point -= 1;
        Vector3 forward = Vector3.zero;
        
        if (_last_line_point < data.timeline.Count && _last_line_point >= 0)
        {
            forward = data.timeline[_last_line_point].facing_direction;
        }
        else if (_last_line_point < 0)
        {
            forward = data.timeline[1].facing_direction;
        }

        Quaternion rotation = Quaternion.LookRotation(forward);
        end_marker.transform.rotation = rotation;

        forward_line_renderer.positionCount = 2;

        Vector3 end_marker_head_pos = end_marker.transform.position + Vector3.up * 1.68f;
        forward_line_renderer.SetPosition(0, end_marker_head_pos);
        forward_line_renderer.SetPosition(1, end_marker_head_pos + (forward * 2f));


        if (path_line_renderer.positionCount > 0)
        {
            Vector3 marker_position = path_line_renderer.GetPosition(path_line_renderer.positionCount - 1);
            end_marker.transform.position = marker_position;
        }
        else
        {
            end_marker.transform.position = locations[0].second.transform.position;
        }
    }
    
    public void clearPathVisualiserData()
    {
        if (data != null)
        {
            data.display_data_button.interactable = true;
        }
        
        for (int i = locations.Count-1; i > -1; --i)
        {
            Destroy(locations[i].second);
        }
        
        locations.Clear();
        path_line_renderer.positionCount = 0;
        
        data = null;
        updateSliderText(0.0f, 0.0f);
    }
    
    public static float mapRange(float _value, float _from_min, float _from_max, float _to_min, float _to_max)
    {
        return (_value - _from_min) / (_from_max - _from_min) * (_to_max - _to_min) + _to_min;
    }
}

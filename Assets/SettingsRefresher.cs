using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsRefresher : MonoBehaviour
{
    public CustomSlideManager[] customSlideManagers;
    public PresetManager presetManager;
    
    public void RefreshAll()
    {
        foreach (var customSlideManager in customSlideManagers)
        {
            customSlideManager.Refresh();
        }
        
        presetManager.Refresh();
    }
}

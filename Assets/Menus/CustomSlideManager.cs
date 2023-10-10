using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CustomSlideManager : MonoBehaviour
{
    private enum SliderType
    {
        VOLUME,
        TURN_SPEED,
        SENSIBILITY
    }

    public GameData data;
    public TextMeshProUGUI meshText;
    public Slider slider;
    
    [SerializeField] private SliderType sliderType;
    [SerializeField] private int increaseValue = 1;

    private void Start()
    {
        Refresh();
    }

    public void OnValueChanged()
    {
        var value = slider.value;
        meshText.text = value.ToString(CultureInfo.InvariantCulture);

        switch (sliderType)
        {
            case SliderType.VOLUME:
                data.volume = (int)value;
                break;
            
            case SliderType.TURN_SPEED:
                data.turnSpeed = (int)value;
                break;
            
            case SliderType.SENSIBILITY:
                data.sensibility = (int)value;
                break;
        }
    }

    public void OnValueIncrease()
    {
        slider.value += increaseValue;
    }

    public void OnValueDecrease()
    {
        slider.value -= increaseValue;
    }

    public void Refresh()
    {
        switch (sliderType)
        {
            case SliderType.VOLUME:
                slider.value = data.volume;
                break;
            
            case SliderType.TURN_SPEED:
                slider.value = data.turnSpeed;
                break;
            
            case SliderType.SENSIBILITY:
                slider.value = data.sensibility;
                break;
        }
        
        meshText.text = slider.value.ToString(CultureInfo.InvariantCulture);
    }
}

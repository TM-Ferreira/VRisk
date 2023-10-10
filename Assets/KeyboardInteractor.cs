using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class KeyboardInteractor : MonoBehaviour
{
    public GameData data;
    private TMP_InputField inputField;
    
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        NonNativeKeyboard.Instance.InputField = inputField;
        data.user_name = "";
    }

    public void SetUsername()
    {
        data.user_name = inputField.text;
    }
}

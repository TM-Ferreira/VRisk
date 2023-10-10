using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Input input_asset { get; private set; }
    public bool game_scene = true;

    private void Awake()
    {
        input_asset = new Input();

        if (game_scene)
        {
            input_asset.VRiskExperienceInputMap.Enable();
            input_asset.DataVisualiserInputMap.Disable();
        }
        else
        {
            input_asset.DataVisualiserInputMap.Enable();
            input_asset.VRiskExperienceInputMap.Disable();
        }
    }
}

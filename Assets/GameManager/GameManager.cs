using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int seed = 0;
    public TextAsset game_data_csv;
    public static GameManager Instance { get; private set; }
    public TimelineManager TimelineManager { get; private set; }
    public BuildingManager BuildingManager { get; private set; }
    public HapticFeedbackHandler HapticFeedbackHandler { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public ParticleManager ParticleManager { get; private set; }
    public DebrisHandler DebrisHandler { get; private set; }
    public DataTracker DataTracker { get; private set; }

    public GameObject Player { get; private set; }

    public GameData Data = null;
    private Dictionary<string, GameDataVariable> string_to_game_data_var_map = new Dictionary<string, GameDataVariable>()
        {{"Track Position Interval", GameDataVariable.RECORD_POSITION_INTERVAL}, {"Grid Cell Size X", GameDataVariable.GRID_CELL_SIZE_X}, {"Grid Cell Size Y", GameDataVariable.GRID_CELL_SIZE_Y}};

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        BuildingManager = GetComponentInChildren<BuildingManager>();
        HapticFeedbackHandler = GetComponentInChildren<HapticFeedbackHandler>();
        InputHandler = GetComponentInChildren<InputHandler>();
        AudioManager = GetComponentInChildren<AudioManager>();
        ParticleManager = GetComponentInChildren<ParticleManager>();
        DebrisHandler = GetComponentInChildren<DebrisHandler>();
        TimelineManager = GetComponentInChildren<TimelineManager>();
        DataTracker = GetComponentInChildren<DataTracker>();

        Player = GameObject.FindWithTag("Player");

        Random.InitState(seed);

        populateGameDataFromFile();
    }

    private void populateGameDataFromFile()
    {
        var data = FileManager.parseCSV(game_data_csv.text);

        foreach (var id_value_pair in data)
        {
            if (id_value_pair.Length == 2)
            {
                string id = id_value_pair[0];
                string value = id_value_pair[1];

                switch (string_to_game_data_var_map[id])
                {
                    case GameDataVariable.RECORD_POSITION_INTERVAL:
                        float.TryParse(value, out Data.record_position_interval);
                        break;

                    case GameDataVariable.GRID_CELL_SIZE_X:
                        float.TryParse(value, out Data.grid_cell_size_x);
                        break;

                    case GameDataVariable.GRID_CELL_SIZE_Y:
                        float.TryParse(value, out Data.grid_cell_size_y);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public static float earthquakeIntensityCurve(float _x)
    {
        return -15.5f * (float)Math.Pow(_x - 0.48f, 4) + (0.3f * _x) + 0.824f;
    }

    public static float debrisIntensityCurve(float _x)
    {
        return -4f * (float)Math.Pow(_x - 0.48f, 4) + (0.3f * _x) + 0.8f;
    }
}

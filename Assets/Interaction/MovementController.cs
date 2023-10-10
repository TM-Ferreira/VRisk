using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private InputAction move_action;
    private Camera cam;
    private Rigidbody rig_rb;
    
    public float min_run_input_mag = 0.7f;
    public float walk_speed = 0.5f;
    public float run_speed = 1.0f;
    public float current_speed = 0.0f;
    public float last_speed = 0.0f;
    
    private Vector3 prev_controller_r; 
    private Vector3 prev_controller_l;

    private const float threshold = 0.10f;

    private bool left_mov = false;
    private bool right_mov = false;

    /// <summary>
    /// PAIN - PLEZ DESTROY - TIMERS SHOULD NOT START AT ANYTHING OTHER THAN 0!
    /// </summary>
    private float timer = 2.0f;

    private float speed_multiplier = 100.0f;
    public float ratio_of_motion = 0.0f;

    public bool gesture_control_moving;
    public bool moving = true;
    
    public bool footsteps_audio_playing = false;
    public GameObject feet_object;
    public AudioSource feet_audio_source;
    public GameObject head;
    public GameData data;

    void Start()
    {
        move_action = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.MoveKeyStick;
        cam = Camera.main;
        rig_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (data.movementType == GameData.MovementType.GESTURE)
        {
            gesture_control_moving = GestureControl();
        }
        else if (move_action.ReadValue<Vector2>() != Vector2.zero && data.movementType == GameData.MovementType.JOYSTICK)
        {
            movePlayer(move_action.ReadValue<Vector2>());
        }
        else 
        {
            moving = false;
        }
        
        handleFootsteps();
    }

    private void handleFootsteps()
    {
        if (moving && !footsteps_audio_playing)
        {
            AudioManager.SoundID sound_clip = AudioManager.SoundID.RUNNING;

            GameManager.Instance.AudioManager.PlaySound(feet_audio_source, true, false, Vector3.zero, feet_object.transform,
                true, sound_clip);
            footsteps_audio_playing = true;
        }
        else if (!moving)
        {
            feet_audio_source.Stop();
            footsteps_audio_playing = false;
        }

        // if (moving && current_speed != last_speed)
        // {
        //     AudioManager.SoundID sound_clip = AudioManager.SoundID.WALKING;
        //
        //     if (current_speed == run_speed)
        //     {
        //         sound_clip = AudioManager.SoundID.RUNNING;
        //     }
        //
        //     GameManager.Instance.AudioManager.PlaySound(feet_audio_source, true, false, Vector3.zero, feet_object.transform, true, sound_clip);
        // }
    }

    public void movePlayer(Vector2 input_vect)
    {
        moving = true;

        // Convert the input 2D vector into a 3D vector for world space movement.
        Vector3 world_move = new Vector3(input_vect.x, 0, input_vect.y);

        // Rotate the movement vector to be relative to the camera's orientation.
        Vector3 rotated_vector = cam.transform.rotation * world_move;
        rotated_vector.y = 0;

        // Normalize the input vector if its magnitude is greater than 1.
        // This ensures the direction of movement is kept without boosting speed.
        input_vect = input_vect.magnitude > 1 ? input_vect.normalized : input_vect;

        last_speed = current_speed;
        // Set the movement speed based on the magnitude of the input.
        current_speed = input_vect.magnitude <= min_run_input_mag ? walk_speed : run_speed;

        // Scale the movement vector by Time.deltaTime to make it frame rate independent.
        Vector3 direction = rotated_vector * (current_speed * Time.deltaTime);
        Vector3 new_pos = rig_rb.position + direction;
        rig_rb.MovePosition(new_pos);
    }


    private bool GestureControl()
    {
        timer += Time.deltaTime;

        Vector3 controller_r = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.MoveRight_Hand.ReadValue<Vector3>();
        Vector3 controller_l = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.MoveLeft_Hand.ReadValue<Vector3>();

        Vector3 controller_delta_r = (controller_r - prev_controller_r) * data.sensibility;
        Vector3 controller_delta_l = (controller_l - prev_controller_l) * data.sensibility;

        left_mov = controller_delta_l.y is > threshold or < -threshold;
        right_mov = controller_delta_r.y is > threshold or < -threshold;

        //If both controllers are being moved at a certain intensity calculate we are moving
        if (right_mov && left_mov)
        {
            ratio_of_motion = ((controller_delta_r.y + controller_delta_l.y) / 2) * speed_multiplier;
            ratio_of_motion = ratio_of_motion < 0 ? ratio_of_motion * -1 : ratio_of_motion;
            timer = 0;
        }

        prev_controller_r = controller_r;
        prev_controller_l = controller_l;
        
        if (timer < 0.5f)
        {
            moving = true;
            movePlayer(new Vector2(0, 1));
            return true;
        }

        moving = false;
        return false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);

        if (contact.otherCollider.CompareTag("Debris"))
        {
            if (contact.otherCollider.gameObject.GetComponent<DebrisScript>().falling)
            {
                GameManager.Instance.AudioManager.PlaySound(false, false, head.transform.position, AudioManager.SoundID.LOSE);
                GameManager.Instance.DataTracker.startSavingProcess(false);
                Debug.Log("die");
            }
        }
    }
}

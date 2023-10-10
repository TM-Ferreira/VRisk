using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
     public enum CameraAction
     {
          NONE,
          PAN,
          ZOOM,
          ROTATE,
          MOVE
     }
     
     public float panning_speed;
     public float zoom_speed;
     public float rotate_speed;
     public float move_speed;
     
     public float move_speed_modifier;
     public float rotate_speed_modifier;
     public float zoom_speed_modifier;
     public float panning_speed_modifier;

     public CameraAction current_action;
     private Camera cam;
     
     private Input.DataVisualiserInputMapActions action_map;
     public bool faster;
     public bool moving;
     
     private Dictionary<string, CameraAction> string_to_camera_dictionary = new Dictionary<string, CameraAction>
     {
          {"PanView", CameraAction.PAN}, {"ZoomView", CameraAction.ZOOM}, {"RotateView", CameraAction.ROTATE},
          {"MoveView", CameraAction.MOVE}
     };

     private void Start()
     {
          cam = GetComponent<Camera>();
          action_map = GameManager.Instance.InputHandler.input_asset.DataVisualiserInputMap;

          action_map.PanView.started += setAction;
          action_map.PanView.performed += setAction;
          action_map.PanView.canceled += setAction;

          action_map.ZoomView.started += setAction;
          action_map.ZoomView.performed += setAction;
          action_map.ZoomView.canceled += setAction;

          action_map.RotateView.started += setAction;
          action_map.RotateView.performed += setAction;
          action_map.RotateView.canceled += setAction;
          
          action_map.MoveView.started += setMoving;
          action_map.MoveView.performed += setMoving;
          action_map.MoveView.canceled += setMoving;

          action_map.ToggleOrthoPerspective.started += toggleOrthoPerspective;
          action_map.Faster.started += setFaster => faster = true;
          action_map.Faster.canceled += setFaster => faster = false;
     }

     private void setMoving(InputAction.CallbackContext _context)
     {
          moving = !isActionCanceled(_context);
     }
     

     private void toggleOrthoPerspective(InputAction.CallbackContext _context)
     {
          cam.orthographic = !cam.orthographic;
     }

     private void setAction(InputAction.CallbackContext _context)
     {
          if (!EventSystem.current.IsPointerOverGameObject())
          {
               CameraAction action = string_to_camera_dictionary.TryGetValue(_context.action.name, out var value)
                    ? value
                    : CameraAction.NONE;

               bool action_canceled = isActionCanceled(_context);

               switch (_context.phase)
               {
                    case InputActionPhase.Performed:
                    case InputActionPhase.Started:
                         if (action_canceled)
                         {
                              current_action = current_action == action ? CameraAction.NONE : current_action;
                              break;
                         }

                         current_action = action;
                         break;

                    case InputActionPhase.Canceled:
                    case InputActionPhase.Disabled:
                         current_action = current_action == action ? CameraAction.NONE : current_action;
                         break;

                    default:
                         throw new ArgumentOutOfRangeException();
               }

               return;
          }

          current_action = CameraAction.NONE;
     }

     private bool isActionCanceled(InputAction.CallbackContext _context)
     {
          var value_type = _context.action.expectedControlType;
          
          if (value_type == nameof(Vector3))
          {
               var value = _context.ReadValue<Vector3>();
               return value.sqrMagnitude < 0.01f;
          }
          if (value_type == "Axis")
          {
               var value = _context.ReadValue<float>();
               return value == 0;
          }

          return false;
     }

     private void Update()
     {

          switch (current_action)
          {
               case CameraAction.PAN:
                    panCamera();
                    break;

               case CameraAction.ROTATE:
                    rotateCamera(action_map.MouseDelta.ReadValue<Vector2>());
                    break;

               case CameraAction.ZOOM:
                    zoomCamera(action_map.ZoomView.ReadValue<float>() / 120);
                    break;

               default:
                    break;
          }

          if (moving)
          {
               moveCamera(action_map.MoveView.ReadValue<Vector3>());
          }
     }

     private void panCamera()
     {
          Vector2 input = action_map.MouseDelta.ReadValue<Vector2>();
          float speed = faster ? panning_speed * panning_speed_modifier : panning_speed;
    
          Vector3 pan_direction = new Vector3(input.x, input.y, 0) * (speed * Time.deltaTime);

          cam.transform.Translate(pan_direction, Space.Self);
     }

     private void zoomCamera(float _direction)
     {
          if (!cam.orthographic)
          {
               _direction *= faster ? zoom_speed * zoom_speed_modifier : zoom_speed;

               Vector3 new_pos = cam.transform.position + _direction * cam.transform.forward;
               cam.transform.position = new_pos;
          }
          else
          {
               cam.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - _direction * zoom_speed, 1f, 60f);
          }

     }

     private void rotateCamera(Vector2 _rotation)
     {

          // Negative to ensure rotation is in the same direction as mouse movement.
          float pitch = -_rotation.y * rotate_speed;
          float yaw = _rotation.x * rotate_speed;

          pitch *= faster ? rotate_speed_modifier : 1;
          yaw *= faster ? rotate_speed_modifier : 1;

          // Convert pitch and yaw to quaternion rotations
          Quaternion pitch_rotation = Quaternion.Euler(pitch, 0, 0);
          Quaternion yaw_rotation = Quaternion.Euler(0, yaw, 0);

          Quaternion current_pitch = Quaternion.Euler(cam.transform.eulerAngles.x, 0, 0);
          Quaternion current_yaw = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

          // Calculate the new rotation
          Quaternion new_pitch = current_pitch * pitch_rotation;
          Quaternion new_yaw = current_yaw * yaw_rotation;
          Quaternion new_rotation = Quaternion.Euler(new_pitch.eulerAngles.x, new_yaw.eulerAngles.y, 0);

          cam.transform.rotation = new_rotation;
     }

     private void moveCamera(Vector3 _direction_vect)
     {
          _direction_vect = _direction_vect.magnitude > 1 ? _direction_vect.normalized : _direction_vect;
          Vector3 relative_to_cam = cam.transform.TransformDirection(_direction_vect);
          float speed = !faster ? move_speed : move_speed * move_speed_modifier;
          Vector3 new_pos = cam.transform.position + relative_to_cam * speed;
          cam.transform.position = new_pos;
     }
}

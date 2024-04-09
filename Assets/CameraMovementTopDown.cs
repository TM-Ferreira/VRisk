using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Camera))]
public class CameraMovementTopDown : MonoBehaviour
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

     public float  min_zoom = 1.0f, max_zoom = 80.0f;
     public float min_pan_speed = 0.11f, max_pan_speed = 9.5f;

     public float zoom_increase_step = 34.0f;

     public CameraAction current_action;
     private Camera cam;
     
     private Input.DataVisualiserInputMapActions action_map;
     private bool faster;
     private bool moving;
     
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
          
          panning_speed = calculatePanningSpeed(cam.orthographicSize);
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

               case CameraAction.ZOOM:
                    zoomCamera(action_map.ZoomView.ReadValue<float>() / 120);
                    break;
          }
     }

     private void panCamera()
     {
          Vector2 input = action_map.MouseDelta.ReadValue<Vector2>();
          
          //USING DELTA TIME WILL CAUSE SLOWER PANNING THE HIGHER THE FRAMERATE
          Vector3 pan_direction = new Vector3(input.x * -1.0f, input.y * -1.0f, 0) * (panning_speed * Time.fixedDeltaTime);

          cam.transform.Translate(pan_direction, Space.Self);
          
          var cam_tran = cam.transform;
          
          if(cam_tran.localPosition.x > 120)
          { 
               cam_tran.localPosition = new Vector3(120, cam_tran.localPosition.y, cam_tran.localPosition.z);
          }
          else if (cam_tran.localPosition.x < -143)
          {
               cam_tran.localPosition = new Vector3(-143, cam_tran.localPosition.y, cam_tran.localPosition.z);
          }

          if (cam_tran.localPosition.z < -110)
          {
               cam_tran.localPosition = new Vector3(cam_tran.localPosition.x, cam_tran.localPosition.y, -110);
          }
          else if (cam_tran.localPosition.z > 105)
          { 
               cam_tran.localPosition = new Vector3(cam_tran.localPosition.x, cam_tran.localPosition.y, 105);
          }
     }

     private void zoomCamera(float _direction)
     {
          float size = cam.orthographicSize;
          float multiplier = size > zoom_increase_step ? 2 : 1;

          cam.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - _direction * (zoom_speed * multiplier), min_zoom, max_zoom);
          
          //Panning speed changed accordingly to the current zoom
          panning_speed = calculatePanningSpeed(cam.orthographicSize);
     }
     
     float calculatePanningSpeed(float zoom)
     {
          // Calculate the slope of the line connecting the points (zoomMin, panningSpeedMin) and (zoomMax, panningSpeedMax)
          float slope = (max_pan_speed - min_pan_speed) / (max_zoom - min_zoom);

          // Calculate the panning speed for the given zoom
          float panningSpeed = min_pan_speed + slope * (zoom - min_zoom);

          return panningSpeed;
     }

     public void rotate(bool CW)
     {
          cam.transform.Rotate(0,0,CW ? 90 : -90, Space.Self);
     }
}

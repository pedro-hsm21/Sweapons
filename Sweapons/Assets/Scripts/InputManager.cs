using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject _go = new GameObject("InputManager");
                _instance = _go.AddComponent<InputManager>() as InputManager;
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    //public float GetHorizontalAxis(int playerNumber)
    //{
    //    if (playerNumber == 1) return (Input.GetAxis("Horizontal_KB1") + Input.GetAxis("Horizontal_C1"));
    //    else return (Input.GetAxis("Horizontal_KB2") + Input.GetAxis("Horizontal_C2"));
    //}

    public bool LeftInputIsPressed (int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal_C1") == -1;
        else return Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal_C2") == -1;
    }

    public bool RightInputIsPressed(int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal_C1") == 1;
        else return Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal_C2") == 1;
    }

    public bool JumpInputIsPressed(int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Joystick1Button1);
        else return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick2Button1);
    }

    public bool FireKeyIsPressed(int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button0);
        else return Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Joystick2Button0);
    }
}

//public static class JoystickManager
//{
//    public static bool JoystickGetKeyDown(PlayerInputs _playerInput, int joystickNumber = 1)
//    {
//        switch (_playerInput)
//        {
//            case PlayerInputs.Jump:
//                return Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button4);
//            case PlayerInputs.Shoot:
//                return Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button3);
//            default:
//                return false;
//        }
//    }

//}


//public enum PlayerInputs { MoveLeft, MoveRight, Jump, Shoot }

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

    public bool LeftInputIsPressed (int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKey(KeyCode.A);
        else return Input.GetKey(KeyCode.LeftArrow);
    }

    public bool RightInputIsPressed(int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKey(KeyCode.D);
        else return Input.GetKey(KeyCode.RightArrow);
    }

    public bool JumpInputIsPressed(int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKeyDown(KeyCode.W);
        else return Input.GetKey(KeyCode.UpArrow);
    }

    public bool FireKeyIsPressed(int playerNumber)
    {
        if (playerNumber == 1) return Input.GetKeyDown(KeyCode.J);
        else return Input.GetKey(KeyCode.Keypad1);
    }
}

using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;
using UnityEngine.SceneManagement;

public class ControllerRegisterManager : MonoBehaviour
{
    public int amountOfPlayers = 2;

    public List<PlayerActions> playerActions { get; private set; }
    private List<PlayerActions> availableActions;
    bool lookingForUpdates = true; // Not the best architecture

    void OnEnable()
    {
        playerActions = new List<PlayerActions>(amountOfPlayers);
        availableActions = new List<PlayerActions>();
        InputManager.OnDeviceDetached += OnDeviceDetached;
        availableActions.Add(PlayerActions.CreateActions(PlayerActions.ControllerType.JOYSTICK));
        availableActions.Add(PlayerActions.CreateActions(PlayerActions.ControllerType.KEYBOARDARROW));
        availableActions.Add(PlayerActions.CreateActions(PlayerActions.ControllerType.KEYBOARDWASD));
        DontDestroyOnLoad(transform);
    }

    void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        for (int i = 0; i < availableActions.Count; ++i)
        {
            availableActions[i].Destroy();
        }
        for (int i = 0; i < playerActions.Count; ++i)
        {
            playerActions[i].Destroy();
        }
    }

    void Update()
    {
        if (!lookingForUpdates)
        {
            return;
        }
        if(GlobalSettings.Instance.flags[GlobalSettings.AvailableFlags.DEBUG.ToString()])
        {
            var inputDevice = InputManager.ActiveDevice;
            CreatePlayer(availableActions[1].controllerType, availableActions[1].Device);
            CreatePlayer(availableActions[2].controllerType, availableActions[2].Device);
            LoadLevel();
        }

        JoinGame();
        checkIfAnyPlayersWantToUnready();
        CheckForFinishRegistering();
    }

    private void JoinGame()
    {
        for(int i = 0; i < availableActions.Count; ++i)
        {
            var playerActions = availableActions[i];
            if (JoinButtonWasPressedOnListener(playerActions))
            {
                InputDevice device; 
                if(playerActions.controllerType == PlayerActions.ControllerType.JOYSTICK)
                {
                    device = InputManager.ActiveDevice;
                }
                else
                {
                    device = playerActions.Device;
                }

                if(playerActions.controllerType == PlayerActions.ControllerType.JOYSTICK)
                {
                    if (!IsThisDeviceAlreadyInUse(playerActions))
                    {
                        CreatePlayer(playerActions.controllerType, device);
                    }
                }
                else
                {
                    if (!IsThisActionTypeAlreadyInUse(playerActions))
                    {
                        CreatePlayer(playerActions.controllerType, device);
                    }
                }
            }
        }
    }

    void CheckForFinishRegistering()
    {
        if (playerActions.Count == amountOfPlayers)
        {
            foreach (PlayerActions player in playerActions)
            {
                if(player.Yellow.State)
                {
                    LoadLevel();
                }
            }
        }
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("Mechanics_Test_Area");
        lookingForUpdates = false;
    }

    void OnGUI()
    {
        const float h = 22.0f;
        var y = 10.0f;

        GUI.Label(new Rect(10, y, 300, y + h), "Active players: " + playerActions.Count + "/" + amountOfPlayers);
        y += h;

        if (playerActions.Count < amountOfPlayers)
        {
            GUI.Label(new Rect(10, y, 320, y + h), "Press any button on the joystick or a/d/f key to join!");
            y += h;
        }

        if (playerActions.Count > 0)
        {
            GUI.Label(new Rect(10, y, 320, y + h), "Press B on the joystick or s key to leave!");
            y += h;
        }
    }

    private void checkIfAnyPlayersWantToUnready()
    {
        for (int i = 0; i < playerActions.Count; ++i)
        {
            if(playerActions[i].Actions != null && playerActions[i].Red.IsPressed)
            {
                RemovePlayer(playerActions[i]);
                Debug.Log("Device has been unregistered for Player " + (i + 1));
            }
        }
    }

    private bool JoinButtonWasPressedOnListener(PlayerActions actions)
    {
        return actions.Green.WasPressed || actions.Red.WasPressed || actions.Blue.WasPressed || actions.Yellow.WasPressed;
    }

    private bool IsThisDeviceAlreadyInUse(PlayerActions playerAction)
    {
        for (int i = 0; i < playerActions.Count; i++)
        {
            if (playerActions[i].Device == playerAction.Device)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsThisActionTypeAlreadyInUse(PlayerActions playerAction)
    {
        for (int i = 0; i < playerActions.Count; i++)
        {
            if (playerActions[i].controllerType == playerAction.controllerType)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDeviceDetached(InputDevice inputDevice)
    {
        for (int i = 0; i < playerActions.Count; i++)
        {
            if (playerActions[i].Device == inputDevice)
            {
                RemovePlayer(playerActions[i]);
                break;
            }
        }
    }

    private PlayerActions CreatePlayer(PlayerActions.ControllerType controllerType, InputDevice device)
    {
        PlayerActions actions = PlayerActions.CreateActions(controllerType);
        actions.Device = device;
        playerActions.Add(actions);
        return actions;
    }

    private void RemovePlayer(PlayerActions player)
    {
        playerActions.Remove(player);
    }
}

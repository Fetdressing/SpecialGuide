using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;
using UnityEngine.SceneManagement;

public class PlayerRegisterManager : MonoBehaviour
{
    public int amountOfPlayers = 2;

    public List<PlayerActions> playerActions { get; private set; }
    private PlayerActions keyboardListener;
    private PlayerActions joystickListener;
    bool lookingForUpdates = true; // Not the best architecture

    void OnEnable()
    {
        playerActions = new List<PlayerActions>(amountOfPlayers);
        InputManager.OnDeviceDetached += OnDeviceDetached;
        keyboardListener = PlayerActions.CreateWithKeyboardBindings();
        joystickListener = PlayerActions.CreateWithJoystickBindings();
        DontDestroyOnLoad(transform);
    }

    void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        joystickListener.Destroy();
        keyboardListener.Destroy();
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
            CreatePlayer(inputDevice);
            CreatePlayer(keyboardListener.Device);
            lookingForUpdates = false;
        }

        JoinGame();
        checkIfAnyPlayersWantToUnready();
        CheckForFinishRegistering();
    }

    private void JoinGame()
    {
        if (JoinButtonWasPressedOnListener(joystickListener))
        {
            var inputDevice = InputManager.ActiveDevice;

            if (!IsThisDeviceAlreadyInUse(inputDevice))
            {
                CreatePlayer(inputDevice);
            }
        }

        if (JoinButtonWasPressedOnListener(keyboardListener))
        {
            if (!IsThisDeviceAlreadyInUse(keyboardListener.Device))
            {
                CreatePlayer(keyboardListener.Device);
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
                    // Find the person who has they keyboard and give him another instance
                    for (int i = 0; i < playerActions.Count; ++i)
                    {
                        if (playerActions[i] == keyboardListener)
                        {
                            playerActions[i] = PlayerActions.CreateWithKeyboardBindings();
                            break;
                        }
                    }
                }
                SceneManager.LoadScene("Mechanics_Test_Area");
                lookingForUpdates = false;
            }
        }
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

    private bool IsThisDeviceAlreadyInUse(InputDevice inputDevice)
    {
        for (int i = 0; i < playerActions.Count; i++)
        {
            if (playerActions[i].Device == inputDevice)
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

    private PlayerActions CreatePlayer(InputDevice inputDevice)
    {
        if (playerActions.Count < amountOfPlayers)
        {
            PlayerActions actions;

            if (inputDevice == null)
            {
                // We could create a new instance, but might as well reuse the one we have
                // and it lets us easily find the keyboard player.
                actions = keyboardListener;
            }
            else
            {
                // Create a new instance and specifically set it to listen to the
                // given input device (joystick).
                actions = PlayerActions.CreateWithJoystickBindings();
                actions.Device = inputDevice;
            }
            playerActions.Add(actions);
            return actions;
        }
        return null;
    }

    private void RemovePlayer(PlayerActions player)
    {
        playerActions.Remove(player);
    }
}

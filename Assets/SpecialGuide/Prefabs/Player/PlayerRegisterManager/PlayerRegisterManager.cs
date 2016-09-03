using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;
using UnityEngine.SceneManagement;

public class PlayerRegisterManager : MonoBehaviour
{
    public GameObject playerControllerTemplate;
    public int amountOfPlayers = 2;

    private List<Player> players;
    private PlayerActions keyboardListener;
    private PlayerActions joystickListener;

    void OnEnable()
    {
        players = new List<Player>(amountOfPlayers);
        InputManager.OnDeviceDetached += OnDeviceDetached;
        keyboardListener = PlayerActions.CreateWithKeyboardBindings();
        joystickListener = PlayerActions.CreateWithJoystickBindings();
    }

    void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        joystickListener.Destroy();
        keyboardListener.Destroy();
    }

    void Update()
    {
        if (JoinButtonWasPressedOnListener(joystickListener))
        {
            var inputDevice = InputManager.ActiveDevice;

            if (ThereIsNoPlayerUsingJoystick(inputDevice))
            {
                CreatePlayer(inputDevice);
            }
        }

        if (JoinButtonWasPressedOnListener(keyboardListener))
        {
            if (ThereIsNoPlayerUsingKeyboard())
            {
                CreatePlayer(null);
            }
        }

        checkIfAnyPlayersWantToUnready();

        if(players.Count == amountOfPlayers)
        {
            if(InputManager.MenuWasPressed)
            {
                SceneManager.LoadScene("TestScene");
            }
        }
    }

    void OnGUI()
    {
        const float h = 22.0f;
        var y = 10.0f;

        GUI.Label(new Rect(10, y, 300, y + h), "Active players: " + players.Count + "/" + amountOfPlayers);
        y += h;

        if (players.Count < amountOfPlayers)
        {
            GUI.Label(new Rect(10, y, 300, y + h), "Press a button or a/s/d/f key to join!");
            y += h;
        }
    }

    private void checkIfAnyPlayersWantToUnready()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if(players[i].Actions != null && players[i].Actions.Red.IsPressed)
            {
                RemovePlayer(players[i]);
                Debug.Log("Device has been unregistered for Player " + (i + 1));
            }
        }
    }

    private bool JoinButtonWasPressedOnListener(PlayerActions actions)
    {
        return actions.Green.WasPressed || actions.Red.WasPressed || actions.Blue.WasPressed || actions.Yellow.WasPressed;
    }

    private Player FindPlayerUsingJoystick(InputDevice inputDevice)
    {
        var playerCount = players.Count;
        for (int i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Actions.Device == inputDevice)
            {
                return player;
            }
        }
        return null;
    }

    private bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice)
    {
        return FindPlayerUsingJoystick(inputDevice) == null;
    }

    private Player FindPlayerUsingKeyboard()
    {
        var playerCount = players.Count;
        for (int i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Actions != null)
            {
                return player;
            }
        }
        return null;
    }

    private bool ThereIsNoPlayerUsingKeyboard()
    {
        return FindPlayerUsingKeyboard() == null;
    }

    private void OnDeviceDetached(InputDevice inputDevice)
    {
        var player = FindPlayerUsingJoystick(inputDevice);
        if (player != null)
        {
            RemovePlayer(player);
        }
    }

    private Player CreatePlayer(InputDevice inputDevice)
    {
        if (players.Count < amountOfPlayers)
        {
            var playerControllerInstance = Instantiate(playerControllerTemplate);
            Player player = playerControllerInstance.GetComponent<Player>();

            if (inputDevice == null)
            {
                // We could create a new instance, but might as well reuse the one we have
                // and it lets us easily find the keyboard player.

                var actions = PlayerActions.CreateWithKeyboardBindings();

                player.Actions = actions;
            }
            else
            {
                // Create a new instance and specifically set it to listen to the
                // given input device (joystick).
                var actions = PlayerActions.CreateWithJoystickBindings();
                actions.Device = inputDevice;

                player.Actions = actions;
            }
            players.Add(player);
            String tag = "PlayerController" + players.Count.ToString();
            playerControllerInstance.tag = tag; 
            return player;
        }
        return null;
    }

    private void RemovePlayer(Player player)
    {
        players.Remove(player);
        player.Actions = null;
        Destroy(player.gameObject);
    }
}

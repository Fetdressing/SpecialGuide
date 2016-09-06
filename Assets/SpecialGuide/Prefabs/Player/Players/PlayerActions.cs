using System;
using InControl;
using UnityEngine;

public class PlayerActions : PlayerActionSet
{
    public enum ControllerType{
        JOYSTICK, 
        KEYBOARD
    }

	public PlayerAction Green { get; private set; }
    public PlayerAction Red { get; private set; }
    public PlayerAction Blue { get; private set; }
    public PlayerAction Yellow { get; private set; }
    public PlayerAction Left { get; private set; }
    public PlayerAction Right { get; private set; }
    public PlayerAction Up { get; private set; }
    public PlayerAction Down { get; private set; }
	public PlayerTwoAxisAction Rotate { get; private set; }
    public ControllerType controllerType { get; private set; }

    protected PlayerActions()
	{
		Green = CreatePlayerAction( "Green" );
		Red = CreatePlayerAction( "Red" );
		Blue = CreatePlayerAction( "Blue" );
		Yellow = CreatePlayerAction( "Yellow" );
		Left = CreatePlayerAction( "Left" );
		Right = CreatePlayerAction( "Right" );
		Up = CreatePlayerAction( "Up" );
		Down = CreatePlayerAction( "Down" );
		Rotate = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
	}

	public static PlayerActions CreateWithKeyboardBindings()
	{
		var actions = new PlayerActions();
        actions.Green.AddDefaultBinding( Key.A );
		actions.Red.AddDefaultBinding( Key.S );
		actions.Blue.AddDefaultBinding( Key.D );
		actions.Yellow.AddDefaultBinding( Key.F );

		actions.Up.AddDefaultBinding( Key.UpArrow );
		actions.Down.AddDefaultBinding( Key.DownArrow );
		actions.Left.AddDefaultBinding( Key.LeftArrow );
		actions.Right.AddDefaultBinding( Key.RightArrow );
        actions.controllerType = ControllerType.KEYBOARD;

        return actions;
    }

    public static PlayerActions CreateWithJoystickBindings()
	{
		var actions = new PlayerActions();

		actions.Green.AddDefaultBinding( InputControlType.Action1 );
		actions.Red.AddDefaultBinding( InputControlType.Action2 );
		actions.Blue.AddDefaultBinding( InputControlType.Action3 );
		actions.Yellow.AddDefaultBinding( InputControlType.Action4 );

		actions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
		actions.Down.AddDefaultBinding( InputControlType.LeftStickDown );
		actions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
		actions.Right.AddDefaultBinding( InputControlType.LeftStickRight );

		actions.Up.AddDefaultBinding( InputControlType.DPadUp );
		actions.Down.AddDefaultBinding( InputControlType.DPadDown );
		actions.Left.AddDefaultBinding( InputControlType.DPadLeft );
		actions.Right.AddDefaultBinding( InputControlType.DPadRight );
        actions.controllerType = ControllerType.JOYSTICK;

        return actions;
	}
}
using System;
using InControl;
using UnityEngine;

public class PlayerActions : PlayerActionSet
{
    public enum ControllerType {
        JOYSTICK, 
        KEYBOARDWASD,
        KEYBOARDARROW
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

    public static PlayerActions CreateActions(ControllerType controllerType)
    {
        PlayerActions actions = null;
        switch(controllerType)
        {
            case ControllerType.JOYSTICK:
                actions = CreateWithJoystickBindings();
                break;
            case ControllerType.KEYBOARDWASD:
                actions = CreateWithKeyboardBindingsWASD();
                break;
            case ControllerType.KEYBOARDARROW:
                actions = CreateWithKeyboardBindingsArrows();
                break;
        }
        return actions;
    }

    private static PlayerActions CreateWithKeyboardBindingsWASD()
	{
		var actions = new PlayerActions();
        actions.Green.AddDefaultBinding( Key.W);
		actions.Red.AddDefaultBinding(Key.LeftShift);
		actions.Blue.AddDefaultBinding(Key.Space);
		actions.Yellow.AddDefaultBinding(Key.LeftControl);

		actions.Up.AddDefaultBinding( Key.W );
		actions.Down.AddDefaultBinding( Key.S );
		actions.Left.AddDefaultBinding( Key.A );
		actions.Right.AddDefaultBinding( Key.D );
        actions.controllerType = ControllerType.KEYBOARDWASD;

        return actions;
    }

    private static PlayerActions CreateWithKeyboardBindingsArrows()
    {
        var actions = new PlayerActions();
        actions.Green.AddDefaultBinding(Key.UpArrow);
        actions.Red.AddDefaultBinding(Key.Pad0);
        actions.Blue.AddDefaultBinding(Key.RightControl);
        actions.Yellow.AddDefaultBinding(Key.RightShift);

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);
        actions.controllerType = ControllerType.KEYBOARDARROW;
        return actions;
    }

    private static PlayerActions CreateWithJoystickBindings()
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
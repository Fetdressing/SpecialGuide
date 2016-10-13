using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerUserController : MonoBehaviour
{
    private PlayerMovement m_Character;
    private bool m_Jump;
    private bool m_push;
    private bool m_grab;
    private bool m_controllerIsAvailable = false;

    ControllerRegisterManager playerControllerManager;
    private PlayerActions playerController;

    private readonly object syncLock = new object();
    private static int index = 0;
    int player;

    private void Start()
    {
        Debug.Log(GlobalSettings.Instance.ToString());

            lock (syncLock)
        {
            index++;
        }
        player = index % 2;

        playerControllerManager = ControllerRegisterManager.Instance;
    }

    private void AttemptToGetControllerForPlayer()
    {
        m_Character = GetComponent<PlayerMovement>();

        try
        {
            m_controllerIsAvailable = true;
            playerController = playerControllerManager.GetPlayerByIndex(player);
        }
        catch(Exception e)
        {
            m_controllerIsAvailable = false;
          //  Debug.Log(e);
        }
    }

    private void Update()
    {
        if(!m_controllerIsAvailable)
        {
            return;
        }
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = playerController.Green.State;
        }
        if (!m_push)
        {
            m_push = playerController.Blue.State;
        }
    }

    private void FixedUpdate()
    {
        if (!m_controllerIsAvailable)
        {
            AttemptToGetControllerForPlayer();
            return;
        }
        // Read the inputs.
        bool crouch = false;
        float h = playerController.Rotate.Value.x;
        // Pass all parameters to the character control script.
        m_Character.Move(h, crouch, m_Jump);

        m_Jump = false;
        if (m_push)
        {
            m_Character.DisableGrab();
            m_Character.Push();
        }
        else
        {
            if (!m_grab && playerController.Yellow.State)
            {
                m_grab = playerController.Yellow.State;
                m_Character.Grab();
            }
            else if (m_grab && !playerController.Yellow.State)
            {
                m_Character.DisableGrab();
            }
            else if (m_grab && playerController.Yellow.State)
            {
                m_Character.Grab();
            }

        }
        
        
        m_push = false;
        
    }
}

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
    private PlayerActions playerController;

    private readonly object syncLock = new object();
    static int index = 0;

    private void Start()
    {
        lock (syncLock)
        {
            index++;
        }
        playerController = GameObject.FindGameObjectWithTag("PlayerController" + index).GetComponent<Player>().Actions;
        m_Character = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
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
        // Read the inputs.
        bool crouch = false;
        float h = playerController.Rotate.Value.x;
        // Pass all parameters to the character control script.
        m_Character.Move(h, crouch, m_Jump);

        m_Jump = false;
        if (m_push)
        {
            m_Character.Push();
        }
        if(!m_grab)
        {
            m_grab = playerController.Yellow.State;
            if(m_grab)
            {
                m_Character.Grab();
            }
        }
        m_grab = false;
        m_push = false;
        
    }
}

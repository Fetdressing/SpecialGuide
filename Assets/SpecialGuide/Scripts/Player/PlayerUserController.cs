using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerUserController : MonoBehaviour
{
    private PlayerMovement m_Character;
    private bool m_Jump;

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
    }

    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = false;
        float h = playerController.Rotate.Value.x;
        // Pass all parameters to the character control script.
        m_Character.Move(h, crouch, m_Jump);
        m_Jump = false;
    }
}

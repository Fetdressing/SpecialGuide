using System;
using UnityEngine;

public class PlayerMovement : UnitBase
{
    [SerializeField]
    private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    //[SerializeField]
    private float m_JumpForce = 15f;                  // Amount of force added when the player jumps.
    [Range(0, 1)]
    [SerializeField]
    private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField]
    private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private float m_cooldownTimer = 0.0f;
    private float m_cooldown = 0.2f;


    // Pushing
    public float m_pushForce = 10f;
    private Transform m_pushGrabCheck;
    public float m_pushGrabRadius = 1.0f;
    public LayerMask m_pushGrabLayers;

    // Grabbing
    public float m_grabForce = 5f;
    public float m_grabbedForceDistance = 6.5f;
    public float m_grabbedMaxDistance = 8.5f;
    private float m_maxGrabForce = 100.0f;
    private float m_grabTime = 0.0f;
    private Rigidbody2D m_grabbedBody;

    private void Awake()
    {
        // Setting up references.
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_pushGrabCheck = transform.Find("PushGrabCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_grabTime = Time.time;
        m_grabbedBody = null;
    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        GetGrounded();
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }
    // 1. Push
    // 2. Grap
    private Rigidbody2D FindInteractableObject(int p_interactionType)
    {
        // Checks for colliding objects that are push/grabable
        Rigidbody2D outRigidBody = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_pushGrabCheck.position, m_pushGrabRadius, m_pushGrabLayers);
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject != gameObject)
            {
                if(m_FacingRight)
                {
                    if (colliders[i].gameObject.transform.position.x > transform.position.x)
                    {
                        outRigidBody = colliders[i].GetComponent<Rigidbody2D>();
                    }
                }
                else
                {
                    if(colliders[i].gameObject.transform.position.x < transform.position.x)
                    {
                        outRigidBody = colliders[i].GetComponent<Rigidbody2D>();
                    }
                }
            }
        }
        return outRigidBody;
    }

    public void Push()
    {
        Rigidbody2D pushObject = FindInteractableObject(1);
        if(pushObject != null)
        {
            Vector2 pushVector =  pushObject.position - m_Rigidbody2D.position;
            pushObject.AddForce(pushVector.normalized * m_pushForce, ForceMode2D.Impulse);
        }
    }

    public void DisableGrab()
    {
        if(m_grabbedBody != null)
        {
            //m_grabbedBody.rotation = 0.0f;
            if(m_grabbedBody.GetComponent<ResetLockedRotation>() != null)
            {
                m_grabbedBody.GetComponent<ResetLockedRotation>().SetGrabbed(false);
            }
            m_grabbedBody = null;
        }
        
    }
    public void Grab()
    {
       // If this is the initial grab
       if(m_grabbedBody == null)
        {
            // Find a grabable target
            Rigidbody2D grabObject = FindInteractableObject(2);
            if (grabObject != null)
            {
                //
                //Vector2 grabVector = m_Rigidbody2D.position - grabObject.position;
                //grabObject.AddForce(grabVector * m_grabForce);
                m_grabbedBody = grabObject;
                if (m_grabbedBody.GetComponent<ResetLockedRotation>() != null)
                {
                    m_grabbedBody.GetComponent<ResetLockedRotation>().SetGrabbed(true);
                }
            }
        }
        else
        {
            
            float distance = 4.5f;
            Vector2 grabVector = m_Rigidbody2D.position - m_grabbedBody.position;
            if(grabVector.magnitude < m_grabbedForceDistance)
            {
                Vector2 objectPosition = new Vector2(4.0f, 2.0f) + m_Rigidbody2D.position;
                m_grabbedBody.position = Vector2.Lerp(m_grabbedBody.position, objectPosition + grabVector * distance, Time.deltaTime * 5.0f);
                if (grabVector.x < 0.0f)
                {
                    Quaternion toRotation = Quaternion.FromToRotation(m_grabbedBody.transform.up, grabVector);
                    m_grabbedBody.transform.rotation = Quaternion.Lerp(m_grabbedBody.transform.rotation, toRotation, Time.deltaTime);
                }
                else
                {
                    Quaternion toRotation = Quaternion.FromToRotation(m_grabbedBody.transform.up, grabVector);
                    m_grabbedBody.transform.rotation = Quaternion.Lerp(m_grabbedBody.transform.rotation, toRotation, Time.deltaTime);
                                       
                }
            }
            else if(grabVector.magnitude <= m_grabbedMaxDistance)
            {
                m_grabbedBody.AddForce(grabVector * m_grabForce);
            }
            else
            {
               
                DisableGrab();
            }
            
        }
        
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            //Debug.Log("Walk");

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground") && Time.time > m_cooldownTimer)
        {
            // Add a vertical force to the player.
            m_cooldownTimer = Time.time + m_cooldown;
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
            //Debug.Log("Jump");
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}


﻿using System;
using UnityEngine;

public class PlayerMovement : UnitBase
{

    // Basic movement
    [SerializeField]
    private float m_MaxSpeed = 5f;                    // The fastest the player can travel in the x axis.
    [SerializeField]
    public float m_MoveForce = 350.0f;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    // General settings, transforms and stuff
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.


    // Jumping
    public float m_JumpForce = 15f;                  // Amount of force added when the player jumps.
    [SerializeField]
    private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    private float m_jumpCooldownTimer = 0.0f;
    private float m_jumpCooldown = 0.2f;

    // Pushing
    public float m_pushForce = 20f;              // Default push Impulse force
    private Transform m_pushGrabCheck;          // Transform point where push and grab checks are performed from
    public float m_pushGrabRadius = 1.0f;       // Radius for the push and grab checks
    public LayerMask m_pushGrabLayers;          // Layers to check for interactable objects within
    private float m_GrabbedInitXDistance; //avståndet man grabbade det aktiva grabobjektet
    private float m_pushCooldown = 1.0f;
    private float m_pushCooldownTimer = 0.0f;
    
    // Grabbing
    public float m_grabbedMaxDistance = 8.5f;
    private float m_grabTime = 0.0f;
    private Rigidbody2D m_grabbedBody;

    // Dashing
    public float m_dashForce = 10.0f;

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
    private void Update()
    {
        if(m_pushCooldownTimer > 0.0f && m_pushCooldownTimer <= m_pushCooldown)
        {
            m_pushCooldownTimer += Time.deltaTime;
        }
        else
        {
            m_pushCooldownTimer = 0.0f;
        }
    }
    
    // 1. Push
    // 2. Grap
    private Rigidbody2D FindInteractableObject(int p_interactionType)
    {
        // Checks for colliding objects that are push/grabable
        Rigidbody2D outRigidBody = null;
        RaycastHit2D hitGrab;
        if (m_FacingRight)
        {
            hitGrab = Physics2D.Raycast(m_Rigidbody2D.position, Vector2.right, m_pushGrabRadius, m_pushGrabLayers);
        }
        else
        {
            hitGrab = Physics2D.Raycast(m_Rigidbody2D.position, Vector2.left, m_pushGrabRadius, m_pushGrabLayers);
        }

        if (hitGrab == true)
        {
            outRigidBody = hitGrab.transform.GetComponent<Rigidbody2D>();
            if(outRigidBody != null)
            {
                return outRigidBody;
            }
        }

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
        if(pushObject != null && m_pushCooldownTimer <= 0.0f)
        {
            Vector2 pushVector =  pushObject.position - m_Rigidbody2D.position;
            pushVector = pushVector + new Vector2(0.0f, 1.0f);
            //pushVector.y += 45;
            pushObject.AddForce(pushVector.normalized * m_pushForce, ForceMode2D.Impulse);
            m_pushCooldownTimer += Time.deltaTime;
        }
    }

    public void DisableGrab()
    {
        if(m_grabbedBody != null)
        {
            m_grabbedBody.velocity = m_Rigidbody2D.velocity;
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
                m_grabbedBody = grabObject;
                m_GrabbedInitXDistance = Mathf.Abs(m_grabbedBody.position.x - m_Rigidbody2D.position.x);
            }
        }
        else
        {
            float maxForceGrab = 10;
            float grabForce = 9000;

            if (Vector2.Distance(m_Rigidbody2D.position, m_grabbedBody.position) < m_grabbedMaxDistance)
            {

                bool isOnLeftSide = false;
                if (m_Rigidbody2D.position.x >= m_grabbedBody.position.x)
                {
                    isOnLeftSide = true;
                }

                Vector2 offSet;
                float offsetAmountX = m_GrabbedInitXDistance + m_grabbedBody.GetComponent<Renderer>().bounds.extents.x;
                if (isOnLeftSide)
                {
                    offSet = new Vector2(-offsetAmountX, 0);
                }
                else
                {
                    offSet = new Vector2(offsetAmountX, 0);
                }

                Vector2 grabVector = ((m_Rigidbody2D.position + offSet) - m_grabbedBody.position).normalized;
                m_grabbedBody.AddForce(grabVector * grabForce * Time.deltaTime, ForceMode2D.Force);
                if(m_grabbedBody.velocity.magnitude > maxForceGrab)
                {
                    m_grabbedBody.velocity = m_grabbedBody.velocity.normalized * maxForceGrab;
                }
            }
            else
            {

                DisableGrab();
            }
            
        }
        
    }

    public void Move(float move, bool crouch, bool jump)
    {

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            // !! Maybe check against magnitude eventually
            if(Mathf.Abs(m_Rigidbody2D.velocity.x) <= m_MaxSpeed)
            {
                m_Rigidbody2D.AddForce(Vector2.right * m_MoveForce * move);
            }
            if(Mathf.Abs(m_Rigidbody2D.velocity.x) > m_MaxSpeed)
            {
                m_Rigidbody2D.velocity = new Vector2(Mathf.Sign(m_Rigidbody2D.velocity.x) * m_MaxSpeed, m_Rigidbody2D.velocity.y );
            }
            
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
        if (m_Grounded && jump && m_Anim.GetBool("Ground") && Time.time > m_jumpCooldownTimer)
        {
            // Add a vertical force to the player.
            m_jumpCooldownTimer = Time.time + m_jumpCooldown;
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


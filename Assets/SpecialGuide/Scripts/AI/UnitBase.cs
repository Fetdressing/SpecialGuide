using UnityEngine;
using System.Collections;

public class UnitBase : MonoBehaviour {
    protected Rigidbody2D m_Rigidbody2D;

    [Header("Grounded Check")]
    public LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [HideInInspector]
    public bool m_Grounded;            // Whether or not the player is grounded.

    [Header("Side Check")]
    public Transform sideCheckerTransform; // bör vara lite ovanför marken
    public float sideCheckDistance = 4.0f;

    public virtual void Reset()
    {
        m_Rigidbody2D.velocity = new Vector2(0, 0);
    }

    public bool GetGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                return true;
            }
        }
        return false;
    }

    public bool CheckGroundCollision(Vector2 pos, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //Debug.Log(colliders[i].gameObject.name);
                return true;
            }
        }
        return false;
    }

    public virtual bool CheckSide(Vector2 dir)
    {
        if (Physics2D.Raycast(sideCheckerTransform.position, dir, sideCheckDistance, m_WhatIsGround))
        {
            return true;
        }
        return false;
    }
}

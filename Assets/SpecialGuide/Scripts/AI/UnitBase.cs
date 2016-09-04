using UnityEngine;
using System.Collections;

public class UnitBase : MonoBehaviour {
    [Header("Grounded Check")]
    public LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [HideInInspector]
    public bool m_Grounded;            // Whether or not the player is grounded.

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
                return true;
            }
        }
        return false;
    }
}

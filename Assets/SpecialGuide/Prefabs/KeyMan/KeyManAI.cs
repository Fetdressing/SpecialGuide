using UnityEngine;
using System.Collections;

public class KeyManAI : MonoBehaviour {

    private Rigidbody2D m_rigidBody;

    public float m_maxSpeed = 2.5f;
    public float m_accelerationRate = 500.0f;

    // Ground Checking Stuff
    public LayerMask m_whatIsGround;
    public Transform m_groundCheck;
    const float m_groundedRadius = .5f;
    private bool m_isGrounded;


	// Use this for initialization
	void Start ()
    {
        m_rigidBody = transform.GetComponent<Rigidbody2D>();
        m_isGrounded = false;
	}
	
	void FixedUpdate ()
    {
        m_isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, m_groundedRadius, m_whatIsGround);
        for(int i = 0; i<colliders.Length; i++)
        {
            if(colliders[i].gameObject != m_rigidBody.gameObject)
            {
                m_isGrounded = true;
            }
        }


        if(m_isGrounded)
        {
            if (m_rigidBody.velocity.magnitude > m_maxSpeed)
            {
                m_rigidBody.velocity = m_rigidBody.velocity.normalized * m_maxSpeed;
            }
            else
            {
                m_rigidBody.AddForce(Vector2.right * m_accelerationRate * Time.deltaTime);
            }
        }
        else
        {

        }
	    



	}
}

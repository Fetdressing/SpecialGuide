using UnityEngine;
using System.Collections;

public class KeyManAI : MonoBehaviour {

    private Rigidbody2D m_rigidBody;

    public float m_maxSpeed = 2.5f;
    public float m_accelerationRate = 500.0f;


	// Use this for initialization
	void Start ()
    {
        m_rigidBody = transform.GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
    {
	    if(m_rigidBody.velocity.magnitude > m_maxSpeed)
        {
            m_rigidBody.velocity = m_rigidBody.velocity.normalized * m_maxSpeed;
        }
        else
        {
            m_rigidBody.AddForce(Vector2.right * m_accelerationRate * Time.deltaTime);
        }



	}
}

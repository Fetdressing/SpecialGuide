using UnityEngine;
using System.Collections;

public class ResetLockedRotation : MonoBehaviour {
    private bool m_isGrabbed;
    public float m_baseRotation;
    private Rigidbody2D m_rigidBody;
    public float m_speedThreshold = 20.0f;

    private float m_time;
	// Use this for initialization
	void Awake ()
    {
        m_isGrabbed = false;
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_baseRotation = m_rigidBody.rotation;
	}

    // Toggles grab status
    public void SetGrabbed(bool p_status)
    {
        m_time = 0.0f;
        m_isGrabbed = p_status;
    }

  
    	// Update is called once per frame
	void FixedUpdate ()
    {
        if(!m_isGrabbed & m_rigidBody.rotation != m_baseRotation)
        {
            if(m_rigidBody.velocity.magnitude < m_speedThreshold)
            {
                m_rigidBody.rotation = Mathf.Lerp(m_rigidBody.rotation, m_baseRotation, m_time);
                m_time += Time.deltaTime * 0.5f;
                Debug.Log(m_rigidBody.velocity.magnitude);
            }
            
        }
	  
	}
}

using UnityEngine;
using System.Collections;

public class PositionCamera : MonoBehaviour {

    public Transform m_targetObject;

    // Target Indicator
    public bool m_showTarget;
    private Renderer m_meshRenderer;

    // My rigidbody, save yes/no?
    private Rigidbody m_rigidBody;

    // Timers for something
    private float m_minThreshhold = 0.08f;

    // Player
    Transform m_player1;
    Transform m_player2;
    Transform m_target;

    // Camera interpolation
    Vector3 m_desiredPosition;
    float m_cameraDeltaFactor = 2.5f;
    float m_maxDistance = 35.0f;

    // Use this for initialization
    void Start () {
        m_player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        m_player2 = GameObject.FindGameObjectWithTag("Player2").transform;
        m_rigidBody = transform.GetComponent<Rigidbody>();
        m_target = (Instantiate(m_targetObject.gameObject, m_desiredPosition, Quaternion.identity) as GameObject).transform;
        m_target.position = m_desiredPosition;

        m_meshRenderer = m_target.GetComponent<MeshRenderer>();
        if (m_showTarget)
        {
            m_meshRenderer.enabled = true;
        }
        else
        {
            m_meshRenderer.enabled = false;
        }
    }

    private void ComputeDesiredVector()
    {
        Vector3 vectorBetweenPlayers = m_player2.position - m_player1.position;
        Vector3 desiredPositonBetweenPlayers = m_player1.position + (vectorBetweenPlayers / 2.0f);
        m_desiredPosition = desiredPositonBetweenPlayers;

        m_desiredPosition.y += 3;
        m_desiredPosition.z -= 10;
        m_desiredPosition.z -= Mathf.Min(vectorBetweenPlayers.magnitude, m_maxDistance);
        //m_desiredPosition.z = 50.0f;
        m_target.position = new  Vector3(m_desiredPosition.x, m_desiredPosition.y, m_player2.position.z);
        
  
    }

    void Update()
    {
        ComputeDesiredVector();
        Vector3 deltaVector = m_desiredPosition - transform.position;
        float length = Vector3.Magnitude(deltaVector);

        if (length < m_minThreshhold)
        {
            transform.position = m_desiredPosition;
            return;
        }
        transform.position += deltaVector * Time.deltaTime * m_cameraDeltaFactor;

       // m_desiredPosition.z = 0.0f;
    }
}

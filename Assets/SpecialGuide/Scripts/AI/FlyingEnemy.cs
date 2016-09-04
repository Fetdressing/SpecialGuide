using UnityEngine;
using System.Collections;

public class FlyingEnemy : UnitBase {
    private Transform thisTransform;
    private Rigidbody2D thisRigidbody;
    private Vector2 thisPos;
    private Transform target;

    public Transform meshObject;

    private GameObject[] players;
    public LayerMask playerLayermask;
    public float playerCheckDistance = 100;

    public float moveForce = 900.0f;

    public GameObject deathExplosion;
    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        thisTransform = this.transform;
        thisRigidbody = thisTransform.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if(target == null)
        {
            CheckPlayerForPlayer();
        }

        thisPos = new Vector2(thisTransform.position.x, thisTransform.position.y);
    }

    void CheckPlayerForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(thisPos, playerCheckDistance, playerLayermask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                target = colliders[i].transform;
            }
        }
    }
}

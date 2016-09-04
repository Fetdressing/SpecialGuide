using UnityEngine;
using System.Collections;

public class SimpleEnemy : UnitBase {
    private Transform thisTransform;
    private Rigidbody2D thisRigidbody;
    private Transform[] players;
    private Vector2 thisPos;

    public Transform meshObject;

    enum Direction { Left, Right}

    Direction currDirection = Direction.Left;

    public float activationDistance = 50.0f;

    public float moveForce = 900.0f;

    public GameObject deathExplosion;
	// Use this for initialization
	void Start () {
        thisTransform = this.transform;
        thisRigidbody = thisTransform.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        thisPos = new Vector2(thisTransform.position.x, thisTransform.position.y);
        Move();
        CheckIfTurn();

        if(!GetGrounded())
        {
            if(meshObject != null)
                meshObject.eulerAngles = new Vector3(0, -90, 0);
        }
	}

    void Move()
    {
        if(currDirection == Direction.Left)
        {
            if (meshObject != null)
                meshObject.eulerAngles = new Vector3(0, -90, 0);
            thisRigidbody.AddForce(Vector3.left * moveForce * Time.deltaTime);
        }
        else
        {
            if (meshObject != null)
                meshObject.eulerAngles = new Vector3(0, 90, 0);
            thisRigidbody.AddForce(Vector3.right * moveForce * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collidingUnit)
    {
        if (collidingUnit.transform.position.x > thisTransform.position.x) //du träffa mig från höger
        {
            if (Physics2D.Raycast(thisPos, Vector2.right, 4.0f))
            {
                //Debug.Log("Träffar mig från höger");
                currDirection = Direction.Left;
            }
        }
        else //du träffa mig från vänster shiiatt
        {
            if (Physics2D.Raycast(thisPos, Vector2.left, 4.0f))
            {
                //Debug.Log("Träffar mig från vänster");
                currDirection = Direction.Right;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collidingUnit)
    {

    }

    public void Die()
    {
        GameObject temp = Instantiate(deathExplosion.gameObject, thisTransform.position, thisTransform.rotation) as GameObject;
        Destroy(temp, 4.0f);
        Destroy(thisTransform.gameObject);
    }

    void CheckIfTurn()
    {
        if(currDirection == Direction.Right)
        {
            if(CheckGroundCollision(thisPos + new Vector2(3, 5.0f), 1.0f))
            {
                currDirection = Direction.Left;
            }
        }
        else
        {
            if (CheckGroundCollision(thisPos + new Vector2(-3, 5.0f), 1.0f))
            {
                currDirection = Direction.Right;
            }
        }
    }

}

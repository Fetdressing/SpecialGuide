using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour {
    private Transform thisTransform;
    private Rigidbody2D thisRigidbody;
    private Vector2 thisPos;
    private Transform target;

    public Transform meshObject;

    private GameObject[] players;
    public LayerMask targetLayermask;
    public float playerCheckDistance = 100;

    private Vector2 startPos;

    private Vector2 currPatrolPos; //positionen som denne rör sig mot atm, gäller när CircleSkies enumeratorn körs
    private float distanceMaxXPatrol = 50;
    private float distanceMaxYPatrol = 15;

    public float moveForce = 900.0f;

    public GameObject deathExplosion;
    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        thisTransform = this.transform;
        thisRigidbody = thisTransform.GetComponent<Rigidbody2D>();

        thisPos = new Vector2(thisTransform.position.x, thisTransform.position.y);
        startPos = thisPos;
        currPatrolPos = thisPos;

        StartCoroutine(CircleSkies());
    }
	
    void CheckForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(thisPos, playerCheckDistance, targetLayermask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                target = colliders[i].transform;
            }
        }
    }

    IEnumerator CircleSkies()
    {
        while(target == null)
        {
            thisPos = new Vector2(thisTransform.position.x, thisTransform.position.y);
            if (Vector2.Distance(currPatrolPos, thisPos) < 0.5f)
            {
                float x = Random.Range(-distanceMaxXPatrol, distanceMaxXPatrol);
                float y = Random.Range(-distanceMaxYPatrol, distanceMaxYPatrol);

                currPatrolPos = new Vector2(thisPos.x + x, thisPos.y + y);
            }
            Debug.Log(currPatrolPos.ToString());
            Vector2 dirToTarget = (new Vector2(currPatrolPos.x, currPatrolPos.y) - thisPos).normalized;
            thisRigidbody.AddForce(dirToTarget * moveForce * Time.deltaTime);

            //CheckForPlayer();
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(DiveAttack(target));
    }

    IEnumerator DiveAttack(Transform t)
    {
        while(thisPos.y >= t.position.y && target != null)
        {
            thisPos = new Vector2(thisTransform.position.x, thisTransform.position.y);
            Vector2 dirToTarget = (new Vector2(t.position.x, t.position.y) - thisPos).normalized;
            thisRigidbody.AddForce(dirToTarget * moveForce * 1.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(CircleSkies());
    }
}

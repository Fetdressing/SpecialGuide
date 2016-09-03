using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour {
    private Transform thisTransform;
    private Rigidbody2D thisRigidbody;
    private BoxCollider2D thisCollider;
    private Vector2 thisPos2D;

    public float speed = 1;

    public Transform[] movePositionTransforms = new Transform[2];
    private List<Vector2> movePositions = new List<Vector2>();

    private int currPositionIndex;

    private Vector2 moveDirection;

	// Use this for initialization
	void Start () {
        thisTransform = this.transform;
        thisRigidbody = thisTransform.GetComponent<Rigidbody2D>();
        thisCollider = thisTransform.GetComponent<BoxCollider2D>();

        currPositionIndex = 0;

        for(int i = 0; i < movePositionTransforms.Length; i++)
        {
            Vector2 temp = movePositionTransforms[i].position;
            movePositions.Add(temp);
        }

        thisPos2D = new Vector2(thisTransform.position.x, thisTransform.position.y);
        UpdateMoveDirection();
    }
	
	// Update is called once per frame
	void Update () {
        thisPos2D = new Vector2(thisTransform.position.x, thisTransform.position.y);
        UpdateMoveDirection();

        thisRigidbody.MovePosition(thisPos2D + moveDirection * Time.deltaTime * speed);

        if(Vector2.Distance(thisTransform.position, movePositions[currPositionIndex]) < 0.1f)
        {
            NextPositionIndex();
        }
	}

    void NextPositionIndex()
    {
        currPositionIndex++;
        if(currPositionIndex >= movePositionTransforms.Length)
        {
            currPositionIndex = 0;
        }
    }

    void UpdateMoveDirection()
    {
        //NextPositionIndex();
        moveDirection = (movePositions[currPositionIndex] - thisPos2D).normalized;
    }
}

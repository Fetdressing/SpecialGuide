using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour {
    [HideInInspector]
    public Transform thisTransform;
    [HideInInspector]
    public Rigidbody2D thisRigidbody;
    [HideInInspector]
    public BoxCollider2D thisCollider;
    [HideInInspector]
    public Vector2 thisPos2D;

    public float speed = 1;

    public Transform[] movePositionTransforms = new Transform[2];
    [HideInInspector]
    public List<Vector2> movePositions = new List<Vector2>();

    [HideInInspector]
    public int currPositionIndex;
    public int startPositionIndex = 0;

    [HideInInspector]
    public Vector2 moveDirection;

	// Use this for initialization
	void Start () {
        Init();
    }

    public virtual void Init()
    {
        thisTransform = this.transform;
        thisRigidbody = thisTransform.GetComponent<Rigidbody2D>();
        thisCollider = thisTransform.GetComponent<BoxCollider2D>();

        if (startPositionIndex >= movePositionTransforms.Length)
        {
            startPositionIndex = 0;
        }

        currPositionIndex = startPositionIndex;

        for (int i = 0; i < movePositionTransforms.Length; i++)
        {
            Vector2 temp = movePositionTransforms[i].position;
            movePositions.Add(temp);
        }

        thisPos2D = new Vector2(thisTransform.position.x, thisTransform.position.y);
        UpdateMoveDirection();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateLoop();
	}

    public virtual void UpdateLoop()
    {
        thisPos2D = new Vector2(thisTransform.position.x, thisTransform.position.y);
        UpdateMoveDirection();

        thisRigidbody.MovePosition(thisPos2D + moveDirection * Time.deltaTime * speed);

        if (Vector2.Distance(thisTransform.position, movePositions[currPositionIndex]) < 0.1f)
        {
            NextPositionIndex();
        }
    }

    public void NextPositionIndex()
    {
        currPositionIndex++;
        if(currPositionIndex >= movePositionTransforms.Length)
        {
            currPositionIndex = 0;
        }
    }

    public void SetPositionIndex(int i)
    {
        currPositionIndex = i;
        if (currPositionIndex >= movePositionTransforms.Length)
        {
            currPositionIndex = movePositionTransforms.Length - 1;
        }
    }

    public void UpdateMoveDirection()
    {
        //NextPositionIndex();
        moveDirection = (movePositions[currPositionIndex] - thisPos2D).normalized;
    }
}

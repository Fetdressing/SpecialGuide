using UnityEngine;
using System.Collections;

public class TriggerPlatform : MovingPlatform {

    public Transform platform;
    public int finalIndex = 1;

    public float triggerCheckRadius = 1.0f;
    public LayerMask triggerLayerMask;

    public override void Init()
    {
        base.Init();
        thisRigidbody = platform.GetComponent<Rigidbody2D>();
    }

    public override void UpdateLoop()
    {
        thisPos2D = new Vector2(platform.position.x, platform.position.y);
        UpdateMoveDirection();

        if (Vector2.Distance(platform.position, movePositions[currPositionIndex]) > 0.1f)
        {
            thisRigidbody.MovePosition(thisPos2D + moveDirection * Time.deltaTime * speed);
        }

        if(IsTouchingTrigger())
        {
            SetPositionIndex(finalIndex);
        }
        else
        {
            SetPositionIndex(startPositionIndex);
        }
    }

    public virtual bool IsTouchingTrigger()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(thisTransform.position, triggerCheckRadius, triggerLayerMask);
        if(cols.Length > 0)
        {
            return true;
        }
        return false;
    }
}

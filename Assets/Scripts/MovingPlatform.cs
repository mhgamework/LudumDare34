using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{

    public float RelativeMinHeight;
    public float RelativeMaxHeight = 3;
    public float MoveSpeed = 1;
    
    public bool AutoMoving = true;
    public int AutoMovingDirection = 1;
    public float AutoMovingWaitTime = 1;
    
    
    private float relativeMoveTarget = 0;


    private Vector3 startPos;


    private float RelativeYPos { get { return transform.localPosition.y - startPos.y; } }

    public void EnableAutoMoving()
    {
        AutoMoving = true;
    }

    public void DisableAutoMoving()
    {
        AutoMoving = false;
    }

    // Use this for initialization
    void Start()
    {
        startPos = transform.localPosition;
        StartCoroutine(doAutoMoving().GetEnumerator());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerable<YieldInstruction> doAutoMoving()
    {
        for (; ; )
        {
            if (!AutoMoving)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }
            if (RelativeYPos - 0.0001f < RelativeMinHeight)
            {
                yield return new WaitForSeconds(AutoMovingWaitTime);
                if (!AutoMoving)
                    continue;
                relativeMoveTarget = RelativeMaxHeight;
            }
            if (RelativeYPos + 0.0001f > RelativeMaxHeight)
            {
                yield return new WaitForSeconds(AutoMovingWaitTime);
                if (!AutoMoving)
                    continue;
                relativeMoveTarget = RelativeMinHeight;
            }
            if (Mathf.Abs(relativeMoveTarget - RelativeYPos) < 0.0001f)
            {
                relativeMoveTarget = AutoMovingDirection > 0 ? RelativeMaxHeight : RelativeMinHeight;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void MoveTo(float relativeTargetHeight)
    {
        relativeMoveTarget = relativeTargetHeight;
    }

    public void FixedUpdate()
    {
        stepToMoveTarget();
    }

    private void stepToMoveTarget()
    {
        var min = startPos.y + RelativeMinHeight;
        var max = startPos.y + RelativeMaxHeight;

        var newPos = transform.localPosition;

        var moveTarget = relativeMoveTarget + startPos.y;
        var diff = moveTarget - newPos.y;
        diff = Mathf.Min(Mathf.Abs(diff), Time.deltaTime * MoveSpeed) * Mathf.Sign(diff);


        newPos += Vector3.up * diff;

        newPos.y = Mathf.Clamp(newPos.y, min, max);

        transform.localPosition = newPos;
    }
}

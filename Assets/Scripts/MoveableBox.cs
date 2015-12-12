using UnityEngine;
using System.Collections;

public class MoveableBox : MonoBehaviour
{

    private float CircleRadius;
    private float startAngle;
    private Quaternion startRotation;

    // Use this for initialization
    void Start()
    {
        var p = transform.position;
        p.y = 0;
        CircleRadius = p.magnitude;

        startAngle = calcAngle();
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        var angle = calcAngle();

        transform.position = new Vector3(CircleRadius*Mathf.Cos(angle), transform.position.y,
            CircleRadius*Mathf.Sin(angle));

        transform.localRotation = startRotation*Quaternion.AngleAxis(Mathf.Rad2Deg*( startAngle- calcAngle()),Vector3.up);


    }

    private float calcAngle()
    {
        return Mathf.Atan2(transform.position.z, transform.position.x);
    }
}

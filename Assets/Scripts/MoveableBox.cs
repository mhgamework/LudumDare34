using UnityEngine;
using System.Collections;

public class MoveableBox : MonoBehaviour
{

    private float CircleRadius;
    private float startAngle;
    private Quaternion startRotation;

    private AudioSource audio;
    // Use this for initialization
    void Start()
    {
        var p = transform.position;
        p.y = 0;
        CircleRadius = p.magnitude;

        startAngle = calcAngle();
        startRotation = transform.localRotation;
        lastPos = transform.position;

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 lastPos;
    public void FixedUpdate()
    {
        if ((lastPos - transform.position).magnitude > 0.01)
        {
            if (!audio.isPlaying) { audio.Play(); }
        }
       /* else
            audio.Pause();*/
        lastPos = transform.position;

        var angle = calcAngle();

        transform.position = new Vector3(CircleRadius * Mathf.Cos(angle), transform.position.y,
            CircleRadius * Mathf.Sin(angle));

        transform.localRotation = startRotation * Quaternion.AngleAxis(Mathf.Rad2Deg * (startAngle - calcAngle()), Vector3.up);


    }

    private float calcAngle()
    {
        return Mathf.Atan2(transform.position.z, transform.position.x);
    }
}

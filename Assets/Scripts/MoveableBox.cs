using UnityEngine;
using System.Collections;

public class MoveableBox : MonoBehaviour
{
    private float circleRadius;
    private float startAngle;
    private Quaternion startRotation;

    private Vector3 Position { get { return transform.position; } set { transform.position = value; } }
    private Quaternion Rotation { get { return transform.rotation; } set { transform.rotation = value; } }

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        var p = Position;
        p.y = 0;
        circleRadius = p.magnitude;

        startAngle = calcAngle();
        startRotation = Rotation;
        lastPos = transform.localPosition;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateAudio();
    }

    public void FixedUpdate()
    {
        var angle = calcAngle();

        Position = new Vector3(circleRadius * Mathf.Cos(angle), Position.y, circleRadius * Mathf.Sin(angle));
        Rotation = startRotation * Quaternion.AngleAxis(Mathf.Rad2Deg * (startAngle - calcAngle()), Vector3.up);
    }
    
    private float calcAngle()
    {
        return Mathf.Atan2(Position.z, Position.x);
    }

    private Vector3 lastPos;
    private void UpdateAudio()
    {
        if ((lastPos - transform.localPosition).magnitude > 0.01)
        {
            if (!audioSource.isPlaying) { audioSource.Play(); }
        }
        /* else
             audio.Pause();*/
        lastPos = transform.localPosition;
    }
}

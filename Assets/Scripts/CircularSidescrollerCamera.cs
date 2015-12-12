using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CircularSidescrollerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    public Vector3 LookAt;
    public Vector3 PlayerTransformOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	    var lookAtOffset = (playerTransform.position+ PlayerTransformOffset) - LookAt;
        lookAtOffset *= 2;
	     

        transform.position = LookAt + lookAtOffset;
	    transform.LookAt(LookAt);


	}
}

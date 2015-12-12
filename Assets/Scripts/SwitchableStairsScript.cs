using UnityEngine;
using System.Collections;
using System.Linq;

public class SwitchableStairsScript : MonoBehaviour
{

    public float RelativeMinY= -2;
    public float RelativeMaxY = 2;
    public float RelativeY = 0;
    public float RelativeTargetY = 0;
    public float SwitchSpeed = 1;

    public bool AnchorLeft = true;


    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	    var diff =  RelativeTargetY- RelativeY;
	    diff = Mathf.Sign(diff)*Mathf.Min(Mathf.Abs( diff), Time.deltaTime*SwitchSpeed);

        RelativeY = Mathf.Clamp(RelativeY + diff   , RelativeMinY, RelativeMaxY);


	    //var startAngle = Mathf.Atan2(transform.position.z, transform.position.x);

	    //var angleLength = Length/new Vector2(transform.position.x, transform.position.z).magnitude;

	    int i = 0;
        foreach (var child in transform.Cast<Transform>())
	    {
	        var p = child.position;

	        //var angle = Mathf.Atan2(p.z, p.x);
	        var factor = 1f/ (transform.childCount+1) * (i+1);//(angle - startAngle)/angleLength;
	        if (!AnchorLeft) factor = 1 - factor;//angleLength - factor;
            p.y = Mathf.Lerp(0, RelativeY, factor);
	        p.y += +transform.position.y;

            child.position = p ;
	        i++;
	    }
	}

    public void Toggle()
    {
        if (RelativeTargetY == RelativeMinY) RelativeTargetY = RelativeMaxY;
        else if (RelativeTargetY == RelativeMaxY) RelativeTargetY = RelativeMinY;
        else RelativeTargetY = RelativeMinY;

    }
}

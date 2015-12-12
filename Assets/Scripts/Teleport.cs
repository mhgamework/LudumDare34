using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{

    public Teleport Target;
    public bool TeleportingActive = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (Target != null)
            Gizmos.DrawLine(transform.position + Vector3.up*0.5f, Target.transform.position + Vector3.up * 0.5f);
    }
}

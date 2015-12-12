using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleport : MonoBehaviour
{
    public Teleport Target;
    public bool TeleportingActive = true;

    public float ChargeTime = 2;

    private float chargeAmount = 0;

    public MeshRenderer meshRenderer;
    public ParticleSystem particleSystem;

    private Color beamStartColor;
    // Use this for initialization
    private void Start()
    {
        beamStartColor = meshRenderer.materials[1].color;
        //StartCoroutine(blip().GetEnumerator());
    }

    // Update is called once per frame
    void Update()
    {
        if (!TeleportingActive)
        {
            chargeAmount = 0;
            particleSystem.Stop();

        }
        else
        {
            if (!particleSystem.isPlaying) particleSystem.Play();
        }


        meshRenderer.materials[1].color = Color.Lerp(beamStartColor, Color.red, chargeAmount / ChargeTime);


    }

    public void FixedUpdate()
    {
        chargeAmount = Mathf.Clamp(chargeAmount - Time.deltaTime*2, 0, ChargeTime + 0.001f);
    }

    public IEnumerable<YieldInstruction> blip()
    {
        for (;;)
        {
            /*var betweenBlip = EasingFunctions.Ease(EasingFunctions.TYPE.In, chargeAmount / ChargeTime, 1, 0.1f);

            while (betweenBlip > 0)
            {
                betweenBlip -= Time.deltaTime;
                meshRenderer.materials[1].color = Color.Lerp(beamStartColor, Color.red, betweenBlip);
                yield return null;
            }*/

        }
        return null;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (Target != null)
            Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, Target.transform.position + Vector3.up * 0.5f);


    }

    public void OnTriggerEnter(Collider other)
    {
        var tp = other.GetComponent<Teleportable>();
        if (!tp) return;
        if (tp.InTeleporter) return;
        //tp.InTeleporter = true;
        //tp.TeleportTo(Target.transform.position);
    }

    public void OnTriggerExit(Collider other)
    {
        var tp = other.GetComponent<Teleportable>();
        if (!tp) return;
        //chargeAmount = 0;
    }

    public void OnTriggerStay(Collider other)
    {
        var tp = other.GetComponent<Teleportable>();
        if (!tp) return;
        chargeAmount += Time.deltaTime*3; // Times two since we decrement at every fixedupdate *2



        if (chargeAmount > ChargeTime)
            tp.TeleportTo(Target.transform.position);
    }
}

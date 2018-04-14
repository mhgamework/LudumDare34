using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleport : MonoBehaviour
{
    public Teleport Target;
    public bool TeleportingActive = true;

    public float ChargeTime = 2;

    [Tooltip("Duration of the audio clip")]
    public float AudioChargeTime = 2;

    private float audioPitchModifier { get { return AudioChargeTime / ChargeTime; } }


    private float chargeAmount = 0;

    public MeshRenderer meshRenderer;
    public ParticleSystem particleSystem;

    private Color beamStartColor;
    private float triggerDelayLeft = 0;
    private float targetDelayAfterTP = 1;

    // Use this for initialization
    private void Start()
    {
        beamStartColor = meshRenderer.materials[1].color;
        //StartCoroutine(blip().GetEnumerator());
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerDelayLeft > 0)
        {
            triggerDelayLeft -= Time.deltaTime;
            chargeAmount = 0;
        }

        meshRenderer.materials[1].color = Color.Lerp(beamStartColor, Color.red, chargeAmount / ChargeTime);

        if (chargeAmount < 0.001)
        {
            //GetComponent<AudioSource>().Stop();
        }
        else
        {

            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

        }

    }

    public void FixedUpdate()
    {
        chargeAmount = Mathf.Clamp(chargeAmount - Time.deltaTime, 0, ChargeTime + 0.001f);
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
        GetComponent<AudioSource>().pitch = 1 * audioPitchModifier;
        //tp.InTeleporter = true;
        //tp.TeleportTo(Target.transform.position);
    }

    public void OnTriggerExit(Collider other)
    {
        var tp = other.GetComponent<Teleportable>();
        if (!tp) return;
        //chargeAmount = 0;
        if (chargeAmount > 0) // Play in revers, otherwise dont do anything to let sound finish
            GetComponent<AudioSource>().pitch = -1 * audioPitchModifier;
    }

    public void OnTriggerStay(Collider other)
    {
        var tp = other.GetComponent<Teleportable>();
        if (!tp)
            return;


        chargeAmount += Time.deltaTime * 2; // Times two since we decrement at every fixedupdate 

        if (chargeAmount > ChargeTime)
        {
            tp.TeleportTo(Target.transform.position);
            chargeAmount = 0f;
            Target.SetDelay(targetDelayAfterTP);
        }
    }

    private void SetDelay(float delay)
    {
        triggerDelayLeft = delay;
    }
}

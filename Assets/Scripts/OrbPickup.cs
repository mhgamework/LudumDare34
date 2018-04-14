using UnityEngine;
using System.Collections;

public class OrbPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!IsCollected && other.GetComponent<OrbCollector>() != null)
        {
            IsCollected = true;
            StartCoroutine("GetCollected");
            
            
        }
    }

    IEnumerator GetCollected()
    {
        pickupAudio.Play();

        CenterStairController.Get.ConsumeOrb();

        var target_transform = FindObjectOfType<RobotGirlController>().transform;

        var the_transform = GetComponent<Transform>();
        var start = the_transform.position;
        var target =target_transform.position;
        var elapsed = 0f;
        while (elapsed < CollectAnimationTime)
        {
            the_transform.position = EasingFunctions.Ease(EaseType, elapsed / CollectAnimationTime, start, target);
            elapsed += Time.deltaTime;
            yield return null;
        }
        backgroundAudio.Stop();
        gameObject.SetActive(false);
    }


    [SerializeField]
    private float CollectAnimationTime = 1f;
    [SerializeField]
    private float DriftAnimationTime = 1f;
    [SerializeField]
    private EasingFunctions.TYPE EaseType = EasingFunctions.TYPE.BackInCubic;

    [SerializeField]
    private AudioSource backgroundAudio;
    [SerializeField]
    private AudioSource pickupAudio;

    private bool IsCollected;
}

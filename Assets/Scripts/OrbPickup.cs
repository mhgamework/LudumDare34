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
        float target_height = CenterStairController.Get.ConsumeOrb();

        var the_transform = GetComponent<Transform>();
        var start = the_transform.localPosition;
        var target = new Vector3(0, target_height, 0);
        var elapsed = 0f;
        //var audioStart = backgroundAudio.volume;
        while (elapsed < CollectAnimationTime)
        {
            //backgroundAudio.volume = EasingFunctions.Ease(EaseType, elapsed/CollectAnimationTime, audioStart, 0);
            the_transform.localPosition = EasingFunctions.Ease(EaseType, elapsed / CollectAnimationTime, start, target);
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

using UnityEngine;
using System.Collections;

public class OrbPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (IsCollected || other.GetComponent<OrbCollector>() != null)
        {
            IsCollected = true;
            StartCoroutine("GetCollected");
        }
    }

    IEnumerator GetCollected()
    {
        var the_transform = GetComponent<Transform>();
        var start = the_transform.position;
        var target = new Vector3(0, start.y, 0);

        var elapsed = 0f;
        while (elapsed < CollectAnimationTime)
        {
            the_transform.position = EasingFunctions.Ease(EaseType, elapsed / CollectAnimationTime, start, target);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }


    [SerializeField]
    private float CollectAnimationTime = 1f;
    [SerializeField]
    private float DriftAnimationTime = 1f;
    [SerializeField]
    private EasingFunctions.TYPE EaseType = EasingFunctions.TYPE.BackInCubic;

    private bool IsCollected;
}

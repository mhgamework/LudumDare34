using UnityEngine;
using System.Collections;

public class HearthController : MonoBehaviour
{
    void Start()
    {
        InitialScales = new Vector3[Hearths.Length];
        for (int i = 0; i < Hearths.Length; i++)
        {
            var hearth = Hearths[i];
            hearth.gameObject.SetActive(false);
            InitialScales[i] = hearth.localScale;
        }
    }

    public void ShowHearts()
    {
        for (int i = 0; i < Hearths.Length; i++)
        {
            var hearth = Hearths[i];
            hearth.gameObject.SetActive(false);
            hearth.localScale = InitialScales[i];
        }

        StartCoroutine("UpdateShowHearts");
    }

    IEnumerator UpdateShowHearts()
    {
        for (int i = 0; i < Hearths.Length; i++)
        {
            Hearths[i].gameObject.SetActive(true);
            StartCoroutine("UpdateHeartScale", i);
            yield return new WaitForSeconds(TimeBetweenHearts);
        }
    }

    IEnumerator UpdateHeartScale(int i)
    {
        var end_scale = InitialScales[i];
        var start_scale = end_scale * 0.05f;

        var elapsed = 0f;
        while (elapsed < ScaleAnimTime)
        {
            Hearths[i].localScale = EasingFunctions.Ease(EaseType, elapsed / ScaleAnimTime, start_scale, end_scale);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Hearths[i].localScale = end_scale;
    }

    [SerializeField]
    private Transform[] Hearths = null;
    [SerializeField]
    private EasingFunctions.TYPE EaseType = EasingFunctions.TYPE.InOut;
    [SerializeField]
    private float ScaleAnimTime = 2f;
    [SerializeField]
    private float TimeBetweenHearts = 1.5f;

    private Vector3[] InitialScales;

}

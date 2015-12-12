using UnityEngine;
using System.Collections;

public class PlatformCylinderMover : MonoBehaviour
{

    void OnEnable()
    {
        TryInitialize();
        StartCoroutine("UpdateCylinders");
    }

    private void TryInitialize()
    {
        if (IsInitialized)
            return;
        IsInitialized = true;

        var this_transform = GetComponent<Transform>();

        StartLocations = new Vector3[CylinderTransforms.Length];
        EndLocations = new Vector3[CylinderTransforms.Length];
        for (int i = 0; i < CylinderTransforms.Length; i++)
        {
            StartLocations[i] = CylinderTransforms[i].localPosition;
            EndLocations[i] = StartLocations[i] - this_transform.up * (i + 1) * (i + 1) * 0.02f;
        }
    }

    IEnumerator UpdateCylinders()
    {
        for (int i = 0; i < CylinderTransforms.Length; i++)
        {
            CylinderTransforms[i].localPosition = StartLocations[i];
        }

        var elapsed = 0f;

        float first_half_time = AnimationTime * 0.5f;
        float second_half_time = AnimationTime - first_half_time;

        while (true)
        {
            if (elapsed < first_half_time)
            {
                for (int i = 0; i < CylinderTransforms.Length; i++)
                {
                    CylinderTransforms[i].localPosition = EasingFunctions.Ease(EaseType_Out, elapsed / first_half_time, StartLocations[i], EndLocations[i]);
                }
            }
            else
            {
                for (int i = 0; i < CylinderTransforms.Length; i++)
                {
                    CylinderTransforms[i].localPosition = EasingFunctions.Ease(EaseType_In, (elapsed - first_half_time) / second_half_time, EndLocations[i], StartLocations[i]);
                }
            }

            elapsed += Time.deltaTime;
            elapsed = elapsed % AnimationTime;
            yield return null;
        }
    }

    [SerializeField]
    private Transform[] CylinderTransforms = null;

    [SerializeField]
    private float AnimationTime = 1f;

    [SerializeField]
    private EasingFunctions.TYPE EaseType_Out = EasingFunctions.TYPE.Regular;
    [SerializeField]
    private EasingFunctions.TYPE EaseType_In = EasingFunctions.TYPE.Regular;

    private bool IsInitialized;
    private Vector3[] StartLocations;
    private Vector3[] EndLocations;
}

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CenterStairController : MonoBehaviour
{
    /// <summary>
    /// Returns the height of the heighest stair that is displayed.
    /// </summary>
    /// <returns></returns>
    public float ConsumeOrb()
    {
        Nb_Consumed_Orbs++;
        Debug.Log("Picked up " + Nb_Consumed_Orbs + " Orbs");

        var renderers = GetRenderersToShow(Nb_Consumed_Orbs);
        foreach (var renderer in renderers)
        {
            ToConsume.Add(renderer);
        }

        if (renderers.Count == 0)
            return 0f;

        return renderers.Last().GetComponent<Transform>().position.y;

    }

    public float GetCurrentHeight()
    {
        return CurrentHeight;
    }


    private float CurrentHeight;

    private static CenterStairController instance;
    public static CenterStairController Get
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject();
                obj.AddComponent<CenterStairController>();
                instance = obj.GetComponent<CenterStairController>();
            }
            return instance;
        }
    }

    void Start()
    {
        if (instance != null && instance != this) throw new InvalidOperationException();
        instance = this;

        var this_transform = GetComponent<Transform>();
        for (int i = 0; i < this_transform.childCount; i++)
        {
            var transform_i = this_transform.GetChild(i);
            var renderer_i = transform_i.GetComponent<MeshRenderer>();

            if (renderer_i != null)
            {
                ChildTransforms.Add(transform_i);
                ChildRenderers.Add(renderer_i);

                foreach (var material in renderer_i.materials)
                {
                    material.color = new Color(material.color.r, material.color.g, material.color.b, 0f);
                }
            }
        }

        TotalOrbCount = FindObjectsOfType<OrbPickup>().Count();
        Debug.Log("Total Orb count: " + TotalOrbCount);

        StartCoroutine("UpdateConsume");
    }


    private List<MeshRenderer> GetRenderersToShow(int orb_count)
    {
        if (orb_count > OrbStairHeights[OrbStairHeights.Length - 1].OrbCount)
        {
            //return ChildRenderers.GetRange(0, 0);
            return ChildRenderers.GetRange(0, Mathf.FloorToInt((float)orb_count / (float)TotalOrbCount * (float)ChildRenderers.Count));
        }



        var renderers = new List<MeshRenderer>();
        for (int i = 0; i < OrbStairHeights.Length - 1; i++)
        {
            var minOrbCount = OrbStairHeights[i].OrbCount;
            if (orb_count < minOrbCount)
                continue;

            var maxOrbCount = OrbStairHeights[i + 1].OrbCount;


            var fraction = (float)(orb_count - minOrbCount) / (maxOrbCount - minOrbCount);
            var max_height = Mathf.Lerp(OrbStairHeights[i].Height, OrbStairHeights[i + 1].Height, fraction);

            for (int j = 0; j < ChildTransforms.Count; j++)
            {
                if (ChildTransforms[j].position.y <= max_height)
                    renderers.Add(ChildRenderers[j]);
            }
        }
        return renderers;
    }

    IEnumerator UpdateConsume()
    {
        while (true)
        {
            while (ToConsume.Count == 0)
            {
                yield return null;
            }

            var to_consume = ToConsume[0];
            ToConsume.RemoveAt(0);

            if (!Consumed.Contains(to_consume))
            {
                Consumed.Add(to_consume);
                StartCoroutine("FadeIn", to_consume);
                yield return new WaitForSeconds(0.25f);
            }

        }
    }

    IEnumerator FadeIn(MeshRenderer renderer)
    {
        var elapsed = 0f;
        while (elapsed < FadeTime)
        {
            var alpha = EasingFunctions.Ease(EaseType, elapsed / FadeTime, 0f, 1f);
            foreach (var material in renderer.materials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        var new_materials = new Material[renderer.materials.Length];
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            var material = renderer.materials[i];
            var albedo_tex = material.GetTexture("_MainTex");
            var albedo_color = material.color;

            var new_material = new Material(Shader.Find("Standard"));
            new_material.color = albedo_color;
            new_material.SetTexture("_MainTex", albedo_tex);
            new_materials[i] = new_material;

            // material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
        }
        renderer.materials = new_materials;

        CurrentHeight = renderer.GetComponent<Transform>().position.y;
    }


    private List<Transform> ChildTransforms = new List<Transform>();
    private List<MeshRenderer> ChildRenderers = new List<MeshRenderer>();

    private int TotalOrbCount;

    [SerializeField]
    private float FadeTime = 0.5f;
    [SerializeField]
    private EasingFunctions.TYPE EaseType = EasingFunctions.TYPE.InOut;

    private List<MeshRenderer> ToConsume = new List<MeshRenderer>();
    private List<MeshRenderer> Consumed = new List<MeshRenderer>();
    private int Nb_Consumed_Orbs;

    [Serializable]
    private class OrbCountHeightTuple
    {
        public int OrbCount;
        public float Height;
    }

    [SerializeField]
    private OrbCountHeightTuple[] OrbStairHeights = null;


}

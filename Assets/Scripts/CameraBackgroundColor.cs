using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraBackgroundColor : MonoBehaviour
{
    [Serializable]
    private class HeightColorTuple
    {
        public float Height;
        public Color Color;
    }

    void Start()
    {
        CameraTransform = TheCamera.GetComponent<Transform>();
        ColorsSorted = Colors.OrderBy(e => e.Height).ToList();
    }

    void Update()
    {
        TheCamera.backgroundColor = GetColorFromHeight(CameraTransform.position.y);
    }

    private Color GetColorFromHeight(float height)
    {
        if (height < ColorsSorted[0].Height)
            return ColorsSorted[0].Color;
        if (height >= ColorsSorted[ColorsSorted.Count - 1].Height)
            return ColorsSorted[ColorsSorted.Count - 1].Color;


        for (int i = 0; i < ColorsSorted.Count - 1; i++)
        {
            if (ColorsSorted[i + 1].Height > height)
            {
                return Color.Lerp(ColorsSorted[i].Color, ColorsSorted[i + 1].Color,
                    (height - ColorsSorted[i].Height) / (ColorsSorted[i + 1].Height - ColorsSorted[i].Height));
            }
        }

        return Color.magenta;
    }

    [SerializeField]
    private Camera TheCamera = null;

    private Transform CameraTransform;

    [SerializeField]
    private HeightColorTuple[] Colors = null;
    private List<HeightColorTuple> ColorsSorted = new List<HeightColorTuple>();
}

using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    public class EditorSnappingScript : MonoBehaviour
    {
        public float SnapSize = 1;
        public Vector3 Offset = new Vector3();


        private float scaledSnapSize { get { return transform.lossyScale.x * SnapSize; } }
        private Vector3 scaledOffset { get { return transform.lossyScale.x * Offset; } }


        public void Update()
        {
            if (Application.isPlaying)
                return;

            if (SnapSize == 0)
                SnapSize = 1;
            
            var snap_size = scaledSnapSize;

            var pos = transform.position;
            pos += scaledOffset;

            pos.x = Mathf.Round(pos.x / snap_size) * snap_size;
            pos.y = Mathf.Round(pos.y / snap_size) * snap_size;
            pos.z = Mathf.Round(pos.z / snap_size) * snap_size;

            pos -= scaledOffset;
            transform.position = pos;
        }
    }
}
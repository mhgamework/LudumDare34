using UnityEngine;
using System.Collections;
using System.Linq;

public class BendAroundPoint : MonoBehaviour
{

    public float CircleCircumference = 36;

    // Use this for initialization
    void Start()
    {

        foreach (var filter in GetComponentsInChildren<MeshFilter>())
        {
            var mesh = filter.mesh;



            var verts = mesh.vertices
                .Select(v =>
                {
                    var vWorld = filter.transform.TransformPoint(v);
                    var vBendWorld = bendVertexWorldSpace(vWorld);
                    return vBendWorld;
                }
                ).ToList();

            filter.transform.Rotate(Vector3.up, -Mathf.Rad2Deg * CalculateAngle(filter.transform.position));

            var originTransformedW = bendVertexWorldSpace(filter.transform.position);

            filter.transform.position = originTransformedW;


            verts = verts
            .Select(v =>
            {
                return filter.transform.InverseTransformPoint(v);
            }
            ).ToList();




            //verts = verts.Select()



            mesh.SetVertices(verts);

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();


            var collider = filter.GetComponent<MeshCollider>();
            if (collider) collider.sharedMesh = mesh;
        }


    }

    /*private Vector3 bendVertex(Vector3 v, Transform trans)
    {
        var vWorld = trans.TransformPoint(v);
        var transformedWorld = bendVertexWorldSpace(vWorld);

        var transformedOrigin = trans.TransformPoint(new Vector3());


        var radius = worldRadius;//v.x;
        var angle = v.z / 36 * Mathf.PI*2;

        return new Vector3(radius*Mathf.Cos(angle), v.y, radius*Mathf.Sin(angle));

    }*/

    private Vector3 bendVertexWorldSpace(Vector3 vWorld)
    {

        var radius = vWorld.x;
        var angle = CalculateAngle(vWorld);

        return new Vector3(radius * Mathf.Cos(angle), vWorld.y, radius * Mathf.Sin(angle));

    }

    private float CalculateAngle(Vector3 vWorld)
    {
        var angle = vWorld.z / CircleCircumference * Mathf.PI * 2;
        return angle;
    }


    // Update is called once per frame
    void Update()
    {

    }
}

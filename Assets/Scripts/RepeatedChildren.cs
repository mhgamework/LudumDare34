using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class RepeatedChildren : MonoBehaviour
{

    public int NumRepeat = 4;
    public GameObject Prefab;
    public Vector3 RelativeOffset = new Vector3(0, 0, 1);
    public Vector3 BaseOffset = new Vector3(0, 0, 0);

    private int lastHash;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) return;
        if (!transform.Cast<Transform>().FirstOrDefault(c => c.name.StartsWith("[GEN]")))
        {
            createChildren();
            return;
        }
        if (lastHash == GetHashCode()) return;
        lastHash = GetHashCode();
        createChildren();
    }

    private void createChildren()
    {
        Debug.Log("Generating children");
        foreach (var c in transform.Cast<Transform>().Where(c => c.name.StartsWith("[GEN]")).ToArray())
        {
            DestroyImmediate(c.gameObject);
        }
        for (int i = 0; i < NumRepeat; i++)
        {
            var child = Instantiate(Prefab);
            child.name = "[GEN] " + child.name;
            child.transform.SetParent(transform);
            child.transform.localPosition = BaseOffset + RelativeOffset * i;
            child.transform.localScale = Prefab.transform.localScale;
        }
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ NumRepeat;
            hashCode = (hashCode * 397) ^ (Prefab != null ? Prefab.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ RelativeOffset.GetHashCode();
            hashCode = (hashCode * 397) ^ BaseOffset.GetHashCode();
            return hashCode;
        }
    }
}

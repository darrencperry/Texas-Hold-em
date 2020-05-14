using UnityEngine;
using System.Collections;
using System.Linq;

public class Util : MonoBehaviour
{
    public static void RemoveChildrenFromTransform(Transform transform)
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject, true);
        }
    }
}

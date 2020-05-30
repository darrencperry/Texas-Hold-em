using System.Linq;
using UnityEngine;

public static class TransformExtensions
{
    public static void RemoveAllChildren(this Transform transform)
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            if (Application.isPlaying)
                Object.Destroy(child.gameObject);
            else
                Object.DestroyImmediate(child.gameObject, true);
        }
    }
}
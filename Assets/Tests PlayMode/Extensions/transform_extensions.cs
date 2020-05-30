using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace tests.playmode.extensions
{
    public class transform_extensions
    {
        [UnityTest]
        public IEnumerator transform_extensions_remove_all_children()
        {
            GameObject parentGameObject = new GameObject();
            for (int i = 0; i < 10; i++)
            {
                GameObject childGameObject = new GameObject();
                childGameObject.transform.SetParent(parentGameObject.transform);
            }
            Assert.AreEqual(10, parentGameObject.transform.childCount);

            yield return null;

            parentGameObject.transform.RemoveAllChildren();

            yield return null;

            Assert.AreEqual(0, parentGameObject.transform.childCount);
        }
    }
}

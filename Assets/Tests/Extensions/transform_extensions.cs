using NUnit.Framework;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace tests.editmode.extensions
{
    public class transform_extensions
    {
        [Test]
        public void transform_extensions_remove_all_children()
        {
            // Use the Assert class to test conditions.
            GameObject parentGameObject = new GameObject();
            for (int i = 0; i < 10; i++)
            {
                GameObject childGameObject = new GameObject();
                childGameObject.transform.SetParent(parentGameObject.transform);
            }

            Assert.AreEqual(10, parentGameObject.transform.childCount);

            parentGameObject.transform.RemoveAllChildren();

            Assert.AreEqual(0, parentGameObject.transform.childCount);

        }
    }
}
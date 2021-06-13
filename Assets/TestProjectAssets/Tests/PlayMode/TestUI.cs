using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using TestTaskProject;

namespace Tests
{
    public class TestUI
    {
        private MainUI mainUI;

        [SetUp]
        public void Setup()
        {
            GameObject gameGameObject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainUI"));
            mainUI = gameGameObject.GetComponent<MainUI>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(mainUI.gameObject);
        }

        [UnityTest]
        public IEnumerator UIDisabledEnableWithMessages()
        {
           
           
            yield return null;

            MessageBroker.Default.Publish(new MessageHideIteractUI());

            yield return null;

            Assert.IsTrue(!mainUI.TextEnabled);

            yield return null;

            MessageBroker.Default.Publish(new MessageShowIteractUI());

            yield return null;

            Assert.IsTrue(mainUI.TextEnabled);
        }


    }
}

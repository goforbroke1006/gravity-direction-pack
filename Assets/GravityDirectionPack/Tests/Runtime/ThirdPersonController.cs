using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using GravityDirectionPack.Scripts;

// using GravityDirectionPack.Scripts;

namespace GravityDirectionPack.Tests.Runtime
{
    public class ThirdPersonController
    {
        private const float UpdateFrameFastExec = 1.0f;
        private const float UpdateFrameSlowExec = 25.0f;
        
        // A Test behaves as an ordinary method
        // [Test]
        // public void NewTestScriptSimplePasses()
        // {
        //     //
        // }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator IsGroundedWhenStayOnTheFloor()
        {
            GameObject env = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment"));
            env.transform.position = Vector3.zero;
            
            GameObject cam = Object.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
            cam.transform.position = new Vector3(-0.45988f, 1.375f, -2.869419f);
            
            // wait camera
            yield return new WaitForSeconds(UpdateFrameFastExec);
            
            GameObject ch = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerArmature GDP"));
            ch.transform.position = new Vector3(-1.711f, 0, 1.130581f);
            yield return null;
            Assert.IsTrue(ch.GetComponent<Scripts.ThirdPersonController>().grounded);
            
            Object.Destroy(env);
            Object.Destroy(cam);
            Object.Destroy(ch);
            
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator IsNotGroundedWhenOverTheFloor()
        {
            GameObject env = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment"));
            env.transform.position = Vector3.zero;
            
            GameObject cam = Object.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
            cam.transform.position = new Vector3(-0.45988f, 100, -2.869419f);

            // wait camera
            yield return new WaitForSeconds(UpdateFrameFastExec);
            
            GameObject ch = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerArmature GDP"));
            ch.transform.position = new Vector3(-1.711f, 100, 1.130581f);
            yield return null;
            Assert.IsFalse(ch.GetComponent<Scripts.ThirdPersonController>().grounded);
            
            Object.Destroy(env);
            Object.Destroy(cam);
            Object.Destroy(ch);
            
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}


using System.Collections;
using GravityDirectionPack.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CharacterController = GravityDirectionPack.Scripts.CharacterController;

namespace GravityDirectionPack.Tests.Runtime
{
    public class CharacterControllerTests
    {
        private const float UpdateFrameFastExec = 1.0f;
        private const float UpdateFrameSlowExec = 25.0f;

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
            Assert.IsTrue(ch.GetComponent<CharacterController>().grounded);

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
            GameObject cam = Object.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
            cam.transform.position = new Vector3(-0.45988f, 5, -2.869419f);

            GameObject env = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment"));
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerArmature GDP"));
            ch.transform.position = new Vector3(-1.711f, 5, 1.130581f);

            yield return null;

            Assert.IsFalse(ch.GetComponent<CharacterController>().grounded);

            Object.Destroy(ch);
            Object.Destroy(env);
            Object.Destroy(cam);

            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator IsGroundedWhenStayOnTheRightWall()
        {
            GameObject cam = Object.Instantiate(getCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(getEnvRes());
            env.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(getCharRes());
            ch.transform.position = new Vector3(5.0f, 1.109f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 90.0f);

            yield return null;

            Assert.IsTrue(ch.GetComponent<CharacterController>().grounded);

            Object.Destroy(ch);
            Object.Destroy(env);
            Object.Destroy(cam);

            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        private static GameObject getEnvRes()
        {
            return Resources.Load<GameObject>("Prefabs/Environment");
        }

        private static GameObject getCamRes()
        {
            return Resources.Load<GameObject>("Prefabs/MainCamera");
        }

        private static GameObject getCharRes()
        {
            return Resources.Load<GameObject>("Prefabs/PlayerArmature GDP");
        }
    }
}
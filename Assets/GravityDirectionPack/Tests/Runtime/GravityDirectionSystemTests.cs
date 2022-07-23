using System.Collections;
using GravityDirectionPack.Scripts;
using UnityEngine;
using UnityEngine.TestTools;

namespace GravityDirectionPack.Tests.Runtime
{
    public class GravityDirectionSystemTests
    {
        private const float MaxStuckInFloorDelta = 0.1f;

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DontStuckInOtherColliderWhenFallingOnTheFloor()
        {
            GameObject cam = Object.Instantiate(getCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(getEnvRes());
            env.transform.position = Vector3.zero;

            GameObject gds = Object.Instantiate(getGDSRes());
            gds.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(getCharRes());
            ch.transform.position = new Vector3(2.03f, 3.109f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 0);

            yield return new WaitForSeconds(2.0f);

            MyAssert.InDelta(ch.transform.position.y, 0, MaxStuckInFloorDelta);

            Object.Destroy(ch);
            Object.Destroy(gds);
            Object.Destroy(env);
            Object.Destroy(cam);

            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DontStuckInOtherColliderWhenFallingOnTheRightWall()
        {
            GameObject cam = Object.Instantiate(getCamRes());
            cam.transform.position = new Vector3(1.52f, 1.375f, -8.46f);

            GameObject env = Object.Instantiate(getEnvRes());
            env.transform.position = Vector3.zero;

            GameObject gds = Object.Instantiate(getGDSRes());
            gds.transform.position = Vector3.zero;

            GameObject ch = Object.Instantiate(getCharRes());
            ch.transform.position = new Vector3(2.03f, 1.109f, 1.18f);
            ch.transform.rotation = Quaternion.Euler(0, 0, 90.0f);

            yield return new WaitForSeconds(2.0f);

            MyAssert.InDelta(ch.transform.position.x, 5.0f, MaxStuckInFloorDelta);

            Object.Destroy(ch);
            Object.Destroy(gds);
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

        private static GameObject getGDSRes()
        {
            return Resources.Load<GameObject>("Prefabs/GravityDirectionSystem");
        }

        private static GameObject getCharRes()
        {
            return Resources.Load<GameObject>("Prefabs/PlayerArmature GDP");
        }
    }
}
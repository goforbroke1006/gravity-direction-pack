using System.Collections;
using GravityDirectionPack.Scripts;
using UnityEngine;
using UnityEngine.TestTools;

namespace GravityDirectionPack.Tests.Runtime
{
    public class GravityDirectionSystemTests
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator OneMoreTest()
        {
            //
            yield return null;
        }

        private static GameObject GetEnvRes()
        {
            return Resources.Load<GameObject>("Prefabs/Environment");
        }

        private static GameObject GetCamRes()
        {
            return Resources.Load<GameObject>("Prefabs/MainCamera");
        }

        private static GameObject GetGDSRes()
        {
            return Resources.Load<GameObject>("Prefabs/GravityDirectionSystem");
        }

        private static GameObject GetCharRes()
        {
            return Resources.Load<GameObject>("Prefabs/PlayerArmature GDP");
        }
    }
}
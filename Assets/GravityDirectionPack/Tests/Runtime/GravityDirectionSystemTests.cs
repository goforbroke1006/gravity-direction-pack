using System.Collections;
using System.Threading;
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
        public IEnumerator GravityAppliesToCharacterCorrectly()
        {
            object[][] dataRows =
            {
                new object[]
                {
                    "Falling to ground, decrease along Y",
                    new Vector3(0.07f, 50, -0.54f),
                    Quaternion.Euler(0, 0, 0),
                    MyAssert.PositionChangedAxis.Y,
                    MyAssert.PositionChangedValueDir.Decrease,
                },
                new object[]
                {
                    "Falling to right, increase along X",
                    new Vector3(-15.91f, 6.52f, -0.534f),
                    Quaternion.Euler(-90, 0, 90),
                    MyAssert.PositionChangedAxis.X,
                    MyAssert.PositionChangedValueDir.Increase,
                },
                new object[]
                {
                    "Falling to left, decrease along X",
                    new Vector3(2.44f, 6.52f, -0.534f),
                    Quaternion.Euler(90, -180, 90),
                    MyAssert.PositionChangedAxis.X,
                    MyAssert.PositionChangedValueDir.Decrease,
                },
            };

            SceneHelper.ClearScene();
            GameObject camObj = Object.Instantiate(SceneHelper.GetCamRes());
            camObj.transform.position = new Vector3(1.52f, 4.04f, -12.24f);

            GameObject envObj = Object.Instantiate(SceneHelper.GetEnvRes());
            envObj.transform.position = Vector3.zero;

            GameObject gdsObj = Object.Instantiate(SceneHelper.GetGdsRes());
            gdsObj.transform.position = Vector3.zero;
            GravityDirectionSystem gdsComponent = gdsObj.GetComponent<GravityDirectionSystem>();

            foreach (object[] dataRow in dataRows)
            {
                Debug.Log($"Test case '{dataRow[0]}'");

                GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
                ch.transform.position = (Vector3)dataRow[1];
                ch.transform.rotation = (Quaternion)dataRow[2];
                Camera.main.transform.LookAt(ch.transform.position);
                yield return null; // update frames

                gdsComponent.ReloadControllersList();

                var prevPos = ch.transform.position;
                for (int i = 0; i < 5; i++)
                {
                    yield return null; // update frames
                    Camera.main.transform.LookAt(ch.transform.position);
                    Thread.Sleep(200);

                    var currPos = ch.transform.position;
                    MyAssert.PositionChanged(
                        prevPos,
                        currPos,
                        (MyAssert.PositionChangedAxis)dataRow[3],
                        (MyAssert.PositionChangedValueDir)dataRow[4]
                    );

                    prevPos = currPos;
                }

                Object.Destroy(ch);

                yield return null;
            }

            SceneHelper.ClearScene();
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
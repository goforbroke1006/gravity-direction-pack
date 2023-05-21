using System.Collections;
using GravityDirectionPack.Scripts;
using UnityEngine;
using UnityEngine.TestTools;
using CharacterController = GravityDirectionPack.Scripts.CharacterController;

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
                    new Vector3(0.07f, 2.0f, -0.54f),
                    Quaternion.Euler(0, 0, 0),
                    new Vector3(0.07f, 0.0f, -0.54f),
                },
                new object[]
                {
                    "Falling to right, increase along X",
                    new Vector3(2.0f, 6.5f, -0.5f),
                    Quaternion.Euler(-90, 0, 90),
                    new Vector3(5.0f, 6.5f, -0.5f),
                },
                new object[]
                {
                    "Falling to forward, increase along Z",
                    new Vector3(-1.7f, 5.9f, 2.0f),
                    Quaternion.Euler(-90, 0, 0),
                    new Vector3(-1.7f, 5.9f, 5.0f),
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

            GameObject ch = Object.Instantiate(SceneHelper.GetCharRes());
            var chTransform = ch.transform;
            var gravityDirectionActor = ch.GetComponent<CharacterController>();

            foreach (object[] dataRow in dataRows)
            {
                Debug.Log($"Test case '{dataRow[0]}'");

                // reset character
                chTransform.position = (Vector3)dataRow[1];
                chTransform.rotation = (Quaternion)dataRow[2];
                gravityDirectionActor.fallingSpeed = 0;
                gravityDirectionActor.Move(Vector3.zero);
                yield return null; // update frames

                gdsComponent.ReloadControllersList();

                // var prevPos = ch.transform.position;
                for (int i = 0; i < 15; i++)
                {
                    yield return null; // update frames
                    camObj.transform.LookAt(ch.transform.position);
                    // Thread.Sleep(500);
                }

                var actualCharPosition = chTransform.position;
                var expectedTransPosition = (Vector3)dataRow[3];

                MyAssert.InDelta(expectedTransPosition.x, actualCharPosition.x, 0.1f);
                MyAssert.InDelta(expectedTransPosition.y, actualCharPosition.y, 0.1f);
                MyAssert.InDelta(expectedTransPosition.z, actualCharPosition.z, 0.1f);

                yield return null; // update frames
            }

            Object.Destroy(ch);

            SceneHelper.ClearScene();
            yield return null;
        }
    }
}
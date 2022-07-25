using UnityEngine;
using Object = UnityEngine.Object;

namespace GravityDirectionPack.Tests.Runtime
{
    public static class SceneHelper
    {
        public static void ClearScene()
        {
            var objects = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject o in objects)
            {
                Object.Destroy(o.gameObject);
            }
        }
        
        public static GameObject GetEnvRes()
        {
            return Resources.Load<GameObject>("Prefabs/Environment");
        }

        public static GameObject GetCamRes()
        {
            return Resources.Load<GameObject>("Prefabs/MainCamera");
        }

        public static GameObject GetGdsRes()
        {
            return Resources.Load<GameObject>("Prefabs/GravityDirectionSystem");
        }

        public static GameObject GetCharRes()
        {
            return Resources.Load<GameObject>("Prefabs/PlayerArmature GDP");
        }
    }
}
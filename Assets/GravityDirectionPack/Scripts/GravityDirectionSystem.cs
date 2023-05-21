using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    /// <summary>
    /// GravityDirectionSystem controls player character and NPC characters.
    /// </summary>
    public class GravityDirectionSystem : MonoBehaviour
    {
        public float GravityAcceleration = 9.8f;

        private CharacterController[] _controllers;

        // Start is called before the first frame update
        private void Start()
        {
            ReloadControllersList();
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (CharacterController controller in _controllers)
            {
                if (!controller.grounded)
                {
                    controller.fallingSpeed += GravityAcceleration * Time.deltaTime;
                }
                else
                {
                    if (controller.fallingSpeed > 0.1f)
                        controller.fallingSpeed = 0.0f;
                }

                Vector3 dir = Vector3.down;
                Vector3 movement = dir * controller.fallingSpeed;

                controller.Move(movement, Space.Self);
            }
        }

        public void ReloadControllersList()
        {
            _controllers = FindObjectsOfType<CharacterController>();
        }
    }
}
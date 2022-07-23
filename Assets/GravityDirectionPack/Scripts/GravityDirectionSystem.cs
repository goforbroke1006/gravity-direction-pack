using System.Collections.Generic;
using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    public class GravityDirectionSystem : MonoBehaviour
    {
        private ThirdPersonController[] _controllers;

        // Start is called before the first frame update
        void Start()
        {
            _controllers = GameObject.FindObjectsOfType<ThirdPersonController>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (ThirdPersonController controller in _controllers)
            {
                //controller.fallingSpeed = 0.8f * Time.deltaTime;

                Vector3 dir = Vector3.down; //getDirection(controller.gravityDirection);
                Vector3 movement = dir * (controller.fallingSpeed * Time.deltaTime);
                
                controller.GetComponent<CharacterController>().Move(movement);
            }
        }
    }
}
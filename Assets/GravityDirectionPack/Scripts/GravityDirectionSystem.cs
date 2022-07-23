using System.Collections.Generic;
using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    /// <summary>
    /// System that control player character and NPC characters.
    /// </summary>
    public class GravityDirectionSystem : MonoBehaviour
    {
        private CharacterController[] _controllers;

        // Start is called before the first frame update
        void Start()
        {
            _controllers = GameObject.FindObjectsOfType<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (CharacterController controller in _controllers)
            {
                //controller.fallingSpeed = 0.8f * Time.deltaTime;

                Vector3 dir = Vector3.down; //getDirection(controller.gravityDirection);
                Vector3 movement = dir * (controller.fallingSpeed * Time.deltaTime);
                
                controller.Move(movement);
            }
        }
    }
}
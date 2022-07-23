using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GravityDirectionPack.Scripts
{
    /// <summary>
    /// InputsReader for inputs that defined in ./InputSystem/gravity-direction-pack.inputactions
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputsReader : MonoBehaviour
    {
        public Vector2 move;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }
    }
}
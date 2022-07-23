using UnityEngine;
using UnityEngine.InputSystem;

namespace GravityDirectionPack.Scripts
{
    /// <summary>
    /// Third person controller with movement, jumping and animation
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputsReader))]
    public class ThirdPersonController : MonoBehaviour
    {
        public LayerMask groundLayers;

        public AudioClip LandingAudioClip;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Header("State")] public bool grounded = true;
        public float fallingSpeed = 0;

        private CharacterController _controller;
        private Animator _animator;
        private bool _hasAnimator;
        private InputsReader _input;

        // animation IDs
        private int _animIDGrounded;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        // Start is called before the first frame update
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _hasAnimator = TryGetComponent(out _animator);
            _input = GetComponent<InputsReader>();

            AssignAnimationIDs();
        }

        // Update is called once per frame
        private void Update()
        {
            //_hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (_controller == null)
            {
                _controller = GetComponent<CharacterController>();
            }

            // Gizmos.color = Color.magenta;
            // Gizmos.DrawWireSphere(GetGroundedSphereLocation(), GetGroundedSphereRadius());
#endif
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center),
                    FootstepAudioVolume);
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void JumpAndGravity()
        {
            if (grounded)
            {
                if (fallingSpeed > 0.1f)
                    fallingSpeed = 0;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, false);
                    _animator.SetBool(_animIDGrounded, grounded);
                }
            }
            else
            {
                fallingSpeed += 0.8f;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }

                // TODO: disable jumping
            }
        }

        private void GroundedCheck()
        {
            grounded = Physics.CheckSphere(
                GetGroundedSphereLocation(),
                GetGroundedSphereRadius(),
                groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void Move()
        {
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            _controller.Move(inputDirection * (15.0f * Time.deltaTime));

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDMotionSpeed, 1);
            }
        }

        private Vector3 GetGroundedSphereLocation()
        {
            return transform.position + transform.rotation * Vector3.down * (-1 * GetGroundedSphereRadius());
        }

        private float GetGroundedSphereRadius()
        {
            return _controller.radius;
        }
    }
}
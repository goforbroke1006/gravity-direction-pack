using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        public LayerMask groundLayers;
        
        public AudioClip LandingAudioClip;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Header("State flags")] public bool grounded = true;

        private CharacterController _controller;
        private Animator _animator;
        private bool _hasAnimator;

        // animation IDs
        private int _animIDGrounded;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        // Start is called before the first frame update
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _hasAnimator = TryGetComponent(out _animator);
            if (_hasAnimator)
            {
                Debug.Log("animator found");
            }

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

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(GetGroundedSphereLocation(), GetGroundedSphereRadius());
#endif
        }
        
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
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
                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, false);
                    _animator.SetBool(_animIDGrounded, grounded);
                }
            }
            else
            {
                _controller.Move(new Vector3(0.0f, -1.2f, 0.0f) * Time.deltaTime);
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
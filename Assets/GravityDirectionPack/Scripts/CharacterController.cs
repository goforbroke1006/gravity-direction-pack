using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    [ExecuteInEditMode]
    public class CharacterController : MonoBehaviour
    {
        public LayerMask groundLayers;

        [Header("State")] public bool grounded = true;
        public float fallingSpeed = 0;

        private CapsuleCollider _collider;
        //private Vector3 lastMove = Vector3.zero;

        // Start is called before the first frame update
        void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            UpdateProperties();
        }

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<CapsuleCollider>();
            UpdateProperties();
        }

        // Update is called once per frame

        void Update()
        {
            GroundedCheck();

            if (grounded)
            {
                if (fallingSpeed > 0.1f)
                    fallingSpeed = 0;
            }
            else
            {
                fallingSpeed += 0.8f;
            }
            UpdateProperties();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetTopSphereCenter(), _collider.radius);
            Gizmos.DrawWireSphere(GetBottomSphereCenter(), _collider.radius);
#endif
        }

        private void GroundedCheck()
        {
            grounded = Physics.CheckSphere(
                GetGroundedSphereLocation(),
                GetGroundedSphereRadius(),
                groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private Vector3 GetGroundedSphereLocation()
        {
            return transform.position + transform.rotation * Vector3.down * (-1 * GetGroundedSphereRadius());
        }

        private float GetGroundedSphereRadius()
        {
            return _collider.radius;
        }


        public void Move(Vector3 movement)
        {
            if (movement.Equals(Vector3.zero))
                return;

            // correct movement if can be collision in next point
            // draft solution - move on 5% of distance
            // TODO: deal with collisions with binary search
            bool collision = Physics.CheckCapsule(
                GetTopSphereCenter(),
                GetBottomSphereCenter(),
                _collider.radius
            );
            if (collision)
            {
                movement *= 0.1f;
            }

            transform.Translate(movement);
        }

        private Vector3 GetTopSphereCenter()
        {
            return _collider.transform.position +
                   _collider.transform.rotation * Vector3.up * (_collider.height - _collider.radius);
        }

        private Vector3 GetBottomSphereCenter()
        {
            return _collider.transform.position + _collider.transform.rotation * Vector3.down * (-1 * _collider.radius);
        }

        private void UpdateProperties()
        {
            radius = _collider.radius;
            center = transform.position;
        }

        public float radius { get; private set; }
        public Vector3 center { get; private set; }
    }
}
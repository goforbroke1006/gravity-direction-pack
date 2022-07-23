using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    [ExecuteInEditMode]
    public class CharacterController : MonoBehaviour
    {
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

        private void UpdateProperties()
        {
            radius = _collider.radius;
            center = transform.position;
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

        public float radius; // { get; private set; }
        public Vector3 center; // { get; private set; }
    }
}
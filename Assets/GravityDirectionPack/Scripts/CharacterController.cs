using System;
using UnityEngine;

namespace GravityDirectionPack.Scripts
{
    /// <summary>
    /// CharacterController allow move character in specified direction.
    /// Can be applied to:
    ///   - player character (be controlled with player input)
    ///   - NPC
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider))]
    [ExecuteInEditMode]
    public class CharacterController : MonoBehaviour
    {
        public LayerMask groundLayers;

        [Header("State")] public bool grounded = true;
        public float fallingSpeed;

        private CapsuleCollider _collider;

        // Start is called before the first frame update
        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            if (!_collider.isTrigger)
            {
                Debug.Log("auto enable _collider.isTrigger");
                _collider.isTrigger = true;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        // Update is called once per frame
        private void Update()
        {
            GroundedCheck();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetTopSphereCenter(), _collider.radius);
            Gizmos.DrawWireSphere(GetBottomSphereCenter(), _collider.radius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(GetGroundedSphereLocation(), GetGroundedSphereRadius());
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

        private Vector3 GetGroundedSphereLocation() // TODO: add tests
        {
            var tr = transform;
            var pos = Vector3.up; // sphere over lowest char point
            pos -= new Vector3(0, 0.05f, 0); // a little bit lower than char to detect grounding event
            pos *= GetGroundedSphereRadius(); // sphere higher on radius distance
            return tr.position + tr.rotation * pos;
        }

        private float GetGroundedSphereRadius()
        {
            return _collider.radius;
        }

        private const float HorHitMargin = 0.05f;
        private Vector3 _horDirFinal = Vector3.zero;
        private Vector3 _horHitDirection;
        private float _horHitDistance;
        private readonly Collider[] _horHitColliders = new Collider[5];

        /// <summary>
        /// Move character relative to ...
        /// </summary>
        /// <param name="motion"></param>
        /// <param name="relativeTo"></param>
        public void Move(Vector3 motion, Space relativeTo = Space.World)
        {
            this.velocity = motion;

            if (motion == Vector3.zero)
                return;

            if (motion.y < 0)
            {
                // movement down
                // try to check with raycast

                // calculate distance between lowest capsule point
                // and potential collision point
                // and correct movement to bottom (along Y axis)
                var tr = transform;
                Ray rayUnder = new Ray(tr.position, tr.rotation * Vector3.down);
                if (Physics.Raycast(rayUnder, out var hit, Math.Abs(motion.y)))
                {
                    motion.y = -1 * hit.distance;
                }
            }

            // movement in horizontal plane
            if (motion.x != 0 || motion.z != 0)
            {
                _horDirFinal.x = motion.x;
                _horDirFinal.z = motion.z;

                int numHit = Physics.OverlapCapsuleNonAlloc(
                    GetTopSphereCenter() + _horDirFinal.normalized * (_horDirFinal.magnitude + HorHitMargin),
                    GetBottomSphereCenter() + _horDirFinal.normalized * (_horDirFinal.magnitude + HorHitMargin),
                    _collider.radius,
                    _horHitColliders,
                    groundLayers,
                    QueryTriggerInteraction.Ignore);

                var thisTr = transform;

                for (int i = 0; i < numHit; i++)
                {
                    var other = _horHitColliders[i];
                    var otherTr = other.transform;

                    bool hasPenetration = Physics.ComputePenetration(
                        _collider, thisTr.position, thisTr.rotation,
                        other, otherTr.position, otherTr.rotation,
                        out _horHitDirection, out _horHitDistance);

                    if (hasPenetration)
                    {
                        if (relativeTo == Space.Self)
                        {
                            _horDirFinal += transform.InverseTransformDirection(_horHitDirection * _horHitDistance);
                        }
                        else
                        {
                            _horDirFinal += _horHitDirection * _horHitDistance;
                        }
                    }
                }

                motion.x = _horDirFinal.x;
                motion.z = _horDirFinal.z;
            }

            transform.Translate(motion, relativeTo);
        }

        private Vector3 GetTopSphereCenter()
        {
            var tr = _collider.transform;
            return tr.position + tr.rotation * Vector3.up * (_collider.height - _collider.radius);
        }

        private Vector3 GetBottomSphereCenter()
        {
            var tr = _collider.transform;
            return tr.position
                   + tr.rotation * Vector3.down * (-1 * _collider.radius);
        }

        /// <summary>
        ///   <para>The current relative velocity of the Character (see notes).</para>
        /// </summary>
        public Vector3 velocity { get; private set; }

        /// <summary>
        ///   <para>The center of the character's capsule relative to the transform's position.</para>
        /// </summary>
        public Vector3 center => transform.position + Vector3.up * _collider.height / 2;

        /// <summary>
        ///   <para>The radius of the character's capsule.</para>
        /// </summary>
        public float radius => _collider.radius;
    }
}
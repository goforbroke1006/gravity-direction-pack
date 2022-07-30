using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace GravityDirectionPack.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    [ExecuteInEditMode]
    public class CharacterController : MonoBehaviour
    {
        public LayerMask groundLayers;

        [Header("State")] public bool grounded = true;
        public float fallingSpeed = 0;
        public Vector3 movement = Vector3.zero;

        private CapsuleCollider _collider;
        //private Vector3 lastMove = Vector3.zero;

        // Start is called before the first frame update
        void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            if (!_collider.isTrigger)
            {
                Debug.Log("auto enable _collider.isTrigger");
                _collider.isTrigger = true;
            }

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
            UpdateProperties();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetTopSphereCenter(), _collider.radius);
            Gizmos.DrawWireSphere(GetBottomSphereCenter(), _collider.radius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(GetGroundedSphereLocation(), GetGroundedSphereRadius());

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetHorizontalRayInitPos() + GetHorizontalRayDirection(), 0.1f);
#endif
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log(other.bounds.center);
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
            Vector3 pos = Vector3.up; // sphere over lowest char point
            pos -= new Vector3(0, 0.05f, 0); // a little bit lower than char to detect grounding event
            pos *= GetGroundedSphereRadius(); // sphere higher on radius distance
            return tr.position + tr.rotation * pos;
        }

        private float GetGroundedSphereRadius()
        {
            return _collider.radius;
        }

        public void Move(Vector3 move) => this.Move(move, Space.World);

        /// <summary>
        /// Move character relative to self
        /// </summary>
        /// <param name="move"></param>
        /// <param name="relativeTo"></param>
        public void Move(Vector3 move, Space relativeTo)
        {
            if (move.Equals(Vector3.zero))
                return;

            movement = move;

            if (move.y < 0)
            {
                // movement down
                // try to check with raycast

                // calculate distance between lowest capsule point
                // and potential collision point
                // and correct movement to bottom (along Y axis)
                Ray rayUnder = new Ray(transform.position, transform.rotation * Vector3.down);
                if (Physics.Raycast(rayUnder, out var hit, Math.Abs(move.y)))
                {
                    move.y = -1 * hit.distance;
                }
            }

            // movement in horizontal plane
            if (move.x != 0 || move.z != 0)
            {
                Vector3 horizontalDirection = new Vector3(move.x, 0, move.z);

                // Ray rayRight = new Ray(GetHorizontalRayInitPos(), GetHorizontalRayDirection());
                // if (Physics.Raycast(rayRight, out var hit, horizontalDirection.magnitude))
                // {
                //     if (hit.distance < horizontalDirection.magnitude)
                //     {
                //         if (Math.Abs(hit.distance - horizontalDirection.magnitude) < 0.05f)
                //         {
                //             move.x = 0;
                //             move.z = 0;
                //         }
                //         else
                //         {
                //             Debug.Log("allowed " + hit.distance + " wanted " + horizontalDirection.magnitude);
                //         
                //             float correctedDistance = horizontalDirection.magnitude - hit.distance;
                //             // distance -= 0.05f; // add margin
                //             Vector3 final = horizontalDirection.normalized * correctedDistance; // makes short
                //             move.x = final.x;
                //             move.z = final.z;
                //         }
                //     }
                // }

                // correct movement if can be collision in next point
                // draft solution - move on 5% of distance
                // TODO: deal with collisions with binary search

                float margin = 0.05f;
                
                bool collision = Physics.CheckCapsule(
                    GetTopSphereCenter() + horizontalDirection.normalized * (horizontalDirection.magnitude + margin),
                    GetBottomSphereCenter() + horizontalDirection.normalized * (horizontalDirection.magnitude + margin),
                    _collider.radius,
                    groundLayers,
                    QueryTriggerInteraction.Ignore
                );
                if (collision)
                {
                    float nextTotalDistance = horizontalDirection.magnitude;
                    float nextDistanceChanges = nextTotalDistance / 2;

                    const int maxAttemptsFindBestDistanceWithBinarySearch = 10;
                    for (int i = 0; i < maxAttemptsFindBestDistanceWithBinarySearch; i++)
                    {
                        Vector3 nextPositionMovement = horizontalDirection.normalized * nextTotalDistance;

                        collision = Physics.CheckCapsule(
                            GetTopSphereCenter() + nextPositionMovement,
                            GetBottomSphereCenter() + nextPositionMovement,
                            _collider.radius
                        );

                        if (collision)
                            nextTotalDistance -= nextDistanceChanges;
                        else
                            nextTotalDistance += nextDistanceChanges;

                        nextDistanceChanges /= 2;
                    }

                    //Debug.Log($"want {horizontalDirection.magnitude} recommend {nextTotalDistance:00.0}");
                    Vector3 final = horizontalDirection.normalized * nextTotalDistance; // makes short

                    move.x = final.x;
                    move.z = final.z;
                }
            }
            
            transform.Translate(move, relativeTo);
        }

        private Vector3 GetHorizontalRayInitPos()
        {
            Vector3 horizontalDirection = new Vector3(movement.x, 0, movement.z);

            var tr = transform;
            var rot = tr.rotation;

            // in middle of body
            Vector3 rayInitPos = tr.position + rot * Vector3.up * _collider.height / 2;

            // move point outside capsule collider
            rayInitPos += rot * horizontalDirection.normalized * _collider.radius;

            return rayInitPos;
        }

        private Vector3 GetHorizontalRayDirection()
        {
            Vector3 horizontalDirection = new Vector3(movement.x, 0, movement.z);

            return transform.rotation * horizontalDirection;
        }

        private Vector3 GetTopSphereCenter()
        {
            var tr = _collider.transform;
            return tr.position
                   + tr.rotation * Vector3.up * (_collider.height - _collider.radius);
        }

        private Vector3 GetBottomSphereCenter()
        {
            var tr = _collider.transform;
            return tr.position
                   + tr.rotation * Vector3.down * (-1 * _collider.radius);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDirectionSystem : MonoBehaviour
{
    public enum GravityDirection
    {
        XNegative,
        XPositive,
        YNegative,
        YPositive,
        ZNegative,
        ZPositive,
    };



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GravityDirectionEntity[] components = GameObject.FindObjectsOfType<GravityDirectionEntity>();
        foreach (GravityDirectionEntity entity in components)
        {
            entity.fallingSpeed += 9.8f * Time.deltaTime;

            Vector3 dir = getDirection(entity.gravityDirection);
            Vector3 move = dir * entity.fallingSpeed * Time.deltaTime;
            //entity.GetComponent<Rigidbody>().AddForce(move, ForceMode.Impulse);
            entity.GetComponent<Rigidbody>().AddForceAtPosition(move, entity.gravityAnchor.position, ForceMode.Impulse);
            //correctRotation(entity);
            //freezeRotation(entity);
        }
    }

    private Vector3 getDirection(GravityDirection gravityDirection)
    {
        Vector3 dir = Vector3.zero;
        switch (gravityDirection)
        {
            case GravityDirection.XNegative:
                return Vector3.left;
            case GravityDirection.XPositive:
                return Vector3.right;
            case GravityDirection.YNegative:
                return Vector3.up;
            case GravityDirection.YPositive:
                return Vector3.down;
            case GravityDirection.ZNegative:
                return Vector3.back;
            case GravityDirection.ZPositive:
                return Vector3.forward;
        }

        return dir;
    }

    private void correctRotation(GravityDirectionEntity entity)
    {
        Quaternion newRot = entity.transform.rotation;
        switch (entity.gravityDirection)
        {
            //case GravityDirection.XNegative:
            //    dir = Vector3.left;
            //    break;
            case GravityDirection.XPositive:
                newRot.x = 0;
                newRot.y = 0;
                newRot.z = -90;
                break;
            //case GravityDirection.YNegative:
            //    dir = new Vector3();
            //    break;
            case GravityDirection.YPositive:
                newRot.x = 0;
                newRot.z = 0;
                break;
                //case GravityDirection.ZNegative:
                //    dir = Vector3.back;
                //    break;
                //case GravityDirection.ZPositive:
                //    dir = Vector3.forward;
                //    break;
        }

        float speedRot = 50; // TODO: move to params
        //if (entity.fallingSpeed < -1)
        //{
        //    speedRot = 200;
        //}
        //Debug.Log("Rotate to " + dir);
        entity.transform.rotation = Quaternion.RotateTowards(entity.transform.rotation, newRot, speedRot * Time.deltaTime);
    }

    private void freezeRotation(GravityDirectionEntity entity)
    {
        RigidbodyConstraints constraints = RigidbodyConstraints.None;
        switch (entity.gravityDirection)
        {
            case GravityDirection.XNegative:
            case GravityDirection.XPositive:
                constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                break;
            case GravityDirection.YNegative:
            case GravityDirection.YPositive:
                constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
            case GravityDirection.ZNegative:
            case GravityDirection.ZPositive:
                constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                break;
        }
        entity.GetComponent<Rigidbody>().constraints = constraints;
    }
}

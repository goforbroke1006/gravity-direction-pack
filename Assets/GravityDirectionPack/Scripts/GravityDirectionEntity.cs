using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GravityDirectionSystem;

[RequireComponent(typeof(Rigidbody))]

public class GravityDirectionEntity : MonoBehaviour
{
    public Transform gravityAnchor = null;

    public GravityDirection gravityDirection = GravityDirection.YPositive;

    private Rigidbody rigidBody = null;
    private bool stayOnSmthFlag = false;

    public float fallingSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();

        if (this.rigidBody.useGravity)
        {
            Debug.LogWarning("You can't use gravity for GravityDirectionEntity. This option will be disabled.");
            this.rigidBody.useGravity = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.stayOnSmthFlag)
        {
            
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("entity hit something");
        this.fallingSpeed = 0;
        this.stayOnSmthFlag = true;
    }

    void OnCollisionExit(Collision other)
    {
        this.stayOnSmthFlag = false;
    }
}

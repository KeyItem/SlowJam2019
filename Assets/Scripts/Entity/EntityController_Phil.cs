using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController_Phil : EntityController
{
    public float jumpStrength = 5;
    public float jumpBoostStrength = 15;
    public float boostStrength = 50;

    Renderer rend;
    
    private bool grounded = true;
    
    void Update()
    {
        if(transform.localScale[0]>0.9)
        {
            transform.localScale -= new Vector3(0.01F, 0.01F, 0.01F);
        } else if (transform.localScale[0] > 0.7)
        {
            transform.localScale -= new Vector3(0.001F, 0.001F, 0.001F);
        }
        else
        {
            transform.localScale -= new Vector3(0.001F, 0.001F, 0.001F);
        }

    }
    
    void FixedUpdate()
    {
        if (Input.GetKey("left"))
        {
            myRigidbody.AddForce(Vector3.left *
                               ((Input.GetKey("b")) ?
                                   boostStrength :
                                   jumpStrength)
            );
        }

        if (Input.GetKey("right"))
        {
            myRigidbody.AddForce(Vector3.right *
                               ((Input.GetKey("b")) ?
                                   boostStrength :
                                   jumpStrength)
            );
        }

        if (Input.GetKey("down"))
        {
            myRigidbody.AddForce(Vector3.down *
                               ((Input.GetKey("b")) ?
                                   boostStrength :
                                   jumpStrength)
            );
        }


        if (Input.GetKey("space") || Input.GetKey("up"))
        {
            Jump();
        }

    }
    
    public void GrowFromItem()
    {
        Vector3 newSize = new Vector3(4, 4, 4);
        transform.localScale = newSize;
    }
    
    void OnCollisionStay(Collision collision)
    {
        grounded = false;
        foreach(ContactPoint contact in collision.contacts)
        {
            if(Vector3.Dot(contact.normal, Vector3.up) > 0.25f)
            {
                grounded = true;
            }
        }
    }

    void Jump()
    {
        if (grounded)
        {
            grounded = false;
            myRigidbody.AddForce(Vector3.up *
                               ((Input.GetKey("b")) ?
                                   jumpBoostStrength :
                                   jumpStrength),
                ForceMode.Impulse);
        }
    }
}

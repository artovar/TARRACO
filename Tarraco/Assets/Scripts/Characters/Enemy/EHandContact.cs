using UnityEngine;

public class EHandContact : MonoBehaviour
{
    public BasicEnemyController APR_Player;

    //Is left or right hand
    public bool Left;

    //Have joint/grabbed
    public bool hasJoint;


    void Update()
    {
        //Left Hand
        //On input release destroy joint
        if (Left)
        {
            if (hasJoint && !APR_Player.drop)
            {
                this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
                hasJoint = false;
            }

            if (hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
            {
                hasJoint = false;
            }
        }

        //Right Hand
        //On input release destroy joint
        if (!Left)
        {
            if (hasJoint && !APR_Player.interact)
            {
                this.gameObject.GetComponent<FixedJoint>().breakForce = 0;
                hasJoint = false;
            }

            if (hasJoint && this.gameObject.GetComponent<FixedJoint>() == null)
            {
                hasJoint = false;
            }
        }
    }

    //Grab on collision when input is used
    void OnCollisionEnter(Collision col)
    {
        //Left Hand
        if (Left)
        {
            if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
            {
                if (APR_Player.drop && !hasJoint && !APR_Player.punchingLeft)
                {
                    hasJoint = true;
                    this.gameObject.AddComponent<FixedJoint>();
                    this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                    this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                }
            }

        }

        //Right Hand
        if (!Left)
        {
            if (col.gameObject.tag == "CanBeGrabbed" && col.gameObject.layer != LayerMask.NameToLayer(APR_Player.thisPlayerLayer) && !hasJoint)
            {
                if (APR_Player.interact && !hasJoint && !APR_Player.attacking)
                {
                    hasJoint = true;
                    this.gameObject.AddComponent<FixedJoint>();
                    this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
                    this.gameObject.GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                }
            }
        }
    }
}
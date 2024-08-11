using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool onGround { get; private set; }
    public bool onWall { get; private set; }

    public float Friction { get; private set; }

    public Vector2 ContactNormal { get; private set; }
    private PhysicsMaterial2D _material;

    public AudioClip bigSlap;
    [SerializeField] AudioSource audioSource;

    private bool usedPortal = false;

    [SerializeField] GameObject targetLocationCave;
    [SerializeField] GameObject targetLocationDesert;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elite4")
            audioSource.PlayOneShot(bigSlap, 0.7f);
        EvaluateCollision(collision);
        RetrieveFriction(collision);



    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        Friction = 0;
        onWall = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Area") 
        {
            collider.GetComponent<BoxCollider2D>().gameObject.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().Play(); 
        }

        if (collider.gameObject.tag == "caveToDesert" && usedPortal == false)
        {
            this.transform.position = targetLocationDesert.transform.position;
            usedPortal = true;
        }
        else if (collider.gameObject.tag == "desertToCave" && usedPortal == false)
        {
            this.transform.position = targetLocationCave.transform.position;
            usedPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Area")
            collider.GetComponent<BoxCollider2D>().gameObject.GetComponent<AudioSource>().Stop();

        if(collider.gameObject.tag == "caveToDesert" ||  collider.gameObject.tag == "desertToCave")
            usedPortal = false;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            onGround |= ContactNormal.y >= 0.9f;
            onWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        _material = collision.rigidbody.sharedMaterial;

        Friction = 0;

        if (_material != null)
        {
            Friction = _material.friction;
        }
    }

    public bool GetOnGround()
    {
        return onGround;
    }

    public float GetFriction()
    {
        return Friction;
    }
}

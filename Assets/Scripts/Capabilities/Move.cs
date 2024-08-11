using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Controller))]

public class Move : MonoBehaviour
{
    Animator animator;
    //[SerializeField] private InputController input = null;
    private Controller _controller;

    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private SpriteRenderer _spriteRenderer;
    private CollisionDataRetriever _collisionDataRetriever;

    public AudioClip gameStart;
    AudioSource audioSource;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        _controller = GetComponent<Controller>();

        audioSource.PlayOneShot(gameStart, 0.7f);
    }


    // Update is called once per frame
    void Update()   
    {
        direction.x = _controller.input.RetrieveMoveInput();
        direction.y = _controller.input.RetrieveVerticalInput();

        if (direction.x != 0 && direction.y == 0)
            animator.SetBool("horizontal", true);

        if (direction.x == 0 && direction.y > 0)
            animator.SetBool("up", true);
        if (direction.x == 0 && direction.y < 0)
            animator.SetBool("down", true);

        if (velocity.x == 0)
        {
            animator.SetBool("horizontal", false);
        }

        if (velocity.y == 0)
        {
            animator.SetBool("up", false);
            animator.SetBool("down", false);
        }


        {
            if ((direction.x > 0) || (velocity.y > 0.01 && velocity.x > 0.01) || (velocity.y < -0.01 && direction.x > 0.01))
                _spriteRenderer.flipX = false;

            else if ((direction.x < 0) || (velocity.y > 0.01 && velocity.x < -0.01) || (velocity.y < -0.01 && direction.x < -0.01))
                _spriteRenderer.flipX = true;
        } //flip sprite don't touch this you stupid lovely beautiful dumb bitch

        desiredVelocity = new Vector2(direction.x, direction.y) * Mathf.Max(maxSpeed - _collisionDataRetriever.GetFriction(), 0f);


    }

    private void FixedUpdate()
    {
        //onGround = _collisionDataRetriever.GetOnGround();

        velocity = body.velocity;

/*        if (Input.GetButton("Shift"))
            maxSpeed = 100f;
        else
            maxSpeed = 4f;*/

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = maxAirAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);

        body.velocity = velocity;
    }
}

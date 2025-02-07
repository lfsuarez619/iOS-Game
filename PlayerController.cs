using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float moveSpeed = 3f;
    private float maxSpeed = 6f;
    private float speedIncrease = 0.5f;
    private float touchStartTime;
    private bool isPressing = false;

    private AudioSource audioSource; // Reference to the audio source

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartTime = Time.time;
                    isPressing = true;
                    break;

                case TouchPhase.Moved:
                    Vector2 swipeDelta = touch.deltaPosition.normalized;
                    moveDirection = swipeDelta;
                    break;

                case TouchPhase.Stationary:
                    if (Time.time - touchStartTime > 1f)
                    {
                        IncreaseSpeed();
                    }
                    break;

                case TouchPhase.Ended:
                    if (Time.time - touchStartTime < 0.2f)
                    {
                        StopMovement();
                    }
                    isPressing = false;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;

        // Get the screen boundaries
        Vector2 minBounds = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 maxBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // Clamp the player's position within the screen boundaries
        Vector2 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
    
        transform.position = clampedPosition;
    }


    void IncreaseSpeed()
    {
        moveSpeed = Mathf.Min(moveSpeed + speedIncrease, maxSpeed);
    }

    void StopMovement()
    {
        moveDirection = Vector2.zero;
        moveSpeed = 3f;  // Reset speed when stopping

        PlaySoundEffect(); // Play sound when stopping
    }

    void PlaySoundEffect()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

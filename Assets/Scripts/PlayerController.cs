using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public ScrollingBackground scrollingBackground;
    public PauseScreen pauseScreen;
    private AnimatorStateInfo stateInfo;
    public AudioSource mainAudioSource;
    public AudioSource runningAudioSource;
    public AudioSource jumpingAudioSource;
    public AudioClip sheriff_speak;
    public AudioClip horse_neighing;
    public AudioClip jumping;
    public AudioClip running;
    public AudioClip falling;

    private bool isJumping = false;
    public bool isFalling = false;
    private float scrollSpeed;
    private float animSpeed;
    public bool flagFall = true;
    private float originalSpeed = 0;
    public bool isPaused;
    private float fallingDuration = -0.3f;
    private bool flagFirstJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animSpeed = anim.speed;

        mainAudioSource = gameObject.AddComponent<AudioSource>();
        runningAudioSource = gameObject.AddComponent<AudioSource>();
        jumpingAudioSource = gameObject.AddComponent<AudioSource>();
        runningAudioSource.clip = running;
        runningAudioSource.loop = true;

        mainAudioSource.volume = 0.2f;
        runningAudioSource.volume = 0.2f;
        jumpingAudioSource.volume = 0.2f;
        
        Jump();
        mainAudioSource.Play();
        mainAudioSource.PlayOneShot(sheriff_speak);
        mainAudioSource.PlayOneShot(horse_neighing);
    }

    void Update()
    {
        isPaused = pauseScreen.isPaused;
        scrollSpeed = scrollingBackground.speed;

        // Checking if any other sound is playing before playing the running sound
        if (!mainAudioSource.isPlaying && !runningAudioSource.isPlaying && !jumpingAudioSource.isPlaying
            && flagFall)
        {
            runningAudioSource.Play();
        }
        else if (mainAudioSource.isPlaying || jumpingAudioSource.isPlaying || !flagFall)
        {
            runningAudioSource.Stop();
        }

        if (!isFalling && flagFall)
        {
            // Move Left and Right
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            // Check if the player does not go out of boundaries
            if (transform.position.x >= 6.5)
            {
                horizontalInput = Mathf.Min(horizontalInput, 0f);
            }
            else if (transform.position.x <= -11.5)
            {
                horizontalInput = Mathf.Max(horizontalInput, 0f);
            }

            // Adjust the speed of the player
            rb.velocity = new Vector2(horizontalInput * scrollSpeed * 25, isJumping ? rb.velocity.y : 0);

            animSpeed = Mathf.Clamp(5 * scrollSpeed, 1f, 2f);
            anim.speed = animSpeed;
            if (originalSpeed != 0)
            {
                originalSpeed = animSpeed;
            }

            if (!isPaused){
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)
                    || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
                {
                    if (runningAudioSource.isPlaying)
                    {
                        runningAudioSource.Stop();
                    }
                    Jump();
                }
            }

            // Adjust the speed of the player running animation if the player is moving left or right
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("running"))
            {
                if (horizontalInput == -1)
                {
                    if (originalSpeed == 0)
                    {
                        originalSpeed = anim.speed;
                    }
                    // Decrease the speed of the running animation if the player is moving left
                    anim.speed = originalSpeed * 0.5f;
                }
                else if (horizontalInput == 1)
                {
                    if (originalSpeed == 0)
                    {
                        originalSpeed = anim.speed;
                    }
                    // Increase the speed of the running animation if the player is moving right
                    anim.speed = originalSpeed * 1.5f;
                }
                else
                {
                    if (originalSpeed != 0)
                    {
                        anim.speed = originalSpeed;
                        originalSpeed = 0;
                    }
                }
            }
            anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
            // TODO: Check if Speed variable exists in the animator
            anim.SetBool("isFalling", isFalling);
            transform.position = new Vector3(transform.position.x, -2.2f, transform.position.z);
        }
        else
        {
            // FALLING STATE
            if (originalSpeed != 0)
            {
                anim.speed = originalSpeed;
                originalSpeed = 0;
            }
            if (flagFall)
            {
                anim.Play("falling");
                mainAudioSource.Stop();
                runningAudioSource.Stop();
                jumpingAudioSource.Stop();
                mainAudioSource.PlayOneShot(falling);
                flagFall = false;
            }
            isFalling = false;
            anim.SetBool("isFalling", isFalling);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Wait for the falling animation to finish before loading the scoring scene
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("falling"))
        {
            fallingDuration += stateInfo.length;
            StartCoroutine(StartElapsedTime(fallingDuration));
        }

        IEnumerator StartElapsedTime(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("ScoringScene");
        }

        // Pause all audio sources if the game is paused
        if (isPaused)
        {
            mainAudioSource.Pause();
            runningAudioSource.Pause();
            jumpingAudioSource.Pause();
        }
        else
        {
            mainAudioSource.UnPause();
            runningAudioSource.UnPause();
            jumpingAudioSource.UnPause();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "barrel(Clone)" && !isFalling)
        {
            isFalling = true;
            anim.SetBool("isFalling", isFalling);
        }
    }

    void Jump()
    {
        if (isFalling)
        {
            return;
        }

        isJumping = true;
        anim.SetBool("isJumping", isJumping);

        anim.Play("jumping");

        // Added this line to check if the first jump is made so that the jump sound does not
        // overlap with the horse neighing sound
        if (flagFirstJump && !jumpingAudioSource.isPlaying)
        {
            jumpingAudioSource.PlayOneShot(jumping);
        }
        flagFirstJump = true;

        isJumping = false;
        anim.SetBool("isJumping", isJumping);
    }
}

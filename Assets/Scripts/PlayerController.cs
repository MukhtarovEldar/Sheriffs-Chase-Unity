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

    private const float minXPosition = -11.5f;
    private const float maxXPosition = 6.5f;

    private const float jumpSpeedMultiplier = 0.5f;
    private const float runSpeedMultiplier = 1.5f;
    private const float minAnimSpeed = 1f;
    private const float maxAnimSpeed = 2f;
    private const int leftMovement = -1;
    private const int rightMovement = 1;

    private const string horizontalInputAxis = "Horizontal";
    private const string runningAnimationState = "running";
    private const string fallingAnimationState = "falling";
    private const string jumpingAnimationState = "jumping";
    private const string scoringSceneName = "ScoringScene";
    private const string barrelObjectName = "barrel(Clone)";
    private const string isFallingString = "isFalling";
    private const string isJumpingString = "isJumping";
    private const string movementSpeed = "Speed";

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
            float horizontalInput = Input.GetAxisRaw(horizontalInputAxis);

            if (transform.position.x >= maxXPosition)
            {
                horizontalInput = Mathf.Min(horizontalInput, 0f);
            }
            else if (transform.position.x <= minXPosition)
            {
                horizontalInput = Mathf.Max(horizontalInput, 0f);
            }

            rb.velocity = new Vector2(horizontalInput * scrollSpeed * 25, isJumping ? rb.velocity.y : 0);

            animSpeed = Mathf.Clamp(5 * scrollSpeed, minAnimSpeed, maxAnimSpeed);
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

            if (anim.GetCurrentAnimatorStateInfo(0).IsName(runningAnimationState))
            {
                if (horizontalInput == leftMovement)
                {
                    if (originalSpeed == 0)
                    {
                        originalSpeed = anim.speed;
                    }
                    anim.speed = originalSpeed * jumpSpeedMultiplier;
                }
                else if (horizontalInput == rightMovement)
                {
                    if (originalSpeed == 0)
                    {
                        originalSpeed = anim.speed;
                    }
                    anim.speed = originalSpeed * runSpeedMultiplier;
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
            anim.SetFloat(movementSpeed, Mathf.Abs(horizontalInput));
            anim.SetBool(isFallingString, isFalling);
            transform.position = new Vector3(transform.position.x, -2.2f, transform.position.z);
        }
        else
        {
            if (flagFall)
            {
                anim.Play(fallingAnimationState);
                mainAudioSource.Stop();
                runningAudioSource.Stop();
                jumpingAudioSource.Stop();
                mainAudioSource.PlayOneShot(falling);
                flagFall = false;
            }
            isFalling = false;
            anim.SetBool(isFallingString, isFalling);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(fallingAnimationState))
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
            SceneManager.LoadScene(scoringSceneName);
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == barrelObjectName && !isFalling)
        {
            isFalling = true;
            anim.SetBool(isFallingString, isFalling);
        }
    }

    private void Jump()
    {
        if (isFalling)
        {
            return;
        }

        isJumping = true;
        anim.SetBool(isJumpingString, isJumping);

        anim.Play(jumpingAnimationState);

        if (flagFirstJump && !jumpingAudioSource.isPlaying)
        {
            jumpingAudioSource.PlayOneShot(jumping);
        }
        flagFirstJump = true;

        isJumping = false;
        anim.SetBool(isJumpingString, isJumping);
    }
}

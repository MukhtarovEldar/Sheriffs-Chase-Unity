using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ObstacleSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject enemyPrefab;
    public PlayerController playerController;
    public AudioSource audioSource;
    public AudioClip barrelHit;
    
    private bool flagFall;
    private bool isPaused;
    private float spawnInterval;

    void Start()
    {
        Invoke("SpawnObstacle", 1);
    }

    void Update()
    {
        flagFall = playerController.flagFall;
        isPaused = playerController.isPaused;

        if (isPaused)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }

        // Remove obstacle copies that leave the -16 from the x axis
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle.transform.position.x < -16)
            {
                if (obstacle.name == "barrel(Clone)")
                {
                    Destroy(obstacle);
                }
                else if (obstacle.name == "barrel")
                {
                    obstacle.transform.position = new Vector3(16, obstacle.transform.position.y, obstacle.transform.position.z);
                }
            }
        }
    }

    void SpawnObstacle()
    {
        if (!flagFall)
        {
            return;
        }

        GameObject newObstacle = Instantiate(enemyPrefab, spawnPoint.position, transform.rotation);
        StartCoroutine(MoveObstacle(newObstacle));
        spawnInterval = Random.Range(1.5f, 5f);
        Invoke("SpawnObstacle", spawnInterval);
    }

    IEnumerator MoveObstacle(GameObject obstacle)
    {
        float targetY = -3.45f;
        float duration = 0.5f;
        float elapsedTime = 0;

        Vector3 startPosition = obstacle.transform.position;

        while (obstacle.transform.position.y > targetY)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(startPosition.y, targetY, t);
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, newY, obstacle.transform.position.z);
            yield return null;
        }
        audioSource.PlayOneShot(barrelHit);
    }
}

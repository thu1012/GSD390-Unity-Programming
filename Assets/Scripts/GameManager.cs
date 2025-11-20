using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float fallYThreshold = -5f;

    [Header("Collectibles")]
    [SerializeField] private int totalCollectibles = 0;

    private int collectedCount = 0;
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (totalCollectibles == 0)
        {
            totalCollectibles = FindObjectsOfType<Collectible>().Length;
        }

        if (statusText != null)
        {
            statusText.text = "Collect all the spheres!";
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            // Restart the game
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        // Lose condition
        if (playerTransform != null && playerTransform.position.y < fallYThreshold)
        {
            LoseGame("You fell off the level!");
        }
    }

    public void RegisterCollectiblePickup()
    {
        if (gameOver) return;

        collectedCount++;

        // Win condition
        if (collectedCount >= totalCollectibles)
        {
            WinGame("You collected all the spheres!");
        }
        else
        {
            if (statusText != null)
            {
                statusText.text = $"Collected {collectedCount}/{totalCollectibles}";
            }
        }
    }

    private void WinGame(string message)
    {
        gameOver = true;

        if (statusText != null)
        {
            statusText.text = message + "\nPress R to restart.";
        }
    }

    private void LoseGame(string message)
    {
        gameOver = true;

        if (statusText != null)
        {
            statusText.text = message + "\nPress R to restart.";
        }
    }

    private void RestartGame()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

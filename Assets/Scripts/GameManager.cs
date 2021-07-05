using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager: MonoSingletonGeneric<GameManager>
{
    [SerializeField]
    private Transform spawnPos;
    [SerializeField]
    private TextMeshProUGUI coinInput;
    [SerializeField]
    private TextMeshProUGUI scoreInput;
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private Button buttonPause;
    [SerializeField]
    private Button buttonRestart;
    [SerializeField]
    private Button buttonLobby;
    [SerializeField]
    private GameObject inputPaused;
    [SerializeField]
    private List<GroundController> pooledItems = new List<GroundController>();
    private ServicePool servicePool;
    private int count = 0;
    public float playerDistance;
    private int coins;
    private bool isGamePaused;

    void Start()
    {
        servicePool = gameObject.GetComponent<ServicePool>();
        GetItemFromPool();
        buttonPause.onClick.AddListener(PauseGame);
        buttonRestart.onClick.AddListener(RestartGame);
        buttonLobby.onClick.AddListener(GoToLobby);
    }

    private void Update()
    {
        UpdateDistanceCovered();
    }

    public void UpdateDistanceCovered()
    {
        scoreInput.text = playerDistance.ToString();
    }

    public void IncrementScore()
    {
        coins++;
        coinInput.text = coins.ToString();
    }

    internal void GetItemFromPool()
    {
        GameObject ground = servicePool.GetObject();
        ground.transform.position = spawnPos.position;
        GroundController controller = ground.gameObject.GetComponent<GroundController>();
        spawnPos = controller.spawnPosition;
        controller.ActivateRandomObstacle();
        controller.SpawnCoins();
        controller.Enable();
        if (pooledItems.Count < 3)
        {
            pooledItems.Add(controller);
        }
        if (pooledItems.Count >= 3)
        {
            pooledItems[count].DeactivateAllObstacles();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        servicePool.ReturnItem(pooledItems[count].gameObject);
        pooledItems[count].Disable();
        if (count >= 2)
        {
            count = 0;
            return;
        }
        count++;
    }

   public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        inputPaused.SetActive(isGamePaused);
        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToLobby()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        panelGameOver.SetActive(true);
    }
}

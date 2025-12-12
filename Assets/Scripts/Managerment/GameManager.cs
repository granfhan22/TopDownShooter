using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsGameActive;
    public float GameTimer;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    public void GameOver()
    {
        AudioController.Instance.PlaySound(AudioController.Instance.GameOver);
        IsGameActive = false;
        StartCoroutine(OpenVictoryPanel());
    }
    public void Victory()
    {
        AudioController.Instance.PlaySound(AudioController.Instance.Victory);
        IsGameActive = false;
        StartCoroutine(OpenVictoryPanel());
    }
    private void Update()
    {
        GameTimer += Time.deltaTime;
        UIManager.Instance.UpdateTime(GameTimer);
    }
    public void Pause()
    {
        if(UIManager.Instance.PausePanel.activeSelf == false && UIManager.Instance.GameOverPanel.activeSelf == false)
        {
            UIManager.Instance.PausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioController.Instance.PlaySound(AudioController.Instance.Pause);
        }
        else
        {
            UIManager.Instance.PausePanel.SetActive(false);
            Time.timeScale = 1f;
            AudioController.Instance.PlaySound(AudioController.Instance.Pause);
        }
    }
    public void SkillTree()
    {
        if (UIManager.Instance.SkillTreePanel.activeSelf == false)
        {
            UIManager.Instance.SkillTreePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            UIManager.Instance.SkillTreePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    IEnumerator OpenGameOverPanel()
    {
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Home");
    }
    
    IEnumerator OpenVictoryPanel()
    {
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.VictoryPanel.SetActive(true);
        Time.timeScale = 0;
    }
}

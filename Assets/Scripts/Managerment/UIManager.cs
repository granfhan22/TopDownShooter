using TMPro;
using Topdown.movement;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Slider HealthBar;
    public Slider ExpBar;
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;
    public GameObject PausePanel;
    public GameObject SkillTreePanel;
    public TextMeshProUGUI Lvtext;
    [SerializeField] TextMeshProUGUI TimerText;
    private void Awake()
    {
        
        if (Instance != null && Instance!= this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this; 
        }
    }
    public void UpdateHealthBar()
    {      
        HealthBar.value = PlayerMovement.Instance.PlayerCurrentHealth;
    }
    public void UpdateExpBar()
    {
        ExpBar.maxValue = PlayerMovement.Instance.PlayerLevels[PlayerMovement.Instance.CurrentLevel - 1];
        ExpBar.value = PlayerMovement.Instance.Exp;
    }
    public void UpdateTime(float time)
    {
        float Min = Mathf.FloorToInt(time / 60f);
        float Sec = Mathf.FloorToInt(time % 60f);
        TimerText.text = Min + ":" + Sec;
    }
    public void UpdateLv(int lv)
    {
        Lvtext.text = "Lvl: " + lv;
    }
}

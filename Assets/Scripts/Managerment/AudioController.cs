using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public AudioSource Pause;
    public AudioSource Unpause;
    public AudioSource EnemyDead;
    public AudioSource NormalAttack;
    public AudioSource FireCircle;
    public AudioSource WaterBullet;
    public AudioSource ToxicLand;
    public AudioSource LvUp;
    public AudioSource GameOver;
    public AudioSource Victory;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    public void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }
    public void PlayModifiedSound(AudioSource sound)
    {
        sound.pitch = Random.Range(0.7f, 1.3f);
        sound.Stop();
        sound.Play();
    }
}

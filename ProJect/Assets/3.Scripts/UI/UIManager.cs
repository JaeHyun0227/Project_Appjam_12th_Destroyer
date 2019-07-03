using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType(typeof(UIManager)) as UIManager;
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }

        return instance;
    }



    public Text ScoreText;
    public Image RazerProgress;
    public Text missileCount;
    public GameObject Menu;

    //결과
    public GameObject ResultWindow;
    public Text ResultScoreText;


    public GameObject[] HealthImage = new GameObject[3];

    // Use this for initialization
    void Start () {

        Score(0);
        RazerBar(0,3);
        Missile(0,3);
	}
	

    public void Score(int Score)
    {
        ScoreText.text = "Score : " + Score.ToString();
    }
    public void RazerBar(float RazerTime,float RazerTimeMax)
    {
        RazerProgress.fillAmount = 1-(RazerTime / RazerTimeMax);
    }
    public void Missile(int missiles,int Maxmissile) {
        missileCount.text = (Maxmissile-missiles).ToString() + "/" + Maxmissile.ToString();
    }
    public void Pause()
    {
        Time.timeScale = 0;
        Menu.SetActive(true);
    }
    public void Back()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Maine()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Health(int health)
    {
        for (int i = 0; i < 3; i++)
        {
            HealthImage[i].SetActive(false);
        }
        for (int i = 0; i < health; i++)
        {
            HealthImage[i].SetActive(true);
        }
    }

    public void Result()
    {
        ResultWindow.SetActive(true);

        int Score = GameManager.GetInstance().Score;
        ResultScoreText.text = "HighScore : " + PlayerPrefs.GetInt("HighScore", Score).ToString();
        ResultScoreText.text += "Score : " + Score.ToString();

        if (PlayerPrefs.GetInt("HighScore", Score) < Score)
            PlayerPrefs.SetInt("HighScore", Score);
    }
}

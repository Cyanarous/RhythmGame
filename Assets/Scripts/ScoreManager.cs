using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSfx;
    public AudioSource missSfx;
    public TextMeshProUGUI scoreText; 
    private int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }

    public static void Hit()
    {
        Instance.comboScore += 1;
        Instance.hitSfx.Play();
    }

    public static void Miss()
    {
        Instance.comboScore = 0; 
        Instance.missSfx.Play();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}

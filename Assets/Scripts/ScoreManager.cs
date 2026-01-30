using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int coinCount = 0;
    public TextMeshProUGUI coinText;
    [SerializeField] private AudioSource getCoinSound;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinUI();
    }

    // Update is called once per frame

    void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount;
        }
    }

    public void AddPoint(int amount)
    {
        coinCount += amount;
        getCoinSound.Play();
        UpdateCoinUI();

    }
}

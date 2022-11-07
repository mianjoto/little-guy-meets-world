using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public Canvas Canvas;
    private GameObject _heartGroup;
    private GameObject _giftGroup;
    [SerializeField]
    private TMP_Text _canvasTimeText;
    public Action OnTimeRanOut;

    void Awake()
    {
        // Naive approach, will not repeat this when given more time
        _heartGroup = GameObject.Find("HeartGroup");
        _giftGroup = GameObject.Find("GiftGroup");
        _canvasTimeText = GameObject.Find("TimeLeft").GetComponent<TMP_Text>();
    }

    void Start()
    {
        GameManager.OnPlayerTakeDamage += LoseOneLife;
        StartCoroutine(CountdownGameTimer());
    }

    void LoseOneLife()
    {
        List<GameObject> listOfHearts = new List<GameObject>();
        foreach (Transform heart in _heartGroup.transform)
        {
            listOfHearts.Add(heart.gameObject);
        }
        GameObject lastHeart = listOfHearts.Last();
        Destroy(lastHeart);
        listOfHearts.Remove(lastHeart);
        if (listOfHearts.Count == 0)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().OnPlayerDied?.Invoke();
            return;
        }
    }

    void PickUpGift()
    {
        List<GameObject> listOfGifts = new List<GameObject>();
        foreach (Transform gift in _giftGroup.transform)
        {
            listOfGifts.Add(gift.gameObject);
        }
        GameObject lastGift = listOfGifts.Last();
        Destroy(lastGift);
        listOfGifts.Remove(lastGift);
        if (listOfGifts.Count == 0)
        {
            return;
        }
    }

    IEnumerator CountdownGameTimer()
    {
        _canvasTimeText.text = GameManager.TotalGameTime.ToString();
        while (GameManager.GameTimeRemaining > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            GameManager.GameTimeRemaining -= 1;
            _canvasTimeText.text = GameManager.GameTimeRemaining.ToString();
        }
        OnTimeRanOut?.Invoke();
    }

    public void DisplayGameOver()
    {
        GameObject gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(true);
        Color initialColor = gameOverScreen.GetComponent<SpriteRenderer>().color;
        Color fullyFadedIn = new Color(initialColor.r, initialColor.g, initialColor.b, 1);
        gameOverScreen.GetComponent<SpriteRenderer>().DOColor(fullyFadedIn, 3f);
    }
}

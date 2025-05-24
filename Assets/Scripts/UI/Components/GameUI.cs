using System.Collections;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextUI countdownUI;

    private void Awake()
    {
        countdownUI = GameObject.Find("Countdown").GetComponent<TextUI>();
    }


    public void BeginCountdown(int n)
    {
        countdownUI.gameObject.SetActive(true);
        StartCoroutine(CountDown(n));
    }

    IEnumerator CountDown(int n)
    {
        for (int i = n; i > 0; i--)
        {
            countdownUI.SetText(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        countdownUI.gameObject.SetActive(false);
        GameManager.Instance.StartRace();
        yield break;
    }
}
using System;
using UnityEngine;
using System.Collections;
using Manager;

public class Timer : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _timerText;
    private bool _isCountingUp;
    private int _startingTime;
    private Action _timerRunOutCallback;

    public void Init(bool isCountingUp = true, int startingTime = 0, Action timerRunOutCallback = null)
    {
        _isCountingUp = isCountingUp;
        _startingTime = startingTime;
        _timerText = GetComponent<TMPro.TextMeshProUGUI>();
        _timerRunOutCallback = timerRunOutCallback;
        
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        var gameManager = GameManager.Singleton;
        var timer = _startingTime;
        while (gameManager.isGameRunning)
        {
            // wait until the game is not paused
            yield return new WaitUntil(() => gameManager.isGamePaused == false);

            // Update the timer based on counting direction
            if (_isCountingUp)
            {
                timer += 1;
            }
            else
            {
                timer -= 1;
                if (timer <= 0)
                {
                    timer = 0;
                    _timerRunOutCallback?.Invoke();
                }
            }

            // show the timer in minutes and seconds
            var minutes = timer / 60;
            var seconds = timer % 60;
            _timerText.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1);
        }
    }
    
    public void StopTimer()
    {
        StopAllCoroutines();
        if (_timerText != null)
        {
            _timerText.text = "00:00";
        }
    }
    
    public string GetTime()
    {
        return _timerText.text;
    }
}
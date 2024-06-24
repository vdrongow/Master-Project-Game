using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private int _countdownTime;
    private TextMeshProUGUI _countdownText;
    private Action _callback;
    
    public void Init(int countdownTime = 3, Action callback = null)
    {
        _countdownTime = countdownTime;
        _countdownText = GetComponent<TextMeshProUGUI>();
        _callback = callback;
        
        StartCoroutine(StartCountdown());
    }
    
    private IEnumerator StartCountdown()
    {
        var count = _countdownTime;
        _countdownText.text = "Ready?...";
        yield return new WaitForSeconds(1);
        _countdownText.gameObject.SetActive(true);
        while (count > 0) {
           
            _countdownText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count --;
        }
        _countdownText.text = "GOOO!";
        yield return new WaitForSeconds(1);
        _countdownText.text = "";
        
        _callback?.Invoke();
        DestroyCountdown();
    }
    
    public void DestroyCountdown()
    {
        Destroy(gameObject);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MistakeVisualizer : MonoBehaviour
{
    private float _timeToVisualizeMistake;
    
    public void Init(float timeToVisualizeMistake)
    {
        _timeToVisualizeMistake = timeToVisualizeMistake;
        
        StartCoroutine(MistakeVisualizerCoroutine());
    }
    
    private IEnumerator MistakeVisualizerCoroutine()
    {
        var visualizerImage = GetComponent<Image>();

        var blinkInterval = 0.1f;
        var blinkCount = Mathf.FloorToInt(_timeToVisualizeMistake / (2 * blinkInterval));

        for (var i = 0; i < blinkCount; i++)
        {
            SetImageAlpha(visualizerImage, 0);
            yield return new WaitForSeconds(blinkInterval);

            SetImageAlpha(visualizerImage, 1);
            yield return new WaitForSeconds(blinkInterval);
        }
        SetImageAlpha(visualizerImage, 1);
        DestroyMistakeVisualizer();
            
        void SetImageAlpha(Graphic image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
    
    public void DestroyMistakeVisualizer()
    {
        Destroy(gameObject);
    }
}
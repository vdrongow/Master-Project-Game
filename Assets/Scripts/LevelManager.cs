using Enums;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject arrayParent;
    [SerializeField]
    private MySlider arraySizeSlider;
    
    private ArrayView arrayView;
    
    private void Start()
    {
        var gameManager = GameManager.Singleton;
        arraySizeSlider.SetMinValue(gameManager.arraySettings.minArraySize);
        arraySizeSlider.SetMaxValue(gameManager.arraySettings.maxArraySize);
        var handleValue = (gameManager.arraySettings.minArraySize + gameManager.arraySettings.maxArraySize) / 2;
        arraySizeSlider.SetValue(handleValue);
        // delete all childs of the array parent
        foreach (Transform child in arrayParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void BackToMainMenu()
    {
        var gameManager = GameManager.Singleton;
        gameManager.LoadNextScene();
    }

    public void CreateArray(int sortType)
    {
        var gameManager = GameManager.Singleton;
        var arraySettings = gameManager.arraySettings;
        var arraySize = (int)arraySizeSlider.GetValue();

        arrayView?.DestroyArray();
        arrayView = new ArrayView(arrayParent, arraySize, arraySettings, (ESortType)sortType);
        arrayView.PrintArray();
    }
    
    public void Click()
    {
        var gameManager = GameManager.Singleton;
        var gameState = gameManager.gameState;
        
        gameState.Count++;
    }
}
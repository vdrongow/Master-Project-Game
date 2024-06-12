using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; // To install the package, install NuGetForUnity Unity Package Manager and then install Newtonsoft.Json package from NuGetForUnity

public class Test : MonoBehaviour
{
    public void Click()
    {
        var gameManager = GameManager.Singleton;
        var gameState = gameManager.gameState;
        
        gameState.Count++;
    }
}

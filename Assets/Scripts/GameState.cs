using System.Linq;
using System.Text;
using Enums;
using UnityEngine;

public sealed class GameState : MonoBehaviour
{
        #region Dump

        public string DumpGameState()
        {
                var gameManager = GameManager.Singleton;
                const string empty  = "-- Empty --";

                var sb = new StringBuilder();
                sb.AppendLine("GameState:");
                sb.AppendLine("-----------------------------");
                
                // try find Started Game
                if(gameManager.Game != null)
                {
                        sb.AppendLine();
                        sb.AppendLine("Game:");
                        sb.AppendLine("-----------------------------");
                        sb.AppendLine($"SortingAlgorithm: {gameManager.Game.SortingAlgorithm.AsString()}");
                        sb.AppendLine($"SortType: {gameManager.Game.SortType.AsString()}");
                        sb.AppendLine($"ArraySize: {gameManager.Game.ArraySize}");
                }
                else
                {
                        sb.AppendLine(empty);
                        sb.AppendLine();
                }
                
                // try find ArrayView
                var levelManager = FindObjectOfType<LevelManager>();
                if (levelManager != null)
                {
                        if(levelManager.ArrayView != null)
                        {
                                sb.AppendLine();
                                sb.AppendLine("Array:");
                                sb.AppendLine("-----------------------------");
                                sb.AppendLine();
                                sb.AppendLine($"{levelManager.ArrayView.SortType.AsString()} Array with {levelManager.ArrayView.ArraySize} elements:");
                                sb.AppendLine(levelManager.ArrayView.IsEmpty
                                        ? empty
                                        : string.Join(", \n", levelManager.ArrayView.ArrayElements.Select((arrayEl, index) => $"[{index.ToString()}] {arrayEl}")));
                                sb.AppendLine($"Using {levelManager.SortingAlgorithm.GetSortingAlgorithm().AsString()} algorithm");
                        }
                        else
                        {
                                sb.AppendLine(empty);
                                sb.AppendLine();
                        }
                        if(levelManager.SortingAlgorithm != null)
                        {
                                sb.AppendLine();
                                sb.AppendLine("Sorting Algorithm Steps:");
                                sb.AppendLine("-----------------------------");
                                sb.AppendLine(string.Join(", \n",
                                        levelManager.SortingAlgorithm.Steps.Select((step, index) =>
                                                $"[{index.ToString()}] (index1: {step.index1}, index2: {step.index2}, swap: {step.swap}, end: {step.end})")));
                                sb.AppendLine();
                        }
                        else
                        {
                                sb.AppendLine(empty);
                                sb.AppendLine();
                        }
                }

                return sb.ToString();
        }
        
        #endregion
}
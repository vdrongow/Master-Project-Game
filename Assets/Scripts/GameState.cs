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
                
                // try find Session
                if(gameManager.Session != null)
                {
                        sb.AppendLine();
                        sb.AppendLine("Session:");
                        sb.AppendLine("-----------------------------");
                        sb.AppendLine($"Learner: {gameManager.Session.Learner.learnerId}");
                        sb.AppendLine($"SortingAlgorithm: {gameManager.Session.SortingAlgorithm.AsString()}");
                        sb.AppendLine($"SortType: {gameManager.Session.SortType.AsString()}");
                        sb.AppendLine($"ArraySize: {gameManager.Session.ArraySize}");
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
                }

                return sb.ToString();
        }
        
        #endregion
}
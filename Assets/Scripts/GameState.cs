using System.Linq;
using System.Text;
using Enums;
using UnityEngine;

public sealed class GameState : MonoBehaviour
{
        public int Count;
        
        #region Dump

        public string DumpGameState()
        {
                const string empty  = "-- Empty --";

                var sb = new StringBuilder();
                sb.AppendLine("GameState:");
                sb.AppendLine("-----------------------------");
                sb.AppendLine();
                sb.AppendLine($"Count: {Count.ToString()}");
                
                // try find LevelManager
                var levelManager = FindObjectOfType<LevelManager>();
                if (levelManager != null)
                {
                        sb.AppendLine();
                        sb.AppendLine("LevelManager:");
                        sb.AppendLine("-----------------------------");
                        if(levelManager.ArrayView != null)
                        {
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
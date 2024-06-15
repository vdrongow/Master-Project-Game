using System.Text;
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
                        sb.AppendLine();
                        if(levelManager.arrayView != null)
                        {
                                sb.AppendLine(levelManager.arrayView.AsString());
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
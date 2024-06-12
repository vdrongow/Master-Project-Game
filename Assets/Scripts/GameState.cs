using System.Text;
using UnityEngine;

public sealed class GameState : MonoBehaviour
{
        public int Count = new();
        
        #region Dump

        public string DumpGameState()
        {
                const string empty  = "-- Empty --";

                var sb = new StringBuilder();
                sb.AppendLine("GameState:");
                sb.AppendLine("-----------------------------");
                sb.AppendLine();
                sb.AppendLine($"Count: {Count.ToString()}");

                return sb.ToString();
        }
        
        #endregion
}
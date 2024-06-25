using System.Linq;
using System.Text;
using Manager;
using UnityEngine;

public sealed class GameState : MonoBehaviour
{
    #region Dump

    public string DumpGameState()
    {
        var gameManager = GameManager.Singleton;
        const string empty = "-- Empty --";

        var sb = new StringBuilder();
        sb.AppendLine("GameState:");
        sb.AppendLine("-----------------------------");

        // try find Started Sorting Game
        if (gameManager.SortingGame.IsRunning)
        {
            sb.AppendLine();
            sb.AppendLine("Sorting Game:");
            sb.AppendLine("-----------------------------");
            sb.AppendLine(gameManager.SortingGame.ToString());
            PrintSortingGameData();
        }
        else
        {
            sb.AppendLine(empty);
            sb.AppendLine();
        }

        // try find Started Basic Game
        if (gameManager.BasicGame.IsRunning)
        {
            sb.AppendLine();
            sb.AppendLine("Basic Game:");
            sb.AppendLine("-----------------------------");
            sb.AppendLine(gameManager.BasicGame.ToString());
            PrintBasicGameData();
        }
        else
        {
            sb.AppendLine(empty);
        }

        return sb.ToString();
        
        void PrintSortingGameData()
        {
            // try find ArrayView
            var levelManager = FindObjectOfType<LevelSortingManager>();
            if (levelManager != null)
            {
                if (levelManager.ArrayView != null)
                {
                    sb.AppendLine();
                    sb.AppendLine("Array:");
                    sb.AppendLine("-----------------------------");
                    sb.AppendLine(levelManager.ArrayView.IsEmpty
                        ? empty
                        : string.Join(", \n",
                            levelManager.ArrayView.ArrayElements.Select((arrayEl, index) =>
                                $"[{index.ToString()}] {arrayEl}")));
                }
                else
                {
                    sb.AppendLine(empty);
                    sb.AppendLine();
                }

                if (levelManager.SortingAlgorithm != null)
                {
                    sb.AppendLine();
                    sb.AppendLine("Sorting Algorithm Steps:");
                    sb.AppendLine(levelManager.SortingAlgorithm.GetCurrentStepAsString());
                    sb.AppendLine("-----------------------------");
                    sb.AppendLine(levelManager.SortingAlgorithm.GetStepsAsString());
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(empty);
                    sb.AppendLine();
                }
            }
        }

        void PrintBasicGameData()
        {
            // try find BasicConcepts
            var levelManager = FindObjectOfType<LevelBasicsManager>();
            if (levelManager != null)
            {
                if (levelManager.BasicSkill != null)
                {
                    sb.AppendLine();
                    sb.AppendLine("Basic Skill:");
                    sb.AppendLine("-----------------------------");
                    sb.AppendLine(levelManager.BasicSkill.GetTaskTitle());
                    sb.AppendLine(levelManager.BasicSkill.GetTaskAsString());
                }
                else
                {
                    sb.AppendLine(empty);
                }
            }
        }
    }

    #endregion
}
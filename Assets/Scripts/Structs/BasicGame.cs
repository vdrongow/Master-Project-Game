using Enums;

namespace Structs
{
    public struct BasicGame
    {
        public EBasicSkill BasicSkill { get; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public bool IsRunning { get; set; }
    
        public BasicGame(EBasicSkill basicSkill, int score = 0, int mistakes = 0, bool isRunning = false)
        {
            BasicSkill = basicSkill;
            Score = score;
            Mistakes = mistakes;
            IsRunning = isRunning;
        }
    
        public override string ToString() => $"Basic Concept: {BasicSkill}, Score: {Score}, Mistakes: {Mistakes}";
    }
}
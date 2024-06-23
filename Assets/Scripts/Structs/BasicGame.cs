using Enums;

namespace Structs
{
    public struct BasicGame
    {
        public EBasicConcepts BasicConcept { get; }
        public int Score { get; set; }
        public bool IsRunning { get; set; }
    
        public BasicGame(EBasicConcepts basicConcept, int score = 0, bool isRunning = false)
        {
            BasicConcept = basicConcept;
            Score = score;
            IsRunning = isRunning;
        }
    
        public override string ToString() => $"Basic Concept: {BasicConcept}, Score: {Score}";
    }
}
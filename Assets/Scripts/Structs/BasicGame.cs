using Enums;

namespace Structs
{
    public struct BasicGame
    {
        public EBasicConcepts BasicConcept { get; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public bool IsRunning { get; set; }
    
        public BasicGame(EBasicConcepts basicConcept, int score = 0, int mistakes = 0, bool isRunning = false)
        {
            BasicConcept = basicConcept;
            Score = score;
            Mistakes = mistakes;
            IsRunning = isRunning;
        }
    
        public override string ToString() => $"Basic Concept: {BasicConcept}, Score: {Score}, Mistakes: {Mistakes}";
    }
}
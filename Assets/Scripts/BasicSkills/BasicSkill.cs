using Configs;
using Manager;

namespace BasicSkills
{
    public abstract class BasicSkill
    {
        internal LevelBasicsManager LevelBasicsManager;
        internal GameSettings GameSettings;
        internal ArraySettings ArraySettings;
        
        public void Init(LevelBasicsManager levelBasicsManager)
        {
            LevelBasicsManager = levelBasicsManager;
            var gameManager = GameManager.Singleton;
            GameSettings = gameManager.gameSettings;
            ArraySettings = gameManager.arraySettings;
            InitTask();
        }

        public abstract string GetTaskTitle();
        
        protected abstract void InitTask();
        
        public abstract string GetTaskAsString();

        public abstract void DestroyTask();
    }
}
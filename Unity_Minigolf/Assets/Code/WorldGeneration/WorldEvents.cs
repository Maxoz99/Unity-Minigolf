using UnityEngine;

namespace Code.WorldGeneration
{
    public class WorldEvents : MonoBehaviour
    {
        private void Update()
        {
            StartEvent();
        }


        /// <summary>
        /// Generate a series of base blocks to ease the player into the level
        /// </summary>
        private void StartEvent()
        {
            WorldGenerator.generator.GenerateWorld();
        }

    
        /// <summary>
        /// Transition into base blocks with an ending sequence
        /// </summary>
        public void EndEvent()
        {
        
        }
    
    
        /// <summary>
        /// Reset the player and stage after hitting an obstacle
        /// </summary>
        public void ResetEvent()
        {
        
        }
    }
}

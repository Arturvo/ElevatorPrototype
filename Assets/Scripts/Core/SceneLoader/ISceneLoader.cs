using System;

namespace ZadanieRekrutacyjne.Core.SceneLoader
{
    public interface ISceneLoader
    {
        public int StartingFloorIndex { get; }
        public int CurrentFloorIndex { get; }

        public void LoadFloorAsync(int floorIndex, Action callback = null, float minTimeToLoad = 0);
    }
}

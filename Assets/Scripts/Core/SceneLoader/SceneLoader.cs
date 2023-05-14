using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace ZadanieRekrutacyjne.Core.SceneLoader
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        public int StartingFloorIndex { get; } = 0;
        public int CurrentFloorIndex { get; private set; } = -1;

        private Stopwatch sceneLoadTimer;

        private const string initSceneId = "Init";
        private const string playerSceneId = "Player";
        private const string elevatorSceneId = "Elevator";
        private const string userInterfaceSceneId = "UserInterface";
        private const string floorSceneIdPrefix = "Floor";

        private void Start ()
        {
            sceneLoadTimer = new Stopwatch ();
            StartCoroutine(StartGame());
        }

        public void LoadFloorAsync(int floorIndex, Action callback = null, float minTimeToLoad = 0)
        {
            Assert.IsFalse(CurrentFloorIndex == floorIndex, "Attempted to load current floor");
            StartCoroutine(LoadFloor(floorIndex, callback, minTimeToLoad));
        }

        private IEnumerator LoadFloor(int floorIndex, Action callback = null, float minTimeToLoad = 0)
        {
            sceneLoadTimer.Start();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(floorSceneIdPrefix + floorIndex, LoadSceneMode.Additive);
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(floorSceneIdPrefix + CurrentFloorIndex);

            while(!asyncLoad.isDone || !asyncUnload.isDone)
            {
                yield return null;
            }

            float timePassed = (float) sceneLoadTimer.Elapsed.TotalSeconds;
            sceneLoadTimer.Reset();
            if (timePassed < minTimeToLoad)
            {
                yield return new WaitForSeconds(minTimeToLoad - timePassed);
            }

            CurrentFloorIndex = floorIndex;
            callback?.Invoke();
        }

        private IEnumerator StartGame()
        {
            CurrentFloorIndex = StartingFloorIndex;
            AsyncOperation floorLoad = SceneManager.LoadSceneAsync(floorSceneIdPrefix + StartingFloorIndex, LoadSceneMode.Additive);
            AsyncOperation elevatorLoad = SceneManager.LoadSceneAsync(elevatorSceneId, LoadSceneMode.Additive);
            AsyncOperation userInterfaceLoad = SceneManager.LoadSceneAsync(userInterfaceSceneId, LoadSceneMode.Additive);

            while (!floorLoad.isDone || !elevatorLoad.isDone || !userInterfaceLoad.isDone)
            {
                yield return null;
            }

            AsyncOperation playerLoad = SceneManager.LoadSceneAsync(playerSceneId, LoadSceneMode.Additive);

            while (!playerLoad.isDone)
            {
                yield return null;
            }

            SceneManager.UnloadSceneAsync(initSceneId);
        }
    }
}

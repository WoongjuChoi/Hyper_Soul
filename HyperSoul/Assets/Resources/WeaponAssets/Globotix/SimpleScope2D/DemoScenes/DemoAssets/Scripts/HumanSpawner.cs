using UnityEngine;
using System.Collections;

    public class HumanSpawner : MonoBehaviour
    {

        public GameObject prefabToSpawn;
        public float initialDelay = 0;
        public float respawnInterval = 5;
        public int totalToSpawn = 0; // 0 is infinite
        private int _spawnedCounter = 0;

        void Start()
        {
            Invoke("spawn", initialDelay);
        }

        public void spawn()
        {

            GameObject go = Instantiate(prefabToSpawn, transform.position, Quaternion.identity) as GameObject;
            if (go != null)
            {
                _spawnedCounter++;
            }
            if (totalToSpawn == 0 || _spawnedCounter < totalToSpawn)
            {
                Invoke("spawn", respawnInterval);
            }
        }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject fallingBlockPrefab;
    public Camera camera;

    public Vector2 secondsBetweenSpawnsMinMax;
    float nextSpawnTime;

    Vector2 screenHalfSizeWorldUnits;

    public Vector2 spawnSizeMinMax;
    public float spawnAngleMax;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfSizeWorldUnits = new Vector2(camera.aspect * camera.orthographicSize, camera.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime) {
            //value = a+(b-a)p->LERP(Linear Interpolation);
            float secondsBetweenSpawns = Mathf.Lerp(secondsBetweenSpawnsMinMax.y, secondsBetweenSpawnsMinMax.x, Difficulty.GetDifficultyPercent());
            nextSpawnTime = Time.time + secondsBetweenSpawns;

            float spawnAngle = Random.Range(-spawnAngleMax, spawnAngleMax);
            float spawnSize = Random.Range(spawnSizeMinMax.x,spawnSizeMinMax.y);
            Vector3 spawnPosition = new Vector2(Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x) + camera.transform.position.x, -16 + spawnSize);
            GameObject newFallingBlock = (GameObject) Instantiate(fallingBlockPrefab,spawnPosition, Quaternion.Euler(Vector3.forward * spawnAngle));
            newFallingBlock.transform.localScale = Vector2.one * spawnSize;
        }
    }
}

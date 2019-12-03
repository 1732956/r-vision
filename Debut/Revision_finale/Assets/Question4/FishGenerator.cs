using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishGenerator : MonoBehaviour {

    public int mapWidth = 256;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate;
    public int seed;   
    public Vector2 offset;
    public float timeforlastmoovement =5;

    public int maxNumberOfFishes = 100;
    public GameObject fishPrefab;
    float[,] noiseMapMovement;
    private int countSeed = 1;
    private void Start() {
        noiseMapMovement = Noise.GenerateNoiseMap(mapWidth, mapWidth, seed + 2, noiseScale, octaves, persistance, lacunarity, offset);

    }

    private void Update()
    {
        MooveFishe();
        if (timeforlastmoovement>0)
        {
            timeforlastmoovement -= Time.deltaTime;
        }
        else
        {
            noiseMapMovement = Noise.GenerateNoiseMap(mapWidth, mapWidth, seed + countSeed, noiseScale, octaves, persistance, lacunarity, offset);
            countSeed++;
            timeforlastmoovement = 5f;
        }
    }

    private void MooveFishe()
    {
    
      
        foreach (Fish currentfish in FindObjectsOfType<Fish>())
        {
            float x = currentfish.transform.position.x % mapWidth;
            float y = currentfish.transform.position.y % mapWidth;
            float value = noiseMapMovement[(int)x, (int)y];

            if (value<=0.2)
            {
                currentfish.transform.Translate(new Vector3(value, value, 0));
            }
            else if (value<=0.4)
            {
                currentfish.transform.Translate(new Vector3(0, value, 0));
            }
            else if (value <= 0.7)
            {
                currentfish.transform.Translate(new Vector3( value, value, 0));
            }
            else
            {
                currentfish.transform.Translate(new Vector3( value, 0, 0));
            }
        }

    }

    public void GenerateFishes() {
        DeleteFishes();
        GenerateFishesWithNoise();
        
    }

    public void DeleteFishes() {
        foreach (Fish fish in FindObjectsOfType<Fish>()) {
            DestroyImmediate(fish.gameObject);
        }
    }
    
  
    private void OnValidate() {
        if(mapWidth< 1) {
            mapWidth = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if(octaves < 0) {
            octaves = 0;
        }

    }

    public void GenerateFishesWithNoise()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapWidth, seed, noiseScale, octaves, persistance, lacunarity, offset);
        for (int i = 0; i < mapWidth; i++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                float value = noiseMap[i, y];
                if (value > 0.80)
                {                   
                    GameObject fish = Instantiate(fishPrefab, new Vector3(i, y, 0), new Quaternion(0, 0, 0, 0));
                }
            }
        }

    }
}


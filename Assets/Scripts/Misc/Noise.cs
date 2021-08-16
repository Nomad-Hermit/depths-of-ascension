using System;
using UnityEngine;


public class Noise
    { 
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset){ 
        float[,] array = new float[mapWidth, mapHeight];
        System.Random random = new System.Random(seed);
        Vector2[] array2 = new Vector2[octaves];

        for (int i = 0; i < octaves; i++){ 
            float x = (float)random.Next(-100000, 100000) + offset.x;
            float y = (float)random.Next(-100000, 100000) + offset.y;
            array2[i] = new Vector2(x, y);
        }

        if (scale <= 0f){ 
            scale = 0.0001f;
        }

        float num = -3.40282347E+38f;
        float num2 = 3.40282347E+38f;

        for (int j = 0; j < mapHeight; j++) { 
            for (int k = 0; k < mapWidth; k++) { 
                float num3 = 1f;
                float num4 = 1f;
                float num5 = 0f;

                for (int l = 0; l < octaves; l++) { 
                    float x2 = (float)k / scale * num4 + array2[l].x;
                    float y2 = (float)j / scale * num4 + array2[l].y;
                    float num6 = Mathf.PerlinNoise(x2, y2) * 2f - 1f;
                    num5 += num6 * num3;
                    num3 *= persistence;
                    num4 *= lacunarity;
                }

                if (num5 > num) { 
                    num = num5;
                } else if (num5 < num2) { 
                    num2 = num5;
                }

                array[k, j] = num5;
            }
        }
        for (int m = 0; m < mapHeight; m++) { 
            for (int n = 0; n < mapWidth; n++) { 
                array[n, m] = Mathf.InverseLerp(num2, num, array[n, m]);
            }
        }
        return array;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField] private GameObject cubeCrackEffectPrefab;
    [SerializeField] private GameObject ballonCrackEffectPrefab;
    [SerializeField] private GameObject rocketTrailEffectPrefab;

    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color purpleColor;

    public static EffectsController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnCubeCrackEffect(Vector3 spawnPos, CubeTypes cubeType)
    {
        GameObject spawnedEffect = Instantiate(cubeCrackEffectPrefab, spawnPos, Quaternion.identity);
        spawnedEffect.GetComponent<ParticleSystem>().startColor = DetectCubeColor(cubeType);
        spawnedEffect.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = DetectCubeColor(cubeType);
        spawnedEffect.GetComponent<ParticleSystem>().Play();
        Destroy(spawnedEffect, 3f);
    }
    public void SpawnBalloonCrackEffect(Vector3 spawnPos)
    {
        GameObject spawnedEffect = Instantiate(ballonCrackEffectPrefab, spawnPos, Quaternion.identity);
        spawnedEffect.GetComponent<ParticleSystem>().Play();
        Destroy(spawnedEffect, 3f);
    }
    public GameObject GetRocketEffect()
    {
        return Instantiate(rocketTrailEffectPrefab);
    }
    private Color DetectCubeColor(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Red:
                {
                    return redColor;
                }
            case CubeTypes.Blue:
                {
                    return blueColor;
                }
            case CubeTypes.Yellow:
                {
                    return yellowColor;
                }
            case CubeTypes.Green:
                {
                    return greenColor;
                }
            case CubeTypes.Purple:
                {
                    return purpleColor;
                }
            default: return redColor;
        }
    }
}

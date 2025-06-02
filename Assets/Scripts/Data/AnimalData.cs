using UnityEngine;
public class AnimalData
{
    public int id;
    public string name;
    public float weight;
    public float height;
    public float width;
    public string color;
    public AnimalData() { }

    public AnimalData(string name, Vector2 heightRange, Vector2 widthRange, float densityFactor, ColorData[] possibleColors, float noisePercentage, float nullPercentage)
    {
        this.name = name;
        if (Random.value < nullPercentage)
            height = float.NaN;
        else
            height = Random.Range(heightRange.x, heightRange.y);
        if (Random.value < nullPercentage)
            width = float.NaN;
        else
            width = Random.Range(widthRange.x, widthRange.y);
        if (Random.value < nullPercentage)
            weight = float.NaN;
        else
        {
            float weightBase = height * width * densityFactor;
            float noise = weightBase * Random.Range(-noisePercentage, noisePercentage);
            weight = weightBase + noise;
        }
        color = possibleColors[Random.Range(0, possibleColors.Length)].colorName;
    }
}
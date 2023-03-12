using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkyTimeData
{
    public int TimeClock; //几点钟
    public Gradient SkyColorGradient;
    public float SunIntensity;
    public float ScatteringIntensity;
    public float StarIntensity;
    public float MilkywayIntensity;

    [HideInInspector]
    public Texture2D SkyColorGradientTex;
}

/*
按照3个小时的间隔记录各个时段的数据值
*/
[CreateAssetMenu(menuName = "PUtils/Skybox/SkyTimeDataCollection")]
public class SkyTimeDataCollection : ScriptableObject
{
    public List<SkyTimeData> SkyTimeDatas;

    public SkyTimeData GetSkyTimeData(float time)
    {
        SkyTimeData curTimeData = new SkyTimeData();
        SkyTimeData startTime = SkyTimeDatas[0];
        SkyTimeData endTime = null;
        for (var i = 1; i < SkyTimeDatas.Count; i++)
        {
            endTime = SkyTimeDatas[i];
            if (time >= startTime.TimeClock && time < endTime.TimeClock)
                break;
            startTime = endTime;
        }

        float lerpValue = (time % 3 / 3f);
        curTimeData.SkyColorGradientTex = GenerateSkyGradientColorTex(startTime.SkyColorGradient, endTime.SkyColorGradient, 128, lerpValue);

        curTimeData.StarIntensity = Mathf.Lerp(startTime.StarIntensity, endTime.StarIntensity, lerpValue);
        curTimeData.MilkywayIntensity = Mathf.Lerp(startTime.MilkywayIntensity, endTime.MilkywayIntensity, lerpValue);
        curTimeData.SunIntensity = Mathf.Lerp(startTime.SunIntensity, endTime.SunIntensity, lerpValue);
        curTimeData.ScatteringIntensity = Mathf.Lerp(startTime.ScatteringIntensity, endTime.ScatteringIntensity, lerpValue);

        return curTimeData;
    }

    public Texture2D GenerateSkyGradientColorTex(Gradient startGradient, Gradient endGradient, int resolution, float lerpValue)
    {
        Texture2D tex = new Texture2D(resolution, 1, TextureFormat.RGBAFloat, false, true);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;


        for (int i = 0; i < resolution; i++)
        {
            Color start = startGradient.Evaluate(i * 1.0f / resolution).linear;
            Color end = endGradient.Evaluate(i * 1.0f / resolution).linear;

            Color fin = Color.Lerp(start, end, lerpValue);

            tex.SetPixel(i, 0, fin);
        }
        tex.Apply(false, false);

        return tex;
    }


}

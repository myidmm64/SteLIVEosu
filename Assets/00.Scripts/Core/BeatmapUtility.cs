using OsuParsers.Beatmaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeatmapExtensions
{
    public static float GetCircleRadius(this Beatmap beatmap)
    {
        return 1f - 0.082f * beatmap.DifficultySection.CircleSize;
    }

    public static double GetAnimationPreemptDuration(this Beatmap beatmap)
    {
        float ar = beatmap.DifficultySection.ApproachRate;
        if (ar < 5)
            return 1200 + 600 * (5 - ar) / 5;
        else if (ar == 5)
            return 1200;
        else if (ar > 5)
            return 1200 - 750 * (ar - 5) / 5;
        return 0;
    }

    public static double GetHitObjectFadeInDuration(this Beatmap beatmap)
    {
        float ar = beatmap.DifficultySection.ApproachRate;
        if (ar < 5)
            return 800 + 400 * (5 - ar) / 5;
        else if (ar == 5)
            return 800;
        else if (ar > 5)
            return 800 - 500 * (ar - 5) / 5;
        return 0;
    }
}

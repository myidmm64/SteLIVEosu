using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeatmapUtility
{
    public static float GetCircleRadius(Beatmap beatmap)
    {
        return 1f - 0.082f * beatmap.osuFile.difficulty.circleSize;
    }

    public static double GetApproachAnimationDuration(Beatmap beatmap)
    {
        float ar = beatmap.osuFile.difficulty.approachRate;
        if (ar < 5)
            return 1200 + 600 * (5 - ar) / 5;
        else if (ar == 5)
            return 1200;
        else if (ar > 5)
            return 1200 - 750 * (ar - 5) / 5;
        return 0;
    }

    public static double GetHitObjectFadeInDuration(Beatmap beatmap)
    {
        float ar = beatmap.osuFile.difficulty.approachRate;
        if (ar < 5)
            return 800 + 400 * (5 - ar) / 5;
        else if (ar == 5)
            return 800;
        else if (ar > 5)
            return 800 - 500 * (ar - 5) / 5;
        return 0;
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AudioDB")]
public class AudioDB : ScriptableObject
{
    public List<SAudioData> audioDatas = new List<SAudioData>();
}

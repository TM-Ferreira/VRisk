using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/ClockCharacterSegmentsMap")]
public class ClockCharacterSegmentsMap : ScriptableObject
{
    public SerializedDictionary<ClockCharacter.Character, List<ClockCharacter.SegmentType>> characters;
}

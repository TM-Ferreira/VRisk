using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/ParticleEffectMap")]
public class ParticleEffectMap : ScriptableObject
{
    public List<ParticleEffectMapElement> elements;
}

[System.Serializable]
public struct ParticleEffectMapElement
{
    public ParticleManager.ParticleID id;
    public GameObject prefab;
    public int count;
}

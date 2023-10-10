using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleManager : MonoBehaviour
{
    public enum ParticleID
    {
        DEBRIS_IMPACT,
        BUILDING_DAMAGE
    }

    public struct ParticleEmitter
    {
        public GameObject parent;
        public List<ParticleSystem> particle_systems;
        
        public ParticleEmitter(GameObject _parent, List<ParticleSystem> _systems)
        {
            parent = _parent;
            particle_systems = _systems;
        }
    }
    
    public Dictionary<ParticleID, List<ParticleEmitter>> particle_emitters;
    public ParticleEffectMap effect_map;
    public float distance_from_player_cutoff = 20;

    public MeshRenderer test;

    private void Awake()
    {
        particle_emitters = new Dictionary<ParticleID, List<ParticleEmitter>>();
        
        foreach (var effect_set in effect_map.elements)
        {
            for (int i = 0; i < effect_set.count; i++)
            {
                var particle_object = Instantiate(effect_set.prefab, transform, true);
                List<ParticleSystem> particle_systems =  new List<ParticleSystem>(particle_object.GetComponentsInChildren<ParticleSystem>());

                if (!particle_emitters.ContainsKey(effect_set.id))
                {
                    particle_emitters.Add(effect_set.id, new List<ParticleEmitter>());
                }
                
                particle_emitters[effect_set.id].Add(new ParticleEmitter(particle_object, particle_systems));
            }
        }
    }
    
    public GameObject triggerBuildingCollapseEffect(ParticleID _id, MeshRenderer _mesh_renderer)
    {
        
        if (Vector3.Distance(GameManager.Instance.Player.transform.position, _mesh_renderer.transform.position) > distance_from_player_cutoff) return null;
            
        if (!particle_emitters.ContainsKey(_id))
        {
            Debug.Log("ParticleEmitters dictionary does not contain key " + _id);
            return null;
        }

        foreach (var emitter in particle_emitters[_id])
        {
            if (!emitter.parent.activeSelf)
            {
                emitter.parent.SetActive(true);

                foreach (var system in emitter.particle_systems)
                {
                    var shape = system.shape;
                    shape.shapeType = ParticleSystemShapeType.MeshRenderer;

                    shape.meshRenderer = _mesh_renderer;
                }

                StartCoroutine(delayedDeactivation(emitter));

                return emitter.parent;
            }
        }
        
        Debug.Log("No available " + _id + " particle in the pool - consider increasing the pool");
        return null;
    }

    public GameObject triggerEffect(ParticleID _id, Vector3 _location, Vector3 _rotation, Transform _parent = null, bool _relative_to_parent = false)
    {
        Vector3 world_location = _relative_to_parent ? _parent.position + _location : _location;
        if (Vector3.Distance(GameManager.Instance.Player.transform.position, world_location) > distance_from_player_cutoff) return null;
            
        if (!particle_emitters.ContainsKey(_id))
        {
            Debug.Log("ParticleEmitters dictionary does not contain key " + _id);
            return null;
        }

        foreach (var emitter in particle_emitters[_id])
        {
            if (!emitter.parent.activeSelf)
            {
                emitter.parent.SetActive(true);
                
                if (_parent != null)
                {
                    emitter.parent.transform.SetParent(_parent);
                }

                if (_relative_to_parent)
                {
                    emitter.parent.transform.localPosition = _location;
                    emitter.parent.transform.localRotation = Quaternion.Euler(_rotation);
                }
                else
                {
                    emitter.parent.transform.position = _location;
                    emitter.parent.transform.rotation = Quaternion.Euler(_rotation);
                }

                StartCoroutine(delayedDeactivation(emitter));

                return emitter.parent;
            }
        }
        
        Debug.Log("No available " + _id + " particle in the pool - consider increasing the pool");
        return null;
    }

    IEnumerator delayedDeactivation(ParticleEmitter _emitter)
    {
        yield return new WaitUntil(() => hasEffectStopped(_emitter));
        _emitter.parent.SetActive(false);
    }

    private bool hasEffectStopped(ParticleEmitter _emitter)
    {
        foreach (var system in _emitter.particle_systems)
        {
            if (system.isPlaying)
            {
                return false;
            }
        }
        return true;
    }
}

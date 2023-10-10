using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fade_on_start = true;
    public bool clear_on_start = false;
    
    public float fade_duration = 2;
    public float unfade_delay = 0.15f;
    public Color fade_color;
    
    private Renderer rend;
    private MeshRenderer mesh_rend;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        mesh_rend = GetComponent<MeshRenderer>();
        
        if(clear_on_start) 
        {
            Color new_color = fade_color;
            new_color.a = 0;
            rend.material.SetColor("_Color", new_color);
            mesh_rend.enabled = false;
        }
        
        if(fade_on_start) FadeIn();
    }

    public void FadeIn()
    {
        Fade(1,0);
        StartCoroutine(DelayedRenderState(false));
    }
    
    public void FadeOut(bool renderAfter = false)
    {
        Fade(0,1);
        StartCoroutine(DelayedRenderState(renderAfter));
    }

    public void Fade(float _alpha_in, float _alpha_out)
    {
        StartCoroutine(FadeRoutine(_alpha_in, _alpha_out));
    }

    // --------------------------------------------------------------------------    
    
    public IEnumerator FadeRoutine(float _alpha_in, float _alpha_out)
    {
        mesh_rend.enabled = true;
        float timer = 0;

        while (timer <= fade_duration)
        {
            Color new_color = fade_color;
            new_color.a = Mathf.Lerp(_alpha_in, _alpha_out, timer / fade_duration);
            
            rend.material.SetColor("_Color", new_color);
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        Color new_color2 = fade_color;
        new_color2.a = _alpha_out;
        rend.material.SetColor("_Color", new_color2);
    }

    public IEnumerator DelayedRenderState(bool _state)
    {
        yield return new WaitForSeconds(fade_duration + unfade_delay);
        mesh_rend.enabled = _state;
    }
}

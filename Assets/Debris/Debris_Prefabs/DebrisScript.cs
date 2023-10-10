using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DebrisScript : MonoBehaviour
{
    private float elapsed_time = 0;
    private Vector3 original_scale;
    public bool falling = true;

    private void Awake()
    {
        original_scale = gameObject.transform.localScale;
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        
        if (falling && contact.otherCollider.CompareTag("Floor"))
        {
            Quaternion rotation = Quaternion.LookRotation(other.contacts[0].normal);

            GameManager.Instance.ParticleManager.triggerEffect(ParticleManager.ParticleID.DEBRIS_IMPACT,
                contact.point, rotation.eulerAngles);

            GameManager.Instance.AudioManager.PlaySound(false, false, contact.point,
                AudioManager.SoundID.DEBRIS_COLLISION);

            falling = false;
        }
    }

    private void FixedUpdate()
    {
        elapsed_time += Time.fixedDeltaTime;

        float progress = elapsed_time / GameManager.Instance.DebrisHandler.debris_lifetime;

        gameObject.transform.localScale = Vector3.Lerp(original_scale, Vector3.zero, progress);
        
        if (GameManager.Instance.DebrisHandler.debris_lifetime < elapsed_time)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        elapsed_time = 0;
        falling = true;
    }
}

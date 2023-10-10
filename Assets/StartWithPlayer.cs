using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWithPlayer : MonoBehaviour
{
    public GameObject playerRig;
    [SerializeField] private float yOffset = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForHeadset());
    }

    private IEnumerator WaitForHeadset()
    {
        yield return new WaitForSeconds(1f);

        var playerPos  = playerRig.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + yOffset, playerPos.z);
    }
}

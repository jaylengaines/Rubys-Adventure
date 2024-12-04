using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player, destination;
    public GameObject playerg;
    // Start is called before the first frame update
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("RubyController"))
        {
            playerg.SetActive(false);
            player.position = destination.position;
            playerg.SetActive(true);
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

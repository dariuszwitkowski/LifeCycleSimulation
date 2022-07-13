using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : MonoBehaviour
{
    Movement _movement;
    int foodEaten = 0;
    void Start()
    {
        _movement = GetComponent<Movement>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == SharedScripts.GlobalTags.FOOD_TAG) {
            _movement._Target = null;
            foodEaten++;
            Destroy(other.gameObject);
        }
    }
}

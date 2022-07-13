using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const int NEGATIVE_ROTATION = 1;
    const int POSITIVE_ROTATION = -1;

    [SerializeField] float Speed = 10;
    Rigidbody2D _npcBody {get; set;}
    float _rotation = 0;
    int _rotationType = POSITIVE_ROTATION;
    public GameObject _Target = null;
    
    
    void Start()
    {
        _npcBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Go();
    }

    void Go() {
        // 
        if(!_Target){
            _npcBody.velocity = transform.up * Speed;
            transform.rotation = Quaternion.Euler(0, 0, _rotation);
        }
        else {
            Vector3 dir = _Target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 20f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }
    }
    void RotateOnWall() {
        _rotation+= _rotationType * Random.Range(5, 30);
    }
    void RotateOnTarget() {
        
        // var direction = _Target.transform.position - transform.position;
        // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // var q = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.FromToRotation(transform.position, _Target.transform.position);
    }
    private void OnCollisionEnter2D(Collision2D other) {
         if(other.gameObject.tag == SharedScripts.GlobalTags.WALL_TAG) {
            var tmpRandom = Random.Range(0,2);
            _rotationType = tmpRandom == 0 ? POSITIVE_ROTATION : NEGATIVE_ROTATION; 
        }
    }
    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == SharedScripts.GlobalTags.WALL_TAG) {
            RotateOnWall();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == SharedScripts.GlobalTags.FOOD_TAG) {
            _Target = other.gameObject;
            RotateOnTarget();
        }
    }
}

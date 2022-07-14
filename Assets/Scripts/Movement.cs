using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  const int NEGATIVE_ROTATION = 1;
  const int POSITIVE_ROTATION = -1;

  [SerializeField] float movementSpeed = 5;
  [SerializeField] float rushSpeed = 10; //Miles* This is for rushing towards food if detected
  [SerializeField] float rotationSpeed; //Miles* This is used for smooth rotation
  [SerializeField] GameObject _target = null;

  Rigidbody2D _npcBody { get; set; }
  int _rotationType = POSITIVE_ROTATION;

  private float _randomRotationAngle;
  private Quaternion _rotation; //Miles* Use Quaternion instead of float to keep rortation data


  #region get & set
  public GameObject Target { set => _target = value; }
  #endregion



  void Start()
  {
    _npcBody = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    MoveForward(); //Miles* Move forward all the time, regardless if you see food or not. 
  }

  void MoveForward()
  {
    _npcBody.velocity = !_target ? transform.up * movementSpeed : transform.up * rushSpeed; //Miles* if targeting food, speed up towards it! :D
    transform.rotation = _rotation;
  }

  void RotateOnTarget()
  {
    Vector3 direction = _target.transform.position - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Miles* Angle between x and y 
    Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward); //Miles* - 90 degree to face the target directly
    _rotation = q;

    ////Miles* ==== USE LINE BELOW IF YOU WANT SMOOTH ROTATION, YOU WILL NEED TO RUN IT ON UPDATE TO MAKE IT WORK ====
    // transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed); //Miles* Average out 2 values, higher rotationSpeed = faster rotation.
  }

  void RotateOnWall()
  {
    _randomRotationAngle += _rotationType * Random.Range(5, 30);
    _rotation = Quaternion.Euler(0, 0, _randomRotationAngle);
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == SharedScripts.GlobalTags.WALL_TAG)
    {
      var tmpRandom = Random.Range(0, 2);
      _rotationType = tmpRandom == 0 ? POSITIVE_ROTATION : NEGATIVE_ROTATION;
    }
  }

  private void OnCollisionStay2D(Collision2D other)
  {
    if (other.gameObject.tag == SharedScripts.GlobalTags.WALL_TAG)
    {
      RotateOnWall();
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == SharedScripts.GlobalTags.FOOD_TAG && !_target) //Miles* target next food only if target is null
    {
      _target = other.gameObject;
      RotateOnTarget();
    }
  }
}

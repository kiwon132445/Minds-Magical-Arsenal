using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
  public float speed = 10;
  private Rigidbody _rigid;
  private float currentTime;

  const float DELETE_TIME = 3;

  private void Start() {
    _rigid = gameObject.GetComponent<Rigidbody>();
    currentTime += Time.deltaTime;
  }
  private void Update() {
    DestroyCountdown();
    if (speed != 0)
    {
      _rigid.velocity = gameObject.transform.forward * speed;//_vectorDirection.eulerAngles.normalized * speed;
    }
  }

  // private void OnTriggerEnter(Collider other) {
  //   if (!other.gameObject.CompareTag("Player"))
  //   {
  //     Destroy(gameObject);
  //   }
  // }

  private void DestroyCountdown()
  {
    currentTime += Time.deltaTime;
    if(currentTime >= DELETE_TIME)
    {
      Destroy(gameObject);
    }
  }
}
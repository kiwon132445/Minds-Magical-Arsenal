using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturn : MonoBehaviour
{
    private Vector3 _position;
    private Quaternion _rotation;
    private Rigidbody _rigidbody;
    const float Return_Time = 10;
    bool inSpawn = true;
    float start = 0;
    // Start is called before the first frame update
    void Start()
    {
        _position = gameObject.transform.position;
        _rotation = gameObject.transform.rotation;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other) 
    {
        Debug.Log(other.name);
        if (other.gameObject.CompareTag("SpawnPoint"))
        {
            inSpawn = false;
            start = 0;
        }
    }

    private void Update() {
        if (!inSpawn && !SelectionManager.Instance.ObjectInUse)
        {
            start += Time.deltaTime;
            if (start >= Return_Time)
            {
                inSpawn = true;
                _rigidbody.velocity = Vector3.zero;
                gameObject.transform.position = _position;
                gameObject.transform.rotation = _rotation;
            }
        }
    }
}

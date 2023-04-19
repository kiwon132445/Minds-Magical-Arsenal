using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            Debug.Log("No player detected on FollowPlayer script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (gameObject.transform.position != player.transform.position)
            {
                gameObject.transform.position = player.transform.position;
            }
        }
        
    }
}

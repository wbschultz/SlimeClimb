using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] bool followDown = true;
    [SerializeField] float smoothTime = 0.4f;

    private Vector2 velocity;
    private Vector3 initPos;
    private GameObject player;
    private float minY;

    private void Awake()
    {
        initPos = transform.position;
        minY = transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // if not following camera down, and the player is lower than the min position, don't follow
        if (!followDown && player.transform.position.y > minY)
        {
            return;
        }

        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTime);
        // never go below initial y position
        posY = Mathf.Max(posY, initPos.y);

        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        minY = Mathf.Max(minY, posY - 6f);
    }
}

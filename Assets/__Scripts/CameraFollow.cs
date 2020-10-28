using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] bool followDown = true;
    [SerializeField] float smoothTime = 0.4f;
    [SerializeField] float cameraOffsetY = 2.0f;

    [SerializeField]
    [Range(0, 1)] float minViewportDistFromEdge = 0.1f;     // always maintain player character at this viewport distance from the edge (10% inward)

    private Vector2 velocity;
    private Vector3 initPos;
    private GameObject player;
    private float minY;
    private Camera cam;

    private void Awake()
    {
        initPos = transform.position;
        minY = transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SmoothFollow();
    }

    private void SmoothFollow()
    {
        // if not following camera down, and the player is lower than the min position, don't follow
        if (!followDown && player.transform.position.y > minY)
        {
            return;
        } else if (OozeEscape.Instance.CurrState == OozeEscape.GameState.Play)
        {
            float playerViewportY = cam.WorldToViewportPoint(player.transform.position).y;
            float smoothTimeScale = Mathf.Min(Mathf.Min(Mathf.Abs(1 - playerViewportY), Mathf.Abs(0 - playerViewportY)) / 0.1f, 1f);

            float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + cameraOffsetY, ref velocity.y, smoothTime*smoothTimeScale);
            // never go below initial y position
            posY = Mathf.Max(posY, initPos.y);

            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            minY = Mathf.Max(minY, posY - 6f);
        }
    }

    /*
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
    }
    */
}

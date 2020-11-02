using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(DragManager))]
[RequireComponent(typeof(JellySprite))]
public class Player : MonoBehaviour
{
    [HideInInspector]
    public event Action OnDeath;

    [Header("Set in Inspector")]
    [SerializeField]
    private int forceScalar = 100;  //Scalar that modifies the tap and drag effect on the player rb (200-300 seems good)
    [Tooltip("Minimum y velocity while slow falling (negative value)")]
    [SerializeField]
    private float slowFallYSpeed = -1.0f;
    [SerializeField]
    private float slowFallXSpeed = 1f;
    [SerializeField]
    private float slowFallSeconds = 2.0f;
    [Tooltip("Time scale multiplier during drag")]
    [SerializeField]
    private float dragTimeScale = 0.5f;
    [SerializeField]
    private GameObject deathParticle;

    private DragManager dragComponent;
    private JellySprite jellySprite;
    private bool canJump = true;
    private bool slowFall = false;  // should clamp vertical fall speed

    #region Public Methods
    /// <summary>
    /// Kill the player
    /// </summary>
    public void Die()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        OnDeath?.Invoke();
    }

    public void OnJellyCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // initiate slow fall
            //slowFall = true;
            //StartCoroutine(EndSlowFall(slowFallSeconds));
            canJump = true;
        }
    }

    public void OnJellyCollisionExit2D(Collision2D collision)
    {
        
    }
    #endregion

    private void Awake()
    {
        // initialize components
        dragComponent = GetComponent<DragManager>();
        jellySprite = GetComponent<JellySprite>();
    }

    void OnEnable()
    {
        dragComponent.StartDrag += OnStartDrag;
        dragComponent.EndDrag += OnEndDrag;
    }

    private void OnDisable()
    {
        dragComponent.StartDrag -= OnStartDrag;
        dragComponent.EndDrag -= OnEndDrag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // slow fall speed
        if (slowFall)
        {
            Rigidbody2D[] rbArray = jellySprite.m_ReferencePointParent.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in rbArray)
            {
                rb.velocity = new Vector2(Mathf.Clamp(-1*slowFallXSpeed,rb.velocity.x,slowFallXSpeed), Mathf.Clamp(rb.velocity.y, slowFallYSpeed, float.MaxValue));
            }
        }
    }

    void OnStartDrag()
    {
        if (canJump)
            Time.timeScale = dragTimeScale;
    }

    void OnEndDrag(Vector2 dragVector)
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        // add force to player in drag direction
        if (canJump)
        {
            // end slow fall early
            //slowFall = false;

            // jump
            canJump = false;
            jellySprite.AddForce(dragVector * forceScalar);
        }
    }

    private IEnumerator EndSlowFall(float numSeconds)
    {
        yield return new WaitForSeconds(numSeconds);

        slowFall = false;
    }
}

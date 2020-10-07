using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    [SerializeField]
    private float maxDrag = 1.5f;

    //TODO make this not a hard reference
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    GameObject bigCircle;
    [SerializeField]
    GameObject littleCircle;

    public event Action StartDrag;
    public event Action<Vector2> EndDrag;

    private bool dragging = false;
    private Vector2 dragStartPos = new Vector2();
    private Vector2 dragCurrPos = new Vector2();
    private Vector2 dragVector = new Vector2();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        EndDrag += (Vector2) =>
        {
            lineRenderer.gameObject.SetActive(false);
        };
#if DEBUG
        StartDrag += () => { print("Starting Drag!"); };
        EndDrag += (Vector2 vector) => { print("Ended Drag with drag vector: " + vector); };
#endif
    }

    private void Update()
    {
        if (OozeEscape.Instance.CurrState == OozeEscape.GameState.Play)
        {
            // track dragging for standalone builds
            #region Standalone Drags
#if UNITY_STANDALONE

            // update current drag if there is one
            if (dragging)
            {
                dragCurrPos = mainCamera.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, mainCamera.transform.position.z));
                UpdateDrag();
            }

            // start a drag
            //if (!EventSystem.current.IsPointerOverGameObject())
            //{
                if (Input.GetMouseButtonDown(0) && !dragging)
                {
                    dragStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, mainCamera.transform.position.z));
                    dragging = true;
                    StartDrag?.Invoke();
                }
            //}

            // on end drag, do end drag behavior
            if (Input.GetMouseButtonUp(0) && dragging)
            {
                dragging = false;
                EndDrag?.Invoke(dragVector);
                ResetDrag();
            }

#endif
            #endregion

            // track dragging for mobile builds
            #region Mobile Drags
#if UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            // update current drag if there is one
            if (dragging && touch.phase == TouchPhase.Moved)
            {
                dragCurrPos = (Vector2)mainCamera.ScreenToWorldPoint(touch.position);
                UpdateDrag();
            }
            
            // start a new drag
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (!dragging && touch.phase == TouchPhase.Began)
                {
                    // start drag
                    dragStartPos = mainCamera.ScreenToWorldPoint(touch.position);
                    dragging = true;
                    StartDrag?.Invoke();
                }
            }
            
            // Do end drag behavior
            if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                dragging = false;
                EndDrag?.Invoke(dragVector);
                ResetDrag();
            }
        }

#endif
            #endregion
        }
    }

    /// <summary>
    /// Update the information related to the current drag
    /// </summary>
    void UpdateDrag()
    {
        Vector2 vecDifference = dragStartPos - dragCurrPos;
        dragVector = vecDifference.magnitude > maxDrag ? vecDifference.normalized * maxDrag : vecDifference;

        // update visualization
        bigCircle.transform.position = dragStartPos;
        littleCircle.transform.position = dragStartPos - dragVector;
        // adjust line
        lineRenderer.SetPosition(0, dragStartPos);
        lineRenderer.SetPosition(1, dragStartPos - dragVector);
        // show visualization
        if (!lineRenderer.gameObject.activeSelf)
            lineRenderer.gameObject.SetActive(true);
    }

    /// <summary>
    /// Reset variables related to current drag
    /// </summary>
    void ResetDrag()
    {
        dragVector = new Vector2();
        dragStartPos = new Vector2();
        dragCurrPos = new Vector2();
    }

    // draw visualization of drag for testing
#if DEBUG
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && dragging)
        {
            Gizmos.DrawWireSphere(dragStartPos, 0.3f);
            Gizmos.DrawWireSphere(dragStartPos - dragVector, 0.2f);
            Gizmos.DrawLine(dragStartPos, dragStartPos - dragVector);
        }
    }
#endif

}

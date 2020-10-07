using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControl : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private float maxTransformX = 0.2f;

    private Rigidbody2D playerRB;
    private GameObject refPointParent;
    private GameObject player;
    private bool isInit = false;

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CheckForRefPoint());
    }

    // Update is called once per frame
    void Update()
    {
        // kind of weird hack for setting the eye position
        // set the eye deviation to the ratio of x component in the velocity vector
        // ie if it's all x motion, maximum deviation
        // if less than a threshhold, set to 0
        if (isInit)
        {
            if (playerRB.velocity.magnitude > 0.1)
                transform.localPosition = new Vector3((playerRB.velocity.normalized.x / 1) * maxTransformX, transform.localPosition.y, transform.localPosition.z);
            else
            {
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            }
        }
            
    }

    /// <summary>
    /// check for the central ref point once every 10 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForRefPoint()
    {
        // check for refPointParent
        refPointParent = player.GetComponent<JellySprite>().m_ReferencePointParent;
        while (refPointParent == null)
        {
            // wait for 1/10th second
            yield return new WaitForSeconds(0.1f);
            // check for ref point parent
            refPointParent = player.GetComponent<JellySprite>().m_ReferencePointParent;

        }
        // set RB ref
        playerRB = refPointParent.transform.Find(player.name + " Central Ref Point").GetComponent<Rigidbody2D>();
        // set flag
        isInit = true;
    }
}

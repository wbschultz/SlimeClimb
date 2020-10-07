using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Electrify : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    [Tooltip("Time in seconds to cycle electrified, then not electrified. 0 indicates always electrified.")]
    private float electricityInterval = 0;

    private ParticleSystem electricityParticles;
    private bool electrified = false;
    private Coroutine cycleRoutine;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        electricityParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        if (electricityParticles == null)
        {
            Debug.LogError("Missing electricity particle system prefab");
        } else
        {
            //start electricity, cycling if interval is nonzero
            if (electricityInterval == 0)
            {
                electrified = true;
                electricityParticles.Play();
            } else if(electricityInterval > 0)
            {
                // call coroutine
                cycleRoutine = StartCoroutine(CycleElectricity(electricityInterval));
            } else
            {
                Debug.LogError("Invalid value for electricityInterval on obj: " + gameObject.name);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (electrified)
        {
            JellySpriteReferencePoint refPoint = collision.gameObject.GetComponent<JellySpriteReferencePoint>();
            if (refPoint != null)
            {
                if (refPoint.ParentJellySprite.gameObject.CompareTag("Player"))
                {
                    // kill player
                    refPoint.ParentJellySprite.gameObject.GetComponent<Player>().Die();
                }
            }
        }
    }

    /// <summary>
    /// Activate relevant effects based on electrified state
    /// </summary>
    private void UpdateElectricity()
    {
        if (electrified)
        {
            electricityParticles.Play();
        } else
        {
            electricityParticles.Stop();
        }
    }

    /// <summary>
    /// toggle electrified status every certain number of seconds
    /// </summary>
    /// <param name="seconds">Float number of seconds to wait before electrifying</param>
    /// <returns></returns>
    private IEnumerator CycleElectricity(float seconds)
    {
        while (true)
        {
            electrified = !electrified;
            UpdateElectricity();

            yield return new WaitForSeconds(seconds);
        }
    }
}

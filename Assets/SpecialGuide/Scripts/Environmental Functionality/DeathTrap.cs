using UnityEngine;
using System.Collections;

public class DeathTrap : MonoBehaviour {
    private Transform thisTransform;
    private Collider2D[] thisColliders;

    public float phaseTime = 1.0f;
    public float phaseCooldown = 1.5f;
    public float startTime = 1.0f; //när hela börjar köras, kan behövas offset för att få dem ur fas

    public ParticleSystem ps;

    void Start()
    {
        thisTransform = this.transform;

        thisColliders = thisTransform.GetComponentsInChildren<Collider2D>();
        //ps = thisTransform.GetComponent<ParticleSystem>();

        Reset();
    }

    public void Reset()
    {
        StopAllCoroutines();
        StartCoroutine(KillZoneLifetime());
    }

    IEnumerator KillZoneLifetime()
    {
        yield return new WaitForSeconds(startTime);
        while (this != null)
        {
            yield return new WaitForSeconds(phaseCooldown);
            ToggleKillZone();
            yield return new WaitForSeconds(phaseTime);
            ToggleKillZone();
        }
    }

    public void ToggleKillZone()
    {
        bool b = thisColliders[0].enabled;

        if (b == false)
        {
            ps.Simulate(0.0f, true, true);
            ParticleSystem.EmissionModule psemit = ps.emission;
            psemit.enabled = true;
            ps.Play();
        }
        else
        {
            ps.Stop();
        }

        for (int i = 0; i < thisColliders.Length; i++)
        {
            thisColliders[i].enabled = !b;
        }
    }

    public void ToggleKillZone(bool b)
    {
        if (b == false)
        {
            ps.Simulate(0.0f, true, true);
            ParticleSystem.EmissionModule psemit = ps.emission;
            psemit.enabled = true;
            ps.Play();
        }
        else
        {
            ps.Stop();
        }

        for (int i = 0; i < thisColliders.Length; i++)
        {
            thisColliders[i].enabled = !b;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Unit") || col.gameObject.layer == LayerMask.NameToLayer("MovableObject"))
        {
            col.gameObject.SetActive(false);
            //Destroy(col.gameObject);
        }
    }
}

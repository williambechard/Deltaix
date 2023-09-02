using System.Collections.Generic;
using UnityEngine;

public class SpreadImpact : Impact
{
    public GameObject electroBulletPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Targets"))
        {
            Damagable d = collision.gameObject.GetComponent<Damagable>();
            if (d != null)
            {
                EventManager.TriggerEvent("Rumble", new Dictionary<string, object>() { { "intensity", .05f }, { "duration", .1f } });
                d.Damage(damageAmount);

                //shoot another bullet towards a close target if one exists
                GameObject[] targets = GameObject.FindGameObjectsWithTag("Targets");
                GameObject closest = null;
                float closestDistance = 40;
                foreach (GameObject target in targets)
                {
                    if (target != collision.gameObject && target != null)
                    {
                        float distance = Vector3.Distance(transform.position, target.transform.position);

                        if (distance < closestDistance)
                        {
                            closest = target;
                            closestDistance = distance;
                        }
                    }
                }
                if (closest != null)
                {
                    GameObject bullet = Instantiate(electroBulletPrefab, transform.position, Quaternion.identity);
                    bullet.name = "Link";
                    bullet.GetComponent<MoveTo>().target = closest.transform;

                }

                Destroy(this.gameObject);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Targets"))
        {
            Damagable d = collision.gameObject.GetComponent<Damagable>();
            if (d != null)
            {
                d.Damage(damageAmount);

                EventManager.TriggerEvent("Rumble", new Dictionary<string, object>() { { "intensity", .25f }, { "duration", .5f } });
                Destroy(this.gameObject);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlanetImpact : MonoBehaviour
{

    public int health;
    public int maxHealth;

    public Vector3 fullHealth = new Vector3(4.5f, 4.5f, 1);
    public Vector3 noHealth = new Vector3(4f, 4f, 1);

    public GameObject GameOverPrefab;
    public bool doNotDestroy = false;
    public SpriteRenderer sr;
    public CircleCollider2D cc;
    public PlaySoundOneShot playSound;

    private void Start() => health = maxHealth;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Targets"))
        {
            adjustHealth(-1);
            collision.gameObject.GetComponent<Damagable>().DestroyObject();
            if (playSound != null) AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
            if (health <= 0)
            {
                GameObject g = Instantiate(GameOverPrefab, transform.position, Quaternion.identity);
                g.name = "DESTRUCTION";
                sr.color = new Color(0, 0, 0, 1);
                if (!doNotDestroy) Destroy(gameObject);
            }
        }
    }

    public void adjustHealth(int amount)
    {
        if (amount < 0) EventManager.TriggerEvent("Rumble", new Dictionary<string, object>() { { "intensity", .5f }, { "duration", .5f } });
        health += amount;
        if (health > maxHealth) health = maxHealth;
        if (health < 0) health = 0;
        float lerpFactor = (float)health / maxHealth; // Ensure lerpFactor is a float
        Vector3 newScale = Vector3.Lerp(noHealth, fullHealth, lerpFactor);
        sr.transform.localScale = newScale;

        if (cc != null) cc.transform.localScale = newScale;
    }

}

using UnityEngine;

public class FireBullet : ExecuteAction
{
    public GameObject bulletPrefab;
    private PauseHandler pauseHandler;
    // Start is called before the first frame update
    void Start()
    {
        pauseHandler = GetComponent<PauseHandler>();
    }

    public override void Execute(GameObject target, Vector3 origin, GameObject parent, int radius)
    {
        if (pauseHandler == null)
        {
            pauseHandler = GetComponent<PauseHandler>();
        }
        Debug.Log("Pause " + pauseHandler.isPaused + " " + gameObject.name);
        if (!pauseHandler.isPaused)
        {
            base.Execute(target, origin, parent); //,may not be needed, but could be useful for effects

            GameObject bullet = Instantiate(bulletPrefab, new Vector3(origin.x, origin.y, -12), transform.rotation);
            MoveTo moveTo = bullet.GetComponent<MoveTo>();
            moveTo.target = target.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

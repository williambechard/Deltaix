using UnityEngine;

public class FireLaser : ExecuteAction
{
    public GameObject laserPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Execute(GameObject target, Vector3 origin, GameObject parent, int radius)
    {

        base.Execute(target, origin, parent);

        GameObject laser = Instantiate(laserPrefab, new Vector3(origin.x, origin.y, -12), transform.rotation);

        laser.transform.SetParent(parent.transform);

    }
}

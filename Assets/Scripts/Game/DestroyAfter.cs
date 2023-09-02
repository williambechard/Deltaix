using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public float TTL = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(ObjectToSpawn);
        obj.transform.position = transform.position;
        Destroy(gameObject, TTL);
    }



    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using UnityEngine;

public class ClusterWatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(watchForDestruction());
    }

    IEnumerator watchForDestruction()
    {
        while (true)
        {
            if (transform.childCount == 0)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

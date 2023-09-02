using UnityEngine;

public class DeleteGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.parent.parent.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

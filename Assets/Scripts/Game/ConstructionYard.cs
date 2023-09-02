using UnityEngine;

public class ConstructionYard : MonoBehaviour
{
    public GameObject buildPrefab;
    public static GameObject ToBeBuilt;

    // Start is called before the first frame update
    void Start()
    {
        ToBeBuilt = buildPrefab;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

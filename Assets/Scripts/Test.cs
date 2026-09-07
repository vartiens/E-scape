using UnityEngine;

public class Test : MonoBehaviour
{
    private const float YDIFF = 0.5f, XDIFF = 1.0f, ZDIFF = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] overlap = Physics.OverlapBox(gameObject.transform.position + new Vector3(0, YDIFF / 2, 0),
            new Vector3(XDIFF / 2, YDIFF / 2, ZDIFF / 2));
        if (overlap.Length > 1)
        {
            foreach (Collider ov in overlap)
                Debug.Log($"Collide with {ov.name}");
        }
        else Debug.Log("Not collide");
    }
}

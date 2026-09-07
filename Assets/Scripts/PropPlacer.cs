using System.Collections.Generic;
using UnityEngine;

public class PropPlacer : MonoBehaviour
{
    // GameObjects denoting the candidate area
    // Rectangle between [0], [1] and [2], [3] and so on
    public GameObject[] targets;
    public GameObject objectToPlace;
    public int objCount;
    public float objGap;

    // Positions that safisfies the distance & overlap conditions
    private Vector3[] checkedPos;

    // Detection range
    // All values are measured from one end to another
    // X, Z: Centered on midpoint of the object, Y: Only up
    // Unit: unity coords
    private const float SIZE = 0.15f, YDIFF = 1.0f;
    private const float FLOORDIST = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        // Get every triangles from target meshes
        List<Vector3[]> triangles = new List<Vector3[]>(); // This will contain the coords of triangle's vertices
        checkedPos = new Vector3[objCount];

        // Get triangles from every two coords' rectangles
        Vector3 t1, t2, t3, t4;
        for (int i = 0; i < targets.Length / 2; i++)
        {
            t1 = targets[2 * i].transform.position;
            t2 = targets[2 * i + 1].transform.position;
            t3 = new Vector3(t1.x, t1.y, t2.z);
            t4 = new Vector3(t2.x, t2.y, t1.z);
            triangles.Add(new Vector3[] { t1, t2, t3 });
            triangles.Add(new Vector3[] { t1, t2, t4 });
        }

        // Sample a random coord from all triangles
        // If that coord is somehow unavailable
        // Sample another coord until available
        int triNo;
        Vector3 targetCoord;
        for (int i = 0; i < objCount; i++)
        {
            int iter = 0;
            do
            {
                iter++;
                triNo = Random.Range(0, triangles.Count);
                targetCoord = RandomInTriangle(triangles[triNo]);
            } while (!IsAvailable(targetCoord, i) || iter < 99);

            checkedPos[i] = targetCoord;
            GameObject newObj = Instantiate(objectToPlace, targetCoord, Quaternion.identity);
            GameObject childs = newObj.transform.GetChild(0).gameObject;
            newObj.name = $"Object{i + 1}";
            newObj.layer = LayerMask.NameToLayer("ClueObject");
            childs.name = $"Floppy{i + 1}";
            childs.layer = LayerMask.NameToLayer("ClueObject");
        }

    }

    // Sample a random location inside a triangle
    // Used algorithm:
    // https://math.stackexchange.com/questions/18686/uniform-random-point-in-triangle-in-3d
    Vector3 RandomInTriangle(Vector3[] triangle)
    {
        float r1 = Random.Range(0f, 1f);
        float r2 = Random.Range(0f, 1f);
        Vector3 point = (1 - Mathf.Sqrt(r1)) * triangle[0] + (Mathf.Sqrt(r1) * (1 - r2)) * triangle[1]
            + (r2 * Mathf.Sqrt(r1)) * triangle[2];
        return point;
    }

    // Is the coord available?
    // Availability == Not obstructed, etc.
    bool IsAvailable(Vector3 target, int setPointsCnt)
    {
        // All meshes will be parallel to the ground (or be it the ground itself)
        // How to check it? Physics.OverlapBox
        // Threshold would be quite small
        Collider[] overlap = Physics.OverlapBox(target + new Vector3(0, YDIFF / 2, 0),
            new Vector3(SIZE / 2, YDIFF / 2, SIZE / 2));
        Collider[] floor = Physics.OverlapBox(target + new Vector3(0, -FLOORDIST / 2, 0),
            new Vector3(SIZE / 2, FLOORDIST / 2, SIZE / 2));
        // WHY NOT WORK

        // Size of disk is about 0.14 * 0.14
        // Must have floor & not be obstructed
        if (overlap.Length <= 0 && floor.Length > 1)
        {
            // Only check unobstructed position
            // How to reduce time complexity?
            for (int i = 0; i < setPointsCnt; i++)
                if (Vector3.Distance(checkedPos[i], target) <= objGap) // Not considering y coords
                    return false;

            return true; // Object itself is always counted
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
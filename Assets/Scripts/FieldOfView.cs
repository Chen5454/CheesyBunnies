using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{ 
    public float fov = 360f;
    [SerializeField] public LayerMask layerMask;
    public bool inside = true;
    int raycount = 60;
    float angle = 0, start_angle=0;
    public float viewDistance = 20f;
    public Sprite BG1, BG2;
    public int doorsAmount = 0;

    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    Mesh mesh;
    Vector3 origin;
    float angleIncrease, timer_out=0;
    PolygonCollider2D collider2D;
    List<Vector2> pathOutside2D;


    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        collider2D = GetComponent<PolygonCollider2D>();
        vertices = new Vector3[raycount + 2];
        uv = new Vector2[vertices.Length];
        triangles = new int[raycount * 3];
        angleIncrease = fov / raycount;
        pathOutside2D = new List<Vector2>();
    }
    private void Update()
    {
        Vector3[] path;
        if (inside)
            path = Inside();
        else
            path = create_one_mesh();
        Vector2[] path2D = new Vector2[path.Length];
        for (int i = 0; i < path2D.Length; i++)
            path2D[i] = new Vector2(path[i].x, path[i].y);
        if (inside)
            collider2D.SetPath(0, path2D);
        else
            collider2D.SetPath(0, pathOutside2D);

        if (timer_out > 0)
        {
            timer_out -= Time.deltaTime;
            if (timer_out < 0) timer_out = 0;
        }
    }
    private Vector3[] create_one_mesh()
    {
        if (transform.parent.GetComponent<FieldOfView>().doorsAmount == 0)
        {
            GetComponent<MeshFilter>().mesh = null;
            return new Vector3[0];
        }
        Mesh[] meshFilters = Outside();
        if (meshFilters.Length == 1)
        {
            GetComponent<MeshFilter>().mesh = meshFilters[0];
            return mesh.vertices;
        }
        GetComponent<MeshFilter>().mesh = null;

        GameObject child;
        for (int i = 0; i < meshFilters.Length; i++) {
            if (transform.childCount < meshFilters.Length)
            {
                child = new GameObject("child of mesher");
                child.transform.parent = transform;
                child.AddComponent<MeshRenderer>();
                child.AddComponent<MeshFilter>();

            }
            else
                child = transform.GetChild(i).gameObject;
            child.GetComponent<MeshFilter>().mesh = meshFilters[i];
        }
        return meshFilters[0].vertices;
    }
    private Vector3[] Inside()
    {
        angle = start_angle;
        origin = Vector3.zero;// transform.InverseTransformPoint(transform.position);

        vertices[0] = origin;
        int vertexIndex = 1;
        int tringleIndex = 0;
        doorsAmount = 0;

        for (int i = 0; i <= raycount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetVector3FromAngle(angle), viewDistance, layerMask);
            if (hit.collider == null || hit.collider.tag != "Portal")
                vertex = origin + GetVector3FromAngle(angle) * viewDistance;
            else
            {
                vertex = hit.point - new Vector2(transform.position.x, transform.position.y);//transform.InverseTransformPoint(hit.point);
                doorsAmount++;
            }
            
            vertices[vertexIndex] = vertex;

            if (vertexIndex > 1)
            {
                triangles[tringleIndex] = 0;
                triangles[tringleIndex + 1] = vertexIndex - 1;
                triangles[tringleIndex + 2] = vertexIndex;
                tringleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);

        return mesh.vertices;
    }
    public Mesh[] Outside()
    {
        pathOutside2D.Clear();
        angle = start_angle;
        origin = Vector3.zero;// transform.InverseTransformPoint(transform.position);

        vertices[0] = origin;
        Vector3[] outsides = new Vector3[vertices.Length];
        Vector3[] insiders = new Vector3[vertices.Length];
        int vertexIndex = 1;
        bool flip_to_inside = false;
        int doors = 0;


        //get an angle doesnt hit a door when the loop starts
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetVector3FromAngle(angle), viewDistance, layerMask);
        bool doorHere = (hit.collider != null && hit.collider.tag == "Portal");
        while (doorHere) {
            angle -= angleIncrease;
            hit = Physics2D.Raycast(transform.position, GetVector3FromAngle(angle), viewDistance, layerMask);
            doorHere = (hit.collider != null && hit.collider.tag == "Portal");
            if (angle < 0) angle += 360;
            if (angle == start_angle) return new Mesh[] { new Mesh() };
        }


        int sizeDoor = 0;
        List<int> doorsSizes = new List<int>();
        List<List<int>> listOfindexOfDoors = new List<List<int>>();
        List<int> currentDoor = new List<int>();
        for (int i = 0; i <= raycount; i++)
        { 
            Vector3 vertex=Vector3.zero;
            Vector3 edgeOfCircle = origin + GetVector3FromAngle(angle) * viewDistance;
            hit = Physics2D.Raycast(transform.position, GetVector3FromAngle(angle), viewDistance, layerMask);
            if (hit.collider != null && hit.collider.tag == "Portal") //found a portal
            {
                if (sizeDoor == 0)
                    pathOutside2D.Add(new Vector2(outsides[vertexIndex - 1].x, outsides[vertexIndex - 1].y));
                vertex = hit.point - new Vector2(transform.position.x, transform.position.y);//transform.InverseTransformPoint(hit.point);
                sizeDoor++;
                flip_to_inside = true;
                currentDoor.Add(vertexIndex);

                pathOutside2D.Add(new Vector2(edgeOfCircle.x, edgeOfCircle.y));
                pathOutside2D.Insert(sizeDoor-1, new Vector2(vertex.x, vertex.y));

            }
            else if (flip_to_inside)
            { //end of portal.
                doors++;
                flip_to_inside = false;
                if (sizeDoor % 2 == 0)
                { //if the size of the door is even then we add anther item
                    sizeDoor++;
                    currentDoor.Add(vertexIndex);
                }
                pathOutside2D.Add(new Vector2(edgeOfCircle.x, edgeOfCircle.y));
                listOfindexOfDoors.Add(currentDoor);
                currentDoor = new List<int>();
                doorsSizes.Add(sizeDoor+2); //add the start and the end of the door.
                sizeDoor = 0;
            }
            
            outsides[vertexIndex] = edgeOfCircle;
            insiders[vertexIndex] = vertex;

            vertexIndex++;
            angle -= angleIncrease;
        }
        if (doors == 0) return new Mesh[] { new Mesh() }; //there are no doors to create mesh
        Mesh[] meshes = new Mesh[doors];
        for (int i = 0; i < doors; i++)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[doorsSizes[i]];
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[(vertices.Length-2)*3];

            List<int> indexOfDoors = listOfindexOfDoors[i];
            bool inside_outside = true;
            vertices[0] = outsides[indexOfDoors[0] - 1];
            for (int j = 0; j < indexOfDoors.Count; j++)
            {
                vertices[j + 1] = inside_outside ?
                    insiders[indexOfDoors[j]] :
                    outsides[indexOfDoors[j]];
                if (vertices[j + 1] == Vector3.zero)
                    vertices[j + 1] = insiders[indexOfDoors[j]];
                inside_outside = !inside_outside;
            }
            vertices[vertices.Length-1] = //the edge of the last item
                outsides[indexOfDoors[indexOfDoors.Count-1] + 1];  //located at the end of the array + 1
            int tringleIndex = 0;
            for (int vertexIndexT = 0; vertexIndexT < vertices.Length-2; vertexIndexT++)
            {
                if (tringleIndex % 2 == 0)
                {
                    triangles[tringleIndex+2] = vertexIndexT;
                    triangles[tringleIndex + 1] = vertexIndexT + 1;
                    triangles[tringleIndex] = vertexIndexT + 2;
                }
                else
                {
                    triangles[tringleIndex+2] = vertexIndexT + 2;

                    triangles[tringleIndex] = vertexIndexT;
                    triangles[tringleIndex + 1] = vertexIndexT + 1;
                }

                tringleIndex += 3;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            meshes[i]= mesh;
        }
        return meshes;
    }

    public void setOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
    public void SetAimDirection(Vector3 aim)
    {
        start_angle = GetAngle(aim) - fov/2f;
    }

    public static float GetAngle(Vector3 aim)
    {
        aim = aim.normalized;
        float n = Mathf.Atan2(aim.x, aim.y) * Mathf.Rad2Deg;
        if (n <= 0) n += 360;
        return n;
    }
    public static Vector3 GetVector3FromAngle(float angle)
    {
        //angle = 0 -> 360
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Portal" && inside)||collision.tag=="FOV")
        {
            return;
        }

        if (inside||collision.gameObject.layer == gameObject.layer || (collision.tag == "Root") || (collision.tag == "FOV")) return;
				
        if(collision.transform.childCount>0)
            collision.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled =false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal" && inside) {
            //SwitchLayers(gameObject.layer);
            return; }
        if (collision.gameObject.layer == gameObject.layer ||(collision.tag == "FOV")|| (collision.tag == "Root")) return;
        if (collision.transform.childCount > 0)
            collision.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }
    public void SwitchLayers()
    {
        int layer = gameObject.layer ;
        timer_out = 2.5f;
        Sprite sprite;

        //NEED TO BE CALLED WHEN ROOT ENTER TELEPORT
        Debug.Log("switching layers");
        if (LayerMask.NameToLayer("Level1") == layer)
        {
            gameObject.layer = LayerMask.NameToLayer("Level2");
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Level1");
            sprite = BG2;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Level1");
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Level2");
            sprite = BG1;
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Background"))
            item.GetComponent<SpriteRenderer>().sprite = sprite;

    }
}

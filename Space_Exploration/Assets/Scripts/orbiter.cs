
using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

public class orbiter : MonoBehaviour
{

    public Transform world;

    public float rotationspeed = 0.3f;
    // Start is called before the first frame update

    void Start()
    {
        world = GetComponent<Transform>();
    }
    // Update is called once per frame

    void Update()
    {
        // rotate the world
        world.Rotate(new Vector3(0, rotationspeed, 0), Space.World);
            }
}
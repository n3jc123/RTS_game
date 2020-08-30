using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 40f;
    public float panBorderThickness = 10f;
    public float scrollLimit = 100f;
    public float scrollSpeed = 20f;

    private float rotationSpeed;
    private Vector2 panLimit;


    void Start()
    {
        GameObject ground = GameObject.Find("Ground");

        Renderer rend = ground.GetComponent<Renderer>();
        panLimit.x = rend.bounds.max.x;
        panLimit.y = rend.bounds.max.z;
            
        rotationSpeed = scrollSpeed * (20f / (scrollLimit - 20f));
    }

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 rot = Camera.main.transform.localEulerAngles;
        if (!Input.GetMouseButton(0))
        {
            if (Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        rot.x -= scroll * rotationSpeed * 100f * Time.deltaTime;
        panSpeed = 40 + transform.position.y / 2;



        pos.x = Mathf.Clamp(pos.x, -panLimit.x + 15, panLimit.x - 15);
        pos.y = Mathf.Clamp(pos.y, 20f, scrollLimit);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y - 40, panLimit.y - 40);

        rot.x = Mathf.Clamp(rot.x, 45f, 65f);

        transform.position = pos;
        transform.eulerAngles = rot;


    }
}

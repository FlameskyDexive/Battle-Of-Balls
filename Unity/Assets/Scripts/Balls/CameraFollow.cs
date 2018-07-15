using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    public float speed = 2;
    public static bool isSpawnPlayer = false;
    private Transform playerRoot;
    public Vector3 offset = new Vector3(1, 6, 8);
    public Vector3 offsetCenter = new Vector3(0, -1, 0);

    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        Time.timeScale = 1;
        playerRoot = GameObject.Find("PlayerSpawnPoint").transform;
        //Debug.Log(GameObject.FindGameObjectWithTag("Player").name);
        //Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject.name);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isSpawnPlayer)
        {
            player = playerRoot.GetChild(0);
            Vector3 targetPos = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position - offsetCenter);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            /*Debug.Log(player.position.ToString() + targetPos.ToString());
            Debug.Log(player.localPosition.ToString());*/
        }
        else
        {
        }

    }

    /*public float smoothing = 5f;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);
    public Vector2 min;
    public Vector2 max;

    private void LateUpdate()
    {
        if (isSpawnPlayer)
        {
            player = playerRoot.GetChild(0);
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = player.position + offset;
            if (targetCamPos.z < min.y)
            {
                targetCamPos.z = min.y;
            }
            if (targetCamPos.z > max.y)
            {
                targetCamPos.z = max.y;
            }
            if (targetCamPos.x < min.x)
            {
                targetCamPos.x = min.x;
            }
            if (targetCamPos.x > max.x)
            {
                targetCamPos.x = max.x;
            }
            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing*Time.deltaTime);
            transform.position = targetCamPos;
            //Debug.Log(player.position.ToString() + targetCamPos.ToString());
        }
    }*/

}

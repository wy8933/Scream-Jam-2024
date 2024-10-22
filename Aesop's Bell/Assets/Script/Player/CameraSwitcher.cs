using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform firstPersonPosition;
    public Transform thirdPersonPosition;
    public float switchSpeed = 5f;

    private bool isFirstPerson = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isFirstPerson = !isFirstPerson;
        }

        if (isFirstPerson)
        {
            transform.position = Vector3.Lerp(transform.position, firstPersonPosition.position, Time.deltaTime * switchSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, firstPersonPosition.rotation, Time.deltaTime * switchSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, thirdPersonPosition.position, Time.deltaTime * switchSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, thirdPersonPosition.rotation, Time.deltaTime * switchSpeed);
        }
    }
}

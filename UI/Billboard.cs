using UnityEngine;
public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation =  Camera.main.transform.rotation;
    }
}

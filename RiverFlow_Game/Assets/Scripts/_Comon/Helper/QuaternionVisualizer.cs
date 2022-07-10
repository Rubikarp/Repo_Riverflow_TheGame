using UnityEngine;

public class QuaternionVisualizer : MonoBehaviour
{
    [Header("Quaternion Values")]
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float w = 1f;

    private void Update()
    {
        //Default
        transform.rotation = new Quaternion(x, y, z, w).normalized;
    }

    [NaughtyAttributes.Button]
    public void Normalized()
    {
        x = transform.rotation.x;
        y = transform.rotation.y;
        z = transform.rotation.z;
        w = transform.rotation.w;
    }
}

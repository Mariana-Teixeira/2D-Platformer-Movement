using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smooth;

    void FixedUpdate()
    {
        transform.position = CalculateCameraPosition();
    }

    Vector3 CalculateCameraPosition()
    {
        return LerpVector(transform.position, target.position + offset, smooth);
    }

    Vector3 LerpVector(Vector3 a, Vector3 b, float t)
    {
        t = Smoothstep(t);
        Vector3 lerpVector = new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);

        return lerpVector;
    }

    float LerpFloat(float a, float b, float t)
    {
        float lerpFloat = a + (b - a) * t;

        return lerpFloat;
    }

    float EaseIn(float t)
    {
        return t * t;
    }

    float EaseOut(float t)
    {
        return 1.0f - (1.0f - t) * (1.0f - t);
    }

    float Smoothstep(float t)
    {
        return LerpFloat(t * t, 1.0f - (1.0f - t) * (1.0f - t), t);
    }
}

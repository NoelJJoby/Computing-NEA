using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private float friction = 1.5f;

    public float GetFriction()
    {
        return friction;
    }
}

using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class Tweening : MonoBehaviour
{
    [SerializeField] private Vector3 _vector;
    [SerializeField] private float _duration;

    private GameObject _cube;

    private void Awake()
    {
        _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    [Button]
    private void Move()
    {
        _cube.transform.DOMove(_vector, _duration);
    }
}

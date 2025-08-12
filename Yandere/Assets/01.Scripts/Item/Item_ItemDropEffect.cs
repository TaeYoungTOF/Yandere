using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_ItemDropEffect : MonoBehaviour
{
    private Transform _playerTransform;
    private float _moveSpeed;
    private bool _isFollowing = false;
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _playerTransform = playerObj.transform;
    }

    public void MoveToPlayerInstantly(float speed)
    {
        if (_playerTransform == null) return;

        _moveSpeed = speed;
        

        if (!_isFollowing)
        {
            _isFollowing = true;
        }
    }

    private void Update()
    {
        if (!_isFollowing || _playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        if (distance < 0.5f)
        {
            _isFollowing = false;
            transform.DOKill();
            CollectItem();
            return;
        }

        // 매 프레임 새 위치로 0.1초짜리 트윈
        transform.DOKill(); // 중첩 트윈 방지
        float duration = distance / _moveSpeed;
        transform.DOMove(_playerTransform.position, duration).SetEase(Ease.Linear);
    }

    private void CollectItem()
    {
        if (TryGetComponent<Item>(out var item) && _playerTransform != null)
        {
            if (_playerTransform.TryGetComponent<Player>(out var player))
            {
                item.Use(player);
            }
        }
        
        Destroy(gameObject);
    }
}

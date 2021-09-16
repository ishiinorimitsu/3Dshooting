using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(0, 100)]
    public float speed = 10f;
    public float DeadSecond = 10f;

    public float Life = 10;

    float _time;
    PlayerController _player;
    void Start()
    {
        _time = 0f;
        _player = FindObjectOfType<PlayerController>();    //�q�G�����M�[�̒��Ɉ�����Ȃ��X�N���v�g�̏ꍇ�A�w�肵���X�N���v�g�������Ă����
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time >= DeadSecond)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 vec = _player.transform.position - transform.position;
            transform.position += vec.normalized * speed * Time.deltaTime;
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("aaaaa");
        _player.ShotBullet(transform.position);
        Debug.Log("����1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            Life -= 10;
            Destroy(other.gameObject);
            if(Life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

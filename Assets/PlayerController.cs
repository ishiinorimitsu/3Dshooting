using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 50)]
    public float MoveSpeed = 10f;
    public float MoveRenge = 40f;

    public Transform[] RoutePoints;

    [Range(0, 200)]
    public float Speed = 10f;

    bool _isHitRootPoint;

    IEnumerator Move()
    {
        Vector3 prevPointPos = transform.position;
        Vector3 basePosition = transform.position;
        Vector2 movedPos = Vector2.zero;
        
        foreach(Transform nextPoint in RoutePoints)
        {
            _isHitRootPoint = false;

            while (!_isHitRootPoint)
            {
                //進行方向の計算
                Vector3 vec = nextPoint.position - prevPointPos;
                vec.Normalize();

                //プレイヤーの移動
                basePosition += vec * Speed * Time.deltaTime;

                //
                movedPos.x += Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
                movedPos.y += Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
                movedPos = Vector2.ClampMagnitude(movedPos, MoveRenge);
                var worldMovePos = Matrix4x4.Rotate(transform.rotation).MultiplyVector(movedPos);

                //ルート上の位置の上下左右の移動量を加えている
                transform.position = basePosition + worldMovePos;

                //次の処理は進行方向を向くように計算している
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), 0.5f);

                yield return null;
            }
            prevPointPos = nextPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RoutePoint")
        {
            other.gameObject.SetActive(false);
            _isHitRootPoint = true;
        }
    }

    void Start()
    {
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Range(0, 50)]
    public float MoveSpeed = 10f;
    public float MoveRenge = 40f;

    public Transform[] RoutePoints;

    [Range(0, 200)]
    public float Speed = 10f;

    public float _InitialLife = 100;　//マックスのHP

    public float Life = 100;  //現在のHP

    public Image LifeGage;

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
                Vector3 worldMovePos = Matrix4x4.Rotate(transform.rotation).MultiplyVector(movedPos);

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
        else if(other.gameObject.tag == "Enemy")
        {
            Life -= 10;
            LifeGage.fillAmount = Life / _InitialLife;

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            if(Life <= 0)
            {
                Camera.main.transform.SetParent(null);
                gameObject.SetActive(true);
                SceneManager sceneManager = FindObjectOfType<SceneManager>();
                sceneManager.ShowGameOver();
            }
        }
        else if(other.gameObject.tag == "ClearRoutePoint")
        {
            SceneManager sceneManager = FindObjectOfType<SceneManager>();
            sceneManager.ShowGameClear();
            _isHitRootPoint = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BackGround")
        {
            Life -= 1;
            LifeGage.fillAmount = Life / _InitialLife;
            Debug.Log("ohno!");

            if (Life <= 0)
            {
                Camera.main.transform.SetParent(null);
                gameObject.SetActive(true);
                SceneManager sceneManager = FindObjectOfType<SceneManager>();
                sceneManager.ShowGameOver();
            }
        }
    }

    public Bullet BulletPrefab;
    public void ShotBullet(Vector3 targetPos)
    {
        Bullet bullet = Object.Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.Init(transform.position, targetPos);
        Debug.Log("完了2");
    }

    void Start()
    {
        StartCoroutine(Move());
    }
}

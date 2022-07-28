using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerを制御するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [SerializeField] float _movingSpeed = default;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float _turnSpeed = default;

    /// <summary>攻撃中判定</summary>
    bool _isAttack;

    Rigidbody _rb;
    Animator _anim;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_isAttack)
        {
            Moving();
        }

        Attack();
    }

    void LateUpdate()
    {
        // 水平方向の速度を求めて Animator Controller のパラメーターに渡す
        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;
        _anim.SetFloat("Speed", horizontalVelocity.magnitude);
    }

    /// <summary>
    /// 動き
    /// </summary>
    void Moving()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 dir = Vector3.forward * v + Vector3.right * h;

        if (dir == Vector3.zero)
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);  // Slerp を使うのがポイント

            Vector3 velo = dir.normalized * _movingSpeed; // 入力した方向に移動する
            velo.y = _rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            _rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _anim.SetBool("Attack", true);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || _anim.GetCurrentAnimatorStateInfo(0).
            IsName("Attack2") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            _isAttack = true;
        }
        else
        {
            _isAttack = false;
        }
    }
}

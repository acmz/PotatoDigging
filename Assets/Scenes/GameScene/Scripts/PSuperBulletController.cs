﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSuperBulletController : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }

    //自弾表示限界
    private const float P_BULLET_DESTROY_POS_RIGHT = 10.0f;

    // Update is called once per frame
    void Update () {
        //画面外に出たら自分自身を破棄する
        if(transform.position.x > P_BULLET_DESTROY_POS_RIGHT) {
            Destroy(gameObject);
        }
	}

    //自弾のオブジェクト
    private Rigidbody2D _pSuperBulletBody;

    //自弾の移動スピード
    private const float P_BULLET_MOVE_SPEED = 15.0f;

    //自弾の発射角度
    private const float P_BULLET_MOVE_ANGLE_X = 1.0f;


    //自弾の発射
    public void PBulletShoot(Vector2 inVector2) {
        //自弾の発射方向を求める
        Vector2 startPos = inVector2;
        Vector2 endPos = new Vector2(inVector2.x + P_BULLET_MOVE_ANGLE_X, inVector2.y);
        Vector2 shootPos = endPos - startPos;

        //自弾Rigidbody取得
        this._pSuperBulletBody = GetComponent<Rigidbody2D>();

        //自弾の発射角度（敵弾の発射方向.normalized）とスピードを設定
        this._pSuperBulletBody.velocity = shootPos.normalized * P_BULLET_MOVE_SPEED;

        //自弾に力を加え、発射
        this._pSuperBulletBody.AddForce(shootPos.normalized);

        // 発射時のSEを鳴らす
        AudioSource se = gameObject.GetComponent<AudioSource>();
        se.Play();
    }
}

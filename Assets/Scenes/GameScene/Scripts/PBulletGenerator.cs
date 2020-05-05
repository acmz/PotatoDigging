using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBulletGenerator : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }

    public GameObject pBulletPrefab;
    private const float P_BULLET_POS_SET = 1.1f;
    private const float P_BULLET_SHOOT_INTERVAL = 0.1f;

    private float _pBulletShootChargeTime = 0f;

    // Update is called once per frame
    void Update () {

        this._pBulletShootChargeTime -= Time.deltaTime;

        // チャージ完了まで弾発射不可
        if(this._pBulletShootChargeTime > 0f) {
            return;
        }

        this._pBulletShootChargeTime = P_BULLET_SHOOT_INTERVAL;

        //スペースキーで弾発射
        if(Input.GetKey(KeyCode.Space)) {

            //GameDirectorと連携
            GameObject gameDirectorObj = GameObject.Find("Game_Director");

            //Wave開始処理中は弾発射不可
            if(gameDirectorObj.GetComponent<GameDirector>().IsWaveInit) {
                return;
            }

            //自機の位置を基に、自弾の発射位置を設定
            Vector2 playerPos = GameObject.Find("Player").transform.position;
            Vector2 pBulletVector2 = new Vector2(playerPos.x + P_BULLET_POS_SET, playerPos.y);

            //自弾を生成し、発射
            GameObject pBullet = Instantiate(this.pBulletPrefab, pBulletVector2, Quaternion.identity) as GameObject;
            pBullet.GetComponent<PBulletController>().PBulletShoot(playerPos);

        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritRockController : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // 岩の表示限界
    private const float ROCK_DESTROY_POS_RIGHT = 12.0f;
    private const float ROCK_DESTROY_POS_LEFT = -10.0f;
    private const float ROCK_DESTROY_POS_TOP = 6.0f;
    private const float ROCK_DESTROY_POS_BOTTOM = -6.0f;

    // Update is called once per frame
    void Update () {

        // 画面外に出たら自分自身を破棄する
        if(transform.position.x > ROCK_DESTROY_POS_RIGHT) {
            Destroy(gameObject);
        }

        if(transform.position.x < ROCK_DESTROY_POS_LEFT) {
            Destroy(gameObject);
        }

        if(transform.position.y > ROCK_DESTROY_POS_TOP) {
            Destroy(gameObject);
        }

        if(transform.position.y < ROCK_DESTROY_POS_BOTTOM) {
            Destroy(gameObject);
        }

    }

    // 点数
    private const int SCORE_POINT = 100;

    // 体力
    private int _lifePoint = 5;

    // 受けるダメージ
    private const int DAMAGE_NORMAL = 1;
    private const int DAMAGE_SPECIAL = 99;

    // プレイヤーの弾に当たったら、点数を加算して自分自身を消す
    private void OnTriggerEnter2D(Collider2D collision) {

        //Debug.Log("collision = " + collision.gameObject.name);

        // プレイヤーの弾に当たったら体力を減らす
        // 体力が0になったら消滅する
        string gameObjectName = collision.gameObject.name;
        if(gameObjectName == "P_Bullet_Prefab(Clone)") {
            this._lifePoint -= DAMAGE_NORMAL;
        }

        if(gameObjectName == "P_SuperBullet_Prefab(Clone)") {
            this._lifePoint -= DAMAGE_SPECIAL;
        }

        if(this._lifePoint <= 0) {
            this.ObjectDestruction();
        }

    }

    // 岩破壊時のパーティクル
    public GameObject rockDestroyParticle;

    private void ObjectDestruction() {

        // Directorと連携
        GameObject gameDirectorObj = GameObject.Find("Game_Director");

        // 倒されたこと、加算する点数をDirectorに伝える
        gameDirectorObj.GetComponent<GameDirector>().ScorePlus(SCORE_POINT);

        // 岩破壊時のパーティクル再生
        GameObject destroyParticleObject = Instantiate(this.rockDestroyParticle, this.transform.position, Quaternion.identity);
        destroyParticleObject.GetComponent<ParticleSystem>().Play();

        // 自分自身を消す
        Destroy(gameObject);

    }

    //岩のオブジェクト
    private Rigidbody2D _rockBody;

    //岩の生成・消滅位置（上下）
    private const float ROCK_POS_Y_MAX = 4.6f;
    private const float ROCK_POS_Y_MIN = -3.6f;

    //岩の移動スピード
    private const float ROCK_MOVE_SPEED = 1.0f;

    public void RockShoot(Vector2 inRockGeneratePos) {

        //岩の生成位置を求める
        Vector2 startPos = inRockGeneratePos;

        //岩の消滅位置を求める
        float rockDestroyPosX = Random.Range(ROCK_DESTROY_POS_LEFT, ROCK_DESTROY_POS_RIGHT);
        float rockDestroyPosY = Random.Range(ROCK_POS_Y_MIN, ROCK_POS_Y_MAX);
        Vector2 endPos = new Vector2(rockDestroyPosX, rockDestroyPosY);

        //岩の発射方向を求める
        Vector2 shootPos = endPos - startPos;

        //岩のRigidbody2D取得
        this._rockBody = GetComponent<Rigidbody2D>();

        //岩の発射角度とスピードを設定
        this._rockBody.velocity = shootPos.normalized * ROCK_MOVE_SPEED;

        //岩に力を加え、発射
        this._rockBody.AddForce(shootPos.normalized);

    }

}

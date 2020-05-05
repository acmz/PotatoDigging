using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetPotatoController : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // 芋の表示限界
    private const float POTATO_DESTROY_POS_RIGHT = 12.0f;
    private const float POTATO_DESTROY_POS_LEFT = -10.0f;
    private const float POTATO_DESTROY_POS_TOP = 6.0f;
    private const float POTATO_DESTROY_POS_BOTTOM = -6.0f;

    // Update is called once per frame
    void Update () {

        // 画面外に出たら自分自身を破棄する
        if(transform.position.x > POTATO_DESTROY_POS_RIGHT) {
            Destroy(gameObject);
        }

        if(transform.position.x < POTATO_DESTROY_POS_LEFT) {
            Destroy(gameObject);
        }

        if(transform.position.y > POTATO_DESTROY_POS_TOP) {
            Destroy(gameObject);
        }

        if(transform.position.y < POTATO_DESTROY_POS_BOTTOM) {
            Destroy(gameObject);
        }

    }

    // 体力
    private int _lifePoint = 1;

    // 受けるダメージ
    private const int DAMAGE_NORMAL = 1;
    private const int DAMAGE_SPECIAL = 99;

    // プレイヤーまたはプレイヤーの弾に当たったら、点数を加算して自分自身を消す
    private void OnTriggerEnter2D(Collider2D collision) {

        //Debug.Log("collision = " + collision.gameObject.name);

        string gameObjectName = collision.gameObject.name;
        // プレイヤーに当たったら消滅する
        if(gameObjectName == "Player") {
            this.ObjectDestruction(true);
            return;
        }

        // プレイヤーの弾に当たったら体力を減らす
        // 体力が0になったら消滅する
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

    // 点数
    private const int SCORE_POINT_DESTROY = 10;
    private const int SCORE_POINT_GET = 5000;

    // 芋取得SE
    public GameObject sweetPotatoSE;

    private void ObjectDestruction(bool inPlayerGet = false) {

        // Directorと連携
        GameObject gameDirectorObj = GameObject.Find("Game_Director");

        // プレイヤーに取得されたこと、加算する点数をDirectorに伝える
        if(inPlayerGet) {
            // 取得されたこと、加算する点数をDirectorに伝える
            gameDirectorObj.GetComponent<GameDirector>().ScorePlus(SCORE_POINT_GET);

            // 芋取得時のSEを鳴らす
            GameObject potatoSEObject = Instantiate(this.sweetPotatoSE, this.transform.position, Quaternion.identity);
            potatoSEObject.GetComponent<AudioSource>().Play();

            // 自分自身を消す
            Destroy(gameObject);

            return;
        }

        // 倒されたこと、加算する点数をDirectorに伝える
        gameDirectorObj.GetComponent<GameDirector>().ScorePlus(SCORE_POINT_DESTROY);

        // 自分自身を消す
        Destroy(gameObject);

    }

    // 芋のオブジェクト
    private Rigidbody2D _potatoBody;

    // 芋の生成・消滅位置（上下）
    private const float POTATO_POS_Y_MAX = 4.6f;
    private const float POTATO_POS_Y_MIN = -3.6f;

    // 芋の移動スピード
    private const float POTATO_MOVE_SPEED = 1.0f;

    public void PotatoShoot(Vector2 inPotatoGeneratePos) {

        // 芋の生成位置を求める
        Vector2 startPos = inPotatoGeneratePos;

        // 芋の消滅位置を求める
        float potatoDestroyPosY = Random.Range(POTATO_POS_Y_MIN, POTATO_POS_Y_MAX);
        Vector2 endPos = new Vector2(POTATO_DESTROY_POS_LEFT, potatoDestroyPosY);

        // 芋の発射方向を求める
        Vector2 shootPos = endPos - startPos;

        // 芋のRigidbody2D取得
        this._potatoBody = GetComponent<Rigidbody2D>();

        // 芋の発射角度とスピードを設定
        this._potatoBody.velocity = shootPos.normalized * POTATO_MOVE_SPEED;

        // 芋に力を加え、発射
        this._potatoBody.AddForce(shootPos.normalized);

    }

}

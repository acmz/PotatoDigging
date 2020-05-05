using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private GameObject _gameDirectorObj;

    // 自機のオブジェクト
    private Rigidbody2D _playerBody;

    // Use this for initialization
    void Start () {
        _playerBody = GetComponent<Rigidbody2D>();

        //GameDirectorと連携
        this._gameDirectorObj = GameObject.Find("Game_Director");
    }

    // 自機の移動スピード
    private const float PLAYER_MOVE_SPEED = 5.0f;

    // 自機の移動可能範囲
    private const float WINDOW_LIMIT_LEFT = -7.6f;
    private const float WINDOW_LIMIT_RIGHT = 7.3f;
    private const float WINDOW_LIMIT_TOP = 4.6f;
    private const float WINDOW_LIMIT_BOTTOM = -3.6f;

    // Update is called once per frame
    void Update () {
        // 自機の移動
        int keyLfRi = 0;
        int keyUpDw = 0;

        // 左右移動
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > WINDOW_LIMIT_LEFT) {
            keyLfRi = -1;
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < WINDOW_LIMIT_RIGHT) {
            keyLfRi = 1;
        }

        // 上下移動
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < WINDOW_LIMIT_TOP) {
            keyUpDw = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > WINDOW_LIMIT_BOTTOM) {
            keyUpDw = -1;
        }

        // 自機の移動制御
        Vector2 playerVector2;
        playerVector2.x = keyLfRi * PLAYER_MOVE_SPEED;
        playerVector2.y = keyUpDw * PLAYER_MOVE_SPEED;

        this._playerBody.velocity = playerVector2;
    }

    // プレイヤー死亡時のパーティクル
    public GameObject playerDestroyParticle;

    // プレイヤー死亡時のSE
    public GameObject playerDestroySE;

    // 岩に当たったら、自分自身を消す
    private void OnTriggerEnter2D(Collider2D collision) {
        bool playerDestroy = false;

        // 岩に当たったら消滅
        if(collision.gameObject.name == "Rock_Prefab(Clone)") {
            playerDestroy = true;
        }

        if(collision.gameObject.name == "Big_Rock_Prefab(Clone)") {
            playerDestroy = true;
        }

        if(collision.gameObject.name == "Sprit_Rock_Prefab(Clone)") {
            playerDestroy = true;
        }

        if(playerDestroy) {
            // プレイヤー死亡時のパーティクル再生
            GameObject destroyParticleObject =  Instantiate(this.playerDestroyParticle, this.transform.position, Quaternion.identity);
            destroyParticleObject.GetComponent<ParticleSystem>().Play();

            // プレイヤー死亡時のSEを鳴らす
            GameObject destroySEObject = Instantiate(this.playerDestroySE, this.transform.position, Quaternion.identity);
            destroySEObject.GetComponent<AudioSource>().Play();

            //自分自身を消す
            Destroy(gameObject);

            //GameDirectorに死亡したことを通知する
            this._gameDirectorObj.GetComponent<GameDirector>().PlayerIsDead();
        }
    }
}

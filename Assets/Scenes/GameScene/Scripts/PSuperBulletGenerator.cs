using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSuperBulletGenerator : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject pSuperBulletPrefab;
    private const float P_BULLET_POS_SET = 1.1f;

    // Update is called once per frame
    void Update()
    {
        // Fキーで弾発射
        if(Input.GetKeyDown(KeyCode.F)) {
            // GameDirectorと連携
            GameObject gameDirectorObj = GameObject.Find("Game_Director");

            // Wave開始処理中は弾発射不可
            if(gameDirectorObj.GetComponent<GameDirector>().IsWaveInit) {
                return;
            }

            // 弾が画面上に残っていたら発射不可
            GameObject oldPBullet = GameObject.Find("P_SuperBullet_Prefab(Clone)");
            if(oldPBullet != null) {
                return;
            }

            // エネルギーゲージが不足している場合、発射不可
            if(!gameDirectorObj.GetComponent<GameDirector>().IsEnergyMax) {
                return;
            }

            // 自機の位置を基に、自弾の発射位置を設定
            Vector2 playerPos = GameObject.Find("Player").transform.position;
            Vector2 pBulletVector2 = new Vector2(playerPos.x + P_BULLET_POS_SET, playerPos.y);

            // 自弾を生成し、発射
            GameObject pBullet = Instantiate(this.pSuperBulletPrefab, pBulletVector2, Quaternion.identity) as GameObject;
            pBullet.GetComponent<PSuperBulletController>().PBulletShoot(playerPos);

            // エネルギーをゼロにしてチャージモードに入る
            gameDirectorObj.GetComponent<GameDirector>().EnergyConsumption();
        }
    }
}

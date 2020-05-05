using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetPotatoGenerator : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}

    private const float POTATO_GENERATE_INTERVAL = 1.0f;
    private float _potatoGenerateTime = 1.0f;

    // Update is called once per frame
    void Update () {

        this._potatoGenerateTime -= Time.deltaTime;

        // 一定時間経過するまで次の芋を生成しない
        if(this._potatoGenerateTime > 0f) {
            return;
        }

        this._potatoGenerateTime = POTATO_GENERATE_INTERVAL;

        // 芋を生成する
        this.GeneratePotato();

    }

    public GameObject potatoPrefab;

    // 芋の生成位置（右）
    private const float POTATO_GENRATE_POS_X = 11.0f;

    // 芋の生成位置（上下）
    private const float POTATO_POS_Y_MAX = 4.6f;
    private const float POTATO_POS_Y_MIN = -3.6f;

    public void GeneratePotato() {

        //芋の出現位置を設定
        float potatoGeneratePosY = Random.Range(POTATO_POS_Y_MIN, POTATO_POS_Y_MAX);
        Vector2 potatoGeneratePos = new Vector2(POTATO_GENRATE_POS_X, potatoGeneratePosY);

        //芋を生成し飛ばす
        GameObject potatoObject = Instantiate(this.potatoPrefab, potatoGeneratePos, Quaternion.identity) as GameObject;
        potatoObject.GetComponent<SweetPotatoController>().PotatoShoot(potatoGeneratePos);

    }

}

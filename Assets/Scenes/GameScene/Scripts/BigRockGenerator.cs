using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRockGenerator : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}

    private const float ROCK_GENERATE_INTERVAL = 1.5f;
    private float _rockGenerateTime = 1.5f;

    // Update is called once per frame
    void Update () {

        this._rockGenerateTime -= Time.deltaTime;

        // 一定時間経過するまで次の岩を生成しない
        if(this._rockGenerateTime > 0f) {
            return;
        }

        this._rockGenerateTime = ROCK_GENERATE_INTERVAL;

        // 岩を生成する
        this.GenerateRock();

    }

    public GameObject bigRockPrefab;

    //岩の生成位置（右）
    private const float ROCK_GENRATE_POS_X = 10.0f;

    //岩の生成位置（上下）
    private const float ROCK_POS_Y_MAX = 4.6f;
    private const float ROCK_POS_Y_MIN = -3.6f;

    public void GenerateRock() {

        //岩の出現位置を設定
        float rockGeneratePosY = Random.Range(ROCK_POS_Y_MIN, ROCK_POS_Y_MAX);
        Vector2 rockGeneratePos = new Vector2(ROCK_GENRATE_POS_X, rockGeneratePosY);

        //岩を生成し飛ばす
        GameObject rockObject = Instantiate(this.bigRockPrefab, rockGeneratePos, Quaternion.identity) as GameObject;
        rockObject.GetComponent<BigRockController>().RockShoot(rockGeneratePos);

    }

}

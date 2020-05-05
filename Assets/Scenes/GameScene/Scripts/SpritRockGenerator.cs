using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritRockGenerator : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {

    }

    public GameObject spritRockPrefab;

    public void GenerateRock(Vector2 inGeneratePos) {

        // 岩を生成し飛ばす
        GameObject rockObject = Instantiate(this.spritRockPrefab, inGeneratePos, Quaternion.identity) as GameObject;
        rockObject.GetComponent<SpritRockController>().RockShoot(inGeneratePos);

    }

}

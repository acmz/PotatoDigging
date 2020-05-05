using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    private ParticleSystem _particle;

    // Start is called before the first frame update
    void Start()
    {
        this._particle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // パーティクル演出が終了した後、パーティクル用ゲームオブジェクトを削除
        if(this._particle.isStopped){ 
            Destroy(this.gameObject);
        }
    }
}

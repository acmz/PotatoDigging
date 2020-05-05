using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEController : MonoBehaviour
{

    private AudioSource _playerDestroySE;

    // Start is called before the first frame update
    void Start()
    {
        this._playerDestroySE = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // SEが終了した後、SE用ゲームオブジェクトを削除
        if(!this._playerDestroySE.isPlaying){ 
            Destroy(this.gameObject);
        }
    }
}

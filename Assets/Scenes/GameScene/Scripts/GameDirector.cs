using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour {

    // 残り時間UI
    private GameObject _timeLeftUI;

    // 残り時間
    private float _timeLeft = 0f;
    public float TimeLeft {
        get {
            return this._timeLeft;
        }
    }

    // スコアUI
    private GameObject _scoreNum;

    // エネルギーゲージUI
    private GameObject _energyGaugeUI;

    // エネルギー残量
    private float _energyLevel = 0f;
    public float EnergyLevel {
        get {
            return this._energyLevel;
        }
    }

    // エネルギー最大値
    private const float ENELGY_LEVEL_MAX = 1.0f;
    public bool IsEnergyMax {
        get {
            if(this._energyLevel >= ENELGY_LEVEL_MAX) {
                return true;
            }

            return false;
        }
    }

    // Wave開始コントロールフラグ
    private bool _isWaveInit = false;
    public bool IsWaveInit {
        get {
            return this._isWaveInit;
        }
    }

    // スコアボード表示フラグ
    private bool _displayScoreBord = false;

    // プレイヤー死亡フラグ
    private bool _playerIsDead = false;

    // Use this for initialization
    void Start () {
        // 残り時間UI取得、初期化
        this._timeLeftUI = GameObject.Find("TimeLeft");
        this.TimeLeftView(this._timeLeft);

        // スコアUI取得、初期化
        this._scoreNum = GameObject.Find("Score");
        this.ScoreReset();

        // エネルギーゲージUI取得、初期化
        this._energyGaugeUI = GameObject.Find("EnergyGauge");
        this.EnergyGaugeView(this._energyLevel);

        // Game開始
        this._isWaveInit = true;
        this._displayScoreBord = false;
        this._playerIsDead = false;
    }

    // スコア
    private int _score = 0;

    // ゲーム終了フラグ
    private bool _isGameEnd = false;
    public bool IsGameEnd {
        get {
            return this._isGameEnd;
        }
    }

    // Update is called once per frame
    void Update () {
        if(this._isWaveInit) {
            // Wave開始前
            // 残り時間とエネルギーのリセット
            StartCoroutine("InitPlayerStatus", 0.5f);
            return;
        }

        // 残り時間が0になったら、ゲーム終了。
        if(this._timeLeft <= 0f && !this._isGameEnd) {
            this._isGameEnd = true;
        }

        // 自機がやられたらゲーム終了
        if(this._playerIsDead && !this._isGameEnd){
            this._isGameEnd = true;
        }

        // Wave開始後
        if(!this._isGameEnd) {
            // ゲーム中は残り時間を減らす
            this.TimeLeftMinus();

            // 時間経過でエネルギー回復
            this.EnergyRecovery();

            return;
        }

        // ゲームが終わったらスコアボードを表示する
        if(!this._displayScoreBord) {
            this._displayScoreBord = true;
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(this._score);
        }
    }

    // コルーチン（処理停止）制御フラグ
    private bool _isSleeping = false;

    // プレイヤーステータス初期化（コルーチン）
    private IEnumerator InitPlayerStatus(float inTime) {
        // 初期化処理中に再度呼ばれたら、何もせずに抜ける
        if (this._isSleeping) {
            yield break;
        }

        this._isSleeping = true;

        yield return new WaitForSeconds(inTime);

        // エネルギーゲージ初期化
        this.EnergyGaugeInit();

        // 残り時間初期化
        this.TimeReset();

        yield return new WaitForSeconds(inTime);

        // Wave開始
        this._isWaveInit = false;

        this._isSleeping = false;
    }

    // スコア初期化
    public void ScoreReset() {
        // スコア初期化
        this._score = 0;

        // 表示更新
        this.ScoreView(this._score);
    }

    // スコア加算(敵撃破時)
    public void ScorePlus(int inScore) {
        // スコア加算
        this._score += inScore;

        // 表示更新
        this.ScoreView(this._score);
    }

    // スコア表示フォーマット
    private const string SCORE_MSG = "Score ";
    private const string SCORE_FORMAT = "{0:#,0}";

    // スコア表示
    private void ScoreView(int inViewScore) {
        // スコア表示を更新(3桁カンマ区切り)
        this._scoreNum.GetComponent<Text>().text = SCORE_MSG + string.Format(SCORE_FORMAT, inViewScore);
    }

    // 残り時間（MAX）
    private const float LIMIT_TIME = 30.0f;

    // 残り時間初期化
    public void TimeReset() {
        // 残り時間初期化
        this._timeLeft = LIMIT_TIME;

        // 表示更新
        this.TimeLeftView(this._timeLeft);
    }

    // 残り時間減算
    public void TimeLeftMinus() {
        // 残り時間が0の場合、何もしない。
        if(this._timeLeft == 0f) {
            return;
        }

        // 残り時間減算
        this._timeLeft -= Time.deltaTime;

        // 残り時間が0以下になったら、0固定にする
        if(this._timeLeft <= 0f) {
            this._timeLeft = 0f;
        }

        // 表示更新
        this.TimeLeftView(this._timeLeft);
    }

    // 残り時間フォーマット
    private const string TIME_LEFT_MSG = "Time : ";
    private const string TIME_LEFT_FORMAT = "F2";

    // 残り時間表示
    private void TimeLeftView(float inTimeLeft) {
        // 残り時間を表示（ss.ms）
        this._timeLeftUI.GetComponent<Text>().text = TIME_LEFT_MSG + inTimeLeft.ToString(TIME_LEFT_FORMAT);
    }

    // エネルギーゲージ初期化
    private void EnergyGaugeInit() {
        this.EnergyGaugeView(ENELGY_LEVEL_MAX);
    }

    // エネルギー消費
    public void EnergyConsumption() {
        // エネルギー残量を0にする
        this._energyLevel = 0f;
        this.EnergyGaugeView(this._energyLevel);
    }

    // エネルギー回復スピード
    private const float ENERGY_RECOVERY_SPEED = 0.5f;

    // エネルギー回復
    public void EnergyRecovery() {
        this._energyLevel += Time.deltaTime * ENERGY_RECOVERY_SPEED;

        // 満タンを超えたら満タンに戻す
        if(this._energyLevel > ENELGY_LEVEL_MAX) {
            this._energyLevel = ENELGY_LEVEL_MAX;
        }

        this.EnergyGaugeView(this._energyLevel);
    }

    // エネルギー残量表示
    private void EnergyGaugeView(float inEnergyLevel) {
        // エネルギーゲージの表示を変える
        this._energyGaugeUI.GetComponent<Image>().fillAmount = this._energyLevel;
    }

    // プレイヤー死亡判定
    public void PlayerIsDead() {
        this._playerIsDead = true;
    }
}

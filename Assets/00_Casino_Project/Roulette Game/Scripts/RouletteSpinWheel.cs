using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class RouletteSpinWheel : MonoBehaviour
{
    public static RouletteSpinWheel Inst;
     List<int> numbers = new List<int> { 0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23,
        10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26 };
    public GameObject ballRotator;
    public GameObject wheelRotator;
    float spinSpeed = 0f;
   float wheelSpeed=40;
    public Transform ballPositions;
    bool isStop = false;
    public GameObject numberContainer;
    public GameObject goContainer;
    public GameObject anchorPos;
    public GameObject ResultNumber_Source, ResultNumber_Destination;

    public Sprite[] sprites;
    //public int num2;

    public static Action StopBall;
    public static Action<int> StartSpin;
    Vector3 startPosition = Vector3.up * -83;
    Vector3 endPosition = Vector3.up * -46.79363f;
    public void OnEnable()
    {
        StartSpin += Play;
        StopBall += StopBallSpin;
        wheelSpeed = 40f;
    }
    public void OnDisable()
    {
        StartSpin -= Play;
        StopBall -= StopBallSpin;
    }
    void StopBallSpin()
    {
        isStop = true;
    }

    private void Start()
    {
        Inst = this;
       // iTween.MoveTo(GameObject.Find("NumberWinnerAnchor"), iTween.Hash("position", ResultNumber_Destination2.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        this.GetComponent<Transform>().position = GameObject.Find("SpinWheel2D_Position").transform.GetComponent<RectTransform>().position;
        //Play(Random.Range(0, 37));
    }

    public void Play(int num2)
    {
        ballRotator.transform.GetChild(0).transform.localPosition = startPosition;
        isStop = false;
        Debug.Log("Target number => "+num2);
        StopCoroutine(RoulleteGame(num2));
        StartCoroutine(RoulleteGame(num2));
    }

    void FixedUpdate()
    {
        //return;
        wheelRotator.transform.Rotate(Vector3.forward * wheelSpeed * Time.deltaTime);
        if (!isStop)
            ballRotator.transform.Rotate(Vector3.back * spinSpeed * 3 * Time.deltaTime);
    }

    int num = 0;

   GameObject FindObject(int index)
    {
        int ind=numbers.FindIndex(x => x == index);

        if (numbers[ind] == numbers[0])
            ind = numbers[numbers.Count - 1];
        else
            ind = numbers[ind-1];
        return ballPositions.GetChild(ind).gameObject;
    }
    IEnumerator RoulleteGame(int num)
    {
        //Roullate_SoundManager.Inst.PlaySFX_Others(37);
        this.num = num;
        spinSpeed = 180f;
        wheelSpeed= 40f;
        ballRotator.transform.GetChild(0).transform.localPosition = Vector3.up * -83f;
        yield return new WaitForSeconds(3.25f);
        spinSpeed = 180f;
        ballRotator.transform.GetChild(0).transform.localPosition = Vector3.up * -73f;
        yield return new WaitForSeconds(1f);
        ballRotator.transform.GetChild(0).transform.localPosition = Vector3.up * -53f;
        spinSpeed = 140f;
        yield return new WaitForSeconds(0.5f);
        BallCollider.TargetObject?.Invoke(ballPositions.GetChild(num).gameObject, FindObject(num));
        ballRotator.transform.GetChild(0).transform.localPosition = endPosition;
        spinSpeed = 75f;
        yield return new WaitForSeconds(2f);
        spinSpeed = 30f;
        ballRotator.transform.GetChild(0).position = ballPositions.GetChild(num).position;
        // Ball position
        yield return new WaitForSeconds(0.3f);
        //wheelSpeed = 0;
        StartCoroutine(FxNumber());
        yield return new WaitForSeconds(0.5f);

        //Finish round
        spinSpeed = 0f;
    }

    IEnumerator FxNumber()
    {
        anchorPos.GetComponent<SpriteRenderer>().sprite = sprites[num];
        //goContainer.SetActive(true);
        iTween.MoveTo(GameObject.Find("NumberWinnerAnchor"), iTween.Hash("position", ResultNumber_Destination.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        Roullate_Manager.Inst.WIN_NO_SOUND(num.ToString());
        yield return new WaitForSeconds(1.7f);
        iTween.MoveTo(GameObject.Find("NumberWinnerAnchor"), iTween.Hash("position", ResultNumber_Source.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        //goContainer.SetActive(false);
    }

    public void SET_FIRST_BALL(int num)
    {
        ballRotator.transform.GetChild(0).position = ballPositions.GetChild(num).position;
    }
}

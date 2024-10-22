using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    private static CurrencyController _instance;
    [SerializeField] private GameObject[] currencyPrefabs;
    public static CurrencyController Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private GameObject GetCoinPrefabByAmount(int amt)
    {
        if (amt <= 10)
        {
            return currencyPrefabs[0];
        }
        else if (amt <= 50)
        {
            return currencyPrefabs[1];
        }
        else if (amt <= 100)
        {
            return currencyPrefabs[2];
        }
        else if (amt <= 1000)
        {
            return currencyPrefabs[3];
        }
        else if (amt <= 5000)
        {
            return currencyPrefabs[4];
        }
        else
        {
            return currencyPrefabs[4];
        }
    }

    public void SpawnChipAndBet(int betAmount = 1, Transform fromT = null, Transform toT = null, Transform pOnComplete = null, float duration = 0.5f, float delay = 0f)
    {
        GameObject chipsGm = Instantiate(GetCoinPrefabByAmount(betAmount), fromT);
        chipsGm.transform.localScale = Vector3.one * 0.2f;
        float sclDuration = 0.2f;
        Sequence chipSeq = DOTween.Sequence();
        chipSeq.SetDelay(delay);
        chipSeq.Join(chipsGm.transform.DOScale(1f, sclDuration).SetEase(Ease.Linear));
        chipSeq.Join(chipsGm.transform.DOMove(toT.position, duration).SetDelay(sclDuration / 2f).SetEase(Ease.InOutSine));
        //chipSeq.Join(chipsGm.transform.DOJump(pOnComplete.position,Random.Range(-5,5),1, duration).SetDelay(sclDuration/4f).SetEase(Ease.InOutSine));
        chipSeq.OnComplete(() =>
        {
            chipsGm.transform.SetParent(pOnComplete);
        });
    }


    public void SpawnChipAndBet(int betAmount = 1, Vector3 targetPOs = new Vector3(), float duration = 0.5f, float delay = 0f, Transform spawnParent = null, Transform targetParent = null)
    {
        GameObject chipsGm = Instantiate(GetCoinPrefabByAmount(betAmount), spawnParent);
        chipsGm.transform.localScale = Vector3.one * 0.2f;
        float sclDuration = 0.2f;
        Sequence chipSeq = DOTween.Sequence();
        chipSeq.SetDelay(delay);
        chipSeq.Join(chipsGm.transform.DOScale(1f, sclDuration).SetEase(Ease.Linear));
        chipSeq.Join(chipsGm.transform.DOMove(targetPOs, duration).SetDelay(sclDuration / 2f).SetEase(Ease.InOutSine));
        //chipSeq.Join(chipsGm.transform.DOJump(pOnComplete.position,Random.Range(-5,5),1, duration).SetDelay(sclDuration/4f).SetEase(Ease.InOutSine));
        chipSeq.OnComplete(() =>
        {
            chipsGm.transform.SetParent(targetParent);
        });
    }

    public Vector3 GetRandomPointInBox(Transform box, float hBounds, float vBounds,Vector3 offset=new Vector3())
    {
        return box.transform.position + new Vector3(Random.Range(hBounds * -1, hBounds), Random.Range(vBounds * -1, vBounds), 0f)+offset;
    }

    public void MoveChipTo(Transform chipsGm, Vector3 toT, Transform pOnComplete = null, float duration = 0.5f, float delay = 0f, bool destroy = false)
    {
        StartCoroutine(MoveChipCoroutine(chipsGm, toT, pOnComplete, duration, delay, destroy));
    }
    IEnumerator MoveChipCoroutine(Transform chipsGm, Vector3 toT, Transform pOnComplete = null, float duration = 0.5f, float delay = 0f, bool destroy = false)
    {
        yield return new WaitForEndOfFrame();
        chipsGm.transform.SetParent(pOnComplete);
        chipsGm.DOJump(toT, 3, 1, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            if (destroy)
            {
                Destroy(chipsGm.gameObject);
            }
            else
            {
                chipsGm.transform.SetParent(pOnComplete);
            }
        });
    }


    public void MoveChipToWinCardAndUser(Transform chipsGm, Vector3 winPos, Transform winT, Vector3 toT, Transform pOnComplete = null, float duration = 0.5f, float delay = 0f, bool destroy = false)
    {
        StartCoroutine(MoveChipToWinCardAndUserCoroutine(chipsGm, winPos, winT, toT, pOnComplete, duration, delay, destroy));
    }
    IEnumerator MoveChipToWinCardAndUserCoroutine(Transform chipsGm, Vector3 winPos, Transform winT, Vector3 toT, Transform pOnComplete = null, float duration = 0.5f, float delay = 0f, bool destroy = false)
    {
        yield return new WaitForEndOfFrame();

        Sequence s = DOTween.Sequence();

        if (winT.transform != chipsGm.parent)
        {
            s.SetDelay(delay);
            s.Append(chipsGm.DOJump(winPos, 3, 1, duration*4f).SetEase(Ease.OutBack));
        }
        else {
            s.SetDelay(delay+duration*4);
        }

        chipsGm.transform.SetParent(pOnComplete);

        s.Append(chipsGm.DOJump(toT, Random.Range(-5,5), 1, duration+Random.Range(-0.2f,0.2f)).SetDelay(Random.Range(0f,0.5f)+1f).SetEase(Ease.InOutSine));
        s.OnComplete(() =>
        {
            if (destroy)
            {
                Destroy(chipsGm.gameObject);
            }
            else
            {
                chipsGm.transform.SetParent(pOnComplete);
            }
        });
    }



}

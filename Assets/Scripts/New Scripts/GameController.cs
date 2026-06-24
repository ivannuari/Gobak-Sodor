using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameObject indicatorObject;

    public Penyerang activePenyerang;
    public Penjaga[] activePenjaga;
    public List<Penyerang> allPenyerang = new List<Penyerang>();

    [SerializeField] private int poin = 0;
    [SerializeField] private int totalPenyerang = 5;

    private int currentPenyerang = 0;

    public Transform checkpoint01;
    public Transform checkpoint02;

    public LayerMask penyerangLayer;
    public LayerMask penjagaLayer;
    public LayerMask groundLayer;

    public event Action<string> OnNotifierShowed;
    public event Action<int> OnScoreUpdated;
    public event Action OnGameOver;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(GameBehaviour.Instance.characterType == CharacterType.Penjaga)
        {
            foreach (var item in activePenjaga)
            {
                item.Activate();
            }
            StartCoroutine(StartAIPenyerang());
        }
    }

    public void StartNewPenyerang()
    {
        StartCoroutine(StartAIPenyerang());

    }

    private IEnumerator StartAIPenyerang()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f,3f));
        activePenyerang = allPenyerang[currentPenyerang];
        activePenyerang.SetAsActiveAi();
        currentPenyerang++;
    }

    public void ShowNotification(string message)
    {
        OnNotifierShowed?.Invoke(message);
    }

    public void AddScore()
    {
        poin += 10;
        totalPenyerang--;
        OnScoreUpdated?.Invoke(poin);
        GameBehaviour.Instance.PlaySound("Victory");

        CheckGameOver();
    }

    public int GetPoin()
    {
        return poin;
    }

    private void Update()
    {
        CharacterType CharaType = GameBehaviour.Instance.characterType;
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (CharaType == CharacterType.Penyerang)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, penyerangLayer))
                {
                    if (hit.transform.TryGetComponent(out Penyerang _ch))
                    {
                        if (activePenyerang == null)
                        {
                            _ch.Activate();
                            activePenyerang = _ch;
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    if (activePenyerang != null)
                    {
                        Vector3 pos = hit.point;
                        ActivateIndicator(hit.point);
                        activePenyerang.MoveTo(pos);
                    }
                }
            }

            if(CharaType == CharacterType.Penjaga)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    Vector3 pos = hit.point;
                    ActivateIndicator(hit.point);
                    foreach (var item in activePenjaga)
                    {
                        item.MoveTo(hit.point);
                    }
                }
            }
        }
    }

    private void ActivateIndicator(Vector3 point)
    {
        point.y = 0.1f;
        indicatorObject.transform.position = point;

        indicatorObject.GetComponent<Animator>().Play("Play");
    }

    public void CatchPenyerang()
    {
        Destroy(activePenyerang.gameObject);
        activePenyerang = null;
        SubstractPoin();

        ShowNotification("Tertangkap!");
        GameBehaviour.Instance.PlaySound("Lose");
    }

    public void SubstractPoin()
    {
        totalPenyerang--;
        poin -= 10;
        
        OnScoreUpdated?.Invoke(poin);
        CheckGameOver();
    }

    public void CatchOpponentPenyerang()
    {
        Destroy(activePenyerang.gameObject);
        activePenyerang = null;
        totalPenyerang--;

        poin += 10;

        OnScoreUpdated?.Invoke(poin);
        ShowNotification("Berhasil!");

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (totalPenyerang < 1)
        {
            OnGameOver?.Invoke();
        }
        else
        {
            if(GameBehaviour.Instance.characterType == CharacterType.Penjaga)
            {
                StartNewPenyerang();
            }
        }
    }
}

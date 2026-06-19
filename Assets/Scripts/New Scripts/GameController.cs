using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Penyerang activePenyerang;
    public Penjaga activePenjaga;

    [SerializeField] private int poin = 0;

    public LayerMask penyerangLayer;
    public LayerMask penjagaLayer;
    public LayerMask groundLayer;

    public event Action<string> OnNotifierShowed;
    public event Action<int> OnScoreUpdated;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowNotification(string message)
    {
        OnNotifierShowed?.Invoke(message);
    }

    public void AddScore()
    {
        poin += 10;
        OnScoreUpdated?.Invoke(poin);
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
                        else
                        {
                            // logika lain jika sudah ada activePenyerang
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    if (activePenyerang != null)
                    {
                        // Gunakan hit.point, bukan ScreenToWorldPoint
                        Vector3 pos = hit.point;
                        activePenyerang.MoveTo(pos);
                    }
                }
            }
        }
    }
}

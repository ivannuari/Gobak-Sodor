using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public CheckpointType type;

    private void OnTriggerEnter(Collider other)
    {
        var _controller = GameController.Instance;
        bool isCollide = other.CompareTag("Penyerang");

        if (isCollide)
        {
            bool isPenyerang = GameBehaviour.Instance.characterType == CharacterType.Penyerang;
            if (isPenyerang)
            {
                Penyerang _penyerang = other.GetComponent<Penyerang>();
                if (_penyerang == null) return;
                
                switch (type)
                {
                    case CheckpointType.Checkpoint:
                        _penyerang.checkpoint = true;
                        _controller.ShowNotification("Saatnya kembali ke awal");
                        break;
                    case CheckpointType.Finish:
                        if(_penyerang.checkpoint)
                        {
                            _controller.activePenyerang = null;
                            _controller.ShowNotification("Berhasil!!");
                            _controller.AddScore();
                            Destroy(other.gameObject);
                        }
                        break;
                }
            }
            else
            {
                Penyerang _penyerang = other.GetComponent<Penyerang>();
                if (_penyerang == null) return;

                switch (type)
                {
                    case CheckpointType.Checkpoint:
                        _penyerang.checkpoint = true;
                        //_controller.ShowNotification("Saatnya kembali ke awal");
                        break;
                    case CheckpointType.Finish:
                        if (_penyerang.checkpoint)
                        {
                            _controller.activePenyerang = null;
                            //_controller.ShowNotification("Berhasil!!");
                            //_controller.AddScore();
                            GameController.Instance.StartNewPenyerang();
                            Destroy(other.gameObject);
                        }
                        break;
                }
            }
        }
    }
}

[System.Serializable]
public enum CheckpointType
{
    Checkpoint,
    Finish
}
using UnityEngine;

public class ColorWheelPanel : MonoBehaviour
{
    private bool _open = false;

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Toggle() {
        _open = !_open;
        gameObject.SetActive(_open);
    }
}

using UnityEngine;
public class UiWindow : MonoBehaviour {
    private bool _open = true;
    public void Open() {
        _open = true;
        gameObject.SetActive(_open);
    }

    public void Close() {
        _open = false;
        gameObject.SetActive(_open);
    }

    public void Toggle() {
        _open = !_open;
        gameObject.SetActive(_open);
    }
}
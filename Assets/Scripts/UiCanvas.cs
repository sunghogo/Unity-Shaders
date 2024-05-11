using UnityEngine;

public class UiCanvas : MonoBehaviour

{
    private PropertyBox[] _propertyBoxes;

    // Start is called before the first frame update
    void Start()
    {
        _propertyBoxes = GetComponentsInChildren<PropertyBox>();
        foreach (var box in _propertyBoxes) {
                Debug.Log(box.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _propertyBoxes.Length; i++) {
            var initialBox = _propertyBoxes[i];
            if (initialBox.Opened && !initialBox.Adjusted) {
                for (int j = i + 1; j < _propertyBoxes.Length; j++) {
                    var subsequentBox = _propertyBoxes[j];
                    subsequentBox.transform.Translate(0, -initialBox.OpenedHeight, 0);
                }
                initialBox.Adjusted = true;
            } else if (!initialBox.Opened && initialBox.Adjusted) {
                for (int j = i + 1; j < _propertyBoxes.Length; j++) {
                    var subsequentBox = _propertyBoxes[j];
                    subsequentBox.transform.Translate(0, initialBox.OpenedHeight, 0);
                }
                initialBox.Adjusted = false;
            }
        }
    }
}

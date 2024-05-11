using UnityEngine;
using TMPro;

public class UiCanvas : MonoBehaviour

{
    public Shaders Shaders;
    public TextMeshProUGUI ShaderTitle;
    private PropertyBox[] _propertyBoxes;

    // Start is called before the first frame update
    void Start()
    {
        Shaders = FindObjectOfType<Shaders>();
        ShaderTitle = GetComponentInChildren<TextMeshProUGUI>();

        UpdatePropertyBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        GeneratePropertyBoxes();
        UpdateCascadingPropertyBoxes();
    }

    private void GeneratePropertyBoxes() {
        foreach (var name in Shaders.TestMaterial.GetPropertyNames(MaterialPropertyType.Vector)) {
            Debug.Log(name);
        };

    }

    private void UpdatePropertyBoxes() {
        _propertyBoxes = GetComponentsInChildren<PropertyBox>();
        foreach (var box in _propertyBoxes) {
                Debug.Log(box.gameObject.name);
        }
    }

    private void UpdateCascadingPropertyBoxes() {
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

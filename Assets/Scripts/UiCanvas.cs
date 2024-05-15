using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UiCanvas : MonoBehaviour

{
    public PropertyBox _propertyBoxZeroSliderPrefab;
    public PropertyBox _propertyBoxOneSliderPrefab;
    public PropertyBox _propertyBoxTwoSlidersPrefab;
    private Canvas _canvas;
    private Shaders _shaders;
    private TextMeshProUGUI _shaderTitleTmp;
    private PropertyBox[] _propertyBoxes;
    private bool _shadersChanged = true; 
    private Vector3 _propertyBoxPosition = new Vector3(240, -240, 0);
    private Vector3 _propertyBoxOffset = new Vector3(0, -70, 0);

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _shaders = FindObjectOfType<Shaders>();
        _shaderTitleTmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_shadersChanged) {
            ClearPropertyBoxes();
            GeneratePropertyBoxes();
            UpdatePropertyBoxes();
            _shadersChanged = false;
        }
        UpdateCascadingPropertyBoxes();
    }

    private void GeneratePropertyBoxes() {
        foreach (var name in _shaders.TestMaterial.GetPropertyNames(MaterialPropertyType.Vector)) {
            string propertyTitle = ParsePropertyTitle(name);
            switch (name) {
                case "_materialColor":
                    GeneratePropertyBoxZeroSlider(propertyTitle, _propertyBoxPosition);
                    break;
                case "_ambientIntensity":
                    GeneratePropertyBoxOneSlider(propertyTitle, _propertyBoxPosition);
                    break;
                case "_diffuseIntensity":
                    GeneratePropertyBoxOneSlider(propertyTitle, _propertyBoxPosition);
                    break;
                case "_rimIntensity":
                    GeneratePropertyBoxTwoSliders(propertyTitle, _propertyBoxPosition);
                    break;
                case "_specularIntensity":
                    GeneratePropertyBoxTwoSliders(propertyTitle, _propertyBoxPosition);
                    break;
            }
        };
        foreach (var name in _shaders.TestMaterial.GetPropertyNames(MaterialPropertyType.Float)) {
            // Debug.Log(name);
        };
    }

    private string ParsePropertyTitle(string name) {
        string splitText = name.Substring(1).SplitWords(' ').Split(' ')[0];
        return char.ToUpper(splitText[0]) + splitText.Substring(1);
    }

    private string ParsePropertyName(string name) {
        string splitText = name.Substring(1).SplitWords(' ');
        return char.ToUpper(splitText[0]) + splitText.Substring(1);
    }

    private void GeneratePropertyBoxZeroSlider(string propertyTitle, Vector3 position) {
        GameObject instance = Instantiate(_propertyBoxZeroSliderPrefab.gameObject);
        instance.transform.SetParent(_canvas.transform, false);
        instance.name = propertyTitle;

        var rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition3D = position;

        PropertyBox propertyBox = instance.GetComponent<PropertyBox>();
        propertyBox.PropertyTitleText = propertyTitle;
        propertyBox.ColorPropertyText = $"{propertyTitle} Color";

        _propertyBoxPosition += _propertyBoxOffset;
    }

    private void GeneratePropertyBoxOneSlider(string propertyTitle, Vector3 position) {
        GameObject instance = Instantiate(_propertyBoxOneSliderPrefab.gameObject);
        instance.transform.SetParent(_canvas.transform, false);
        instance.name = propertyTitle;

        var rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition3D = position;

        PropertyBox propertyBox = instance.GetComponent<PropertyBox>();
        propertyBox.PropertyTitleText = propertyTitle;
        propertyBox.ColorPropertyText = $"{propertyTitle} Color";
        propertyBox.SliderOnePropertyText = $"{propertyTitle} Strength";

        _propertyBoxPosition += _propertyBoxOffset;
    }

    private void GeneratePropertyBoxTwoSliders(string propertyTitle, Vector3 position) {
        GameObject instance = Instantiate(_propertyBoxTwoSlidersPrefab.gameObject);
        instance.transform.SetParent(_canvas.transform, false);
        instance.name = propertyTitle;

        var rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition3D = position;

        PropertyBox propertyBox = instance.GetComponent<PropertyBox>();
        propertyBox.PropertyTitleText = propertyTitle;
        propertyBox.ColorPropertyText = $"{propertyTitle} Color";
        propertyBox.SliderOnePropertyText = $"{propertyTitle} Strength";
        propertyBox.SliderTwoPropertyText = $"{propertyTitle} Radius";

        _propertyBoxPosition += _propertyBoxOffset;
    }

    private void UpdatePropertyBoxes() {
        _propertyBoxes = GetComponentsInChildren<PropertyBox>();
    }

    private void ClearPropertyBoxes() {
        if (_propertyBoxes is null) return;
        for (int i = 0; i < _propertyBoxes.Length; i ++) DestroyImmediate(_propertyBoxes[i].gameObject); // DestroyImmediate otherwise game objects persist in memory
        _propertyBoxes = null;
        _propertyBoxPosition = new Vector3(240, -240, 0);
    }

    private void UpdateCascadingPropertyBoxes() {
        if (_propertyBoxes is null) return;
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

    public void ShadersChanged() {
        _shadersChanged = true;
    }
}

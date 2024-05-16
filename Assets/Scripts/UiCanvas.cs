using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UiCanvas : MonoBehaviour

{
    public PropertyBox _propertyBoxZeroSliderPrefab;
    public PropertyBox _propertyBoxOneSliderPrefab;
    public PropertyBox _propertyBoxTwoSlidersPrefab;
    private Canvas _canvas;
    private Shaders _shaders;
    private TextMeshProUGUI _shaderTitleTmp;
    private Dictionary<string, PropertyBox> _propertyBoxes;
    private List<PropertyBox> _propertyBoxesList;
    private bool _shadersChanged; 
    private Vector3 _initialPropertyBoxPosition;
    [SerializeField] private Vector3 _propertyBoxPosition;
    private Vector3 _propertyBoxOffset;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _shaders = FindObjectOfType<Shaders>();
        _shaderTitleTmp = GetComponentInChildren<TextMeshProUGUI>();

        _propertyBoxes = new Dictionary<string, PropertyBox>();
        _propertyBoxesList = new List<PropertyBox>();
        UpdatePropertyBoxes();

        _shadersChanged = true;
        _initialPropertyBoxPosition = new Vector3(240, -240, 0);
        _propertyBoxPosition = _initialPropertyBoxPosition;
        _propertyBoxOffset = new Vector3(0, -70, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_shadersChanged) {
            CloseCascadingPropertyBoxes();
            UpdateCascadingPropertyBoxes();
            DeactivatePropertyBoxes();
            ResetBoxPositions();
            GeneratePropertyBoxes();
            UpdatePropertyBoxes();
            _shadersChanged = false;
        }
        UpdateCascadingPropertyBoxes();
    }

    private void GeneratePropertyBoxes() {
        // Property names are retrieved sequentially in the same order as in the shader so property boxes will always appear int the order
        foreach (var name in _shaders.TestMaterial.GetPropertyNames(MaterialPropertyType.Vector)) {
            if (name.Split('_').Length > 2) continue; // Ignore default Unity Shader properties with the same property title
            string propertyTitle = ParsePropertyTitle(name);
            if (!_propertyBoxes.ContainsKey(propertyTitle)) {
                switch (name) {
                    case "_materialColor":
                        _propertyBoxesList.Add(GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition));
                        break;
                    case "_ambientIntensity":
                        _propertyBoxesList.Add(GeneratePropertyBox(1, propertyTitle, _propertyBoxPosition));
                        break;
                    case "_diffuseIntensity":
                        _propertyBoxesList.Add(GeneratePropertyBox(1, propertyTitle, _propertyBoxPosition));
                        break;
                    case "_rimIntensity":
                        _propertyBoxesList.Add(GeneratePropertyBox(2, propertyTitle, _propertyBoxPosition));
                        break;
                    case "_specularIntensity":
                        _propertyBoxesList.Add(GeneratePropertyBox(2, propertyTitle, _propertyBoxPosition));
                        break;
                }
            } else {
                var box = _propertyBoxes[propertyTitle];
                ActivatePropertyBox(box);
                var rectTransform = box.GetComponent<RectTransform>();
                rectTransform.anchoredPosition3D = _propertyBoxPosition;
                _propertyBoxPosition += _propertyBoxOffset;
            }
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

    private PropertyBox GeneratePropertyBox(int numSliders, string propertyTitle, Vector3 position) {
        GameObject instance;
        switch (numSliders) {
            case 0:
                instance = Instantiate(_propertyBoxZeroSliderPrefab.gameObject);
                break;
            case 1:
                instance = Instantiate(_propertyBoxOneSliderPrefab.gameObject);
                break;
            case 2:
                instance = Instantiate(_propertyBoxTwoSlidersPrefab.gameObject);
                break;
            default:
                return null;
        }

        instance.transform.SetParent(_canvas.transform, false);
        instance.name = propertyTitle;

        var rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition3D = position;

        PropertyBox propertyBox = instance.GetComponent<PropertyBox>();
        propertyBox.PropertyTitleText = propertyTitle;
        propertyBox.ColorPropertyText = $"{propertyTitle} Color";
        if (numSliders > 0 ) propertyBox.SliderOnePropertyText = $"{propertyTitle} Strength";
        if (numSliders > 1) propertyBox.SliderTwoPropertyText = $"{propertyTitle} Radius";

        _propertyBoxPosition += _propertyBoxOffset;
        return propertyBox;
    }

    private void UpdatePropertyBoxes() {
        var propertyBoxes = GetComponentsInChildren<PropertyBox>();
        foreach (var box in propertyBoxes) {
            _propertyBoxes[box.name] = box;
        }
    }

    private void DeactivatePropertyBoxes() {
        foreach (var box in _propertyBoxes.Values) {
            box.gameObject.SetActive(false);
        }
    }
    
    private void ActivatePropertyBox(PropertyBox box) {
        box.gameObject.SetActive(true);
    }

    private void ResetBoxPositions() {
        _propertyBoxPosition = _initialPropertyBoxPosition;
    }

    private void CloseCascadingPropertyBoxes() {
        for (int i = 0; i < _propertyBoxesList.Count; i++) _propertyBoxesList[i].Close();
    }

    private void UpdateCascadingPropertyBoxes() {
        for (int i = 0; i < _propertyBoxesList.Count; i++) {
            var initialBox = _propertyBoxesList[i];
            if (initialBox.Opened && !initialBox.Adjusted) {
                for (int j = i + 1; j < _propertyBoxesList.Count; j++) {
                    var subsequentBox = _propertyBoxesList[j];
                    subsequentBox.transform.Translate(0, -initialBox.OpenedHeight, 0);
                }
                initialBox.Adjusted = true;
            } else if (!initialBox.Opened && initialBox.Adjusted) {
                for (int j = i + 1; j < _propertyBoxesList.Count; j++) {
                    var subsequentBox = _propertyBoxesList[j];
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

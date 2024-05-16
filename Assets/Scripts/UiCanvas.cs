using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;

public class UiCanvas : MonoBehaviour
{
    public PropertyBox PropertyBoxZeroSliderPrefab;
    public PropertyBox PropertyBoxOneSliderPrefab;
    public PropertyBox PropertyBoxTwoSlidersPrefab;
    private TextMeshProUGUI _shaderTitleTmp;
    private Canvas _canvas;
    private Shaders _shaders;
    private Dictionary<string, PropertyBox> _propertyBoxes;
    private List<PropertyBox> _propertyBoxesList;
    private Vector3 _initialPropertyBoxPosition;
    [SerializeField] private Vector3 _propertyBoxPosition;
    private Vector3 _propertyBoxOffset;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _shaders = FindObjectOfType<Shaders>();
        _shaderTitleTmp = GetComponentInChildren<TextMeshProUGUI>();

        _propertyBoxes = new Dictionary<string, PropertyBox>();
        _propertyBoxesList = new List<PropertyBox>();

        _initialPropertyBoxPosition = new Vector3(240, -240, 0);
        _propertyBoxPosition = _initialPropertyBoxPosition;
        _propertyBoxOffset = new Vector3(0, -70, 0);

        UpdateUI();
    }

    void LateUpdate()
    {
        DropDownCascadePropertyBoxes();
    }

    public void UpdateUI() {
        UpdateShaderTitle();
        CloseAllPropertyBoxes();
        DropDownCascadePropertyBoxes();
        DeactivatePropertyBoxes();
        ResetBoxPositions();
        GenerateUpdatePropertyBoxes();
        ReopenAllPreviouslyOpenPropertyBoxes();
        DropDownCascadePropertyBoxes();
    }

    private void GenerateUpdatePropertyBoxes() {
        // Property names are retrieved sequentially in the same order as in the shader so property boxes will always appear int the order
        foreach (var name in _shaders.TestMaterial.GetPropertyNames(MaterialPropertyType.Vector)) {
            if (name.Split('_').Length > 2) continue; // Ignore default Unity Shader properties with the same property title
            string propertyTitle = ParsePropertyTitle(name);
            if (!_propertyBoxes.ContainsKey(propertyTitle)) {
                PropertyBox box = null;
                
                switch (name) {
                    case "_materialColor":
                        box = GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_ambientIntensity":
                        box = GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_diffuseIntensity":
                        box = GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_rimIntensity":
                        box = GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition);
                        break;
                    case "_specularIntensity":
                        box = GeneratePropertyBox(0, propertyTitle, _propertyBoxPosition);
                        break;
                }

                if (box is not null) {
                    _propertyBoxesList.Add(box);
                    _propertyBoxes[box.name] = box;
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

    private void UpdateShaderTitle() {
        _shaderTitleTmp.text = $"{ParseShaderName(_shaders.GetCurrentShader().name)}";
    }

    private string ParseShaderName(string shaderName) {
        string shaderTitle = shaderName.Split('/').ElementAtOrDefault(1);
        shaderTitle = shaderTitle?.SplitWords(' ') ?? shaderName;
        if (shaderTitle.Contains('-')) shaderTitle = shaderTitle.Substring(0, shaderTitle.IndexOf('-') + 1) + shaderTitle.Substring(shaderTitle.IndexOf('-') + 2, shaderTitle.Length - shaderTitle.IndexOf('-') - 2);
        return shaderTitle;
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
                instance = Instantiate(PropertyBoxZeroSliderPrefab.gameObject);
                break;
            case 1:
                instance = Instantiate(PropertyBoxOneSliderPrefab.gameObject);
                break;
            case 2:
                instance = Instantiate(PropertyBoxTwoSlidersPrefab.gameObject);
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

    private void CloseAllPropertyBoxes() {
        for (int i = 0; i < _propertyBoxesList.Count; i++) {
            var box = _propertyBoxesList[i];
            if (box.gameObject.activeSelf) box.PreviouslyOpened = box.Opened;
            box.Close();
       }
    }

    private void ReopenAllPreviouslyOpenPropertyBoxes() {
        for (int i = 0; i < _propertyBoxesList.Count; i++) {
            var box = _propertyBoxesList[i];
            if (box.gameObject.activeSelf && box.PreviouslyOpened) box.Open();
       }
    }

    // Drop down cascading only works when all property boxes are initially closed due to hard position resets when cycling shaders
    private void DropDownCascadePropertyBoxes() {
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
}

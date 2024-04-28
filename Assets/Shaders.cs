using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
public class Shaders : MonoBehaviour
{
    // Add the following shaders and material in the Inspector
    [SerializeField] private Shader _noLighting;
    [SerializeField] private Shader _ambientLighting;
    [SerializeField] private Shader _lambertDiffusionLighting;
    [SerializeField] private Shader _rimLighting;
    [SerializeField] private Shader _lambertWithRimLighting;
    [SerializeField] private Shader _specularLighting;
    [SerializeField] private Shader _phongLighting;
    [SerializeField] private Material _testMaterial;

    private TextMeshProUGUI _tmp;
    private Shader[] _shaders;
    private int _shadersIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _shaders = new Shader[7] {_noLighting, _ambientLighting, _lambertDiffusionLighting, _rimLighting, _lambertWithRimLighting, _specularLighting, _phongLighting};
        _tmp = FindObjectOfType<Canvas>().GetComponentInChildren<TextMeshProUGUI>();

        UpdateShaders(GetCurrentShader());
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CycleShaders();
            UpdateText();
        }
    }

    private void CycleShaders() {
        _shadersIndex = _shadersIndex >= _shaders.Length - 1 ? 0 : _shadersIndex + 1;
        UpdateShaders(GetCurrentShader());
    }

    private void UpdateShaders(Shader shader) {
        _testMaterial.shader = shader;
    }

    private Shader GetCurrentShader() {
        return _shaders[_shadersIndex];
    }

    private void UpdateText() {
        _tmp.text = $"{ParseShaderName(_shaders[_shadersIndex].name)}";
    }

    private string ParseShaderName(string shaderName) {
        string shaderTitle = shaderName.Split('/').ElementAtOrDefault(1);
        return shaderTitle?.SplitWords(' ') ?? shaderName;
    }
}

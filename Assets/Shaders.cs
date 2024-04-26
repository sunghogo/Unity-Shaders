using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
public class Shaders : MonoBehaviour
{
    [SerializeField] private Shader _noLighting;
    [SerializeField] private Shader _ambientLighting;
    [SerializeField] private Shader _viewDirectionLighting;
    [SerializeField] private Shader _lambertDiffusionLighting;
    [SerializeField] private Shader _lambertViewLighting;
    [SerializeField] private Shader _phongLighting;
    [SerializeField] private Material _testMaterial;

    private TextMeshProUGUI _tmp;

    private LODGroup _lodGroup;
    private Shader[] _shaders;
    private int _shadersIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _lodGroup = GetComponent<LODGroup>();
        _shaders = new Shader[6] {_noLighting, _ambientLighting, _viewDirectionLighting, _lambertDiffusionLighting, _lambertViewLighting, _phongLighting};
        _tmp = FindObjectOfType<Canvas>().GetComponentInChildren<TextMeshProUGUI>();

        UpdateChildShaders(GetCurrentShader());
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
        UpdateChildShaders(GetCurrentShader());
    }

    private void UpdateChildShaders(Shader shader) {
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

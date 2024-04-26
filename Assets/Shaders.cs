using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
public class Shaders : MonoBehaviour
{
    [SerializeField] private Shader _noLighting;
    [SerializeField] private Shader _viewDirection;
    [SerializeField] private Shader _lambertDiffusion;
    [SerializeField] private Shader _lambertView;
    private TextMeshProUGUI _tmp;

    private LODGroup _lodGroup;
    private Shader[] _shaders;
    private int _shadersIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _lodGroup = GetComponent<LODGroup>();
        _shaders = new Shader[4] {_noLighting, _viewDirection, _lambertDiffusion, _lambertView};
        _tmp = FindObjectOfType<Canvas>().GetComponentInChildren<TextMeshProUGUI>();

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeChildShaders();
            UpdateText();
        }
    }

    private void ChangeChildShaders() {
        _shadersIndex = _shadersIndex >= _shaders.Length - 1 ? 0 : _shadersIndex + 1;

        foreach (var lod in _lodGroup.GetLODs()) {
            foreach (var renderer in lod.renderers) {
                renderer.material.shader = _shaders[_shadersIndex];
            }
        }
    }

    private void UpdateText() {
        _tmp.text = $"{ParseShaderName(_shaders[_shadersIndex].name)}";
    }

    private string ParseShaderName(string shaderName) {
        string shaderTitle = shaderName.Split('/').ElementAtOrDefault(1);
        return shaderTitle?.SplitWords(' ') ?? shaderName;
    }
}

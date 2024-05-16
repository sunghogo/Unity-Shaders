using UnityEngine;

public class Shaders : MonoBehaviour
{
    // Add the following shaders and material in the Inspector
    public Shader _noLighting;
    public Shader _ambientLighting;
    public Shader _lambertDiffusionLighting;
    public Shader _rimLighting;
    public Shader _lambertWithRimLighting;
    public Shader _phongSpecularReflection;
    public Shader _phongLighting;
    public Shader _BlinnPhongSpecularReflection;
    public Shader _BlinnPhongLighting;
    public Material TestMaterial;
    private UiCanvas _uiCanvas;
    private Shader[] _shaders;
    private int _shadersIndex;

    // Need Awake to make sure Shaders is initialized before UiCanvas
    void Awake()
    {
        _shadersIndex = 0;
        _shaders = new Shader[9] {_noLighting, _ambientLighting, _lambertDiffusionLighting, _rimLighting, _lambertWithRimLighting, _phongSpecularReflection, _phongLighting, _BlinnPhongSpecularReflection, _BlinnPhongLighting};
        _uiCanvas = FindObjectOfType<Canvas>().GetComponent<UiCanvas>();

        UpdateShaders(GetCurrentShader());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CycleShaders();
            _uiCanvas.UpdateUI();
        }
    }

    private void CycleShaders() {
        _shadersIndex = _shadersIndex >= _shaders.Length - 1 ? 0 : _shadersIndex + 1;
        UpdateShaders(GetCurrentShader());
    }

    private void UpdateShaders(Shader shader) {
        TestMaterial.shader = shader;
    }

    public Shader GetCurrentShader() {
        return _shaders[_shadersIndex];
    }
}

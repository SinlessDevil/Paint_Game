using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;

    [Range(2,512)]
    [SerializeField] private int _resalution = 128;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Color _color;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private float _radiusOut = 0.5f;
    [SerializeField] private float _radiusIn = 0.3f;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Gradient _gradient;

    public enum ModeLesson
    {
        Gradient,
        OnlyOneColor,
        Circle
    }

    public ModeLesson modeLesson;

    private void OnValidate()
    {
        if (_texture == null){
            _texture = new Texture2D(_resalution, _resalution);
            GetComponent<Renderer>().material.mainTexture = _texture;
        }
        if(_texture.width != _resalution){
            _texture.Reinitialize(_resalution, _resalution);
        }

        _texture.filterMode = _filterMode;
        _texture.wrapMode = _textureWrapMode;

        float step = 1f / _resalution;
        // _texture.SetPixel(5, 5, Color.red);
        // _texture.SetPixel(20, 20, Color.yellow);
        for (int y = 0; y < _resalution; y++)
        {
            for (int x = 0; x < _resalution; x++)
            {
                switch (modeLesson)
                {
                    case ModeLesson.Gradient:
                        _texture.SetPixel(x, y, new Color((x + 0.5f) * step, (y + 0.5f) * step, 0f, 1f));
                        break;
                    case ModeLesson.OnlyOneColor:
                        _texture.SetPixel(x, y, _color);
                        break;
                    case ModeLesson.Circle:

                        float x2 = Mathf.Pow((x + 0.5f) * step - _offset.x, 2);
                        float y2 = Mathf.Pow((y + 0.5f) * step - _offset.y, 2);
                        float rOut2 = Mathf.Pow(_radiusOut, 2);
                        float rIn2 = Mathf.Pow(_radiusIn, 2);
                        float result = x2 + y2;

                        float interpolant = Mathf.InverseLerp(rIn2, rOut2, result);
                        Color color = _gradient.Evaluate(interpolant);
                        _texture.SetPixel(x, y, color);

                        break;
                }
            }
        }
        _texture.Apply();
    }

}

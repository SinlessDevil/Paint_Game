using UnityEngine;

public class Paint : MonoBehaviour{
    [Range(5, 512)]
    [SerializeField] private int _textureSize = 128;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Texture2D _texture2D;
    [SerializeField] private Material _material;

    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 8;

    private int _oldRayX, _oldRayY;
    private void OnValidate(){
        if(_texture2D == null){
            _texture2D = new Texture2D(_textureSize, _textureSize);
        }
        if(_texture2D.width != _textureSize){
            _texture2D.Reinitialize(_textureSize, _textureSize);
        }
        _texture2D.wrapMode = _textureWrapMode;
        _texture2D.filterMode = _filterMode;
        _material.mainTexture = _texture2D;
        _texture2D.Apply();
    }

    private void Update(){
        _brushSize += (int)Input.mouseScrollDelta.y;

        if (Input.GetMouseButton(0)){
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 100f)){
                int rayX = (int)(hit.textureCoord.x * _textureSize);
                int rayY = (int)(hit.textureCoord.y * _textureSize);

                //DrowQuad(rayX, rayY);
                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    _oldRayX = rayX;
                    _oldRayY = rayY;
                    DrowCircle(rayX, rayY);
                }
                _texture2D.Apply();
            }
        }
    }


    private void DrowQuad(int rayX, int rayY){
        for (int y = 0; y < _brushSize; y++){
            for (int x = 0; x < _brushSize; x++){
                _texture2D.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
            }
        }
    }

    private void DrowCircle(int rayX, int rayY){
        for (int y = 0; y < _brushSize; y++){
            for (int x = 0; x < _brushSize; x++){

                int centerBrush = _brushSize / 2;
                float x2 = Mathf.Pow(x - centerBrush, 2);
                float y2 = Mathf.Pow(y - centerBrush, 2);
                float r2 = Mathf.Pow(centerBrush - 0.5f, 2);

                if(x2 + y2 < r2){
                    int pixelX = rayX + x - centerBrush;
                    int pixelY = rayY + y - centerBrush;

                    if(pixelX >= 0 && pixelX < _textureSize && pixelY >= 0 && pixelY < _textureSize)
                    {
                        Color oldColor = _texture2D.GetPixel(pixelX, pixelY);
                        Color resultColor = Color.Lerp(oldColor, _color, _color.a);

                        _texture2D.SetPixel(pixelX, pixelY, resultColor);
                    }
                }
            }
        }
    }
}

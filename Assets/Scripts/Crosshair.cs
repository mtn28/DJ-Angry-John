using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public float crosshairScale = 1.0f; // Adicione uma variável pública para a escala do crosshair

    void Start()
    {
        if (crosshairTexture != null)
        {
            // Esconder o cursor padrão do sistema
            Cursor.visible = false;
        }
        else
        {
            Debug.LogError("Crosshair texture is not assigned!");
        }
    }

    void OnGUI()
    {
        if (crosshairTexture != null)
        {
            // Calcule as dimensões redimensionadas
            float width = crosshairTexture.width * crosshairScale;
            float height = crosshairTexture.height * crosshairScale;

            // Obter a posição do mouse
            Vector3 mousePos = Input.mousePosition;

            // Inverter a coordenada y do mouse para corresponder ao sistema de coordenadas da GUI
            mousePos.y = Screen.height - mousePos.y;

            // Desenhe a mira na posição do mouse com as novas dimensões
            GUI.DrawTexture(new Rect(mousePos.x - (width / 2), mousePos.y - (height / 2), width, height), crosshairTexture);
        }
    }
}

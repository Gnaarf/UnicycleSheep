using System.Collections.Generic;
using SFML.Graphics;

public class AssetManager
{
    static Dictionary<TextureName, Texture> textures = new Dictionary<TextureName, Texture>();

    public static Texture getTexture(TextureName textureName)
    {
        if (textures.Count == 0)
        {
            LoadTextures(); 
        }
        return textures[textureName];
    }

    static void LoadTextures()
    {
        textures.Add(TextureName.WhitePixel, new Texture("Assets/Textures/pixel.png"));
        textures.Add(TextureName.MainMenuBackground, new Texture("Assets/Textures/Background.png"));
    }

    public enum TextureName
    {
        WhitePixel,
        MainMenuBackground
    }
}

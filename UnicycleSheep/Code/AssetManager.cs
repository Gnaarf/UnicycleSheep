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

        textures.Add(TextureName.ShoopWheel, new Texture("Assets/Textures/Wheel.png"));
        textures.Add(TextureName.Shoop, new Texture("Assets/Textures/pixel.png"));

        textures.Add(TextureName.Hand1, new Texture("Assets/Textures/Hands/Hand1/Base.png"));
        textures.Add(TextureName.Hand1_LeftThumb_Leftmost, new Texture("Assets/Textures/Hands/Hand1/leftThumb_leftmost.png"));
        textures.Add(TextureName.Hand1_LeftThumb_Left, new Texture("Assets/Textures/Hands/Hand1/leftThumb_left.png"));
        textures.Add(TextureName.Hand1_LeftThumb_Mid, new Texture("Assets/Textures/Hands/Hand1/leftThumb_mid.png"));
        textures.Add(TextureName.Hand1_LeftThumb_Right, new Texture("Assets/Textures/Hands/Hand1/leftThumb_right.png"));
        textures.Add(TextureName.Hand1_LeftThumb_Rightmost, new Texture("Assets/Textures/Hands/Hand1/leftThumb_rightmost.png"));

        textures.Add(TextureName.Hand1_RightThumb_Leftmost, new Texture("Assets/Textures/Hands/Hand1/rightThumb_leftmost.png"));
        textures.Add(TextureName.Hand1_RightThumb_Left, new Texture("Assets/Textures/Hands/Hand1/rightThumb_left.png"));
        textures.Add(TextureName.Hand1_RightThumb_Mid, new Texture("Assets/Textures/Hands/Hand1/rightThumb_mid.png"));
        textures.Add(TextureName.Hand1_RightThumb_Right, new Texture("Assets/Textures/Hands/Hand1/rightThumb_right.png"));
        textures.Add(TextureName.Hand1_RightThumb_Rightmost, new Texture("Assets/Textures/Hands/Hand1/rightThumb_rightmost.png"));

    }

    public enum TextureName
    {
        WhitePixel,
        MainMenuBackground,

        //characterStuff
        ShoopWheel,
        Shoop,

        Hand1,
        Hand1_LeftThumb_Leftmost,
        Hand1_LeftThumb_Left,
        Hand1_LeftThumb_Mid,
        Hand1_LeftThumb_Right,
        Hand1_LeftThumb_Rightmost,
        Hand1_RightThumb_Leftmost,
        Hand1_RightThumb_Left,
        Hand1_RightThumb_Mid,
        Hand1_RightThumb_Right,
        Hand1_RightThumb_Rightmost,
        
    }
}

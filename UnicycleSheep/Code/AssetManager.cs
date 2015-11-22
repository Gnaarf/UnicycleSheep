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
        textures[TextureName.MainMenuBackground].Smooth = true;

        textures.Add(TextureName.ShoopWheel, new Texture("Assets/Textures/Wheel.png"));
        textures.Add(TextureName.ShoopRed, new Texture("Assets/Textures/SheepRed.png"));
        textures.Add(TextureName.ShoopGreen, new Texture("Assets/Textures/SheepGreen.png"));


        textures.Add(TextureName.Hand, new Texture("Assets/Textures/Hands/Hand1/Base.png"));

        textures.Add(TextureName.LeftThumb_Leftmost, new Texture("Assets/Textures/Hands/Hand1/leftThumb_leftmost.png"));
        textures.Add(TextureName.LeftThumb_Left, new Texture("Assets/Textures/Hands/Hand1/leftThumb_left.png"));
        textures.Add(TextureName.LeftThumb_Mid, new Texture("Assets/Textures/Hands/Hand1/leftThumb_mid.png"));
        textures.Add(TextureName.LeftThumb_Right, new Texture("Assets/Textures/Hands/Hand1/leftThumb_right.png"));
        textures.Add(TextureName.LeftThumb_Rightmost, new Texture("Assets/Textures/Hands/Hand1/leftThumb_rightmost.png"));

        textures.Add(TextureName.RightThumb_Leftmost, new Texture("Assets/Textures/Hands/Hand1/rightThumb_leftmost.png"));
        textures.Add(TextureName.RightThumb_Left, new Texture("Assets/Textures/Hands/Hand1/rightThumb_left.png"));
        textures.Add(TextureName.RightThumb_Mid, new Texture("Assets/Textures/Hands/Hand1/rightThumb_mid.png"));
        textures.Add(TextureName.RightThumb_Right, new Texture("Assets/Textures/Hands/Hand1/rightThumb_right.png"));
        textures.Add(TextureName.RightThumb_Rightmost, new Texture("Assets/Textures/Hands/Hand1/rightThumb_rightmost.png"));

        textures.Add(TextureName.LeftFinger_Down, new Texture("Assets/Textures/Hands/Hand1/leftFinger_down.png"));
        textures.Add(TextureName.LeftFinger_Up, new Texture("Assets/Textures/Hands/Hand1/leftFinger_up.png"));
        textures.Add(TextureName.RightFinger_Down, new Texture("Assets/Textures/Hands/Hand1/rightFinger_down.png"));
        textures.Add(TextureName.RightFinger_Up, new Texture("Assets/Textures/Hands/Hand1/rightFinger_up.png"));

        textures.Add(TextureName.Jumpbar1, new Texture("Assets/Textures/Hands/Hand1/Jumpbar1.png"));
        textures.Add(TextureName.Jumpbar2, new Texture("Assets/Textures/Hands/Hand1/Jumpbar2.png"));
        textures.Add(TextureName.Jumpbar3, new Texture("Assets/Textures/Hands/Hand1/Jumpbar3.png"));
        textures.Add(TextureName.Jumpbar4, new Texture("Assets/Textures/Hands/Hand1/Jumpbar4.png"));
        textures.Add(TextureName.Jumpbar5, new Texture("Assets/Textures/Hands/Hand1/Jumpbar5.png"));
        textures.Add(TextureName.Jumpbar6, new Texture("Assets/Textures/Hands/Hand1/Jumpbar6.png"));

        textures.Add(TextureName.RemoteFlagGreen, new Texture("Assets/Textures/Hands/Hand1/FlagGreen.png"));
        textures.Add(TextureName.RemoteFlagRed, new Texture("Assets/Textures/Hands/Hand1/FlagRed.png"));


        textures.Add(TextureName.FlagRed, new Texture("Assets/Textures/flagRed.png"));
        textures.Add(TextureName.FlagGreen, new Texture("Assets/Textures/flagGreen.png"));

    }

    public enum TextureName
    {
        WhitePixel,
        MainMenuBackground,

        //characterStuff
        ShoopWheel,
        ShoopGreen,
        ShoopRed,

        Hand,
        LeftThumb_Leftmost,
        LeftThumb_Left,
        LeftThumb_Mid,
        LeftThumb_Right,
        LeftThumb_Rightmost,
        RightThumb_Leftmost,
        RightThumb_Left,
        RightThumb_Mid,
        RightThumb_Right,
        RightThumb_Rightmost,
        LeftFinger_Down,
        LeftFinger_Up,
        RightFinger_Down,
        RightFinger_Up,

        Jumpbar1,
        Jumpbar2,
        Jumpbar3,
        Jumpbar4,
        Jumpbar5,
        Jumpbar6,

        RemoteFlagGreen,
        RemoteFlagRed,

        FlagRed,
        FlagGreen,
    }
}

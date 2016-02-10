using System.Collections.Generic;
using SFML.Graphics;
using SFML.Audio;

public class AssetManager
{
    static Dictionary<TextureName, Texture> textures = new Dictionary<TextureName, Texture>();
    static Dictionary<SoundName, Sound> sounds = new Dictionary<SoundName, Sound>();

    public static Texture getTexture(TextureName textureName)
    {
        if (textures.Count == 0)
        {
            LoadTextures();
        }
        return textures[textureName];
    }

    public static Sound getSound(SoundName soundName)
    {
        if (sounds.Count == 0)
        {
            LoadSounds();
        }
        return sounds[soundName];
    }

    static void LoadTextures()
    {
        string texturePath = "Assets/Textures/";

        textures.Add(TextureName.WhitePixel, new Texture(texturePath + "pixel.png"));
        textures.Add(TextureName.MainMenuBackground, new Texture(texturePath + "Background.png"));
        textures[TextureName.MainMenuBackground].Smooth = true;

        textures.Add(TextureName.ShoopInfronUnicycle, new Texture(texturePath + "SheepFront.png"));
        textures.Add(TextureName.ShoopBehindUnicycle, new Texture(texturePath + "SheepBack.png"));
        textures.Add(TextureName.ShoopUnicycle, new Texture(texturePath + "unicycle.png"));
        textures.Add(TextureName.ShoopWheel, new Texture(texturePath + "Wheel.png"));


        textures.Add(TextureName.Hand, new Texture(texturePath + "Hands/Hand1/Base.png"));

        textures.Add(TextureName.LeftThumb_Leftmost, new Texture(texturePath + "Hands/Hand1/leftThumb_leftmost.png"));
        textures.Add(TextureName.LeftThumb_Left, new Texture(texturePath + "Hands/Hand1/leftThumb_left.png"));
        textures.Add(TextureName.LeftThumb_Mid, new Texture(texturePath + "Hands/Hand1/leftThumb_mid.png"));
        textures.Add(TextureName.LeftThumb_Right, new Texture(texturePath + "Hands/Hand1/leftThumb_right.png"));
        textures.Add(TextureName.LeftThumb_Rightmost, new Texture(texturePath + "Hands/Hand1/leftThumb_rightmost.png"));

        textures.Add(TextureName.RightThumb_Leftmost, new Texture(texturePath + "Hands/Hand1/rightThumb_leftmost.png"));
        textures.Add(TextureName.RightThumb_Left, new Texture(texturePath + "Hands/Hand1/rightThumb_left.png"));
        textures.Add(TextureName.RightThumb_Mid, new Texture(texturePath + "Hands/Hand1/rightThumb_mid.png"));
        textures.Add(TextureName.RightThumb_Right, new Texture(texturePath + "Hands/Hand1/rightThumb_right.png"));
        textures.Add(TextureName.RightThumb_Rightmost, new Texture(texturePath + "Hands/Hand1/rightThumb_rightmost.png"));

        textures.Add(TextureName.LeftFinger_Down, new Texture(texturePath + "Hands/Hand1/leftFinger_down.png"));
        textures.Add(TextureName.LeftFinger_Up, new Texture(texturePath + "Hands/Hand1/leftFinger_up.png"));
        textures.Add(TextureName.RightFinger_Down, new Texture(texturePath + "Hands/Hand1/rightFinger_down.png"));
        textures.Add(TextureName.RightFinger_Up, new Texture(texturePath + "Hands/Hand1/rightFinger_up.png"));

        textures.Add(TextureName.Jumpbar1, new Texture(texturePath + "Hands/Hand1/Jumpbar1.png"));
        textures.Add(TextureName.Jumpbar2, new Texture(texturePath + "Hands/Hand1/Jumpbar2.png"));
        textures.Add(TextureName.Jumpbar3, new Texture(texturePath + "Hands/Hand1/Jumpbar3.png"));
        textures.Add(TextureName.Jumpbar4, new Texture(texturePath + "Hands/Hand1/Jumpbar4.png"));
        textures.Add(TextureName.Jumpbar5, new Texture(texturePath + "Hands/Hand1/Jumpbar5.png"));
        textures.Add(TextureName.Jumpbar6, new Texture(texturePath + "Hands/Hand1/Jumpbar6.png"));

        textures.Add(TextureName.RemoteFlag, new Texture(texturePath + "Hands/Hand1/Flag.png"));
        textures.Add(TextureName.RemoteFlagWhitePart, new Texture(texturePath + "Hands/Hand1/FlagWhitePart.png"));


        textures.Add(TextureName.FlagRed, new Texture(texturePath + "flagRed.png"));
        textures.Add(TextureName.FlagGreen, new Texture(texturePath + "flagGreen.png"));
        textures.Add(TextureName.FlagGray, new Texture(texturePath + "flagGray.png"));

        textures.Add(TextureName.InGameBackGroundBack, new Texture(texturePath + "backgroundBack.png"));
        textures.Add(TextureName.InGameBackGroundFront, new Texture(texturePath + "backgroundFront.png"));

    }

    static void LoadSounds()
    {
        string soundPath = "Assets/Audio/";
        sounds.Add(SoundName.Background, new Sound(new SoundBuffer(soundPath + "BackGround1.wav")));
    }

    public enum TextureName
    {
        WhitePixel,
        MainMenuBackground,

        //characterStuff
        ShoopInfronUnicycle,
        ShoopBehindUnicycle,
        ShoopUnicycle,
        ShoopWheel,

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

        RemoteFlag,
        RemoteFlagWhitePart,

        FlagRed,
        FlagGreen,
        FlagGray,

        InGameBackGroundBack,
        InGameBackGroundFront,
    }

    public enum SoundName
    {
        Background,
    }
}

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace AmazingMazeDesktop;

public static class Assets
{
    public static void Load(ContentManager content)
    {
        var bodyBodiesMaleLight = content.Load<Texture2D>("GFX/body-bodies-male-light");
        var headHeadsHumanMaleLight = content.Load<Texture2D>("GFX/head-heads-human-male-light");
        var torsoArmourLeatherMaleLeather = content.Load<Texture2D>("GFX/torso-armour-leather-male-leather");
        var weaponMagicWandMaleWand = content.Load<Texture2D>("GFX/weapon-magic-wand-male-wand");
        FogOfWarEffect = content.Load<Effect>("SFX/fog_of_war");
        
        BodyBodiesMaleLight = new SpriteSheet("Spritesheet/BodyBodiesMaleLight",
            Texture2DAtlas.Create("Atlas/BodyBodiesMaleLight", bodyBodiesMaleLight, 64, 64));
        HeadHeadsHumanMaleLight = new SpriteSheet("Spritesheet/HeadHeadsHumanMaleLight",
            Texture2DAtlas.Create("Atlas/HeadHeadsHumanMaleLight", headHeadsHumanMaleLight, 64, 64));
        TorsoArmourLeatherMaleLeather = new SpriteSheet("Spritesheet/TorsoArmourLeatherMaleLeather",
            Texture2DAtlas.Create("Atlas/TorsoArmourLeatherMaleLeather", torsoArmourLeatherMaleLeather,
                64, 64));
        WeaponMagicWandMaleWand = new SpriteSheet("Spritesheet/WeaponMagicWandMaleWand",
            Texture2DAtlas.Create("Atlas/WeaponMagicWandMaleWand", weaponMagicWandMaleWand, 64, 64));

        Generate([
            BodyBodiesMaleLight, HeadHeadsHumanMaleLight, TorsoArmourLeatherMaleLeather, WeaponMagicWandMaleWand
        ]);
    }

    public static Texture2D OrangePlaceholderTexture, WhitePlaceholderTexture, YellowPlaceholderTexture, GreenPlaceholderTexture;

    public static SpriteSheet BodyBodiesMaleLight,  HeadHeadsHumanMaleLight,   TorsoArmourLeatherMaleLeather, WeaponMagicWandMaleWand;

    public static Effect FogOfWarEffect;
    private static void Generate(List<SpriteSheet> list)
    {
        foreach (var spriteSheet in list)
        {
            spriteSheet.DefineAnimation("spellcast_n", builder =>
            {
                builder.IsLooping(false)
                    .AddFrame(0, TimeSpan.FromSeconds(0.1))
                    .AddFrame(1, TimeSpan.FromSeconds(0.1))
                    .AddFrame(2, TimeSpan.FromSeconds(0.1))
                    .AddFrame(3, TimeSpan.FromSeconds(0.1))
                    .AddFrame(4, TimeSpan.FromSeconds(0.1))
                    .AddFrame(5, TimeSpan.FromSeconds(0.1))
                    .AddFrame(6, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("spellcast_w", builder =>
            {
                builder.IsLooping(false)
                    .AddFrame(13, TimeSpan.FromSeconds(0.1))
                    .AddFrame(14, TimeSpan.FromSeconds(0.1))
                    .AddFrame(15, TimeSpan.FromSeconds(0.1))
                    .AddFrame(16, TimeSpan.FromSeconds(0.1))
                    .AddFrame(17, TimeSpan.FromSeconds(0.1))
                    .AddFrame(18, TimeSpan.FromSeconds(0.1))
                    .AddFrame(19, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("spellcast_s", builder =>
            {
                builder.IsLooping(false)
                    .AddFrame(26, TimeSpan.FromSeconds(0.1))
                    .AddFrame(27, TimeSpan.FromSeconds(0.1))
                    .AddFrame(28, TimeSpan.FromSeconds(0.1))
                    .AddFrame(29, TimeSpan.FromSeconds(0.1))
                    .AddFrame(30, TimeSpan.FromSeconds(0.1))
                    .AddFrame(31, TimeSpan.FromSeconds(0.1))
                    .AddFrame(32, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("spellcast_e", builder =>
            {
                builder.IsLooping(false)
                    .AddFrame(39, TimeSpan.FromSeconds(0.1))
                    .AddFrame(40, TimeSpan.FromSeconds(0.1))
                    .AddFrame(41, TimeSpan.FromSeconds(0.1))
                    .AddFrame(42, TimeSpan.FromSeconds(0.1))
                    .AddFrame(43, TimeSpan.FromSeconds(0.1))
                    .AddFrame(44, TimeSpan.FromSeconds(0.1))
                    .AddFrame(45, TimeSpan.FromSeconds(0.1));
                ;
            });
            spriteSheet.DefineAnimation("thrust_n", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame(52, TimeSpan.FromSeconds(0.1))
                    .AddFrame(53, TimeSpan.FromSeconds(0.1))
                    .AddFrame(54, TimeSpan.FromSeconds(0.1))
                    .AddFrame(55, TimeSpan.FromSeconds(0.1))
                    .AddFrame(56, TimeSpan.FromSeconds(0.1))
                    .AddFrame(57, TimeSpan.FromSeconds(0.1))
                    .AddFrame(58, TimeSpan.FromSeconds(0.1))
                    .AddFrame(59, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("thrust_w", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame(65, TimeSpan.FromSeconds(0.1))
                    .AddFrame(66, TimeSpan.FromSeconds(0.1))
                    .AddFrame(67, TimeSpan.FromSeconds(0.1))
                    .AddFrame(68, TimeSpan.FromSeconds(0.1))
                    .AddFrame(69, TimeSpan.FromSeconds(0.1))
                    .AddFrame(70, TimeSpan.FromSeconds(0.1))
                    .AddFrame(71, TimeSpan.FromSeconds(0.1))
                    .AddFrame(72, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("thrust_s", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame(78, TimeSpan.FromSeconds(0.1))
                    .AddFrame(79, TimeSpan.FromSeconds(0.1))
                    .AddFrame(80, TimeSpan.FromSeconds(0.1))
                    .AddFrame(81, TimeSpan.FromSeconds(0.1))
                    .AddFrame(82, TimeSpan.FromSeconds(0.1))
                    .AddFrame(83, TimeSpan.FromSeconds(0.1))
                    .AddFrame(84, TimeSpan.FromSeconds(0.1))
                    .AddFrame(85, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("thrust_e", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame(91, TimeSpan.FromSeconds(0.1))
                    .AddFrame(92, TimeSpan.FromSeconds(0.1))
                    .AddFrame(93, TimeSpan.FromSeconds(0.1))
                    .AddFrame(94, TimeSpan.FromSeconds(0.1))
                    .AddFrame(95, TimeSpan.FromSeconds(0.1))
                    .AddFrame(96, TimeSpan.FromSeconds(0.1))
                    .AddFrame(97, TimeSpan.FromSeconds(0.1))
                    .AddFrame(98, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("walk_n", builder =>
            {
                builder.IsLooping(true)
                    //.AddFrame(104, TimeSpan.FromSeconds(0.1))
                    .AddFrame(105, TimeSpan.FromSeconds(0.1))
                    .AddFrame(106, TimeSpan.FromSeconds(0.1))
                    .AddFrame(107, TimeSpan.FromSeconds(0.1))
                    .AddFrame(108, TimeSpan.FromSeconds(0.1))
                    .AddFrame(109, TimeSpan.FromSeconds(0.1))
                    .AddFrame(110, TimeSpan.FromSeconds(0.1))
                    .AddFrame(111, TimeSpan.FromSeconds(0.1))
                    .AddFrame(112, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("walk_w", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame(117, TimeSpan.FromSeconds(0.1))
                    .AddFrame(118, TimeSpan.FromSeconds(0.1))
                    .AddFrame(119, TimeSpan.FromSeconds(0.1))
                    .AddFrame(120, TimeSpan.FromSeconds(0.1))
                    .AddFrame(121, TimeSpan.FromSeconds(0.1))
                    .AddFrame(122, TimeSpan.FromSeconds(0.1))
                    .AddFrame(123, TimeSpan.FromSeconds(0.1))
                    .AddFrame(124, TimeSpan.FromSeconds(0.1))
                    .AddFrame(125, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("walk_s", builder =>
            {
                builder.IsLooping(true)
                    //.AddFrame(130, TimeSpan.FromSeconds(0.1))
                    .AddFrame(131, TimeSpan.FromSeconds(0.1))
                    .AddFrame(132, TimeSpan.FromSeconds(0.1))
                    .AddFrame(133, TimeSpan.FromSeconds(0.1))
                    .AddFrame(134, TimeSpan.FromSeconds(0.1))
                    .AddFrame(135, TimeSpan.FromSeconds(0.1))
                    .AddFrame(136, TimeSpan.FromSeconds(0.1))
                    .AddFrame(137, TimeSpan.FromSeconds(0.1))
                    .AddFrame(138, TimeSpan.FromSeconds(0.1));
            });
            spriteSheet.DefineAnimation("walk_e", builder =>
            {
                builder.IsLooping(true)
                    .AddFrame(143, TimeSpan.FromSeconds(0.1))
                    .AddFrame(144, TimeSpan.FromSeconds(0.1))
                    .AddFrame(145, TimeSpan.FromSeconds(0.1))
                    .AddFrame(146, TimeSpan.FromSeconds(0.1))
                    .AddFrame(147, TimeSpan.FromSeconds(0.1))
                    .AddFrame(148, TimeSpan.FromSeconds(0.1))
                    .AddFrame(149, TimeSpan.FromSeconds(0.1))
                    .AddFrame(150, TimeSpan.FromSeconds(0.1))
                    .AddFrame(151, TimeSpan.FromSeconds(0.1));
            });
        }
    }
}
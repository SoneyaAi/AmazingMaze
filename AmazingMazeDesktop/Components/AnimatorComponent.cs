using System.Collections.Generic;
using MonoGame.Extended.Graphics;

namespace AmazingMazeDesktop.Components;

public class AnimatorComponent
{
    
    public List<AnimatedSprite> AnimatedSprites;
    private AnimatedSprite Body, Head, Torso, Weapon;

    public AnimatorComponent()
    {
        Body = new AnimatedSprite(Assets.BodyBodiesMaleLight, "spellcast_s");
        Body.Origin = Body.Size.ToVector2() / 2;
        Head = new AnimatedSprite(Assets.HeadHeadsHumanMaleLight, "spellcast_s");
        Head.Origin = Head.Size.ToVector2() / 2;
        Torso = new AnimatedSprite(Assets.TorsoArmourLeatherMaleLeather, "spellcast_s");
        Torso.Origin = Torso.Size.ToVector2() / 2;
        Weapon = new AnimatedSprite(Assets.WeaponMagicWandMaleWand, "spellcast_s");
        Weapon.Origin = Weapon.Size.ToVector2() / 2;
        AnimatedSprites = [Body, Head, Torso, Weapon];
    }
        
}
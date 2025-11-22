using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using RenderingLibrary;

namespace AmazingMazeDesktop.Systems;

public class RenderSystem(GraphicsDevice graphicsDevice, OrthographicCamera camera)
    : EntityDrawSystem(Aspect.One(typeof(Texture2D), typeof(AnimatorComponent), typeof(ColliderComponent),
        typeof(MovementComponent)))
{
    private SpriteBatch _spriteBatch = new(graphicsDevice);
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<Texture2D> _texture2DMapper;
    private ComponentMapper<AnimatorComponent> _animatorMapper;
    private ComponentMapper<StateComponent> _stateMapper;
    private ComponentMapper<TagsComponent> _tagsMapper;
    private Vector2 _playerPosition;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
        _texture2DMapper = mapperService.GetMapper<Texture2D>();
        _animatorMapper = mapperService.GetMapper<AnimatorComponent>();
        _stateMapper = mapperService.GetMapper<StateComponent>();
        _tagsMapper = mapperService.GetMapper<TagsComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        foreach (var entity in ActiveEntities)
        {
            if (_tagsMapper.TryGet(entity, out var tags))
            {
                if (tags.TagsList.Contains(Tags.Player))
                {
                    _playerPosition = _transformMapper.Get(entity).Position;
                }
            }

            if (_transformMapper.TryGet(entity, out var transform))
            {
                if (_texture2DMapper.TryGet(entity, out var texture2D))
                {
                    _spriteBatch.Draw(texture2D,
                        transform.Position,
                        null,
                        Color.White,
                        transform.Rotation,
                        texture2D.Bounds.Size.ToVector2() / 2,
                        transform.Scale * 20,
                        SpriteEffects.None,
                        1
                    );
                }

                if (_animatorMapper.TryGet(entity, out var animator))
                {
                    foreach (var aspr in animator.AnimatedSprites)
                    {
                        var state = _stateMapper.Get(entity);
                        string animName = "";
                        if (state.Current == MoveState.Walk)
                            animName = "walk";
                        else
                            animName = "spellcast";

                        switch (state.Facing)
                        {
                            case Facing.North:
                                animName += "_n";
                                break;
                            case Facing.South:
                                animName += "_s";
                                break;
                            case Facing.East:
                                animName += "_e";
                                break;
                            case Facing.West:
                                animName += "_w";
                                break;
                        }


                        if (state.Current == MoveState.Walk && aspr.CurrentAnimation != animName)
                            aspr.SetAnimation(animName);

                        aspr.Update(gameTime);
                        _spriteBatch.Draw(aspr, transform);
                    }
                }
            }
        }

        _spriteBatch.End();

        // FOW
        if (!DebugSettings.HideFoW)
        {
            var vp = graphicsDevice.Viewport;
            var screenRect = new Rectangle(0, 0, vp.Width, vp.Height);

            Vector2 playerScreenPos = camera.WorldToScreen(_playerPosition);

            Assets.FogOfWarEffect.Parameters["FogColor"].SetValue(new Vector4(0f, 0f, 0f, 1f));
            Assets.FogOfWarEffect.Parameters["PlayerPosPixels"].SetValue(playerScreenPos);
            Assets.FogOfWarEffect.Parameters["Radius"].SetValue(128);
            Assets.FogOfWarEffect.Parameters["Softness"].SetValue(128);
            Assets.FogOfWarEffect.Parameters["TextureWidth"].SetValue(vp.Width);
            Assets.FogOfWarEffect.Parameters["TextureHeight"].SetValue(vp.Height);
            Assets.FogOfWarEffect.Parameters["Zoom"].SetValue(camera.Zoom);
            _spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                effect: Assets.FogOfWarEffect
            );

            _spriteBatch.Draw(Assets.WhitePlaceholderTexture, screenRect, Color.White);
            _spriteBatch.End();
        }
    }
}
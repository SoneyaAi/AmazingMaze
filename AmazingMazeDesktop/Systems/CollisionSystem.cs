using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class CollisionSystem() : EntityUpdateSystem(Aspect.All(typeof(ColliderComponent)))
{
    private CollisionComponent _collisionComponent;
    private ComponentMapper<ColliderComponent> _colliderMapper;
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<TagsComponent> _tagsMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _colliderMapper = mapperService.GetMapper<ColliderComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
        _tagsMapper = mapperService.GetMapper<TagsComponent>();
        
        _collisionComponent = new CollisionComponent(new RectangleF(0, 0, 1300, 720));
        var shash = new SpatialHash(new Vector2(100 * 64, 100 * 64));
        var wallsLayer = new Layer(shash);
        _collisionComponent.Add("Walls", wallsLayer);
    }

    public override void Update(GameTime gameTime)
    {
        // Update  CollisionComponent
        foreach (var entity in ActiveEntities)
        {
            var collider = _colliderMapper.Get(entity);
            var transform = _transformMapper.Get(entity);

            collider.SetPosition(transform.Position);
        }
        _collisionComponent.Update(gameTime);

        // Check collisions from CollisionComponent
        foreach (var entity in ActiveEntities.Where(e => _colliderMapper.Get(e).ColliisionInfo != null))
        {
            var collisionInfo = _colliderMapper.Get(entity).ColliisionInfo;
            
            var entityCollider = _colliderMapper.Get(entity);
            var entityTransform = _transformMapper.Get(entity);
            var entityTags =  _tagsMapper.Get(entity);
            
            var otherCollider = (ColliderComponent)collisionInfo.Other;
            var otherTransform = _transformMapper.Get(otherCollider.OwnerEntity);
            var otherTags =  _tagsMapper.Get(otherCollider.OwnerEntity);

            if (entityTags.HasTag(Tags.Wall))
            {
                if (otherTags.HasTag(Tags.Player))
                {
                    //Debug.WriteLine(entityTransform.Position + " " + collisionInfo.PenetrationVector);
                    otherTransform.Position += collisionInfo.PenetrationVector;
                    entityCollider.ClearCollisionInfo();
                    continue;
                }
            }
            
            
            
            // if (_tagsMapper.Get(entity).TagsList.Contains(Tags.Projectile))
            // {
            //     if (otherTags.HasTag(Tags.Wall))
            //     {
            //         _collisionComponent.Remove(_colliderMapper.Get(entity));
            //         DestroyEntity(entity);
            //
            //         continue;
            //     }
            //
            //     _collisionComponent.Remove(_colliderMapper.Get(entity));
            //     DestroyEntity(entity);
            //     
            //     _collisionComponent.Remove(otherCollider);
            //     DestroyEntity(otherCollider.OwnerEntity);
            // }
            // if (_tagsMapper.Get(entity).TagsList.Contains(Tags.Wall))
            // {
            //     var tranform = _transformMapper.Get(entity);
            //     var otherTransform = _transformMapper.Get(other.OwnerEntity);
            //
            //     tranform.Position += _colliderMapper.Get(entity).ColliisionInfo.PenetrationVector;
            //     
            //     
            //     // _collisionComponent.Remove(_colliderMapper.Get(entity));
            //     // DestroyEntity(entity);
            //     //
            //     // _collisionComponent.Remove(other);
            //     // DestroyEntity(other.OwnerEntity);
            // }
            
            
        }
    }

    public void AddEntity(int entity)
    {
        _collisionComponent.Insert(_colliderMapper.Get(entity));
    }

    public void RemoveEntity(int entity)
    {
        _collisionComponent.Remove(_colliderMapper.Get(entity));
    }
}
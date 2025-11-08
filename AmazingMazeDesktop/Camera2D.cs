using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace AmazingMazeDesktop;

public enum CameraMovementMode
{
    WSAD,
    TrackPlayer
}
public class Camera2D
{
    
    private readonly Viewport _viewport;
    private float _cameraMoveSpeed = 10f;
    private float _cameraZoomSpeed = 0.05f;
    private float _cameraTargetZoom = 1f;
    private float _cameraCurrentZoom = 1f;
    private float _cameraZoomInterpolationSpeed = 0.1f;

    public CameraMovementMode MovementMode = CameraMovementMode.TrackPlayer;
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Zoom { get; set; }

    public float ZoomInterpolationSpeed { get; set; }
    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;
        Position = Vector2.Zero;
        Rotation = 0f;
        Zoom = 1f;
    }

    public void MoveToDirection(Vector2 direction)
    {
        Position += direction;
    }

    public void MoveToPosition(float x, float y)
    {
        Position = new Vector2(x, y);
    }
    
    public Matrix GetTransformation()
    {
        Matrix transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                           Matrix.CreateRotationZ(Rotation) *
                           Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                           Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
        return transform;
    }

    public void Update()
    {
        // ---------- Movement
        if (MovementMode == CameraMovementMode.WSAD)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                MoveToDirection(new Vector2(0, -_cameraMoveSpeed));
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                MoveToDirection(new Vector2(0, _cameraMoveSpeed));
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                MoveToDirection(new Vector2(-_cameraMoveSpeed, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                MoveToDirection(new Vector2(_cameraMoveSpeed, 0));
        }

        // --------- Zoom
        int scrollDelta = MouseExtended.GetState().DeltaScrollWheelValue;
        if (scrollDelta > 0)
            _cameraTargetZoom += _cameraZoomSpeed;
        else if (scrollDelta < 0)
            _cameraTargetZoom -= _cameraZoomSpeed;

        // ------------ Smooth zoom
        _cameraTargetZoom = MathHelper.Clamp(_cameraTargetZoom, 0.1f, 10f);
        _cameraCurrentZoom = MathHelper.Lerp(_cameraCurrentZoom, _cameraTargetZoom, _cameraZoomInterpolationSpeed);
        Zoom = _cameraCurrentZoom;
    }
}

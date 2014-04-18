using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VidyaTutorial
{

    class Camera
    {

        private Vector3 position = Vector3.Zero;
        private float rotationY;
        private float rotationX;
        
        public Matrix projection { get; private set; }

        private Vector3 lookAt;
        private Vector3 baseCameraReferenceLookat = new Vector3(0, 0, 1);
        private bool needViewResync = true;

        public Vector3 Position { get { return position; } set { position = value; UpdateLookAt(); } }
        public float RotationY { get { return rotationY; } set { rotationY = value; UpdateLookAt(); } }
        public float RotationX { get { return rotationX; } set { rotationX = value; UpdateLookAt(); } }
        
        public Matrix View
        {
            get
            {
                if (needViewResync)
                {
                    cachedViewMatrix = Matrix.CreateLookAt(position, lookAt, Vector3.Up);

                }
                return cachedViewMatrix;
            }
        }
        private Matrix cachedViewMatrix;

        public Camera(Vector3 position, float rotation, float aspectRatio, float nearClip, float farClip)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, nearClip, farClip);
            MoveTo(position, 0,0);

        }

        private void MoveTo(Vector3 position, float rotationY, float rotationX)
        {
            this.position = position;
            this.rotationY = rotationY;
            this.rotationX = rotationX;
            UpdateLookAt();
        }
        
        public Vector3 PreviewMove(float scaleX, float scaleY)
        {
            Matrix rotate = new Matrix();
            if (RotationY != 0) { rotate = Matrix.CreateRotationY(rotationY); }
            if (RotationX != 0) { rotate = Matrix.CreateRotationX(rotationX); }
            Vector3 forward = new Vector3(scaleX, 0, scaleY);
            forward = Vector3.Transform(forward, rotate);
            return (position + forward);
        }
        
        public void MoveForward(float scaleX, float scaleY)
        {
            MoveTo(PreviewMove(scaleX, scaleY), rotationY, rotationX);
        }

        private void UpdateLookAt()
        {

            Matrix rotationMatrix = new Matrix();
            if (RotationY != 0) { rotationMatrix = Matrix.CreateRotationY(rotationY); }
            if (RotationX != 0) { rotationMatrix = Matrix.CreateRotationX(rotationX); }
            Vector3 lookAtOffset = Vector3.Transform(baseCameraReferenceLookat, rotationMatrix);
            lookAt = position + lookAtOffset;
            needViewResync = true;
        }
    }
}

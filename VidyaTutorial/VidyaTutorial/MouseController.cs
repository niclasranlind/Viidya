using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VidyaTutorial
{
	public class MouseController
	{
		private readonly int _centerScreenX;
		private readonly int _centerScreenY;
		private const float RotateScale = MathHelper.PiOver2;

		public MouseController(int windowWidth, int windowHeight)
		{
			_centerScreenX = windowWidth / 2;
			_centerScreenY = windowHeight / 2;
		}

		public void Move(Camera camera, GameTime gameTime)
		{
			var mouseState = Mouse.GetState();
			var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if(mouseState.X > _centerScreenX)
			{
				camera.RotationY = MathHelper.WrapAngle(camera.RotationY - (RotateScale * elapsed));
			}

			if(mouseState.X < _centerScreenX)
			{
				camera.RotationY = MathHelper.WrapAngle(camera.RotationY + (RotateScale * elapsed));
			}

			if(mouseState.Y > _centerScreenY)
			{
				camera.RotationX = MathHelper.WrapAngle(camera.RotationX - (RotateScale * elapsed));
			}

			if(mouseState.Y < _centerScreenY)
			{
				camera.RotationX = MathHelper.WrapAngle(camera.RotationX + (RotateScale * elapsed));
			}
			
		}

		public void Draw()
		{
			Mouse.SetPosition(_centerScreenX, _centerScreenY);
		}
	}
}

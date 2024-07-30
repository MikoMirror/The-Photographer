using Godot;
using System;

public partial class Player
{
	private void ApplyCameraShakes(double delta)
	{
		ApplyWalkShake(delta);
		ApplyLandingShake(delta);
	}

	private void ApplyWalkShake(double delta)
	{
		if (IsOnFloor())
		{
			ShakeTime += (float)delta * WalkShakeSpeed;
			float shakeOffset = Mathf.Sin(ShakeTime) * WalkShakeAmount;
			Camera.Position = InitialCameraPosition + new Vector3(0, shakeOffset, 0);
		}
	}

	private void ApplyLandingShake(double delta)
	{
		if (LandingShake > 0)
		{
			LandingShake = Mathf.MoveToward(LandingShake, 0, (float)delta * 4);
			float landingOffset = Mathf.Sin(LandingShake * 20) * LandingShake;
			Camera.Position = InitialCameraPosition + new Vector3(0, landingOffset, 0);
		}
		else if (IsOnFloor() && Velocity.X == 0 && Velocity.Z == 0)
		{
			Camera.Position = InitialCameraPosition;
		}
	}
}

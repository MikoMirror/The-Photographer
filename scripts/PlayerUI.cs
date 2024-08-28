using Godot;
using System;

public partial class PlayerUI : Control
{
	private const int MAX_JUMPS = 3;
	private int _remainingJumps = MAX_JUMPS;
	private TextureRect[] _jumpIndicators;
	private Label _restartLabel;
	private Label _teleportLabel;
	private bool _canTeleport = false;

	[Export]
	private Color InactiveColor = Colors.Gray;

	[Export]
	private Color ActiveColor = new Color(180 / 255f, 0f, 70 / 255f);
	
	[Export]
	private Color ReplenishColor = new Color(180 / 255f, 0f, 70 / 255f);

	[Signal]
	public delegate void TeleportRequestedEventHandler();

	public override void _Ready()
	{
		_jumpIndicators = new TextureRect[MAX_JUMPS];
		for (int i = 0; i < MAX_JUMPS; i++)
		{
			_jumpIndicators[i] = GetNode<TextureRect>($"Jump{i + 1}");
			_jumpIndicators[i].Modulate = ActiveColor; // Start with all jumps active
		}

		_restartLabel = new Label();
		_restartLabel.Text = "Press 'G' to restart the level";
		_restartLabel.Visible = false;
		_restartLabel.AnchorBottom = 1;
		_restartLabel.AnchorTop = 1;
		_restartLabel.AnchorLeft = 0.5f;
		_restartLabel.AnchorRight = 0.5f;
		_restartLabel.GrowHorizontal = GrowDirection.Both;
		_restartLabel.VerticalAlignment = VerticalAlignment.Bottom;
		_restartLabel.HorizontalAlignment = HorizontalAlignment.Center;
		AddChild(_restartLabel);

		_teleportLabel = GetNode<Label>("teleportLabel");
		_teleportLabel.Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (_canTeleport && @event.IsActionPressed("ui_interaction"))
		{
			EmitSignal(SignalName.TeleportRequested);
		}
	}

	public bool CanJump() => _remainingJumps > 0;

	public void UseJump()
	{
		if (CanJump())
		{
			_remainingJumps--;
			UpdateIndicator();
		}
	}

	public void ReplenishOneJump()
	{
		if (_remainingJumps < MAX_JUMPS)
		{
			_remainingJumps++;
			UpdateIndicator(true);
		}
	}

	private void UpdateIndicator(bool isReplenishing = false)
	{
		for (int i = 0; i < MAX_JUMPS; i++)
		{
			if (i < _remainingJumps)
			{
				if (isReplenishing && i == _remainingJumps - 1)
				{
					_jumpIndicators[i].Modulate = ReplenishColor;
					CreateTween().TweenProperty(_jumpIndicators[i], "modulate", ActiveColor, 0.5f).SetDelay(0.5f);
				}
				else
				{
					_jumpIndicators[i].Modulate = ActiveColor;
				}
			}
			else
			{
				_jumpIndicators[i].Modulate = InactiveColor;
			}
		}
		
		_restartLabel.Visible = _remainingJumps == 0;
	}

	public void ResetJumps()
	{
		_remainingJumps = MAX_JUMPS;
		UpdateIndicator();
	}

	public void UpdateUI()
	{
		UpdateIndicator();
	}

	public void ShowTeleportLabel(bool show)
	{
		_canTeleport = show;
		_teleportLabel.Visible = show;
	}
}

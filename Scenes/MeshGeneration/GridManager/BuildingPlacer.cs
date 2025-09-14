using Godot;
using System;

public partial class BuildingPlacer : Node3D
{
	[Export] public PackedScene BuildingScene;
	[Export] public Vector2I BuildingSize = new Vector2I(3, 2);

	private MeshInstance3D ghost;
    private StandardMaterial3D ghostMat;
    private GridManager grid;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		grid = GetNode<GridManager>("../GridManager");

		ghost = new MeshInstance3D();
        ghost.Mesh = new BoxMesh { Size = new Vector3(BuildingSize.X * grid.CellSize, 1, BuildingSize.Y * grid.CellSize) };

        ghostMat = new StandardMaterial3D();
        ghostMat.AlbedoColor = new Color(0, 1, 0, 0.5f);
        ghostMat.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
        ghost.SetSurfaceOverrideMaterial(0, ghostMat);

        AddChild(ghost);
	}

	public override void _Process(double delta)
	{
		var camera = GetViewport().GetCamera3D();
		if (camera == null) return;

		var mousePos = GetViewport().GetMousePosition();
		var from = camera.ProjectRayOrigin(mousePos);
		var dir = camera.ProjectRayNormal(mousePos);
		var to = from + dir * 1000;

		var spaceState = GetWorld3D().DirectSpaceState;

		var query = PhysicsRayQueryParameters3D.Create(from, to);
		var result = spaceState.IntersectRay(query);
		GD.Print("from: " + from + " to: " + to + " result:" + result);

		if (result.Count > 0)
		{
			Vector3 pos = (Vector3)result["position"];
			Vector2I gridPos = grid.WorldToGrid(pos);

			ghost.Position = grid.GridToWorld(gridPos) 
							+ new Vector3(grid.CellSize * BuildingSize.X / 2, 0, grid.CellSize * BuildingSize.Y / 2);

			if (grid.CanPlace(gridPos, BuildingSize.X, BuildingSize.Y))
				ghostMat.AlbedoColor = new Color(0, 1, 0, 0.5f);
			else
				ghostMat.AlbedoColor = new Color(1, 0, 0, 0.5f);
		}
	}

}

using Godot;
using System;

public partial class GridManager : Node3D
{
	[Export] public int GridWidth = 5;
    [Export] public int GridHeight = 5;
    [Export] public float CellSize = 2.0f;

    public bool[,] grid;

    public override void _Ready()
    {
        grid = new bool[GridWidth, GridHeight];
		GD.Print(grid);
        DrawGridVisual();
    }

	private void DrawGridVisual()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                var mesh = new MeshInstance3D();
                mesh.Mesh = new PlaneMesh { Size = new Vector2(CellSize, CellSize) };
                mesh.Position = new Vector3(x * CellSize, 0, y * CellSize);

                var mat = new StandardMaterial3D();
                mat.AlbedoColor = new Color(0.3f, 0.3f, 0.3f, 0.2f);
                mat.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
                mesh.SetSurfaceOverrideMaterial(0, mat);

				// StaticBody3D + CollisionShape3D hinzufügen
				var body = new StaticBody3D();
				var shape = new CollisionShape3D();
				shape.Shape = new BoxShape3D { Size = new Vector3(CellSize, 0.1f, CellSize) };

				body.AddChild(shape);
				body.Position = mesh.Position;
				AddChild(body);

				AddChild(mesh);
            }
        }
    }

    public Vector3 GridToWorld(Vector2I pos) =>
        new Vector3(pos.X * CellSize, 0, pos.Y * CellSize);

    public Vector2I WorldToGrid(Vector3 world) =>
        new Vector2I(
            Mathf.RoundToInt(world.X / CellSize),
            Mathf.RoundToInt(world.Z / CellSize)
        );

    public bool CanPlace(Vector2I pos, int w, int h)
    {
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                int gx = pos.X + x;
                int gy = pos.Y + y;
                if (gx < 0 || gy < 0 || gx >= GridWidth || gy >= GridHeight) return false;
                if (grid[gx, gy]) return false;
            }
        return true;
    }

    public void Place(Vector2I pos, int w, int h)
    {
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                grid[pos.X + x, pos.Y + y] = true;
    }
}

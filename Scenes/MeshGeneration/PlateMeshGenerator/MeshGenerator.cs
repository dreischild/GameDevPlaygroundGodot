using Godot;
using System;
using System.Collections.Generic;

/**
 * Tutorial unter: https://www.youtube.com/watch?v=Sg4mrxbiPyU
 */

[Tool]
public partial class MeshGenerator : MeshInstance3D
{
	/** https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_exports.html */
	[Export]
	public bool Reload
	{
		get => _reload;
		set
		{
			RegenerateMesh();
			_reload = false;
		}
	}
	private bool _reload = false;

	private ArrayMesh arrayMesh = null;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RegenerateMesh();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void RegenerateMesh()
	{
		GD.Print("RegenerateMesh called");
		if (arrayMesh == null)
		{
			arrayMesh = new ArrayMesh();
			Mesh = arrayMesh;
		}

		Godot.Collections.Array surfaceArray = [];
		surfaceArray.Resize((int)Mesh.ArrayType.Max);

		List<Vector3> vertices = new List<Vector3>
		{
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(1, 0, 1),
			new Vector3(0, 0, 1),
		};

		List<Vector2> uvs = new List<Vector2>
		{
			Vector2.Up,
			Vector2.Up,
			Vector2.Up,
			Vector2.Up,
		};

		List<Vector3> normals = new List<Vector3>
		{
			Vector3.Up,
			Vector3.Up,
			Vector3.Up,
			Vector3.Up,
		};

		/** Verbindung der Vertices (Es muss gegen den Uhrzeigersinn sein, damit die Normalen nach oben zeigen) */
		List<int> indices = new List<int>
		{
			0, 1, 2,
			0, 2, 3,
		};

		surfaceArray[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
		surfaceArray[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
		surfaceArray[(int)Mesh.ArrayType.Normal] = normals.ToArray();
		surfaceArray[(int)Mesh.ArrayType.Index] = indices.ToArray();

		arrayMesh.ClearSurfaces();
		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);

		GD.Print("Regenerating mesh...");
		GD.Print(surfaceArray);
	}
}
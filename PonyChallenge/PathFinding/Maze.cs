using System.Collections.Generic;
using PonyApiClient;
using PonyApiClient.Constants;

namespace PonyChallenge.PathFinding
{
	public class Maze
	{
		public Node[,] Grid { get; set; }

		//Pony position node
		public Node Start { get; set; }

		//Exit node
		public Node End { get; set; }

		//Forbidden node
		public Node Block { get; set; }

		public List<Node> GetNeighbors(Node node)
		{
			var neighbors = new List<Node>();
			foreach (var direction in node.AllowedDirections)
			{
				switch (direction)
				{
					case Direction.East:
						neighbors.Add(Grid[node.X + 1, node.Y]);
						break;
					case Direction.West:
						neighbors.Add(Grid[node.X - 1, node.Y]);
						break;
					case Direction.North:
						neighbors.Add(Grid[node.X, node.Y - 1]);
						break;
					case Direction.South:
						neighbors.Add(Grid[node.X, node.Y + 1]);
						break;
				}
			}

			return neighbors;
		}

		public static Maze Build(int ponyPosition, int domokunPosition, int endPoint, List<List<string>> walls, int width, int height)
		{
			var maze = new Maze
			{
				Grid = new Node[width, height]
			};

			for (var i = 0; i < walls.Count; i++)
			{
				var currentWallInfo = walls[i];
				var allowedDirections = new List<string>();
				var x = i % width;
				var y = i / width;

				if (!currentWallInfo.Contains(Direction.North))
				{
					allowedDirections.Add(Direction.North);
				}

				if (!currentWallInfo.Contains(Direction.West))
				{
					allowedDirections.Add(Direction.West);
				}

				var easternNode = i + 1 < walls.Count ? walls[i + 1] : null;

				if (easternNode != null && !easternNode.Contains(Direction.West))
				{
					allowedDirections.Add(Direction.East);
				}

				var southernNode = i + width < walls.Count ? walls[i + width] : null;

				if (southernNode != null && !southernNode.Contains(Direction.North))
				{
					allowedDirections.Add(Direction.South);
				}

				if (y == height - 1)
				{

				}

				var node = new Node(x, y, allowedDirections.ToArray());

				maze.Grid[x, y] = node;

				if (i == ponyPosition)
				{
					maze.Start = node;
				}

				if (i == endPoint)
				{
					maze.End = node;
				}

				if (i == domokunPosition)
				{
					maze.Block = node;
				}
			}

			return maze;
		}
		
		public static List<string> GetDirections(Node start, List<Node> nodes)
		{
			var prevNode = start;
			var directions = new List<string>();

			foreach (var node in nodes)
			{
				if (prevNode != null)
				{
					directions.Add(GetDirection(prevNode, node));
				}

				prevNode = node;
			}

			return directions;
		}

		private static string GetDirection(Node from, Node to)
		{
			var diffX = from.X - to.X;
			var diffY = from.Y - to.Y;

			if (diffX == -1)
			{
				return Direction.East;
			}

			if (diffX == 1)
			{
				return Direction.West;
			}

			if (diffY == 1)
			{
				return Direction.North;
			}

			if (diffY == -1)
			{
				return Direction.South;
			}

			return string.Empty;
		}
	}
}

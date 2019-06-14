using System;
using System.Collections.Generic;

namespace PonyChallenge.PathFinding
{
	public class PathFinder : IPathFinder
	{
		/// <summary>
		/// Find path based on A* algorithm
		/// https://en.wikipedia.org/wiki/A*_search_algorithm
		/// </summary>
		/// <param name="maze"></param>
		/// <returns></returns>
		public List<Node> Find(Maze maze)
		{
			//todo: min heap is a better choice to store openList
			var openList = new List<Node>();
			var closedSet = new HashSet<Node>();

			openList.Add(maze.Start);

			while (openList.Count > 0)
			{
				var currentNode = openList[0];

				foreach (var node in openList)
				{
					if (node.FCost < currentNode.FCost ||
						node.FCost == currentNode.FCost && node.HCost < currentNode.HCost)
					{
						currentNode = node;
					}
				}

				openList.Remove(currentNode);
				closedSet.Add(currentNode);

				if (currentNode.Equals(maze.Block))
				{
					continue;
				}
				
				if (currentNode.Equals(maze.End))
				{
					return GetPath(maze.Start, maze.End);
				}

				foreach (var neighbor in maze.GetNeighbors(currentNode))
				{
					if (closedSet.Contains(neighbor))
					{
						continue;
					}

					var movementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

					if (movementCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
					{
						neighbor.GCost = movementCostToNeighbor;
						neighbor.HCost = GetDistance(neighbor, maze.End);
						neighbor.ParentNode = currentNode;

						if (!openList.Contains(neighbor))
						{
							openList.Add(neighbor);
						}
					}
				}
			}

			return new List<Node>();
		}

		private List<Node> GetPath(Node start, Node end)
		{
			var path = new List<Node>();
			var currentNode = end;

			while (!currentNode.Equals(start))
			{
				path.Add(currentNode);
				currentNode = currentNode.ParentNode;
			}

			path.Reverse();

			return path;
		}

		/// <summary>
		/// Heuristic function
		/// </summary>
		/// <param name="nodeA"></param>
		/// <param name="nodeB"></param>
		/// <returns></returns>
		private int GetDistance(Node nodeA, Node nodeB)
		{
			var diffX = nodeA.X - nodeB.X;
			var diffY = nodeA.Y - nodeB.Y;

			return (int)Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));
		}
	}
}

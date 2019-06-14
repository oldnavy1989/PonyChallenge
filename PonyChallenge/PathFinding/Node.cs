using System;
using PonyApiClient;
using PonyApiClient.Constants;

namespace PonyChallenge.PathFinding
{
	public class Node: IEquatable<Node>
	{
		//Position on the grid
		public readonly int X;
		public readonly int Y;

		public int FCost => GCost + HCost;

		//Cost to get here from starting node
		public int GCost { get; set; }

		//Assuming cost to get to the target from this node
		public int HCost { get; set; }

		/// <summary>
		/// List of possible <see cref="Direction"/>
		/// </summary>
		public string[] AllowedDirections;

		public Node ParentNode { get; set; }

		public Node(int x, int y, string[] allowedDirections)
		{
			X = x;
			Y = y;
			AllowedDirections = allowedDirections;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Node) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X * 397) ^ Y;
			}
		}

		public bool Equals(Node other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return X == other.X && Y == other.Y;
		}
	}
}

using System.Collections.Generic;

namespace PonyChallenge.PathFinding
{
	public interface IPathFinder
	{
		List<Node> Find(Maze maze);
	}
}
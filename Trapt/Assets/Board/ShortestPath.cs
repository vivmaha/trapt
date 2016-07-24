using System;
using System.Collections.Generic;
using System.Linq;

public class ShortestPath<T>
{
    private Func<T, T[]> getAdjacentNodes;

    public ShortestPath(Func<T, T[]> getAdjacentNodes)
    {
        this.getAdjacentNodes = getAdjacentNodes;
    }

    public T FindAdjacentNodeClosestToDestination(T fromNode, Func<T, bool> isDestinationNode)
    {
        var visitedNodes = new HashSet<T>();
        var visitingNodes = new List<T>() { fromNode };
        var visitedFromNodes = new Dictionary<T, T>();
        T destinationNode = default(T);
        while (destinationNode == null)
        {
            var visitNextNodes = new List<T>();
            foreach (var visitingNode in visitingNodes)
            {
                var adjacentNodes = this
                    .getAdjacentNodes(visitingNode)
                    .Where(adjacentNode => !visitedNodes.Contains(adjacentNode))
                    .ToArray();

                foreach (var adjacentNode in adjacentNodes)
                {
                    visitedFromNodes[adjacentNode] = visitingNode;
                    visitedNodes.Add(adjacentNode);
                    if (isDestinationNode(adjacentNode))
                    {
                        destinationNode = adjacentNode;
                    }
                }
                visitNextNodes.AddRange(adjacentNodes);
            }
            visitingNodes = visitNextNodes;
        }

        var adjacentNodeClosestToDestination = destinationNode;
        while (!visitedFromNodes[adjacentNodeClosestToDestination].Equals(fromNode))
        {
            adjacentNodeClosestToDestination = visitedFromNodes[adjacentNodeClosestToDestination];
        }
        return adjacentNodeClosestToDestination;
    }
}
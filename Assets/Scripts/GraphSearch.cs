using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphSearch 
{

    public static BFSResult BFSGetRange(Vector3Int startPoint, int movementPoints)
    {
        //Diccionario nodo actual-nodo anterior (nodo padre, nodo del que tenimos)
        Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
        //Diccionario del coste total para moverse hacia la casilla 
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
        //lista de nodos restantes (cola)
        Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        while (nodesToVisitQueue.Count > 0){

        
            Vector3Int currentNode = nodesToVisitQueue.Dequeue();
            foreach (var neighbourPosition in HexGrid.instance.GetNeighboursFor(currentNode))
            {
                if(HexGrid.instance.GetTileAt(neighbourPosition).IsObstacle()) continue;

                int nodeCost =  HexGrid.instance.GetTileAt(neighbourPosition).GetCost();
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                if (newCost <= movementPoints)
                {
                    //si la posicion vecina no existe en el diccionario
                    if (!visitedNodes.ContainsKey(neighbourPosition))
                    {
                        visitedNodes[neighbourPosition] = currentNode;
                        costSoFar[neighbourPosition] = newCost;
                        nodesToVisitQueue.Enqueue(neighbourPosition);
                    }
                    //si existe. Podemos acceder de una manera mas abrata con otro camino
                    else if (costSoFar[neighbourPosition] > newCost)
                    {
                        costSoFar[neighbourPosition] = newCost;
                        visitedNodes[neighbourPosition] = currentNode;
                    }
                }

            }
        }

        //visitedNodes.Remove(startPoint);
        
        return new BFSResult { visitedNodesDict = visitedNodes };
    }
    
    public static List<Vector3Int> GeneratePathBFS(Vector3Int current, Dictionary<Vector3Int,
        Vector3Int?> visitedNodesDict)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(current);
        
        
        /*foreach(var items in visitedNodesDict)
        {
            Debug.Log("You have " + items.Value + " " + items.Key);

        }*/
        
        while (visitedNodesDict[current] != null)
        {
            path.Add(visitedNodesDict[current].Value);
            current = visitedNodesDict[current].Value;
        }
        path.Reverse();
        return path.Skip(1).ToList();
    }

}


public struct BFSResult
{
    public Dictionary<Vector3Int, Vector3Int?> visitedNodesDict;

    public List<Vector3Int> GetPathTo(Vector3Int destination)
    {
        if (visitedNodesDict.ContainsKey(destination) == false)
            return new List<Vector3Int>();
        return GraphSearch.GeneratePathBFS(destination, visitedNodesDict);
    }

    public bool isHexPositionInRange(Vector3Int position)
    {
        return visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetRangePositions()
        => visitedNodesDict.Keys;
    
}






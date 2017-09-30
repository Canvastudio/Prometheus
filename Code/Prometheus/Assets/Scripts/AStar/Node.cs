using UnityEngine;
using System.Collections;

namespace Pathfinding
{   
    [System.Serializable]
    public class Node
    {
        //Node's position in the grid
        public int x;
        public int y;
        public int z;

		//current node Cost
		public float nCost;

        //Node's costs for pathfinding purposes
        public float hCost;

		//parent gCost
        public float gCost;
        
        public float fCost
        {
            get //the fCost is the gCost+hCost so we can get it directly this way
            {
                return gCost + nCost;
            }
        }

        public Node parentNode;

        public bool isWalkable = true;
        
        //Reference to the world object so we can have the world position of the node among other things
        public MonoBehaviour behavirour;

        //Types of nodes we can have, we will use this later on a case by case examples
        public NodeType nodeType;

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                var n = obj as Node;

                return (x == n.x && y == n.y && z == n.z);
            }
            else
            {
                return false;
            }
        }
        public enum NodeType
        {
            ground,
            air
        }
    }
}

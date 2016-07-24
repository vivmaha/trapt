using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {
    public Tile originTile;
    public Enemy enemy;

    public int size = 5;
    
    class TilePosition
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            TilePosition other = (TilePosition)obj;
            if (other == null)
            {
                return false;
            }


            return
                this.X == other.X &&
                this.Y == other.Y;
        }        

        public TilePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    private Dictionary<TilePosition, Tile> tilePositions;
    private TilePosition enemyPosition;

    private bool IsTileOnEdge(TilePosition position)
    {
        return
            Math.Abs(position.X) == this.size ||
            Math.Abs(position.Y) == this.size;
    }

    private bool IsTileOnBoard(TilePosition position)
    {
        return
            Math.Abs(position.X) <= this.size &&
            Math.Abs(position.Y) <= this.size;
    }

    private void MoveEnemy()
    {        
        var visited = new HashSet<TilePosition>();
        var toVisit = new List<TilePosition>() { this.enemyPosition };
        var previous = new Dictionary<TilePosition, TilePosition>();
        TilePosition closestExit = null;
        while (closestExit == null)
        {
            var nextToVisit = new List<TilePosition>();
            foreach (var tile in toVisit)
            {
                var neighbours = new List<TilePosition>();
                neighbours.Add(new TilePosition(tile.X + 1, tile.Y + 0));
                neighbours.Add(new TilePosition(tile.X - 1, tile.Y + 0));
                neighbours.Add(new TilePosition(tile.X + 0, tile.Y - 1));
                neighbours.Add(new TilePosition(tile.X + 0, tile.Y + 1));
                if (Math.Abs(tile.Y) % 2 == 1)
                {
                    neighbours.Add(new TilePosition(tile.X + 1, tile.Y - 1));
                    neighbours.Add(new TilePosition(tile.X + 1, tile.Y + 1));                    
                } else
                {
                    neighbours.Add(new TilePosition(tile.X - 1, tile.Y - 1));
                    neighbours.Add(new TilePosition(tile.X - 1, tile.Y + 1));
                }


                neighbours = neighbours.Where(
                    neighbour =>
                        this.IsTileOnBoard(neighbour) &&
                        !visited.Contains(neighbour) && 
                        this.tilePositions[neighbour].isActiveAndEnabled
                ).ToList();

                foreach (var neighbour in neighbours)
                {
                    previous[neighbour] = tile;
                    visited.Add(neighbour);
                    if (this.IsTileOnEdge(neighbour))
                    {
                        closestExit = neighbour;
                    }
                }
                nextToVisit.AddRange(neighbours);
            }
            toVisit = nextToVisit;
        }
        var nextEnemyPosition = closestExit;
        while (previous[nextEnemyPosition] != this.enemyPosition)
        {
            nextEnemyPosition = previous[nextEnemyPosition];
        }

        this.enemyPosition = nextEnemyPosition;
        var enemyTile = this.tilePositions[this.enemyPosition];
        this.enemy.MoveTo(enemyTile.transform.position);
    }
    
	public void Start () {

        this.tilePositions = new Dictionary<TilePosition, Tile>();
        

	    for (int i = -this.size; i <= this.size; i++)
        {
            for (int j = -this.size; j <= this.size; j++)
            {
                var newTile = Instantiate<Tile>(originTile);
                
                newTile.transform.position = new Vector2(originTile.transform.position.x + i, originTile.transform.position.y + j * (float)(Math.Sqrt(3) / 2.0));
                
                if (Math.Abs(j) % 2 == 1)
                {
                    newTile.transform.position += new Vector3(0.5f, 0, 0);
                }

                newTile.transform.parent = this.transform;

                var tilePosition = new TilePosition(i, j);
                this.tilePositions[tilePosition] = newTile;

                if (this.IsTileOnEdge(tilePosition))
                {
                    newTile.GetComponent<SpriteRenderer>().color = Color.gray;
                    newTile.IsImmortal = true;
                }

                if (UnityEngine.Random.value < 0.1)
                {
                    newTile.OnMouseDown();
                }

                newTile.OnDeactivate += (tile, e) =>
                {
                    this.MoveEnemy();
                };
            }
        }
        Destroy(originTile);

        this.enemyPosition = new TilePosition(0, 0);
	}
}

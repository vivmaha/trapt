using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour {
    public Tile originTile;
    public Enemy enemy;
    public int size = 5;

    private Dictionary<TilePosition, Tile> tilePositions;
    private TilePosition enemyPosition;
    private ShortestPath<TilePosition> shortestPath;

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
        this.enemyPosition = this.shortestPath.FindAdjacentNodeClosestToDestination(this.enemyPosition, this.IsTileOnEdge);
        if (this.enemyPosition == null)
        {
            Game.level++;
            SceneManager.LoadScene("Win");
        }
        var enemyTile = this.tilePositions[this.enemyPosition];
        this.enemy.MoveTo(enemyTile.transform.position);
    }
    
	public void Start () {

        this.tilePositions = new Dictionary<TilePosition, Tile>();

        this.shortestPath = new ShortestPath<TilePosition>(
            getAdjacentNodes: position =>
            {   
                var adjacentNodes = new List<TilePosition>();
                adjacentNodes.Add(new TilePosition(position.X + 1, position.Y + 0));
                adjacentNodes.Add(new TilePosition(position.X - 1, position.Y + 0));
                adjacentNodes.Add(new TilePosition(position.X + 0, position.Y - 1));
                adjacentNodes.Add(new TilePosition(position.X + 0, position.Y + 1));
                if (Math.Abs(position.Y) % 2 == 1)
                {
                    adjacentNodes.Add(new TilePosition(position.X + 1, position.Y - 1));
                    adjacentNodes.Add(new TilePosition(position.X + 1, position.Y + 1));
                }
                else
                {
                    adjacentNodes.Add(new TilePosition(position.X - 1, position.Y - 1));
                    adjacentNodes.Add(new TilePosition(position.X - 1, position.Y + 1));
                }

                adjacentNodes = adjacentNodes.Where(
                    neighbour =>
                        this.IsTileOnBoard(neighbour) &&
                        this.tilePositions[neighbour].isActiveAndEnabled
                ).ToList();

                return adjacentNodes.ToArray();
            });
        

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

                var pOfEmpty = Mathf.Lerp((float)0.1, 0, (Game.level - 1) / 10.0f);
                                
                if (UnityEngine.Random.value < pOfEmpty)
                {
                    newTile.OnMouseDown();
                }

                newTile.OnDeactivate += (tile, e) =>
                {
                    this.MoveEnemy();
                };
            }
        }
        Destroy(originTile.gameObject);

        this.enemyPosition = new TilePosition(0, 0);
	}
}

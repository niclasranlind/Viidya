using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VidyaTutorial
{
    class Maze
    {
        public const int mazeHeight = 20;
        public const int mazeWidth = 20;

        GraphicsDevice device;
        VertexBuffer floorBuffer;
        Color[] floorColors = new Color[2] { Color.White, Color.BlueViolet };

        private Random rand = new Random();
        public MazeCell[,] MazeCells = new MazeCell[mazeWidth, mazeHeight];

        public Maze(GraphicsDevice graphicsDevice)
        {
            this.device = graphicsDevice;
            BuildFloorBuffer();
            for (int x = 0; x < mazeWidth; x++)
                for (int z = 0; z < mazeHeight; z++)
                {
                    MazeCells[x, z] = new MazeCell();
                }
            GenerateMaze();
        }

        private void BuildFloorBuffer()
        {
            List<VertexPositionColor> vertexList =
            new List<VertexPositionColor>();
            int counter = 0;
            for (int x = 0; x < mazeWidth; x++)
            {
                counter++;
                for (int z = 0; z < mazeHeight; z++)
                {
                    counter++;
                    foreach (VertexPositionColor vertex in FloorTile(x, z, floorColors[counter % 2]))
                    {
                        vertexList.Add(vertex);
                    }
                }
            }
            floorBuffer = new VertexBuffer(
            device,
            VertexPositionColor.VertexDeclaration,
            vertexList.Count,
            BufferUsage.WriteOnly);
            floorBuffer.SetData<VertexPositionColor>(vertexList.
            ToArray());
        }
        private List<VertexPositionColor> FloorTile(int xOffset,
        int zOffset,
        Color tileColor)
        {
            List<VertexPositionColor> vList = new List<VertexPositionColor>();

            vList.Add(
                    new VertexPositionColor(
                        new Vector3(0 + xOffset, 0, 0 + zOffset), tileColor)
                        );

            vList.Add(new VertexPositionColor(
            new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor));

            vList.Add(new VertexPositionColor(
            new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor));

            vList.Add(new VertexPositionColor(
            new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor));

            vList.Add(new VertexPositionColor(
            new Vector3(1 + xOffset, 0, 1 + zOffset), tileColor));

            vList.Add(new VertexPositionColor(
            new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor));

            return vList;
        }

        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = camera.View;
            effect.Projection = camera.projection;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(
                PrimitiveType.TriangleList,
                0,
                floorBuffer.VertexCount / 3);
            }
        }
        #region mazegeneration

        public void GenerateMaze()
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int z = 0; z < mazeHeight; z++)
                {
                    MazeCells[x, z].Walls[0] = true;
                    MazeCells[x, z].Walls[1] = true;
                    MazeCells[x, z].Walls[2] = true;
                    MazeCells[x, z].Walls[3] = true;
                    MazeCells[x, z].Visited = false;
                }
                MazeCells[0, 0].Visited = true;
                EvaluateCell(new Vector2(0, 0));
            }
        }

        private void EvaluateCell(Vector2 cell)
        {
            List<int> neighborCells = new List<int>();
            neighborCells.Add(0);
            neighborCells.Add(1);
            neighborCells.Add(2);
            neighborCells.Add(3);
            while (neighborCells.Count > 0)
            {
                int pick = rand.Next(0, neighborCells.Count);
                int selectedNeighbor = neighborCells[pick];
                neighborCells.RemoveAt(pick);
                Vector2 neighbor = cell;
                switch (selectedNeighbor)
                {
                    case 0: neighbor += new Vector2(0, -1);
                        break;
                    case 1: neighbor += new Vector2(1, 0);
                        break;
                    case 2: neighbor += new Vector2(0, 1);
                        break;
                    case 3: neighbor += new Vector2(-1, 0);
                        break;
                }
                if (
                (neighbor.X >= 0) &&
                (neighbor.X < mazeWidth) &&
                (neighbor.Y >= 0) &&
                (neighbor.Y < mazeHeight)
                )
                {
                    if (!MazeCells[(int)neighbor.X, (int)neighbor.Y].
                    Visited)
                    {
                        MazeCells[
                        (int)neighbor.X,
                        (int)neighbor.Y].Visited = true;
                        MazeCells[
                        (int)cell.X,
                        (int)cell.Y].Walls[selectedNeighbor] = false;
                        MazeCells[
                        (int)neighbor.X,
                        (int)neighbor.Y].Walls[
                        (selectedNeighbor + 2) % 4] = false;
                        EvaluateCell(neighbor);
                    }
                }
            }

        }

        #endregion

    }

}

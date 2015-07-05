using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Barley_Break
{
    
    class AI
    {
        int globalIndex = 0;
        public AI(Map map)
        {
            Point desiredLocation;
            while (!Map.CheckWin())
            {
                globalIndex++;
                desiredLocation = GetNeededPosition(globalIndex);
                if (desiredLocation.X == Map.CountCells - 1 && desiredLocation.Y <= Map.CountCells - 2)
                {
                    UpCornerConflict(desiredLocation);
                    globalIndex++;
                }
                else if (desiredLocation.Y == Map.CountCells - 1 && desiredLocation.X <= Map.CountCells - 2)
                {
                    DownCornerConflict(desiredLocation);
                }
                else MoveToDesiredPosition(globalIndex, desiredLocation);  
            }
        }

        public void UpCornerConflict(Point desiredLocation)
        {
            Point cellLocation = Map.FindCell(globalIndex);
            Point nearCellLocation = Map.FindCell(globalIndex + 1);
            if (Map.Table[cellLocation.X, cellLocation.Y].index != globalIndex || Map.Table[nearCellLocation.X, nearCellLocation.Y].index != globalIndex + 1)
            {
                Point nearDesiredLocation = GetNeededPosition(globalIndex + 1);
                desiredLocation.Y++;
                nearDesiredLocation.X--;
                MoveToDesiredPosition(globalIndex + 1, nearDesiredLocation);
                if (Map.Table[nearDesiredLocation.X + 1, nearDesiredLocation.Y].element == null)
                {
                    Map.MoveCell(new Point(nearDesiredLocation.X + 1, nearDesiredLocation.Y + 1));
                }
                cellLocation = Map.FindCell(globalIndex);
                if (nearDesiredLocation.X + 1 == cellLocation.X && nearDesiredLocation.Y == cellLocation.Y)
                {
                    GoNullX(0, new Point(nearDesiredLocation.X + 1, nearDesiredLocation.Y));
                    GoNullY(1, new Point(nearDesiredLocation.X + 1, nearDesiredLocation.Y));
                    MakeCycling(-1, -1, true);
                    MakeCycling(1, 1, true);
                    MoveToDesiredPosition(globalIndex + 1, nearDesiredLocation);
                    if (Map.Table[nearDesiredLocation.X + 1, nearDesiredLocation.Y].element == null)
                    {
                        Map.MoveCell(new Point(nearDesiredLocation.X + 1, nearDesiredLocation.Y + 1));
                    }
                }
                MoveToDesiredPosition(globalIndex, desiredLocation);
                GoNullX(1, desiredLocation);
                GoNullY(0, desiredLocation);
                MakeCycling(-1, -1, true);
            }
        }

        public void DownCornerConflict(Point desiredLocation)
        {
            Point cellLocation = Map.FindCell(globalIndex);
            Point nearCellLocation = Map.FindCell(globalIndex + Map.CountCells);
            if (Map.Table[cellLocation.X, cellLocation.Y].index != globalIndex || Map.Table[nearCellLocation.X, nearCellLocation.Y].index != globalIndex + Map.CountCells)
            {
                Point nearDesiredLocation = GetNeededPosition(globalIndex + Map.CountCells);
                desiredLocation.X++;
                nearDesiredLocation.Y--;
                MoveToDesiredPosition(globalIndex + Map.CountCells, nearDesiredLocation);
                if (Map.Table[nearDesiredLocation.X, nearDesiredLocation.Y + 1].element == null)
                {
                    Map.MoveCell(new Point(nearDesiredLocation.X + 1, nearDesiredLocation.Y + 1));
                }
                cellLocation = Map.FindCell(globalIndex);
                if (nearDesiredLocation.X == cellLocation.X && nearDesiredLocation.Y + 1 == cellLocation.Y)
                {
                    GoNullX(1, new Point(nearDesiredLocation.X, nearDesiredLocation.Y + 1));
                    GoNullY(0, new Point(nearDesiredLocation.X, nearDesiredLocation.Y + 1));
                    MakeCycling(-1, -1, false);
                    MakeCycling(1, 1, false);
                    MoveToDesiredPosition(globalIndex + Map.CountCells, nearDesiredLocation);
                    if (Map.Table[nearDesiredLocation.X, nearDesiredLocation.Y + 1].element == null)
                    {
                        Map.MoveCell(new Point(nearDesiredLocation.X + 1, nearDesiredLocation.Y + 1));
                    }
                }
                MoveToDesiredPosition(globalIndex, desiredLocation);
                GoNullX(0, desiredLocation);
                GoNullY(1, desiredLocation);
                MakeCycling(-1, -1, false);
            }

        }

        public void MakeCycling(int directionX, int directionY, bool Cycling) //Cycling = true - по часовой и Cycling = false - против часовой
        {
            Point nullCell = Map.FindCell(0);
            if (Cycling)
            {
                nullCell.Y += directionY;
                Map.MoveCell(new Point(nullCell.X, nullCell.Y));
                nullCell.X += directionX;
                Map.MoveCell(new Point(nullCell.X, nullCell.Y));
                nullCell.Y += directionY * -1;
                Map.MoveCell(new Point(nullCell.X, nullCell.Y));
            }
            else 
            {
                nullCell.X += directionX;
                Map.MoveCell(new Point(nullCell.X, nullCell.Y));
                nullCell.Y += directionY;
                Map.MoveCell(new Point(nullCell.X, nullCell.Y));
                nullCell.X += directionX * -1;
                Map.MoveCell(new Point(nullCell.X, nullCell.Y));
            }
        }

        public Point GetNeededPosition(int index)
        {
            for (int i = 1; i <= Map.CountCells; i++)
            {
                for (int j = 1; j <= Map.CountCells; j++)
                {
                    if (Map.Table[j, i].index == index)
                    {
                        return new Point(j, i);
                    }
                }
            }
            return new Point(4, 4);
        }

        public void MoveToDesiredPosition(int index, Point desiredLocation)
        {
            Point cellLocation = Map.FindCell(index);
            int direction;
            while (desiredLocation != cellLocation)
            {
                direction = GetDirection(cellLocation.X, desiredLocation.X);
                GoNullX(direction, cellLocation);
                if (direction == 0) { direction = GetDirection(cellLocation.Y, desiredLocation.Y); }
                else direction = 0;
                GoNullY(direction, cellLocation);
                Map.MoveCell(cellLocation);
                cellLocation = Map.FindCell(index);
            }
        }

        public int GetDirection(int cellColOrRow, int desiredColOrRow)
        {
            if (cellColOrRow < desiredColOrRow)
            {
                return 1;
            }
            else if (cellColOrRow > desiredColOrRow)
            {
                return -1;
            }
            else return 0;
        }

        public void GoNullX(int directionX, Point cellLocation) 
        {
            Point nullCell = Map.FindCell(0);
            int direction = GetDirection(nullCell.X, cellLocation.X + directionX);
            while (nullCell.X != cellLocation.X + directionX)
            {
                if ((nullCell.X + direction == cellLocation.X && nullCell.Y == cellLocation.Y) || !CheckOpportunity(new Point(nullCell.X + direction, nullCell.Y)))              
                {
                    if (nullCell.Y < Map.CountCells) { nullCell.Y++; }
                    else nullCell.Y--;
                }
                else nullCell.X += direction;
                Map.MoveCell(nullCell);
            }
        }

        public void GoNullY(int directionY, Point cellLocation) 
        {
            Point nullCell = Map.FindCell(0);
            int direction = GetDirection(nullCell.Y, cellLocation.Y + directionY);
            while (nullCell.Y != cellLocation.Y + directionY)
            {
                if ((nullCell.Y + direction == cellLocation.Y && nullCell.X == cellLocation.X) || !CheckOpportunity(new Point(nullCell.X, nullCell.Y + direction))) 
                {
                    if (nullCell.X < Map.CountCells) { nullCell.X++; }
                    else nullCell.X--;
                }
                else nullCell.Y += direction;
                Map.MoveCell(nullCell);
            }
        }

        public bool CheckOpportunity(Point nullCell)
        {
            if (Map.Table[nullCell.X, nullCell.Y].index == Map.Table[nullCell.X, nullCell.Y].element.TabIndex)
            {
                if (globalIndex <= Map.Table[nullCell.X, nullCell.Y].index) return true;
                else return false;
            }
            else return true;
        }
 
    }
}

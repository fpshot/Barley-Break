using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Barley_Break
{
    public class Map : Panel
    {
        public struct Ttable
        {
            public int index;
            public Control element;
        }

        public static Ttable[,] Table;
        public static int CountCells = 4;
        public int SizeBarleyBreak = 65;      
        public int[] FullTable;

        public Map()
        {                                
            Location = new Point(0, 0);
            BackColor = SystemColors.ButtonHighlight;
            Size = new Size(SizeBarleyBreak * CountCells, SizeBarleyBreak * CountCells);
            CreateMap();
        }

        public void CreateMap()
        {
            FullTable = getRandom(CountCells);
            Table = new Ttable[CountCells + 1, CountCells + 1];
            int x = 0;
            int y = 0;
            int cellCol = 1;
            int cellRow = 1;
            BarleyBreak button;
            for (int i = 0; i < CountCells * CountCells; i++)
            {
                if (FullTable[i] != 0)
                {
                    button = new BarleyBreak();
                    button.Location = new Point(x, y);
                    Table[cellCol, cellRow].element = button;
                    int r = FullTable[i];
                    button.TabIndex = r;
                    button.Text = r.ToString();
                    this.Controls.Add(button);
                }
                Table[cellCol, cellRow].index = i + 1;
                if (x == SizeBarleyBreak * (CountCells - 1))
                {
                    cellRow++;
                    cellCol = 1;
                    x = 0;
                    y += SizeBarleyBreak;
                }
                else
                {
                    cellCol++;
                    x += SizeBarleyBreak;
                }
            } 
        }

        public int[] getRandom(int n)
        {
            int length = n*n;
            Random rand = new Random();
            int[] startArr = new int[length];
            int[] resultArr = new int[length];
            for (int i=0; i<length; i++)
            {
                startArr[i] = i;
            }
            int temp;
            int index = 0;
            int lenghtTemp = length;
            while (lenghtTemp != 0)
            {
                temp = rand.Next(lenghtTemp);
                resultArr[index] = startArr[temp];
                for (int j = temp; j < lenghtTemp - 1; j++)
                {
                    startArr[j] = startArr[j + 1];
                }
                index++;
                lenghtTemp--;
            }
            if (!CheckSolvability(n,resultArr))
            {
                for (int i = length - 1; i >= 0; i--)
                {
                    if (resultArr[i] != i+1 && resultArr[i] != 0)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            if (i+1 == resultArr[j])
                            {
                                resultArr[j] = resultArr[i];
                                resultArr[i] = i+1;
                                return resultArr;
                            }
                        }
                    }
                }
            }
            return resultArr;
        }

        public bool CheckSolvability(int n,int[] array)
        {
            int[] tempoArray = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                tempoArray[i] = array[i];
            }
            int temp = 1;
            int index = 0;
            int count = 0;
            int space_position = 0;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (tempoArray[index] == 0) { space_position = i; }
                    else
                    {
                        if (tempoArray[index] != temp)
                        {
                            for (int z = index + 1; z < tempoArray.Length; z++)
                            {
                                if (tempoArray[z] == temp)
                                {
                                    tempoArray[z] = tempoArray[index];
                                    tempoArray[index] = temp;
                                    count++;
                                }
                            }
                        }
                        temp++;
                    }
                    index++;
                }
            }
            if (n % 2 == 0) { count = count + space_position; }
            if (count % 2 == 0) { return true; }
            else return false;
        }

        public static void MoveCell(Point cell)
        {
            Point nullCell = FindCell(0);
            if (Math.Abs(nullCell.X - cell.X) > 1 || Math.Abs(nullCell.Y - cell.Y) > 1) return;
            else if (Math.Abs(nullCell.X - cell.X) == 1 && Math.Abs(nullCell.Y - cell.Y) == 1) return;
            else
            {
                ((BarleyBreak)Table[cell.X, cell.Y].element).Animation(nullCell);
                Table[nullCell.X, nullCell.Y].element = Table[cell.X, cell.Y].element;
                Table[cell.X, cell.Y].element = null;
                if (CheckWin()) { MessageBox.Show("Победа!"); }
            }
        }

        public static Point FindCell(int index)
        {
            for (int i = 1; i <= CountCells; i++)
            {
                for (int j = 1; j <= CountCells; j++)
                {
                    if (Table[j, i].element != null)
                    {
                        if (Table[j, i].element.TabIndex == index)
                        {
                            return new Point(j, i);
                        }
                    }
                    else if(index == 0)
                    {
                        return new Point(j, i);
                    }
                }
            }
            return new Point(0,0); 
        }

        public static bool CheckWin()
        {
            for (int i = 1; i <= CountCells; i++)
            {
                for (int j = 1; j <= CountCells; j++)
                {
                    if (Table[j, i].element != null)
                    {
                        if (Table[j, i].element.TabIndex != Table[j, i].index)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }       
    }
}

using System;
using System.Collections.Generic;
using GameOfLifeXamarin.Enums;

namespace GameOfLifeXamarin.Models
{
    /// <summary>
    /// Represents game of life board and all business operations connected ti it.
    /// </summary>
    public class Board
    {
        public List<List<Field>> Fields { get; set; } = new List<List<Field>>();
        public int Width { get; set; }
        public int Height { get; set; }
        public int Generation = 1;
        public List<List<Change>> History { get; set; } = new List<List<Change>>();

        /// <summary>
        /// Create board of given size.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            for (int i = 0; i < Height; i++)
            {
                Fields.Add(new List<Field>());
                for (int j = 0; j < Width; j++)
                {
                    Fields[i].Add(new Field());
                }
            }
        }

        public Board() { }

        /// <summary>
        /// Recalculate all states to be in advanced mode.
        /// </summary>
        public void Recalculate()
        {
            BoardSpecialToNormal();
            ApplyPreviousStatus();
            ApplyFutureStatus();
        }

        /// <summary>
        /// Calculate next generation status.
        /// </summary>
        /// <param name="isAdvanced">true if calculation should be done in advance mode</param>
        public void NextGen(bool isAdvanced)
        {
            if (isAdvanced)
            {
                NextGenAdvance();
            }
            else
            {
                NextGenNormal();
            }
        }

        /// <summary>
        /// Restore previous generation.
        /// </summary>
        /// <param name="isAdvanced"></param>
        public void PreviousGen(bool isAdvanced)
        {
            if (isAdvanced)
            {
                PreviousGenAdvanced();
            }
            else
            {
                PreviousGenNormal();
            }
        }

        /// <summary>
        /// Calculate next gen and apply advanced status.
        /// </summary>
        public void NextGenAdvance()
        {
            BoardSpecialToNormal();
            NextGenNormal();
            ApplyPreviousStatus();
            ApplyFutureStatus();
        }

        /// <summary>
        /// Calculate next generation.
        /// </summary>
        public void NextGenNormal()
        {
            Generation++;
            List<Change> listOfChanges = CalculateChangeList();

            foreach (var change in listOfChanges)
            {
                Fields[change.XPos][change.YPos].FieldStatus = change.NewStatus;
            }

            History.Add(listOfChanges);
        }

        /// <summary>
        /// Restore previous generation from history.
        /// </summary>
        public void PreviousGenNormal()
        {
            if (Generation > 1)
            {
                List<Change> listOfChanges = History[History.Count - 1];
                History.Remove(listOfChanges);

                foreach (var change in listOfChanges)
                {
                    Fields[change.XPos][change.YPos].FieldStatus = change.OldStatus;
                }

                Generation--;
            }
        }

        /// <summary>
        /// Restore previous generation and apply advanced status.
        /// </summary>
        public void PreviousGenAdvanced()
        {
            if (Generation > 1)
            {
                BoardSpecialToNormal();
                PreviousGenNormal();
                ApplyPreviousStatus();
                ApplyFutureStatus();
            }
        }

        /// <summary>
        /// Cast board in special mode to normal.
        /// </summary>
        public void BoardSpecialToNormal()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Fields[i][j].SpecialToNormal();
                }
            }
        }

        /// <summary>
        /// Apply status connected to future states.
        /// </summary>
        public void ApplyFutureStatus()
        {
            List<Change> listOfChanges = CalculateChangeList();
            foreach (var change in listOfChanges)
            {
                if (change.NewStatus == Status.Alive)
                {
                    Fields[change.XPos][change.YPos].FieldStatus = Status.WillRise;
                }
                else
                {
                    Fields[change.XPos][change.YPos].FieldStatus = Status.WillDie;
                }
            }
        }

        /// <summary>
        /// Apply status connected to history.
        /// </summary>
        public void ApplyPreviousStatus()
        {
            if (Generation > 1)
            {
                List<Change> listOfChanges = History[History.Count - 1];
                foreach (var change in listOfChanges)
                {
                    if (change.NewStatus == Status.Alive)
                    {
                        Fields[change.XPos][change.YPos].FieldStatus = Status.Born;
                    }
                    else
                    {
                        Fields[change.XPos][change.YPos].FieldStatus = Status.Died;
                    }
                }
            }
        }

        /// <summary>
        /// Calculate all changes made in next generation
        /// </summary>
        /// <returns>List of changes in next generation.</returns>
        private List<Change> CalculateChangeList()
        {
            List<Change> listOfChanges = new List<Change>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if ((!Fields[i][j].IsAlive()) &&
                        (NumberOfNeighbors(i, j) == 3))
                    {
                        listOfChanges.Add(new Change(
                            i, j, Status.Dead, Status.Alive));
                    }
                    else if ((Fields[i][j].IsAlive()) &&
                             ((NumberOfNeighbors(i, j) != 3) && (
                                 NumberOfNeighbors(i, j) != 2)))
                    {
                        listOfChanges.Add(new Change(
                            i, j, Status.Alive, Status.Dead));
                    }
                }
            }

            return listOfChanges;
        }

        /// <summary>
        /// Calculate number of neighbors.
        /// </summary>
        /// <param name="yPos"> Y position of field</param>
        /// <param name="xPos"> X position of field</param>
        /// <returns></returns>
        private int NumberOfNeighbors(int yPos, int xPos)
        {
            int numberOfNeighbors = 0;
            if (xPos > 0)
            {
                if ((yPos > 0) && (Fields[yPos - 1][xPos - 1].IsAlive()))
                {
                    numberOfNeighbors++;
                }

                if (Fields[yPos][xPos - 1].IsAlive())
                {
                    numberOfNeighbors++;
                }

                if ((yPos < Height - 1) && (Fields[yPos + 1][xPos - 1].IsAlive()))
                {
                    numberOfNeighbors++;
                }
            }

            if ((yPos > 0) && (Fields[yPos - 1][xPos].IsAlive()))
            {
                numberOfNeighbors++;
            }

            if ((yPos < Height - 1) && (Fields[yPos + 1][xPos].IsAlive()))
            {
                numberOfNeighbors++;
            }

            if (xPos < Width - 1)
            {
                if ((yPos > 0) && (Fields[yPos - 1][xPos + 1].IsAlive()))
                {
                    numberOfNeighbors++;
                }

                if (Fields[yPos][xPos + 1].IsAlive())
                {
                    numberOfNeighbors++;
                }

                if ((yPos < Height - 1) && (Fields[yPos + 1][xPos + 1].IsAlive()))
                {
                    numberOfNeighbors++;
                }
            }

            return numberOfNeighbors;
        }


        /// <summary>
        /// Appends changes from outside board to history. Note that this should be call BEFORE change.
        /// </summary>
        /// <param name="field">Field to be changed</param>
        public void AddToLast(Field field)
        {
            if (Generation > 1)
            {
                List<Change> listOfChanges = History[History.Count - 1];

                Tuple<int, int> pos = FindPos(field);

                if (field.IsAlive())
                {
                    listOfChanges.Add(new Change(
                        pos.Item1, pos.Item2, Status.Alive, Status.Dead));
                }
                else
                {
                    listOfChanges.Add(new Change(
                        pos.Item1, pos.Item2, Status.Dead, Status.Alive));
                }
            }
        }

        /// <summary>
        /// Find coordinates of field
        /// </summary>
        /// <param name="field"></param>
        /// <returns>Tuple(ypos,xpos) position of field</returns>
        private Tuple<int, int> FindPos(Field field)
        {
            int i, j = 0;
            for (i = 0; i < Height; i++)
            {
                for (j = 0; j < Width; j++)
                {
                    if (Fields[i][j].Equals(field))
                        return new Tuple<int, int>(i,j);
                }
            }

            throw new ArgumentException();
        }
    }
}
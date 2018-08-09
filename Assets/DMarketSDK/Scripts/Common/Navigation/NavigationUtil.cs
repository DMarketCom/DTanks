using System.Collections.Generic;
using SHLibrary.AdditionalTypes;

namespace DMarketSDK.Common.Navigation
{
    public static class NavigationUtil
    {
        public static T GetTarget<T>(List<List<T>> grid, IntVector2 current, NavigationType navigationType, bool moveByCircle = true)
        where T : class 
        {
            if (navigationType == NavigationType.Left)
            {
                var isFirstInRow = current.X == 0;
                if (isFirstInRow && moveByCircle)
                {
                    var isFirstRow = current.Z == 0;
                    if (isFirstRow)
                    {
                        current.Z = grid.Count - 1;
                    }
                    else
                    {
                        current.Z--;
                    }
                    current.X = grid[current.Z].Count - 1;
                }
                else
                {
                    current.X = isFirstInRow ? 0 : current.X - 1;
                }
            }
            else if (navigationType == NavigationType.Right)
            {
                var lastIndexInRow = grid[current.Z].Count - 1;
                var isLastInRow = current.X == lastIndexInRow;
                if (isLastInRow && moveByCircle)
                {
                    var isLastRow = current.Z == grid.Count - 1;
                    if (isLastRow)
                    {
                        current.Z = 0;
                    }
                    else
                    {
                        current.Z++;
                    }
                    current.X = 0;
                }
                else
                {
                    current.X = isLastInRow ? lastIndexInRow : current.X + 1;
                }
            }
            else if (navigationType == NavigationType.Up)
            {
                var isFirstInColumn = current.Z == 0;
                var targetPosZ = current.Z;
                if (isFirstInColumn && moveByCircle)
                {
                    var lastRowIndex = grid.Count - 1;
                    targetPosZ = grid[lastRowIndex].Count > current.X ?
                        lastRowIndex : current.Z;
                }
                else
                {
                    targetPosZ = isFirstInColumn ? 0 : current.Z - 1;
                }
                current.Z = grid[targetPosZ].Count > current.X ?
                    targetPosZ : current.Z;
            }
            else if (navigationType == NavigationType.Down)
            {
                var isLastInColumn = current.Z == grid.Count - 1;
                var targetPosZ = current.Z;
                if (isLastInColumn && moveByCircle)
                {
                    var firstRowIndex = 0;
                    targetPosZ = grid[firstRowIndex].Count > current.X ?
                        firstRowIndex : current.Z;
                }
                else
                {
                    targetPosZ = isLastInColumn ? 0 : current.Z + 1;
                }
                current.Z = grid[targetPosZ].Count > current.X ?
                    targetPosZ : current.Z;
            }

            if (current.Z > grid.Count && current.X > grid[current.Z].Count)
            {
                return grid[current.Z][current.X];
            }
            else
            {
                return null;
            }
        }
    }
}
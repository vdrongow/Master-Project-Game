using System.Linq;

namespace SortingAlgorithms
{
    public sealed class BubbleSort : SortingAlgorithm
    {
        protected override void PrepareSteps()
        {
            Steps.Clear();
            // break down the algorithm into steps 
            var array = ArrayView.ArrayElements.Select(x => x.Value).ToArray();
            var isSorted = false;
            while (!isSorted)
            {
                isSorted = true;
                for (var i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        isSorted = false;
                        Steps.Add((i, i + 1, swap: true));
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                    } 
                    else
                    {
                        Steps.Add((i, i + 1, swap: false));
                    }
                }
            }
        }
    }
}
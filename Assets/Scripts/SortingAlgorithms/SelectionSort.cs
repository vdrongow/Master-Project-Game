using System.Linq;
using Enums;

namespace SortingAlgorithms
{
    public sealed class SelectionSort : SortingAlgorithm
    {
        protected override void PrepareSteps()
        {
            Steps.Clear();
            // break down the Selection Sort algorithm into steps
            var array = ArrayView.ArrayElements.Select(x => x.Value).ToArray();
            for (var i = 0; i < array.Length - 1; i++)
            {
                var minIndex = i;
                for (var j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    Steps.Add((i, minIndex, swap: true, i));
                    ArrayView.SwapElements(i, minIndex);
                }
                else
                {
                    Steps.Add((i, minIndex, swap: false, i));
                }
            }
        }

        protected override EBarEffect GetEffect(int index, int currentStepIndex, int end)
        {
            throw new System.NotImplementedException();
        }

        protected override EBarEffect GetEffect(int index, int currentIndex, int minIndex, int end)
        {
            if (index == currentIndex)
            {
                return EBarEffect.Highlight;
            }
            if (index == minIndex)
            {
                return EBarEffect.HighlightCurrent;
            }
            if (index <= end)
            {
                return EBarEffect.Sorted;
            }
            return EBarEffect.None;
        }
    }
}
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

        protected override void ApplyEffects((int index1, int index2, bool swap, int end) step)
        {
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                var effect = GetEffect(i, step.index1, step.index2, step.end);
                ArrayView.ApplyBarEffect(i, effect);
            }
        }

        private EBarEffect GetEffect(int i, int currentIndex, int minIndex, int end)
        {
            if (i == currentIndex)
            {
                return EBarEffect.Highlight;
            }
            if (i == minIndex)
            {
                return EBarEffect.HighlightCurrent;
            }
            if (i <= end)
            {
                return EBarEffect.Sorted;
            }
            return EBarEffect.None;
        }
    }
}
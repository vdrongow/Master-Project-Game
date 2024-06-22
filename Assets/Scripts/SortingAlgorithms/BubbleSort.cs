using System.Linq;
using Enums;

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
            var end = array.Length;
            while (!isSorted)
            {
                isSorted = true;
                for (var i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        isSorted = false;
                        Steps.Add((i, i + 1, swap: true, end));
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                    } 
                    else
                    {
                        // add a step without swapping to highlight the elements that are compared, but only if they are not already sorted
                        if(i + 1 < end)
                        {
                            Steps.Add((i, i + 1, swap: false, end));
                        }
                    }
                }
                end--;
            }
        }
        
        protected override EBarEffect GetEffect(int i, int currentStepIndex, int end)
        {
            if (i >= end)
            {
                return EBarEffect.Sorted;
            }
            if (i == currentStepIndex || i == currentStepIndex + 1)
            {
                return EBarEffect.Highlight;
            }
            return EBarEffect.None;
        }

        protected override EBarEffect GetEffect(int index, int index1, int index2, int end)
        {
            throw new System.NotImplementedException();
        }
    }
}
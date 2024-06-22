using System.Linq;
using Enums;

namespace SortingAlgorithms
{
    public sealed class InsertionSort : SortingAlgorithm
    {
        protected override void PrepareSteps()
        {
            Steps.Clear();
            // break down the Insertion Sort algorithm into steps
            var array = ArrayView.ArrayElements.Select(x => x.Value).ToArray();
            for (var i = 1; i < array.Length; i++)
            {
                var key = array[i];
                var j = i - 1;
                var swapped = false;
                while (j >= 0 && array[j] > key)
                {
                    Steps.Add((j + 1, j, swap: true, i));
                    array[j + 1] = array[j];
                    swapped = true;
                    j--;
                }

                if (swapped)
                {
                    if (i > 1 && j + 1 > 0)
                    {
                        Steps.Add((j + 2, j + 1, swap: false, i));
                    }
                }
                else
                {
                    Steps.Add((j + 1, j, swap: false, i));
                }
                
                array[j + 1] = key;
            }
        }

        protected override EBarEffect GetEffect(int index, int currentStepIndex, int end)
        {
            if(index == currentStepIndex || index == currentStepIndex - 1)
            {
                return EBarEffect.Highlight;
            }
            if(index <= end)
            {
                return EBarEffect.Sorted;
            }
            return EBarEffect.None;
        }

        protected override EBarEffect GetEffect(int index, int index1, int index2, int end)
        {
            throw new System.NotImplementedException();
        }
    }
}
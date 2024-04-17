using UnityEngine;
using System;
using System.Collections.Generic;

public static class RandomWeightedPicker // from ChatGPT:)
{
    private static System.Random random = new System.Random();

    public static int Pick(List<int> values, List<double> probabilities)
    {
        if (values.Count != probabilities.Count)
        {
            throw new ArgumentException("Number of values must be equal to number of probabilities.");
        }

        double randomNumber = random.NextDouble();
        double cumulativeProbability = 0;

        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeProbability += probabilities[i];

            if (randomNumber < cumulativeProbability)
            {
                return values[i];
            }
        }

        // Default return, should never be reached
        return values[values.Count - 1];
    }
}

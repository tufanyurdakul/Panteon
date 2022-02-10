using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatterns : MonoBehaviour
{
    public Dictionary<int, int[]> Patterns { get; private set; }
    void Start()
    {
        Patterns = new Dictionary<int, int[]>();
        int[] patternOne = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] patternTwo = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        int[] patternThree = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        int[] patternFour = { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        int[] patternFive = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1 };
        int[] patternSix = { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
        Patterns.Add(1, patternOne);
        Patterns.Add(2, patternTwo);
        Patterns.Add(3, patternThree);
        Patterns.Add(4, patternFour);
        Patterns.Add(5, patternFive);
        Patterns.Add(6, patternSix);
    }
}

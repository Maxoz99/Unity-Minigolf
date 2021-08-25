using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Statistics
{
    //public int subjectNr;
    //public int trialNr;
    public bool isDecision;
    public float time;
    public int items;
    public int interactions;
    public float metres;
    public int failures;
    //public int levels;
    public bool csvCreated = false;
    public bool dataCollected = false;
    public List<string> csvHeader = new List<string>();
    public List<List<string>> csvData = new List<List<string>>();
}
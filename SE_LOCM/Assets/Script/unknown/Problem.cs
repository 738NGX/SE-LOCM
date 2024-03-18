using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Problem : MonoBehaviour
{
    public ProblemInfo UsingProblem{get; private set;}
    public TextMeshProUGUI problemUI;
    public Button option1;
    public Button option2; 
    private List<int> problemIds;
    private int seed;
    private void Start()
    {
        seed=Random.Range(0,1);
        problemIds=ProblemDatabase.data.Keys.ToList();
        UsingProblem=ProblemDatabase.data[problemIds[Random.Range(0,problemIds.Count)]];

        problemUI.text=UsingProblem.name;

        if(seed==0)
        {
            option1.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.TrueAnswer;
            option2.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.FalseAnswer;
        }
        else
        {
            option2.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.TrueAnswer;
            option1.GetComponentInChildren<TextMeshProUGUI>().text=UsingProblem.FalseAnswer;
        }
    }
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementSM : StateMachine {

    [HideInInspector] public Walking walkingState;
    [HideInInspector] public Inspecting inspectingState;

    [HideInInspector] public List<GameObject> targets;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public int amountOfTargetsToVisit;
    [HideInInspector] public bool isInspecting;
    [HideInInspector] public bool isAngry;

    private StorageDictionary storageDictionary;
    private CashierDictionary cashierDictionary;
    private GameObject exitPositionObject;
    private GameObject cashierCounter;

    public NavMeshAgent agent;
    public Renderer modelRenderer;
    public float distanceToDestinationThreshhold;
    public int maxSecondsToInspect;
    public int minSecondsToInspect;

    public int money;


    private void Awake() {
        SetupStates();
        InitializeData();
        SetAmountOfTargetsToVisit();
    }

    private void SetupStates() {
        walkingState = new Walking(this);
        inspectingState = new Inspecting(this);
    }

    private void InitializeData() {
        var _gameManager = GameObject.FindGameObjectWithTag("GameManager");

        storageDictionary = _gameManager.GetComponent<StorageDictionary>();
        cashierDictionary = _gameManager.GetComponent<CashierDictionary>();
        exitPositionObject = _gameManager.GetComponent<CustomerSpawning>().GetRandomSpawnPoint();

        cashierCounter = cashierDictionary.GetRandomMannedCashierCounter();
        targets = new List<GameObject>(storageDictionary.GetStorageShelvesThatHaveProducts());

        SetCurrentTarget();
    }

    public void SetCurrentTarget() {
        currentTarget = GetTarget();
        agent.destination = currentTarget.transform.position;
    }

    public void DoneInspecting() {
        targets.Remove(currentTarget);

        if (!OnTheWayToCashier())
            amountOfTargetsToVisit--;
    }

    public void TryAnotherTarget() {
        if (!(amountOfTargetsToVisit + 1 > targets.Count))
            amountOfTargetsToVisit++;
        else
            isAngry = true;
    }

    private void SetAmountOfTargetsToVisit() {
        amountOfTargetsToVisit = Random.Range(1, targets.Count);
    }

    protected override BaseState GetInitialState() {
        return walkingState;
    }

    public GameObject GetTarget() {
        if (HasTargetsToVisit())
            return targets[Random.Range(0, targets.Count - 1)];
        
        if (!OnTheWayToCashier())
            return cashierCounter;
        
        return exitPositionObject;
    }

    public bool OnTheWayToCashier() {
        return currentTarget == cashierCounter;
    }

    public bool HasTargetsToVisit() {
        return amountOfTargetsToVisit > 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

enum CustomerAIState : ushort
{
    Standing = 0,
    Walking = 1,
    Inspecting = 2,
    CheckingOut = 3,
    Talking = 4
}

public class CustomerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Renderer modelRenderer;

    private CustomerAIState state;

    [SerializeField] private List<GameObject> shelvesToVisit;
    [SerializeField] private int amountOfShelvesToVisit;
    private int nextShelfIndex;

    [SerializeField] public float distanceToDestinationThreshhold;
    [SerializeField] private int maxSecondsToInspect;
    [SerializeField] private int minSecondsToInspect;

    private bool isInspecting;
    private StorageDictionary storageDictionary;
    public Vector3 destination;
    [HideInInspector] public Vector3 exitPosition;

    private void Start()
    {
        var _gameManager = GameObject.FindGameObjectWithTag("GameManager");

        storageDictionary = _gameManager.GetComponent<StorageDictionary>();
        exitPosition = _gameManager.GetComponent<CustomerSpawning>().GetRandomSpawnPoint().transform.position;
        shelvesToVisit = storageDictionary.GetStorageShelvesThatHaveProducts();

        state = CustomerAIState.Standing;
        isInspecting = false;


        SetAmountOfShelvesToVisit();
        WalkToNextShelf();
    }

    private void SetAmountOfShelvesToVisit()
    {
        amountOfShelvesToVisit = Random.Range(0, shelvesToVisit.Count - 1);
    }

    private void Update()
    {
        agent.isStopped = false;
        switch (state)
        {
            case CustomerAIState.Standing:
                modelRenderer.material.SetColor("_Color", Color.green);
                agent.isStopped = true;
                break;
            case CustomerAIState.Walking:
                modelRenderer.material.SetColor("_Color", Color.cyan);
                if (IsDestinationTheExit()) modelRenderer.material.SetColor("_Color", Color.magenta);
                EnterWalkState();
                break;
            case CustomerAIState.Inspecting:
                modelRenderer.material.SetColor("_Color", Color.yellow);
                if (!isInspecting)
                {
                    StartCoroutine(EnterInspectState(Random.Range(minSecondsToInspect, maxSecondsToInspect)));
                    isInspecting = true;
                }
                break;
            case CustomerAIState.CheckingOut:
                modelRenderer.material.SetColor("_Color", Color.white);
                StartCoroutine(EnterCheckingOutState(Random.Range(minSecondsToInspect, maxSecondsToInspect)));
                break;
            case CustomerAIState.Talking:
                agent.isStopped = true;
                break;
        }
    }

    private void EnterWalkState()
    {
        if (IsAtDestination())
        {
            if (IsDestinationTheExit())
                Exit();

            if (HasShelvesToVisit())
                state = CustomerAIState.Inspecting;
            else
                state = CustomerAIState.CheckingOut;
        }
    }

    private bool IsAtDestination()
    {
        return agent.remainingDistance < distanceToDestinationThreshhold && agent.remainingDistance != 0;
    }

    private bool IsDestinationTheExit()
    {
        return agent.destination.x == exitPosition.x && agent.destination.z == exitPosition.z;
    }

    private bool HasShelvesToVisit()
    {
        return amountOfShelvesToVisit > -1;
    }

    private void WalkToNextShelf()
    {
        nextShelfIndex = Random.Range(0, shelvesToVisit.Count);
        agent.destination = shelvesToVisit[nextShelfIndex].transform.position;
        state = CustomerAIState.Walking;
    }

    private void WalkToCashierCounter()
    {
        agent.destination = new Vector3(0, 0, 0);
        state = CustomerAIState.Walking;
    }

    private void WalkToExit()
    {
        agent.destination = exitPosition;
        state = CustomerAIState.Walking;
    }

    private void Exit()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator EnterInspectState(int inspectTime)
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(inspectTime);

        if (HasShelvesToVisit())
        {
            shelvesToVisit.Remove(shelvesToVisit[nextShelfIndex]);
            amountOfShelvesToVisit--;
            WalkToNextShelf();
        }
        if (!HasShelvesToVisit())
        {
            WalkToCashierCounter();
        }

        isInspecting = false;
    }

    private IEnumerator EnterCheckingOutState(int inspectTime)
    {
        Debug.Log("Checking out");
        agent.isStopped = true;

        yield return new WaitForSeconds(inspectTime);
        Debug.Log("Done");

        WalkToExit();
    }

}

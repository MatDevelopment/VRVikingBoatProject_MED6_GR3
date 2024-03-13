using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWood : MonoBehaviour
{
    public LevelChanger levelChanger;
    public int GatheredWood = 0;

    [SerializeField] private GameObject closedDoorGameObject;
    [SerializeField] private GameObject halfOpenDoorGameObject;
    // Start is called before the first frame update
    void Start()
    {
        levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelChanger.countStackedWood >= 2 && levelChanger.WoodStacked == false)
        {
            //levelChanger.CountWoodStacked();
            levelChanger.WoodStacked = true;
            closedDoorGameObject.SetActive(false);
            halfOpenDoorGameObject.SetActive(true);
        }
        else
        {
            levelChanger.WoodStacked = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChoppedLog"))
        {
            levelChanger.countStackedWood += 1;
            levelChanger.CountWoodStacked(); //Makes sure a prompt is sent to ChatGPT about the user just having finished stacking the wood.
        }

        else if (other.CompareTag("WoodLog"))
        {
            Debug.Log("Wood Has Not Been Chopped!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ChoppedLog"))
        {
            levelChanger.countStackedWood -= 1;
        }
    }
}

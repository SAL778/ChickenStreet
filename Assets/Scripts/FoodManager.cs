using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    int FoodLimit = 1;
    int spawnedFoods = 0;

    public static FoodManager Instance;

    private Camera cam;
    public GameObject foodPrefab;
    public UnityEvent onFoodSpawned;
    public UnityEvent onFoodDespawned;

    Vector3 mousePos;
    Vector3 worldPos;

    Ray pointerRay;
    RaycastHit pointerHit;

    // Start is called before the first frame update
    void Start()
    {
        //There can be only one!
        if (Instance == null) { Instance = this; } else { Destroy(this); }

        cam = Camera.main;
        if (onFoodSpawned == null)
            onFoodSpawned = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            pointerRay = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(pointerRay, out pointerHit))
            {
                //Debug.Log("Pointer position World: " + pointerHit.point);
                worldPos = pointerHit.point;
                worldPos.y = 0.5f;
                //if (pointerHit.collider.tag == "Default") transform.position = pointerHit.point + EyelineOffset;
                if (pointerHit.collider.tag == "Objective")
                {
                    DespawnFood(pointerHit.collider.gameObject);
                } 
                else
                {
                    SpawnFood();
                }

            }
            
        }
        
    }
    void SpawnFood()
    {
        if(spawnedFoods < FoodLimit)
        {
            spawnedFoods++;
            GameObject newFood = Instantiate(foodPrefab, worldPos, Quaternion.identity);
            newFood.transform.position = worldPos;
            onFoodSpawned.Invoke();
        } else
        {
            Debug.Log("Food limit reached!");
        }
    }

    void DespawnFood(GameObject food)
    {
        spawnedFoods--;
        Destroy(food);
    }
}
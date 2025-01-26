using TMPro;
using UnityEngine;

public class CALORIE : MonoBehaviour
{
    float speed = 2f;
    float calories = 0f; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "CALORIES: " + calories.ToString();

        calories += speed * Time.deltaTime; 
    }
}

// Importing necessary libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declaring the FruitManager class
public class FruitManager : MonoBehaviour
{
    // Declaring variables
    [Header(" Elements ")]
    [SerializeField] private Fruit fruitPrefab; // Reference to the Fruit prefab
    [SerializeField] private LineRenderer FruitSpawnLine; // Reference to the LineRenderer
    private Fruit CurrentFruit; // Reference to the currently active Fruit

    [Header(" Settings ")]
    [SerializeField] private float fruitYaxisSpawnPos; // The y-axis position where fruits will spawn
    private bool CanSpawnFruit; // Flag to check if a new fruit can be spawned
    private bool ControllingFruit; // Flag to check if the player is controlling a fruit

    [Header(" Debug ")]
    [SerializeField] private bool enableGizmos; // Flag to enable or disable gizmos

    // Start is called before the first frame update
    void Start()
    {
        CanSpawnFruit = true; // Allowing fruit spawning
        HideLine(); // Hiding the spawn line
    }

    // Update is called once per frame
    void Update()
    {
        if(CanSpawnFruit) // If a new fruit can be spawned
            ManagePlayerInput(); // Check for player input
    }

    // Check whether player is clicking down once, holding down, or unclicking
    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0)) // If the player clicks the mouse button
            MouseDownCallback(); // Call the MouseDownCallback method
        
        else if (Input.GetMouseButton(0)) // If the player holds down the mouse button
        {
            if (ControllingFruit) // If the player is controlling a fruit
                MouseDragCallback(); // Call the MouseDragCallback method
            else
                MouseDownCallback(); // Call the MouseDownCallback method
        }
        
        else if (Input.GetMouseButtonUp(0) && ControllingFruit) // If the player releases the mouse button and is controlling a fruit
            MouseUpCallback(); // Call the MouseUpCallback method
    }

    // If player is clicking down once
    private void MouseDownCallback()
    {
        DisplayLine(); // Display the spawn line

        PlaceLineAtClickedPosition(); // Place the line at the clicked position

        SpawnFruit(); // Spawn a new fruit

        ControllingFruit = true; // The player is now controlling a fruit
    }

    // If player is holding down click
    private void MouseDragCallback()
    {
        PlaceLineAtClickedPosition(); // Place the line at the clicked position

        CurrentFruit.MoveTo(GetSpawnPosition()); // Move the current fruit to the spawn position
    }

    // If player is unclicking
    private void MouseUpCallback()
    {
        HideLine(); // Hide the spawn line
        CurrentFruit.EnablePhysics(); // Enable physics on the current fruit

        CanSpawnFruit = false; // A new fruit cannot be spawned
        StartFruitActiveTimer(); // Start the fruit active timer

        ControllingFruit = false; // The player is no longer controlling a fruit
    }

    // Spawn a new fruit
    private void SpawnFruit()
    {
        Vector2 spawnPosition = GetClickedWorldPosition(); // Get the clicked world position
        spawnPosition.y = fruitYaxisSpawnPos; // Set the y-axis position of the spawn position

        CurrentFruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity); // Instantiate a new fruit at the spawn position
    }

    // Get the clicked world position
    private Vector2 GetClickedWorldPosition() 
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convert the mouse's screen position to a world position
    }

    // Get the spawn position
    private Vector2 GetSpawnPosition()
    {
        Vector2 WorldClickedPosition = GetClickedWorldPosition(); // Get the clicked world position
        WorldClickedPosition.y = fruitYaxisSpawnPos; // Set the y-axis position of the world clicked position
        return WorldClickedPosition; // Return the world clicked position
    }

    // Place the line at the clicked position
    private void PlaceLineAtClickedPosition()
    {
        FruitSpawnLine.SetPosition(0, GetSpawnPosition()); // Set the start position of the line
        FruitSpawnLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15); // Set the end position of the line
    }

    // Hide the spawn line
    private void HideLine()
    {
        FruitSpawnLine.enabled = false; // Disable the LineRenderer
    }

    // Display the spawn line
    private void DisplayLine()
    {
        FruitSpawnLine.enabled = true; // Enable the LineRenderer
    }

    // Start the fruit active timer
    private void StartFruitActiveTimer()
    {
        Invoke("StopFruitActiveTimer", .5f); // Call the StopFruitActiveTimer method after 1 second
    }

    // Stop the fruit active timer
    private void StopFruitActiveTimer()
    {
        CanSpawnFruit = true; // A new fruit can be spawned
    }

#if UNITY_EDITOR
    // Draw gizmos in the Unity editor
    private void OnDrawGizmos()
    {
        if (!enableGizmos) // If gizmos are not enabled
            return; // Return

        Gizmos.color = Color.yellow; // Set the gizmo color to yellow
        Gizmos.DrawLine(new Vector3(-50, fruitYaxisSpawnPos, 0), new Vector3(50, fruitYaxisSpawnPos, 0)); // Draw a line at the fruit spawn position
    }
#endif
}
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DrawManager : MonoBehaviour
{
    public GameObject linePrefab;

    private DrawingControls controls;
    private LineRenderer currentLine;
    private EdgeCollider2D currentCollider;
    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing;
    private bool isPicking;
    [SerializeField]
    private GameObject pickColorMenu;

    private void Awake()
    {
        controls = new DrawingControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        // Check if the button is currently pressed
        bool pressed = controls.Drawing.StartDraw.ReadValue<float>() > 0f;
        bool menu = controls.Drawing.PickColor.ReadValue<float>() > 0f;
        if (pressed && !isDrawing)
        {
            StartDrawing();
        }
        else if (!pressed && isDrawing)
        {
            EndDrawing();
        }

        if (menu)
        {
            pickColorMenu.SetActive(!pickColorMenu.activeSelf);
        }
        
        if (isDrawing)
            Draw();
    }

    private void StartDrawing()
    {
        isDrawing = true;
        GameObject newLine = Instantiate(linePrefab);
        currentLine = newLine.GetComponent<LineRenderer>();
        currentCollider = newLine.GetComponent<EdgeCollider2D>();
        points.Clear();

        Vector2 startPos = ScreenToWorldPos(controls.Drawing.Position.ReadValue<Vector2>());
        AddPoint(startPos);
    }

    private void Draw()
    {
        Vector2 mousePos = ScreenToWorldPos(controls.Drawing.Position.ReadValue<Vector2>());
        if (points.Count == 0 || Vector2.Distance(mousePos, points[^1]) > 0.1f)
            AddPoint(mousePos);
    }

    private void AddPoint(Vector2 point)
    {
        points.Add(point);

        currentLine.positionCount = points.Count;
        currentLine.SetPosition(points.Count - 1, new Vector3(point.x, point.y, 0));

        if (points.Count > 1)
            currentCollider.SetPoints(points);
    }

    private void EndDrawing()
    {
        isDrawing = false;
        currentLine = null;
        currentCollider = null;
    }

    private void PickColor()
    {
        pickColorMenu.SetActive(true);
        isPicking = true;
    }
    private Vector2 ScreenToWorldPos(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }
}

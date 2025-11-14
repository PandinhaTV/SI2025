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
    
    [SerializeField]
    private SoundController soundController;

    public int ColorPicked = 0;

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

    private void FixedUpdate()
    {
        
        
    }
    private void Update()
    {
        bool isHeld = controls.Drawing.PickColor.ReadValue<float>() > 0f;
            pickColorMenu.SetActive(isHeld);
        
        // Check if the button is currently pressed
        bool pressed = controls.Drawing.StartDraw.ReadValue<float>() > 0f;
        
        if (pressed && !isDrawing && !isHeld)
        {
            StartDrawing();
        }
        else if (!pressed && isDrawing)
        {
            EndDrawing();
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
        currentLine.useWorldSpace = false;
        points.Clear();
        
        // if X == 1, make it move left
        ColorScript mover = newLine.GetComponent<ColorScript>();
        if (mover != null)
        {
            // Wait 1 second before starting
            mover.startDelay = 1f;

            // Destroy after 6 seconds (you can tweak this)
            mover.lifetime = 6f;

            // Decide direction based on X
            // X = 1 → Left, X = 2 → Right, X = 3 → Up, X = 4 → Down
            switch (ColorPicked)
            {
                case 1:
                    currentLine.startColor = Color.blue;
                    currentLine.endColor = Color.blue;
                    mover.direction = Vector2.left;
                    break;
                case 2:
                    currentLine.startColor = Color.green;
                    currentLine.endColor = Color.green;
                    mover.direction = Vector2.right;
                    break;
                case 3:
                    currentLine.startColor = Color.yellow;
                    currentLine.endColor = Color.yellow;
                    mover.direction = Vector2.up;
                    break;
                case 4:
                    currentLine.startColor = Color.red;
                    currentLine.endColor = Color.red;
                    mover.direction = Vector2.down;
                    break;
                default:
                    mover.direction = Vector2.zero;
                    break;
            }
        }
        
        Vector2 startPos = ScreenToWorldPos(controls.Drawing.Position.ReadValue<Vector2>());
        AddPoint(newLine.transform.InverseTransformPoint(startPos));

        soundController.StartPaintingSound();   // Plays the starting sound
    
    }

    private void Draw()
    {
        Vector2 mousePos = ScreenToWorldPos(controls.Drawing.Position.ReadValue<Vector2>());
        if (points.Count == 0 || Vector2.Distance(mousePos, points[^1]) > 0.1f)
            AddPoint(mousePos);

        if (!isDrawing)
            return;
        else
            soundController.WhilePaintingSound();
            
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

        soundController.EndPaintingSound();
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

    public void SetColor(int color)
    {
        ColorPicked = color;
    }
}

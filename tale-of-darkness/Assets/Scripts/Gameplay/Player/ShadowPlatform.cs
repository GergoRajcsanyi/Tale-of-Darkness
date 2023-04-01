using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class ShadowPlatform : MonoBehaviour, DataInterface
{
    #region Line Drawing Ability

    [Header("TIMERS")]
    [Range(0f, 15f)] public float cooldown;
    [Range(0f, 5f)] public float despawnTimer;

    [Header("LINE")]
    [Range(0f, 30f)] public int lengthOfLineRenderer;
    public GameObject linePrefab;
    public static bool shadowPlatform = false;

    [Header("OBJECTS")]
    public GameObject currentLine;
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public List<Vector2> touchPositions;
    private bool isShadowSpawned = false;
    private float canUse = -1f;
    private bool isFingerDown = false;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (shadowPlatform && !EventSystem.current.IsPointerOverGameObject(id) && (touch.phase == TouchPhase.Began) && !PauseMenu.GameIsPaused)
            {
                if (!isShadowSpawned && Time.time > canUse && Input.touchCount == 1)
                {
                    canUse = Time.time + cooldown;

                    currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                    lineRenderer = currentLine.GetComponent<LineRenderer>();
                    edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
                    touchPositions.Clear();

                    touchPositions.Add(Camera.main.ScreenToWorldPoint(touch.position));
                    touchPositions.Add(Camera.main.ScreenToWorldPoint(touch.position));

                    lineRenderer.SetPosition(0, touchPositions[0]);
                    lineRenderer.SetPosition(1, touchPositions[1]);

                    edgeCollider.points = touchPositions.ToArray();
                    isShadowSpawned = true;
                    isFingerDown = true;
                }
            }
            if ((touch.phase == TouchPhase.Moved) && !PauseMenu.GameIsPaused && lineRenderer != null)
            {
                Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(touch.position);
                if (Vector2.Distance(tempFingerPos, touchPositions[touchPositions.Count - 1]) > .1f)
                {
                    touchPositions.Add(tempFingerPos);
                    if (lineRenderer.positionCount < lengthOfLineRenderer && isFingerDown && Input.touchCount == 1)
                    {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tempFingerPos);
                        edgeCollider.points = touchPositions.ToArray();
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                isFingerDown = false;
                DeleteShadow();
            }
        }    
    }

    private void DeleteShadow()
    {
        Destroy(currentLine, despawnTimer);
        isShadowSpawned = false;
    }
    #endregion

    #region Save System
    public void LoadData(GameData data)
    {
        foreach(KeyValuePair<string, bool> pair in data.abilityCollected)
        {
            if (pair.Key == "shadow-platform")
            {
                shadowPlatform = pair.Value;
            }
        }
    }

    public void SaveData(GameData data)
    {
        //Nothing to save here...
    }
    #endregion
}
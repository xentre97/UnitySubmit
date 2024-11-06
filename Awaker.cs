using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Awaker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI P1TurnTXT;
    [SerializeField] GameObject control;
    [SerializeField] GameObject  cameraObject;
    [SerializeField] List<Button> buttons;
    [SerializeField] GameObject backScene;
    private Control contrl;
    private CameraMove cameraMove;

    // Start is called before the first frame update
    void Start()
    {
        // ÉâÉxÉãÇèâä˙âªÇ∑ÇÈ
        label.text = $"Start";
    }

    public void OnPressed()
    {
        contrl = control.GetComponent<Control>();
        cameraMove = cameraObject.GetComponent<CameraMove>();
        contrl.Awaken();
        contrl.Awake = true;
        cameraMove.cameraAwake = true;
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        backScene.SetActive(false);
        gameObject.SetActive(false);
        P1TurnTXT.gameObject.SetActive(true);
    }
}
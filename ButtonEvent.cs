using TMPro;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] GameObject control;
    private Control contrl;

    public int click;
    public int Number;

    // Start is called before the first frame update
    void Start()
    {
        click = 0;
        // ラベルを初期化する
        label.text = $"Default";
        contrl = control.GetComponent<Control>();
        contrl.in1 = 1; contrl.in2 = 2; contrl.in3 = 3;
        contrl.in4 = 11; contrl.in5 = 12; contrl.in6 = 13;
    }

    public void OnPressed()
    {
        contrl = control.GetComponent<Control>();
        // ボタンが押されたらカウンタを一つ増やし、ラベルを更新する
        click++;
        if ((Number==1))
        {
            if (click == 1)
            {
                label.text = $"Spear";
                contrl.in1 = 1;
            }
            else if (click == 2)
            {
                label.text = $"Cavalry";
                contrl.in1 = 2;
            }
            else if (click == 3)
            {
                label.text = $"Bow";
                contrl.in1 = 3;
            }
            else if (click == 4)
            {
                label.text = $"Ji";
                contrl.in1 = 4;
            }
            if (click > 4)
            {
                click = 1;
                label.text = $"Spear";
                contrl.in1 = 1;
            }
            
        }
        
        if ((Number==2))
        {
            if (click == 1)
            {
                label.text = $"Spear";
                contrl.in2 = 1;
            }
            if (click == 2)
            {
                label.text = $"Cavalry";
                contrl.in2 = 2;
            }
            if (click == 3)
            {
                label.text = $"Bow";
                contrl.in2 = 3;
            }
            if (click == 4)
            {
                label.text = $"Ji";
                contrl.in2 = 4;
            }
            if (click > 4)
            {
                click = 1;
                label.text = $"Spear";
                contrl.in2 = 1;
            }
        }
        if((Number==3))
        {
            if (click == 1)
            {
                label.text = $"Spear";
                contrl.in3 = 1;
            }
            if (click == 2)
            {
                label.text = $"Cavalry";
                contrl.in3 = 2;
            }
            if (click == 3)
            {
                label.text = $"Bow";
                contrl.in3 = 3;
            }
            if (click == 4)
            {
                label.text = $"Ji";
                contrl.in3 = 4;
            }
            if (click > 4)
            {
                click = 1;
                label.text = $"Spear";
                contrl.in3 = 1;
            }
        }
        if (Number == 4)
        {
            if (click == 1)
            {
                label.text = $"Spear";
                contrl.in4 = 11;
            }
            if (click == 2)
            {
                label.text = $"Cavalry";
                contrl.in4 = 12;
            }
            if (click == 3)
            {
                label.text = $"Bow";
                contrl.in4 = 13;
            }
            if (click == 4)
            {
                label.text = $"Ji";
                contrl.in4 = 14;
            }
            if (click > 4)
            {
                click = 1;
                label.text = $"Spear";
                contrl.in4 = 11;
            }
        }
        if (Number == 5)
        {
            if (click == 1)
            {
                label.text = $"Spear";
                contrl.in5 = 11;
            }
            if (click == 2)
            {
                label.text = $"Cavalry";
                contrl.in5 = 12;
            }
            if (click == 3)
            {
                label.text = $"Bow";
                contrl.in5 = 13;
            }
            if (click == 4)
            {
                label.text = $"Ji";
                contrl.in5 = 14;
            }
            if (click > 4)
            {
                click = 1;
                label.text = $"Spear";
                contrl.in5 = 11;
            }
        }
        if (Number == 6)
        {
            if (click == 1)
            {
                label.text = $"Spear";
                contrl.in6 = 11;
            }
            if (click == 2)
            {
                label.text = $"Cavalry";
                contrl.in6 = 12;
            }
            if (click == 3)
            {
                label.text = $"Bow";
                contrl.in6 = 13;
            }
            if (click == 4)
            {
                label.text = $"Ji";
                contrl.in6 = 14;
            }
            if (click > 4)
            {
                click = 1;
                label.text = $"Spear";
                contrl.in6 = 11;
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Images : MonoBehaviour
{

    public static Sprite BoxClear { get { if (boxClear == null) { boxClear = Resources.Load<Sprite>("Images/box"); } return boxClear; } set { boxClear = value; } }
    public static Sprite BoxCross { get { if (boxCross == null) { boxCross = Resources.Load<Sprite>("Images/box_cross"); } return boxCross; } set { boxCross = value; } }
    public static Sprite BoxCircle { get { if (boxCircle == null) { boxCircle = Resources.Load<Sprite>("Images/box_circle"); } return boxCircle; } set { boxCircle = value; } }

    private static Sprite boxClear;
    private static Sprite boxCross;
    private static Sprite boxCircle;

}

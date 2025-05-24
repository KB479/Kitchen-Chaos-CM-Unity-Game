using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;



    private void Start()
    {
        /*Awake ile subscribe olmamal�s�n ��nk� player instance� da awake ile olu�uyor, awakeler �ak���r da 
        burdaki �nce okunursa Player.Instance = null olacak ve exception atacak! */

        //7* Awake ile �al��t�rma bu kodu! Script Execution Order
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;



    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }

    }


    private void Show()
    {
        foreach(GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchen object here
            if(player.HasKitchenObject())
            {
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //Player not carring anything
            }
        }
        else
        {
            //There is a kitchen object here
            if (player.HasKitchenObject())
            {
                //Player is carriyng something
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //Player is not carrying Plate but something else 
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) 
                    {
                        // Counter is holding a plate 
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf(); 
                        }
                    }
                }
            }
            else
            {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player); 
            }

        }

    }





}

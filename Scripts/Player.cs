using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance {  get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter; 
    }


    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;



    private bool isWalking; 
    private Vector3 lastInteracDirec;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;



    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player instance!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }   
    }


    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    private void HandleInteractions() 
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirec = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDirec != Vector3.zero)
        {
            lastInteracDirec = moveDirec;
        }

        float interactDistence = 2f; 
        if(Physics.Raycast(transform.position, lastInteracDirec, out RaycastHit raycastHit, interactDistence, countersLayerMask))
        {
            //TryGetComponent
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Has ClearCounter
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else 
        {
            SetSelectedCounter(null);
        }

    }

    private void HandleMovement()  
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirec = new Vector3(inputVector.x, 0f, inputVector.y);


        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirec, moveDistance);

        if (!canMove)
        {
            // Cannot move towards MoveDirec

            // Attempt only X movement
            Vector3 moveDirecX = new Vector3(moveDirec.x, 0f, 0f).normalized;
            canMove = (moveDirec.x < -.5f || moveDirec.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirecX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDirec = moveDirecX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirecZ = new Vector3(0f, 0f, moveDirec.z).normalized;
                canMove = (moveDirec.z < -.5f || moveDirec.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirecZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDirec = moveDirecZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }


        if (canMove)
        {
            transform.position += moveDirec * moveDistance;
        }


        isWalking = moveDirec != Vector3.zero;
; 

        //Karakterin dönüþü
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirec, Time.deltaTime * rotateSpeed);

    }



    public bool IsWalking()
    {
        return isWalking;
    }


    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        { selectedCounter = selectedCounter });

    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }


    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }








}

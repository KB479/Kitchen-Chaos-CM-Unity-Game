using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
  
    private const string IS_WALKING = "IsWalking"; 

    [SerializeField] private Player player;

    private Animator animator;



    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        /* SetBool string alıyor, string sıkıntılı o yüzden const kullandık sağlama almak için.player.IsWalking 
        ile true/false getiriyor, update de kontorl ediyor, IsWalking booluna da atama  yapılıyor, ona göre 
        animasyon devreye giriyor. */

    }



}

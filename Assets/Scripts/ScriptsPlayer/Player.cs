using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;


public class Player : MonoBehaviour
{
    private PlayerActions playerActions;

    private Rigidbody2D rb2D;

    [SerializeField]
    private float currentPlayerSpeed;

    [SerializeField]
    private float walkingPlayerSpeed;

    [SerializeField]
    private float rollSpeedIns;
    [SerializeField]
    private float rollResta;

    private float rollSpeed;
    private Vector3 movementInput;
    private float movementSpeedAnim;

    [SerializeField]
    public static float damage = 1;
    [SerializeField]
    public static float PlayerHP = 100;

    [SerializeField]
    private Animator animator;



    private State state;

    [SerializeField]
    private Slider HealthBar;

    [SerializeField]
    private Slider StaminaBar;


    //[SerializeField]
    //private Slider MagicBar;


    private enum State
    {
        Normal,
        DodgeRollSliding,
        Attacking,
        Blocking
    }

    private void Awake() {
        try{
        playerActions = new PlayerActions();
        rb2D = GetComponent<Rigidbody2D>();
        state = State.Normal;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnEnable() {
        try{
            playerActions.PlayerMain.Enable();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnDisable() {
        try{
        playerActions.PlayerMain.Disable();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        try{
        PlayerHP = 100;
        HealthBar.value = PlayerHP;
        StaminaBar.value = 100;
        currentPlayerSpeed = walkingPlayerSpeed;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void FixedUpdate()
    {        
        try{ 
            Animate();

            switch(state)
            {
                case State.Normal:
                    movementInput = playerActions.PlayerMain.Move.ReadValue<Vector2>(); 

                    //pasar de vector 3 a float
                    movementSpeedAnim = Mathf.Clamp(movementInput.magnitude,0.0f,currentPlayerSpeed);

                    movementInput.Normalize();
                    rb2D.velocity = movementInput * currentPlayerSpeed;
                    HandleHealth();
                    HandleAttacking();
                    HandleDodgeRoll();
                    HandleBlocking();
                break;

                case State.DodgeRollSliding:
                    DodgeRollSliding();
                break;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void HandleHealth()
    {
        try{
            HealthBar.value = PlayerHP;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

    public void HandleAttacking()
    {   

        try{
            if(playerActions.PlayerMain.Attack.IsPressed()){  
                StartCoroutine(AttackCo());
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator AttackCo()
    {              
            animator.SetBool("Attacking", true);
            state = State.Attacking;
            yield return null;
            animator.SetBool("Attacking", false);
            yield return new WaitForSeconds(0.25f);
            state = State.Normal;
            currentPlayerSpeed = walkingPlayerSpeed;
    }
 
    public void HandleBlocking()
    {
        try{
            if(playerActions.PlayerMain.Block.IsPressed())
            {
                animator.SetBool("Blocking", true);
                currentPlayerSpeed = 0;
            }
            else
            {            
                animator.SetBool("Blocking",false);
                
                currentPlayerSpeed = walkingPlayerSpeed;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    } 
    
    public void HandleDodgeRoll()
    {   
        try{
            if(playerActions.PlayerMain.Roll.IsPressed() && playerActions.PlayerMain.Block.IsPressed() == false){  
                state = State.DodgeRollSliding;

                //StaminaBar.value = StaminaBar.value * Time.deltaTime;
                if (StaminaBar.value >= 25)
                {            
                StaminaBar.value = StaminaBar.value-25;
                Debug.Log("Rolling");
                rollSpeed = rollSpeedIns;
                }
            
            }

            StaminaBar.value = StaminaBar.value * 1.005f ;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void DodgeRollSliding()
    {
        try{
            transform.position += movementInput * rollSpeed * Time.deltaTime;

            rollSpeed -= rollSpeed * rollResta * Time.deltaTime;
            if(rollSpeed < 2f)
            {
                state = State.Normal;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void TakeDamage(float enemyDamage)
    {
        try{
            PlayerHP = PlayerHP - enemyDamage;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Animate() {
        try{
        if(movementInput != Vector3.zero && state != State.Attacking)
            {
            animator.SetFloat("Horizontal",movementInput.x);
            animator.SetFloat("Vertical",movementInput.y);
            }


            if(state == State.DodgeRollSliding)
            {
                animator.SetFloat("Speed",rollSpeed);
            }
            else
            {
                animator.SetFloat("Speed",movementSpeedAnim);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}

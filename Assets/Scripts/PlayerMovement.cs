using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float modeSpeed = 2f; //скорость бега
    public float modeRoll = 3f; //скорость переката

    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    Vector2 movementRoll;
    float lastIdleState = 0; // в какую сторону персонаж смотрит 0 - вверх, 1 - вниз, 2 - вправо, 3 - влево
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //нажатие клавиш по x
        movement.y = Input.GetAxisRaw("Vertical"); //нажатие клавиш по y
 
        animator.SetFloat("Speed", movement.sqrMagnitude); // выставление скорости в состоянии (не является скоростью)

        if (animator.GetFloat("Speed") == 0) //если перемещение закончилось, зафиксировать в состояниях текущее направление героя
        {
            animator.SetFloat("IdleState", lastIdleState); 
        }
        else { //получить текущее направление перемещения
            if (movement.x != 0)
                lastIdleState =  (movement.x>0)?2:3;
            else lastIdleState = (movement.y>0)?0:1;
        }

        animator.SetFloat("Horizontal", movement.x); //установить в состояниях нажатия на клавиши по оси x и y
        animator.SetFloat("Vertical", movement.y);

        bool roll = Input.GetButtonDown("Roll"); 
        if (roll) 
        {

            animator.SetFloat("IdleState", lastIdleState); //зафиксировать направление героя
            setDirectionRoll();
            animator.SetBool("Roll", true); //установить в состояних перекат на true
            
        }
    }
    void FixedUpdate() 
    {
        if (!animator.GetBool("Roll"))
        rb.MovePosition(rb.position + movement * modeSpeed * Time.fixedDeltaTime);

    }

    void setDirectionRoll() 
    {
        if (movement.x != 0 || movement.y != 0) //перекат во время бега
            movementRoll = movement; 
        else //перекат после покоя
        {
            movementRoll.x = (lastIdleState == 2) ? 1 : (lastIdleState == 3) ? -1 : 0;
            movementRoll.y = (lastIdleState == 0) ? 1 : (lastIdleState == 1) ? -1 : 0;
        }
    }

    void moveRoll() //функция event для перемещения на каждый кадр анимации переката
    {

        rb.MovePosition(rb.position + movementRoll * modeRoll * Time.fixedDeltaTime);
        
    }
    void stopRoll() //функция event для прекращения переката
    {
        animator.SetBool("Roll", false);
    }


}

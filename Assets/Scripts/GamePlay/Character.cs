using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private SpriteRenderer playerRenderer;
    

    
    private bool isGrounded;
    private int speedX;
    private float speedY;
    private int extraJump;

    [Header("Objects with Interaction")]
    public Transform groundCheck;
    public Transform carrotLaunchPosition;
    public LayerMask whatIsGround;
    public GameObject carrotsBullet;


    [Header("Character Settings")]
    public float speedMove;
    public float jumpForce;
    public bool isFaceLeft;
    public int jumpPlus;
    public float strenght;
    
    

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();

        extraJump = jumpPlus;
    }
    
    void Update()
    {
        Locomotion();        
    }

    private void FixedUpdate()
    {
        // faz com que a variavel bool ao ter contato com box collider do ch�o se torne verdadeira. Para isto ela cria um raio de 0.02f para verificar o contato do ch�o com o transform criado no player.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, whatIsGround); 
    }

    private void LateUpdate()
    {
        // Define os valores do Animator conforme os valores das variaveis criadas aqui.
        playerAnimator.SetInteger("speedX", speedX);
        playerAnimator.SetBool("isGrounded", isGrounded);      
        playerAnimator.SetFloat("speedY", speedY);        
    }

    void Locomotion()
    {
        
        // define o valor da variav�l speedX conforme a movimenta��o, esta variavel passar� os valores para o Animator
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal !=0)        
        {
            speedX = 1;            
        }
        else 
        {
            speedX = 0;            
        }
        if(isFaceLeft == true && horizontal > 0)
        {
            flipCharacter();
        }
        if(isFaceLeft == false && horizontal < 0)
        {
            flipCharacter();
        }
        

        // a vari�vel speedY receber� os valores da movimenta��o do rigidbody2D no eixo Y.
        speedY = playerRB.velocity.y;

        // faz a movimenta��o do personagem no eixo X.
        playerRB.velocity = new Vector2(horizontal * speedMove, speedY);

        doubleJump();

        // Atira cenouras
        if (Input.GetButtonDown("Fire1"))
        {
            fireCarrot();
        }


    }

    // Fun��o respons�vel por virar o personagem
    void flipCharacter()
    {
        isFaceLeft = !isFaceLeft;
        float x = transform.localScale.x;
        x *= -1; //inverte o sinal do scale;
        strenght *= -1; // inverte a for�a do tiro para ir do lado oposto.

        transform.localScale= new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
    //Fun��o respons�vel pelo pulo do personagem.
    public void jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
       
    }
    //Fun��o respons�vel pelo pulo duplo do jogador (caso a vari�vel extra jump seja maior que 1).
    void doubleJump()
    {

        if (isGrounded == true)
        {
            extraJump = jumpPlus;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0) // caso tenha o double jump, diminui o pulo extra a cada a��o de jump no ar.
        {
            jump();
            extraJump--;
        }
        else if (Input.GetButtonDown("Jump") && extraJump == 0 && isGrounded == true) // caso n�o tenha double jump.
        {
            jump();
        }
    }
    //fun��o respos�vel por instanciar o objeto e move-lo.
    void fireCarrot()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject temp = Instantiate(carrotsBullet);
            temp.transform.position = carrotLaunchPosition.position;
            temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(strenght, 0));

            Destroy(temp, 1f);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text win;

    public Text lives;

    public Text dead;

    private int scoreValue = 0;

    private int livesValue = 3;

    bool isDone = false;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    public AudioSource musicSource2;

    Animator anim;

    private bool facingRight = true;

    private bool isOnGround;

    public Transform groundcheck;

    public float checkRadius;

    public LayerMask allGround;





    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Coins  " + scoreValue.ToString();
        lives.text = "Lives  " + livesValue.ToString();
        win.text = "";
        dead.text = "";
        anim = GetComponent<Animator>();
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (isOnGround  ==  true)
        {
            anim.SetInteger("State", 0);
        }
        if (isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }
        if (hozMovement <0 && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }
        if (hozMovement >0 && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Coins  " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 8)
            {
                win.text = "You Win! Game By Alexander Williams";
                {
                    musicSource.clip = musicClipOne;
                    musicSource.Stop();
                    musicSource2.clip = musicClipTwo;
                    musicSource2.Play();

                }
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives  " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (livesValue <= 0)
        {
            dead.text = "You Lose";
            transform.position = new Vector3(100.0f, 100.0f, 0.0f);
        }

        if (!isDone)
        {
            if (scoreValue == 4)
            {
                transform.position = new Vector3(37.0f, 1.62f, 0.0f);
                livesValue = 3;
                lives.text = "Lives  " + livesValue.ToString();
                isDone = true;
            }
         
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}

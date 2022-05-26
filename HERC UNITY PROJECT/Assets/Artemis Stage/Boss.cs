using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    GameObject player;
    public float health;
    SpriteRenderer sr;
    [SerializeField][Range(1,7)] int noOfCounters;
    int attackCounter = 0;
    float attackTimer = 0f;
    float basicCharge = 1;
    float shotgunCharge = 1;
    bool hasAttacked = true;
    float rng;
    bool immune;

    Rigidbody2D rb;
    Animator anim;
    bool hasCharged;

    [SerializeField] GameObject arrow;
    [SerializeField] AudioSource arrowSound;
    [SerializeField] AudioSource parrySound;
    [SerializeField] AudioSource hurtSound;
    [SerializeField] AudioSource footsteps;
    ParticleSystem particles;

    Vector2 playerDirection;
    Vector3 newPos;
    GameObject hitbox;
    bool alive = true;

    // Start is called before the first frame update
    void Start()
    { 
        sr = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        newRandomPos();
        particles = gameObject.GetComponent<ParticleSystem>();
        anim = gameObject.GetComponent<Animator>();;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.x > transform.position.x)
        { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
        else
        { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }

        if (alive == true)
        {

            if ((transform.position - newPos).magnitude > 1)
            {
                Vector3 moveDirection = (newPos - transform.position).normalized;
                rb.velocity = moveDirection * 3;
                footsteps.volume = 1;
            }
            else
            {
                footsteps.volume = 0;
                rb.velocity = Vector2.zero;

                if (attackCounter < noOfCounters)
                {
                    immune = true;
                    if (hasAttacked == true)
                    { rng = Mathf.Floor(Random.Range(1f, 3f)); hasAttacked = false; }

                    //Debug.Log(rng);
                    if (rng == 1f)
                    {
                        //attack
                        if (attackTimer < basicCharge)
                        {
                            attackTimer += Time.deltaTime;
                            if (hasCharged == false && attackTimer < basicCharge - .5)
                            {
                                anim.Play("Artemis attack");
                                hasCharged = true;
                            }
                        }
                        else
                        {
                            rangeAttack();
                            attackCounter += 1;
                            hasCharged = false;
                            hasAttacked = true;
                            attackTimer = 0f;
                            anim.Play("Artemis Idle");
                        }
                    }
                    else if (rng == 2f)
                    {
                        //Shotgun
                        if (attackTimer < shotgunCharge)
                        {
                            attackTimer += Time.deltaTime;
                            if (hasCharged == false && attackTimer < shotgunCharge - .5)
                            {
                                anim.Play("Artemis attack");
                                hasCharged = true;
                            }
                        }
                        else
                        {
                            shotgunAttack();
                            attackCounter += 1;
                            hasCharged = false;
                            hasAttacked = true;
                            attackTimer = 0f;
                            anim.Play("Artemis Idle");
                        }
                    }
                }
                else if (attackCounter >= noOfCounters)
                {
                    immune = false;
                    if (particles.isPlaying == false)
                    { particles.Play(); }
                }
            }
        }
    }

    void rangeAttack()
    {
        //Debug.Log("Pew");
        arrowSound.time = 0;
        arrowSound.Play();
        hitbox = Instantiate(arrow, transform.position, Quaternion.identity);
        rotateTo2D(hitbox, player.transform.position);
        playerDirection = (player.transform.position - transform.position).normalized;
        hitbox.GetComponent<Rigidbody2D>().AddForce(playerDirection * 500, ForceMode2D.Force);
        Object.Destroy(hitbox, 10);
    }

    void shotgunAttack()
    {
        //Debug.Log("Pew");

        arrowSound.time = 0;
        arrowSound.Play();
        float shotgunAmmount = 5;
        for (int i = 0; i < shotgunAmmount; i++)
        {
            hitbox = Instantiate(arrow, transform.position, Quaternion.identity);
            rotateTo2D(hitbox, player.transform.position);
            float varienceScale = 5;
            Vector3 varience = new Vector3(Random.Range(-varienceScale, varienceScale), Random.Range(-varienceScale, varienceScale),1);
            //Debug.Log(varience);
            playerDirection = ((player.transform.position + varience) - transform.position).normalized;
            rotateTo2D(hitbox, player.transform.position + varience);
            hitbox.GetComponent<Rigidbody2D>().AddForce((playerDirection * 500), ForceMode2D.Force);
            Object.Destroy(hitbox, 10);
        }
    }

    void rotateTo2D(GameObject objecy, Vector3 destination)
    {
        //Rotate Towards Player
        float yVal = objecy.transform.position.y - destination.y;
        float xVal = objecy.transform.position.x - destination.x;
        float zAngRotation = Mathf.Atan2(yVal, xVal) * Mathf.Rad2Deg;
        objecy.transform.rotation = Quaternion.Euler(0, 0, zAngRotation + 90);
    }

    public void takeDamage()
    {
        if (immune == false)
        {
            immune = true;
            newRandomPos();
            hurtSound.time = 0;
            hurtSound.Play();
            particles.Stop();
            health -= .5f;
            StartCoroutine(onHurt());
            attackCounter = 0;
            if (health <= 0)
            {
                alive = false;
                StartCoroutine(death());
                StartCoroutine(changeScene());
            }
        }
        else
        {
            parrySound.time = 0;
            parrySound.Play(); 
        }

    }

    IEnumerator death()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.Play("Artemis Death");
        GameObject.Find("Dialogue").GetComponent<Dialogue>().newText(gameObject, "AUGHHH",3,Color.green);
        yield return new WaitForSeconds(3);
        GameObject.Find("Dialogue").GetComponent<Dialogue>().newText(gameObject, "I... I Guess you have proven your self Hercules...", 5, Color.green);
    }

    IEnumerator changeScene()
    {
        //
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("Credits");
    }

    IEnumerator onHurt()
    {
        //Debug.Log("Hurt");
        sr.color = Color.red;
        yield return new WaitForSeconds(1);
        sr.color = Color.white;
    }

    void newRandomPos()
    {
        float centeredScale = .75f;
        float inverse = Mathf.Abs(1 - centeredScale);

        float RandomY = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0, 0 + Screen.height * inverse, 1)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * centeredScale, 1)).y);

        float RandomX = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector3(0 + Screen.width * inverse, 0, 1)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * centeredScale, 0, 1)).x);

        newPos = new Vector3(RandomX, RandomY, 1);
    }
}

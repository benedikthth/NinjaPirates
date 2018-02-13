using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    //getters as shortcuts variables
    public Rigidbody2D rb { get { return GetComponent<Rigidbody2D>(); } }
    public Collider2D coll { get { return GetComponent<PolygonCollider2D>(); } }

    //this list keeps track of all current player colliders touching this players attack trigger/colliders.
    private List<Player> collision = new List<Player>();

    //A bool to keep track of if the player is alive atm.
    private bool alive = true;
    public bool Alive { get { return alive; } }

    //A bool to keep track of if the player is stunned atm.
    private bool stunned = false;
    public bool Stunned { get { return stunned; } }

    //A bool to keep track of if the player is jumping (aka freefalling in the air)
    private bool jumping;
    public bool Jumping { get { return jumping; } }

    //this players score
    private int score;
    public int Score { get { return score; } }

    //reference to the sprite renderer.
    public SpriteRenderer playerSprite;
    public SpriteRenderer playerHeadSprite;

    //Color getter and setter that actually get/set the color of the player sprite.
    public Color Color{ set { playerSprite.color = value;  } get { return playerSprite.color; } }

    //variables and references concerning the kick action.
    float currentKickCooldown = 0;
    float KickCD = 1.5f;
    float kickAnimDuration = 0.5f;
    public GameObject kickWind;
    public ParticleSystem stun;
    public UnityEngine.UI.Text kickCoolDownText;

    //old player options
    public float kickForce;
    public float jumpForce;
    public float stunDuration;
    public int maxAirJumps;
    private int airJumps;
    public float maxRunSpeed;

    //variable that holds the current would be killer, if the player dies with this variable set, the killer get the score.
    private Player killer;

    private void OnTriggerEnter2D(Collider2D target)
    {
        //if a player enters this players collider/trigger, add the player to collision list.
        if (!target.isTrigger && target.tag == "Player" && !collision.Contains(target.GetComponent<Player>()))
        {
            collision.Add(target.GetComponent<Player>());
        }
        else if(target.tag == "Ocean") //if this player enters a trigger with the tag ocean, then he dies.
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        //if a player leaves this players collider/trigger, remove him from the collision list.
        if (!target.isTrigger && target.tag == "Player" && collision.Contains(target.GetComponent<Player>()))
        {
            collision.Remove(target.GetComponent<Player>());
        }
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        //if this player collides with the ship, then he landed.
        if (!target.collider.isTrigger && target.collider.tag == "Ship")
        {
            jumping = false;
            airJumps = maxAirJumps;
        }
    }

    private void OnCollisionExit2D(Collision2D target)
    {
        //if this player is not colliding with the ship, he is jumping.
        if (!target.collider.isTrigger && target.collider.tag == "Ship")
        {
            jumping = true;
        }
    }

    //coroutine function that displays the kick sprite for kickAnimDuration time.
    IEnumerator KickAnimation()
    {
        kickWind.SetActive(true);
        yield return new WaitForSeconds(kickAnimDuration);
        kickWind.SetActive(false);
    }

    //coroutine function that displays the kick cooldown above the player for the duration of the cooldown.
    IEnumerator KickCoolDown()
    {
        kickCoolDownText.gameObject.SetActive(true);
        float currTime = KickCD;
        while (currTime > 0)
        {
            kickCoolDownText.text = currTime.ToString("#.##");
            yield return new WaitForSeconds(0.1f);
            currTime -= .1f;
        }
        kickCoolDownText.gameObject.SetActive(false);
    }

    //coroutine function that sets stunned variable, and removes it after "duration"
    IEnumerator ApplyStun(float duration)
    {
        stunned = true;
        stun.Play();
        float currTime = duration;
        yield return new WaitForSeconds(duration);
        stun.Stop();
        stunned = false;
    }

    //a function that starts the stun process (via coroutines)
    public void Stun()
    {
        StartCoroutine(ApplyStun(stunDuration));
    }

    //reference to the tagKillerCoroutine coroutine, used to interrupt it.
    Coroutine tagKillerCoroutine;
    //coroutine function that sets the killer tag, if it the player is still alive after 5sec, remove killer tag.
    IEnumerator ApplyKiller(Player murderedBy)
    {
        killer = murderedBy;
        yield return new WaitForSeconds(5f);
        killer = null;
        tagKillerCoroutine = null;
    }


    //function that starts the apply killer coroutine
    public void TagKiller(Player murderedBy)
    {
        if(tagKillerCoroutine != null)
        {
            StopCoroutine(tagKillerCoroutine);
        }
        tagKillerCoroutine = StartCoroutine(ApplyKiller(murderedBy));
    }

    //function that delivers the kick to the player target and sets off visuals/cooldowns.
    public void Kick()
    {
        //StartCoroutine("KickCoolDown");
        for (int i = 0; i < collision.Count; i++)
        {
            bool targetLeft = (collision[i].transform.position.x < transform.position.x);
            collision[i].rb.AddForce((Vector2)(collision[i].transform.position - transform.position).normalized * kickForce);
            kickWind.transform.LookAt(collision[i].transform);
            Vector3 pos = kickWind.transform.localScale;
            kickWind.transform.eulerAngles = new Vector3(0, 0,  (targetLeft ? kickWind.transform.eulerAngles.x + 180 : -kickWind.transform.eulerAngles.x + -180));
            kickWind.transform.localScale =  new Vector3(targetLeft ? Mathf.Abs(pos.x) : -Mathf.Abs(pos.x), -Mathf.Abs(pos.y), pos.z);
            
            StartCoroutine("KickAnimation");

            collision[i].Stun();
            collision[i].TagKiller(this);
        }
    }


    //function that jumps the player if he has jumps left.
    public void Jump()
    {
        if ((!jumping) || (jumping && airJumps > 0))
        {
            airJumps--;
            rb.velocity = new Vector2(rb.velocity.x/2, (jumping ? 8 : 10));
            //rb.AddForce(new Vector2(0, jumpForce));
        }
    }

    //function that calls the jump/kick functions at the right time.
    //if player is in range of another player, kick, else jump.
    public void Action()
    {
        if(!stunned && alive)
        {
            if (collision.Count == 0)
            {
                Jump();
            }
            else
            {
                Kick();
            }
        }
    }

    //a function that moves the player to the left
    public void Left()
    {
        if (!stunned && alive)
        {
            //Debug.Log("left");
            float x = Mathf.Clamp(rb.velocity.x - (jumping ? .25f : .5f), -10, 10);
            rb.velocity = new Vector2(x, rb.velocity.y);
            playerHeadSprite.flipX = true;
            //rb.AddForce(new Vector2(-maxRunSpeed, 0));
        }
    }

    //a function that moves the player to the right
    public void Right()
    {
        if (!stunned && alive)
        {
            //Debug.Log("right");
            float x = Mathf.Clamp(rb.velocity.x + (jumping ? .25f : .5f), -10, 10);
            rb.velocity = new Vector2(x, rb.velocity.y);
            playerHeadSprite.flipX = false;
            //rb.AddForce(new Vector2(maxRunSpeed, 0));
        }
    }

    //a function that adds 1 to this player's score.
    public void Kill()
    {
        score++;
    }

    // a function that sets the player as dead, gives score to the player's killer, updates the score ui and respawns the player after death.
    public void Die()
    {
        alive = false;
        if(killer != null)
        {
            killer.Kill();
            killer = null;
        }
        else
        {
            score = (int)Mathf.Clamp(score-1, 0, float.MaxValue);
        }


        rb.velocity = new Vector2(0, 0);
        stun.Stop();
        transform.position = Game.Instance.tempRespawnList[Random.Range(0, Game.Instance.tempRespawnList.Count - 1)];
        alive = true;

        UIManager.Instance.UpdateScore();
    }
}

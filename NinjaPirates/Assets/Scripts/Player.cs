using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public Rigidbody2D RigidBody { get { return GetComponent<Rigidbody2D>(); } }
    public Collider2D Collider { get { return GetComponent<PolygonCollider2D>(); } }

    private List<Player> collision = new List<Player>();

    private bool alive = true;
    public bool Alive { get { return alive; } }

    private bool stunned = false;
    public bool Stunned { get { return stunned; } }

    private bool jumping;
    public bool Jumping { get { return jumping; } }

    private int score;
    public int Score { get { return score; } }

    public SpriteRenderer playerSprite;

    public Color Color{ set { playerSprite.color = value;  } get { return playerSprite.color; } }

    float currentKickCooldown = 0;
    float KickCD = 1.5f;
    float kickAnimDuration = 0.5f;
    public GameObject leftKickWind, rightKickWind;
    public UnityEngine.UI.Text kickCoolDownText;

    //oldplayeroptions
    public float kickForce;
    public float jumpForce;
    public float stunDuration;
    public int maxAirJumps;
    private int airJumps;
    public float maxRunSpeed;

    public Player killer;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (!target.isTrigger && target.tag == "Player" && !collision.Contains(target.GetComponent<Player>()))
        {
            collision.Add(target.GetComponent<Player>());
        }
        else if(target.tag == "Ocean")
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (!target.isTrigger && target.tag == "Player" && collision.Contains(target.GetComponent<Player>()))
        {
            collision.Remove(target.GetComponent<Player>());
        }
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (!target.collider.isTrigger && target.collider.tag == "Ship")
        {
            jumping = false;
            airJumps = maxAirJumps;
        }
    }

    private void OnCollisionExit2D(Collision2D target)
    {
        if (!target.collider.isTrigger && target.collider.tag == "Ship")
        {
            jumping = true;
        }
    }

    IEnumerator rightKick()
    {
        rightKickWind.SetActive(true);
        yield return new WaitForSeconds(kickAnimDuration);
        rightKickWind.SetActive(false);
    }
    IEnumerator leftKick()
    {
        leftKickWind.SetActive(true);
        yield return new WaitForSeconds(kickAnimDuration);
        leftKickWind.SetActive(false);
    }

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

    IEnumerator ApplyStun(float duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    public void Stun()
    {
        StartCoroutine(ApplyStun(stunDuration));
    }

    Coroutine tagKillerCoroutine;
    IEnumerator ApplyKiller(Player murderedBy)
    {
        killer = murderedBy;
        yield return new WaitForSeconds(5f);
        killer = null;
        tagKillerCoroutine = null;
    }

    public void TagKiller(Player murderedBy)
    {
        if(tagKillerCoroutine != null)
        {
            StopCoroutine(tagKillerCoroutine);
        }
        tagKillerCoroutine = StartCoroutine(ApplyKiller(murderedBy));
    }

    public void Kick()
    {
        StartCoroutine("KickCoolDown");
        for (int i = 0; i < collision.Count; i++)
        {
            if (collision[i].transform.position.x < transform.position.x)
            {
                //start left kickWind
                collision[i].RigidBody.AddForce(new Vector2(-kickForce, 0));
                StartCoroutine("leftKick");
            }
            else
            {
                //start Right kickwind
                StartCoroutine("rightKick");
                collision[i].RigidBody.AddForce(new Vector2(kickForce, 0));
            }

            collision[i].Stun();
            collision[i].TagKiller(this);
        }
    }

    public void Jump()
    {
        if ((!jumping) || (jumping && airJumps > 0))
        {
            airJumps--;
            RigidBody.AddForce(new Vector2(0, jumpForce));
        }
    }

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

    public void Left()
    {
        if (!stunned && alive)
        {
            //Debug.Log("left");
            RigidBody.AddForce(new Vector2(-maxRunSpeed, 0));
        }
    }

    public void Right()
    {
        if (!stunned && alive)
        {
            //Debug.Log("right");
            RigidBody.AddForce(new Vector2(maxRunSpeed, 0));
        }
    }

    public void Kill()
    {
        score++;
    }

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


        RigidBody.velocity = new Vector2(0, 0);
        transform.position = Game.Instance.tempRespawnList[Random.Range(0, Game.Instance.tempRespawnList.Count - 1)];
        alive = true;

        UIManager.Instance.UpdateScore();
    }
}

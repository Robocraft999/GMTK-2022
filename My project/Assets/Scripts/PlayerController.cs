using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Rigidbody2D ProjectilePrefab;
    private float shootPower = 40f;

    public int Score { get; private set; } = 1000; //0
    public List<ActionSlot> actionSlots { get; set; }
    public List<ActionItem> actions { get; set; }

    void Awake()
    {
        if(!GameManager.Instance.Players.Contains(this))Destroy(gameObject);
        DontDestroyOnLoad(this);

        //GameManager.Instance.OnStateChange += StateChangeHandler;
        gameObject.SetActive(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*void StateChangeHandler(GameState oldState, GameState newState)
    {
        if(newState == GameState.CLASH)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }*/

    public void InitActions(int amount)
    {
        actionSlots = new List<ActionSlot>();
        actions = new List<ActionItem>();
        for (int j = 0; j < amount; j++)
        {
            ActionSlot slot = new ActionSlot
            {
                slotId = j,
                MaxItems = 3
            };
            actionSlots.Add(slot);
        }

        ActionItem actionItem = new ActionItem
        {
            Type = GameManager.Instance.RandomAction()
        };
        actions.Add(actionItem);
    }

    public IEnumerator PerformAction(int input)
    {
        foreach (ActionSlot slot in actionSlots.Where(slot => slot.slotId + 1 == input).Where(slot => slot.Items.Count > 0))
        {
            foreach(ActionItem item in slot.Items)
            {
                ActionType type = item.Type;
                if (type.ApplyForce) ApplyForce(type.Force);
                if (type.Attack) Attack(GameManager.Instance.Players.Where(p => p != this).ToList()[0].gameObject);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void AddAction(ActionType type)
    {
        actions.Add(new ActionItem
        {
            Type = type
        });
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void Attack(GameObject other)
    {
        Vector3 dir = new Vector3(other.transform.position.x - transform.position.x, transform.position.y);
        Vector3 extend = new Vector3(GetComponent<Collider2D>().bounds.extents.x, 0, 0) * dir.normalized.x;
        Rigidbody2D projectile = Instantiate(ProjectilePrefab, transform.position + extend, transform.rotation);
        projectile.AddForce(dir * shootPower);
        StartCoroutine(ProjectileTravelHandler(projectile, other.GetComponent<Collider2D>()));
    }

    public IEnumerator ProjectileTravelHandler(Rigidbody2D projectile, Collider2D other)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("ProjectileHit"));
        float maxFrames = 1000;
        float runningTime = 0;
        while (runningTime < maxFrames)
        {
            if(projectile.IsTouching(other))
            {
                other.GetComponent<Rigidbody2D>().AddForce(projectile.velocity / 4f);
                AddScore(250);
                break;
            }
            if (projectile.IsTouching(contactFilter))
            {
                runningTime = maxFrames;
            }
            yield return new WaitForSeconds(0.01f);
            runningTime++;
        }
        Destroy(projectile.gameObject);
        yield return null;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if(ClashSceneUIManager.Instance is object)
        {
            ClashSceneUIManager.Instance.UpdateScoreText();
        }
    }

    public void RemoveScore(int amount)
    {
        AddScore(-amount);
    }

    public Collider2D Collider()
    {
        return GetComponent<Collider2D>();
    }

    public void Activate(bool state)
    {
        GetComponent<SpriteRenderer>().enabled = state;
        gameObject.SetActive(state);
    }
}

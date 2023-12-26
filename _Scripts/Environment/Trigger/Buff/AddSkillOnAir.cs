using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class AddSkillOnAir : MonoBehaviour
{
    public bool AddDash;
    public bool AddJump;

    bool isActive = true;
    float coolTime = 3f;
    Player player;
    public SpriteRenderer sprite;
    public bool useFB;
    public MMFeedbacks addSkill;
    private void Start()
    {
        isActive = true;
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && isActive)
        {
            Debug.Log("Disappear");

            player = Player.instance;
            if (AddDash && !player.canDash)
            {
                player.canDash = true;
                isActive = false;
            }
            if (AddJump && !player.canJumpOnAir)
            {
                player.canJumpOnAir = true;
                isActive = false;
            }
            if (!isActive)
            {
                if (useFB)
                    addSkill.PlayFeedbacks();
                SoundManager.PlayAward();
                StartCoroutine(Dis());
            }
        }
    }

    public IEnumerator Dis()
    {
        Color temp = sprite.color;
        temp.a = 0.5f;
        sprite.color = temp;
        yield return new WaitForSeconds(coolTime);
        isActive = true;
        temp.a = 1f;
        sprite.color = temp;
    }
}
